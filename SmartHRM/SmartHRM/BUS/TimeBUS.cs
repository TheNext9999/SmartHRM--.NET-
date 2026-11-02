using ClosedXML.Excel;
using SmartHRM.DAL;
using SmartHRM.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace SmartHRM.BUS
{
    public class ShiftBUS : GenericBUS<ShiftDTO>
    {
        public ShiftBUS() : base(Const.M_SHIFT) { }

        protected override void Validate(ShiftDTO e)
        {
            base.Validate(e);
            TimeSpan a, b;
            if (!TimeSpan.TryParse(e.StartTime, out a) || !TimeSpan.TryParse(e.EndTime, out b))
                throw new ValidationException("Giờ ca làm việc phải có dạng HH:mm.");
            if (e.BreakMinutes < 0) throw new ValidationException("Thời gian nghỉ không hợp lệ.");
        }
    }

    public class WorkScheduleBUS : GenericBUS<WorkScheduleDTO>
    {
        public WorkScheduleBUS() : base(Const.M_SCHEDULE) { }
        protected override void Validate(WorkScheduleDTO e)
        {
            base.Validate(e);
            if (e.ToDate.HasValue && e.ToDate < e.FromDate) throw new ValidationException("\"Đến ngày\" phải sau \"Từ ngày\".");
        }
    }

    public class ImportResult
    {
        public int Ok;
        public List<string> Errors = new List<string>();
    }

    public class AttendanceBUS : GenericBUS<AttendanceDTO>
    {
        readonly AttendanceDAL dal;
        readonly ShiftDAL shifts = new ShiftDAL();
        public AttendanceBUS() : this(new AttendanceDAL()) { }
        AttendanceBUS(AttendanceDAL d) : base(Const.M_ATTENDANCE, d) { dal = d; }

        public override List<AttendanceDTO> GetAll()
        {
            Session.Require(Module, "view");
            if (Session.SelfOnly(Module)) return dal.GetAll("employee_id=@e", "work_date DESC", DbHelper.P("@e", Session.EmployeeId.Value));
            return dal.GetAll(null, "work_date DESC, id DESC");
        }

        public override AttendanceDTO CreateNew() { return new AttendanceDTO { WorkDate = DateTime.Today, Method = "Thủ công" }; }

        protected override void Validate(AttendanceDTO e)
        {
            base.Validate(e);
            if (e.CheckIn.HasValue && e.CheckOut.HasValue && e.CheckOut < e.CheckIn) throw new ValidationException("Giờ ra phải sau giờ vào.");
            var ex = dal.Find(e.EmployeeId.Value, e.WorkDate.Value);
            if (ex != null && ex.Id != e.Id) throw new ValidationException("Nhân viên này đã có dữ liệu chấm công ngày " + e.WorkDate.Value.ToString("dd/MM/yyyy") + ".");
            if (new TimesheetDAL().IsLocked(e.WorkDate.Value.Month, e.WorkDate.Value.Year))
                throw new ValidationException("Bảng công tháng " + e.WorkDate.Value.Month + "/" + e.WorkDate.Value.Year + " đã chốt, không thể sửa.");
        }

        protected override void BeforeSave(AttendanceDTO e, bool isNew) { Compute(e); }

        /// <summary>Tính đi muộn, về sớm, số công theo ca làm việc của nhân viên trong ngày.</summary>
        public void Compute(AttendanceDTO e)
        {
            e.LateMinutes = 0; e.EarlyMinutes = 0; e.WorkDays = 0;
            var shift = shifts.GetForEmployee(e.EmployeeId.Value, e.WorkDate.Value);
            if (shift == null || !e.CheckIn.HasValue) return;

            var date = e.WorkDate.Value.Date;
            var start = date + TimeSpan.Parse(shift.StartTime);
            var end = date + TimeSpan.Parse(shift.EndTime);
            if (end <= start) end = end.AddDays(1);

            e.LateMinutes = (int)Math.Max(0, Math.Floor((e.CheckIn.Value - start).TotalMinutes));
            if (!e.CheckOut.HasValue) return;
            e.EarlyMinutes = (int)Math.Max(0, Math.Floor((end - e.CheckOut.Value).TotalMinutes));

            var effStart = e.CheckIn.Value > start ? e.CheckIn.Value : start;
            var effEnd = e.CheckOut.Value < end ? e.CheckOut.Value : end;
            double standard = (end - start).TotalMinutes - shift.BreakMinutes;
            double worked = (effEnd - effStart).TotalMinutes - (effEnd > effStart ? shift.BreakMinutes : 0);
            double ratio = standard <= 0 ? 0 : Math.Max(0, Math.Min(1, worked / standard));
            e.WorkDays = ratio >= 0.9 ? 1m : ratio >= 0.45 ? 0.5m : 0m;
        }

        /// <summary>Chấm công vào/ra cho chính người đăng nhập (Wifi / GPS / thủ công trên giao diện).</summary>
        public string CheckInOut(string method)
        {
            if (!Session.EmployeeId.HasValue) throw new ValidationException("Tài khoản chưa được liên kết với hồ sơ nhân viên.");
            int emp = Session.EmployeeId.Value;
            string verify;
            if (method == "Wifi" && !LocationService.VerifyWifi(out verify)) throw new ValidationException(verify);
            if (method == "GPS" && !LocationService.VerifyGps(out verify)) throw new ValidationException(verify);

            var now = DateTime.Now;
            var rec = dal.Find(emp, now.Date);
            var y = dal.Find(emp, now.Date.AddDays(-1));          // ca đêm: ra ca sang ngày hôm sau
            if (y != null && y.CheckIn.HasValue && !y.CheckOut.HasValue && (now - y.CheckIn.Value).TotalHours < 14)
            {
                var ys = shifts.GetForEmployee(emp, y.WorkDate.Value);
                if (ys != null && ys.IsNight) rec = y;
            }
            string msg;
            if (rec == null)
            {
                rec = new AttendanceDTO { EmployeeId = emp, WorkDate = now.Date, CheckIn = now, Method = method };
                msg = "Đã chấm công VÀO lúc " + now.ToString("HH:mm");
            }
            else
            {
                if (new TimesheetDAL().IsLocked(rec.WorkDate.Value.Month, rec.WorkDate.Value.Year)) throw new ValidationException("Bảng công đã chốt.");
                if (!rec.CheckIn.HasValue) rec.CheckIn = now;
                rec.CheckOut = now; rec.Method = method;
                msg = "Đã chấm công RA lúc " + now.ToString("HH:mm");
            }
            Compute(rec);
            if (rec.Id == 0) dal.Insert(rec); else dal.Update(rec);
            AuditBUS.Log("Chấm công", Module, msg + " (" + method + ")");
            return msg;
        }

        // ---------- Nhập dữ liệu từ máy chấm công (file Excel) ----------
        // Cột: A Mã NV | B Ngày | C Giờ vào | D Giờ ra
        public ImportResult ImportExcel(string path)
        {
            Session.Require(Module, "add");
            var res = new ImportResult();
            var empDal = new EmployeeDAL();
            using (var wb = new XLWorkbook(path))
            {
                var ws = wb.Worksheet(1);
                foreach (var row in ws.RowsUsed().Skip(1))
                {
                    int line = row.RowNumber();
                    try
                    {
                        string code = row.Cell(1).GetString().Trim();
                        if (code == "") continue;
                        var emp = empDal.GetByCode(code);
                        if (emp == null) throw new ValidationException("Không có nhân viên mã " + code);
                        var date = ReadDate(row.Cell(2));
                        if (!date.HasValue) throw new ValidationException("Ngày không hợp lệ");
                        var tin = ReadTime(row.Cell(3));
                        var tout = ReadTime(row.Cell(4));
                        if (!tin.HasValue) throw new ValidationException("Thiếu giờ vào");

                        var rec = dal.Find(emp.Id, date.Value) ?? new AttendanceDTO { EmployeeId = emp.Id, WorkDate = date.Value };
                        rec.CheckIn = date.Value + tin.Value;
                        rec.CheckOut = null;
                        if (tout.HasValue)
                        {
                            rec.CheckOut = date.Value + tout.Value;
                            if (rec.CheckOut < rec.CheckIn) rec.CheckOut = rec.CheckOut.Value.AddDays(1);   // ca đêm
                        }
                        rec.Method = "Excel";
                        if (new TimesheetDAL().IsLocked(date.Value.Month, date.Value.Year)) throw new ValidationException("Bảng công tháng đã chốt");
                        Compute(rec);
                        if (rec.Id == 0) dal.Insert(rec); else dal.Update(rec);
                        res.Ok++;
                    }
                    catch (Exception ex) { res.Errors.Add("Dòng " + line + ": " + ex.Message); }
                }
            }
            AuditBUS.Log("Nhập chấm công", Module, "Excel: " + res.Ok + " dòng thành công, " + res.Errors.Count + " lỗi");
            return res;
        }

        static DateTime? ReadDate(IXLCell c)
        {
            if (c.IsEmpty()) return null;
            if (c.DataType == XLDataType.DateTime) return c.GetDateTime().Date;
            if (c.DataType == XLDataType.Number) return DateTime.FromOADate(c.GetDouble()).Date;
            DateTime d;
            return DateTime.TryParse(c.GetString().Trim(), new CultureInfo("vi-VN"), DateTimeStyles.None, out d) ? (DateTime?)d.Date : null;
        }

        static TimeSpan? ReadTime(IXLCell c)
        {
            if (c.IsEmpty()) return null;
            if (c.DataType == XLDataType.DateTime) return c.GetDateTime().TimeOfDay;
            if (c.DataType == XLDataType.TimeSpan) return c.GetTimeSpan();
            if (c.DataType == XLDataType.Number) return TimeSpan.FromDays(c.GetDouble());
            TimeSpan t;
            return TimeSpan.TryParse(c.GetString().Trim(), out t) ? (TimeSpan?)t : null;
        }
    }

    /// <summary>Xác thực vị trí chấm công: Wi-Fi (SSID) hoặc GPS (bán kính quanh công ty).</summary>
    public static class LocationService
    {
        public static string CurrentSsid()
        {
            try
            {
                var psi = new ProcessStartInfo("netsh", "wlan show interfaces")
                { RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = true };
                using (var p = Process.Start(psi))
                {
                    var text = p.StandardOutput.ReadToEnd();
                    p.WaitForExit(3000);
                    var m = Regex.Match(text, @"^\s*SSID\s*:\s*(.+?)\s*$", RegexOptions.Multiline);
                    return m.Success ? m.Groups[1].Value : null;
                }
            }
            catch { return null; }
        }

        public static bool VerifyWifi(out string message)
        {
            var allowed = AppSettings.Get("AllowedSsid", "");
            var ssid = CurrentSsid();
            if (string.IsNullOrWhiteSpace(allowed)) { message = null; return true; }   // chưa cấu hình = không kiểm tra
            if (ssid == null) { message = "Không đọc được Wi-Fi hiện tại. Hãy kết nối Wi-Fi của công ty."; return false; }
            bool ok = allowed.Split(',').Select(s => s.Trim()).Any(s => string.Equals(s, ssid, StringComparison.OrdinalIgnoreCase));
            message = ok ? null : "Wi-Fi \"" + ssid + "\" không nằm trong danh sách được phép chấm công.";
            return ok;
        }

        public static bool VerifyGps(out string message)
        {
            double lat, lng;
            var inv = CultureInfo.InvariantCulture;
            if (!double.TryParse(AppSettings.Get("CompanyLat", ""), NumberStyles.Float, inv, out lat) ||
                !double.TryParse(AppSettings.Get("CompanyLng", ""), NumberStyles.Float, inv, out lng))
            { message = "Chưa cấu hình tọa độ công ty (CompanyLat/CompanyLng)."; return false; }
            double radius = (double)AppSettings.GetDecimal("GpsRadius", 200);
            using (var w = new GeoCoordinateWatcher(GeoPositionAccuracy.High))
            {
                if (!w.TryStart(false, TimeSpan.FromSeconds(8)) || w.Position.Location.IsUnknown)
                { message = "Không lấy được vị trí GPS. Hãy bật Dịch vụ vị trí của Windows."; return false; }
                double dist = w.Position.Location.GetDistanceTo(new GeoCoordinate(lat, lng));
                message = dist <= radius ? null : "Bạn đang cách công ty " + Math.Round(dist) + " m (cho phép " + radius + " m).";
                return dist <= radius;
            }
        }
    }

    public class LeaveBUS : GenericBUS<LeaveRequestDTO>
    {
        readonly LeaveDAL dal;
        public LeaveBUS() : this(new LeaveDAL()) { }
        LeaveBUS(LeaveDAL d) : base(Const.M_LEAVE, d) { dal = d; }

        public override List<LeaveRequestDTO> GetAll()
        {
            Session.Require(Module, "view");
            if (Session.SelfOnly(Module)) return dal.GetAll("employee_id=@e", "id DESC", DbHelper.P("@e", Session.EmployeeId.Value));
            return dal.GetAll();
        }

        public override LeaveRequestDTO CreateNew()
        {
            return new LeaveRequestDTO { EmployeeId = Session.SelfOnly(Module) ? Session.EmployeeId : null, FromDate = DateTime.Today, ToDate = DateTime.Today };
        }

        protected override void Validate(LeaveRequestDTO e)
        {
            if (Session.SelfOnly(Module)) e.EmployeeId = Session.EmployeeId;
            base.Validate(e);
            if (e.ToDate < e.FromDate) throw new ValidationException("\"Đến ngày\" phải sau hoặc bằng \"Từ ngày\".");
            if (e.Id != 0)
            {
                var old = dal.GetById(e.Id);
                if (old != null && old.Status != Const.Pending) throw new ValidationException("Đơn đã được xử lý, không thể sửa.");
            }
            if (e.Type != Const.LeaveAnnual && e.Hours <= 0) throw new ValidationException("Hãy nhập số giờ cho đơn này.");
        }

        protected override void BeforeSave(LeaveRequestDTO e, bool isNew)
        {
            if (e.Type == Const.LeaveAnnual && e.Days <= 0) e.Days = (decimal)(e.ToDate.Value.Date - e.FromDate.Value.Date).TotalDays + 1;
            if (isNew) e.Status = Const.Pending;
        }

        protected override void BeforeDelete(LeaveRequestDTO e)
        {
            if (e.Status != Const.Pending) throw new ValidationException("Chỉ xóa được đơn đang chờ duyệt.");
        }

        /// <summary>Duyệt / từ chối đơn. Duyệt đơn nghỉ phép sẽ tự trừ vào quỹ phép năm.</summary>
        public void Decide(LeaveRequestDTO e, bool approve)
        {
            Session.Require(Module, "edit");
            if (e.Status != Const.Pending) throw new ValidationException("Đơn đã được xử lý.");
            if (approve && e.Type == Const.LeaveAnnual)
            {
                var emp = new EmployeeDAL().GetById(e.EmployeeId.Value);
                if (emp.AnnualLeaveBalance < e.Days)
                    throw new ValidationException("Quỹ phép năm của " + emp.FullName + " chỉ còn " + emp.AnnualLeaveBalance + " ngày, không đủ " + e.Days + " ngày.");
                new EmployeeDAL().AdjustLeaveBalance(emp.Id, -e.Days);
            }
            e.Status = approve ? Const.Approved : Const.Rejected;
            e.ApprovedBy = Session.User.Id;
            Dal.Update(e);
            AuditBUS.Log(approve ? "Duyệt đơn" : "Từ chối đơn", Module, Describe(e));
        }
    }

    public class TimesheetBUS
    {
        readonly TimesheetDAL dal = new TimesheetDAL();

        public DataTable View(int month, int year) { Session.Require(Const.M_TIMESHEET, "view"); return dal.View(month, year); }
        public DataTable GetIssues(int month, int year) { Session.Require(Const.M_TIMESHEET, "view"); return dal.GetIssues(month, year); }

        public void Calculate(int month, int year)
        {
            Session.Require(Const.M_TIMESHEET, "edit");
            if (dal.IsLocked(month, year)) throw new ValidationException("Bảng công tháng " + month + "/" + year + " đã chốt. Hãy mở khóa nếu cần tính lại.");
            dal.Calculate(month, year);
            AuditBUS.Log("Tổng hợp công", Const.M_TIMESHEET, month + "/" + year);
        }

        public void Lock(int month, int year)
        {
            Session.Require(Const.M_TIMESHEET, "edit");
            if (dal.View(month, year).Rows.Count == 0) throw new ValidationException("Chưa có dữ liệu - hãy bấm \"Tổng hợp công\" trước.");
            dal.SetLock(month, year, true);
            AuditBUS.Log("Chốt công", Const.M_TIMESHEET, month + "/" + year);
        }

        public void Unlock(int month, int year)
        {
            Session.Require(Const.M_TIMESHEET, "delete");
            if (DbHelper.ScalarInt("SELECT COUNT(*) FROM payroll WHERE month=@m AND year=@y AND status<>'Nháp'", DbHelper.P("@m", month), DbHelper.P("@y", year)) > 0)
                throw new ValidationException("Bảng lương tháng này đã chốt, không thể mở khóa bảng công.");
            dal.SetLock(month, year, false);
            AuditBUS.Log("Mở khóa công", Const.M_TIMESHEET, month + "/" + year);
        }
    }
}