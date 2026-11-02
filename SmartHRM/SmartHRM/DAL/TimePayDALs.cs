using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySqlConnector;
using SmartHRM.DTO;

namespace SmartHRM.DAL
{
    public class ShiftDAL : GenericDAL<ShiftDTO>
    {
        /// <summary>Ca làm việc của nhân viên tại một ngày: theo lịch phân ca, mặc định ca đầu tiên.</summary>
        public ShiftDTO GetForEmployee(int employeeId, DateTime date)
        {
            var dt = DbHelper.Query(
                "SELECT s.* FROM work_schedules w JOIN shifts s ON s.id=w.shift_id " +
                "WHERE w.employee_id=@e AND w.from_date<=@d AND (w.to_date IS NULL OR w.to_date>=@d) ORDER BY w.from_date DESC LIMIT 1",
                DbHelper.P("@e", employeeId), DbHelper.P("@d", date.Date));
            if (dt.Rows.Count > 0) return Map(dt.Rows[0]);
            return GetAll(null, "id").FirstOrDefault();
        }
    }

    public class WorkScheduleDAL : GenericDAL<WorkScheduleDTO> { }

    public class AttendanceDAL : GenericDAL<AttendanceDTO>
    {
        public AttendanceDTO Find(int employeeId, DateTime date)
        {
            return GetAll("employee_id=@e AND work_date=@d", "id", DbHelper.P("@e", employeeId), DbHelper.P("@d", date.Date)).FirstOrDefault();
        }
    }

    public class LeaveDAL : GenericDAL<LeaveRequestDTO>
    {
        public int CountPending() { return DbHelper.ScalarInt("SELECT COUNT(*) FROM leave_requests WHERE status='Chờ duyệt'"); }
    }

    public class TimesheetDAL : GenericDAL<TimesheetDTO>
    {
        public bool IsLocked(int month, int year)
        {
            return DbHelper.ScalarInt("SELECT COUNT(*) FROM monthly_timesheets WHERE month=@m AND year=@y AND is_locked=1",
                DbHelper.P("@m", month), DbHelper.P("@y", year)) > 0;
        }

        public void Calculate(int month, int year)
        {
            var employees = new EmployeeDAL().GetAll("status<>'Nghỉ việc'", "id");
            var att = DbHelper.Query(
                "SELECT employee_id, SUM(work_days) wd, SUM(CASE WHEN late_minutes>0 THEN 1 ELSE 0 END) lc, SUM(late_minutes) lm, SUM(early_minutes) em " +
                "FROM attendance WHERE MONTH(work_date)=@m AND YEAR(work_date)=@y GROUP BY employee_id",
                DbHelper.P("@m", month), DbHelper.P("@y", year));
            var leave = DbHelper.Query(
                "SELECT employee_id, type, SUM(days) d, SUM(hours) h FROM leave_requests " +
                "WHERE status='Đã duyệt' AND MONTH(from_date)=@m AND YEAR(from_date)=@y GROUP BY employee_id, type",
                DbHelper.P("@m", month), DbHelper.P("@y", year));

            foreach (var emp in employees)
            {
                var t = GetAll("employee_id=@e AND month=@m AND year=@y", "id",
                    DbHelper.P("@e", emp.Id), DbHelper.P("@m", month), DbHelper.P("@y", year)).FirstOrDefault()
                    ?? new TimesheetDTO { EmployeeId = emp.Id, Month = month, Year = year };

                var a = att.Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["employee_id"]) == emp.Id);
                t.WorkDays = a == null ? 0 : Convert.ToDecimal(a["wd"]);
                t.LateCount = a == null ? 0 : Convert.ToInt32(a["lc"]);
                t.LateMinutes = a == null ? 0 : Convert.ToInt32(a["lm"]);
                t.EarlyMinutes = a == null ? 0 : Convert.ToInt32(a["em"]);

                t.LeaveDays = leave.Rows.Cast<DataRow>()
                    .Where(r => Convert.ToInt32(r["employee_id"]) == emp.Id && Convert.ToString(r["type"]) == Const.LeaveAnnual)
                    .Sum(r => Convert.ToDecimal(r["d"]));
                t.OtHours = leave.Rows.Cast<DataRow>()
                    .Where(r => Convert.ToInt32(r["employee_id"]) == emp.Id && Convert.ToString(r["type"]) == Const.LeaveOt)
                    .Sum(r => Convert.ToDecimal(r["h"]));

                if (t.Id == 0) Insert(t); else Update(t);
            }
        }

        public void SetLock(int month, int year, bool locked)
        {
            DbHelper.Execute("UPDATE monthly_timesheets SET is_locked=@l WHERE month=@m AND year=@y",
                DbHelper.P("@l", locked), DbHelper.P("@m", month), DbHelper.P("@y", year));
        }

        public DataTable View(int month, int year)
        {
            return DbHelper.Query(
                "SELECT t.id, e.code AS `Mã NV`, e.full_name AS `Họ tên`, IFNULL(d.name,'') AS `Phòng ban`, " +
                "t.work_days AS `Công thực tế`, t.leave_days AS `Nghỉ phép`, t.late_count AS `Số lần muộn`, t.late_minutes AS `Phút muộn`, " +
                "t.early_minutes AS `Phút về sớm`, t.ot_hours AS `Giờ OT`, IF(t.is_locked,'Đã chốt','Chưa chốt') AS `Trạng thái` " +
                "FROM monthly_timesheets t JOIN employees e ON e.id=t.employee_id LEFT JOIN departments d ON d.id=e.department_id " +
                "WHERE t.month=@m AND t.year=@y ORDER BY e.code", DbHelper.P("@m", month), DbHelper.P("@y", year));
        }
    }

    public class SalaryFormulaDAL : GenericDAL<SalaryFormulaDTO>
    {
        public List<SalaryFormulaDTO> GetActive() { return GetAll("is_active=1", "sort_order, id"); }
    }

    public class PayrollDAL : GenericDAL<PayrollDTO>
    {
        public PayrollDTO Find(int employeeId, int month, int year)
        {
            return GetAll("employee_id=@e AND month=@m AND year=@y", "id",
                DbHelper.P("@e", employeeId), DbHelper.P("@m", month), DbHelper.P("@y", year)).FirstOrDefault();
        }

        public DataTable View(int month, int year, int? employeeId = null)
        {
            return DbHelper.Query(
                "SELECT p.id, p.employee_id, e.code AS `Mã NV`, e.full_name AS `Họ tên`, IFNULL(d.name,'') AS `Phòng ban`, e.email AS `Email`, " +
                "p.work_days AS `Công`, p.gross_income AS `Tổng thu nhập`, p.bhxh AS `BHXH`, p.bhyt AS `BHYT`, p.bhtn AS `BHTN`, " +
                "p.taxable_income AS `TN tính thuế`, p.pit AS `Thuế TNCN`, p.other_deduction AS `Khấu trừ khác`, p.net_salary AS `Thực lĩnh`, p.status AS `Trạng thái` " +
                "FROM payroll p JOIN employees e ON e.id=p.employee_id LEFT JOIN departments d ON d.id=e.department_id " +
                "WHERE p.month=@m AND p.year=@y AND (@emp IS NULL OR p.employee_id=@emp) ORDER BY e.code",
                DbHelper.P("@m", month), DbHelper.P("@y", year), DbHelper.P("@emp", employeeId));
        }

        public void SetStatus(int id, string status)
        {
            DbHelper.Execute("UPDATE payroll SET status=@s WHERE id=@i", DbHelper.P("@s", status), DbHelper.P("@i", id));
        }
    }
}
