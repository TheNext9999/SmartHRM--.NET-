using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using SmartHRM.DAL;
using SmartHRM.DTO;

namespace SmartHRM.BUS
{
    /// <summary>Bộ tính công thức lương động. Biến: BASIC, ALLOWANCE, STD_DAYS, WORK_DAYS, OT_HOURS, LATE_COUNT, LATE_MINUTES,
    /// EARLY_MINUTES, LEAVE_DAYS, DEPENDENTS và mã của các khoản đã tính trước đó. Hỗ trợ + - * / ( ) và IIF(điều_kiện, a, b).</summary>
    public static class FormulaEngine
    {
        public static readonly string[] BuiltInVariables =
            { "BASIC", "ALLOWANCE", "STD_DAYS", "WORK_DAYS", "OT_HOURS", "LATE_COUNT", "LATE_MINUTES", "EARLY_MINUTES", "LEAVE_DAYS", "DEPENDENTS" };

        static readonly Regex IdRx = new Regex(@"\b[A-Za-z_][A-Za-z0-9_]*\b(?!\s*\()", RegexOptions.Compiled);

        public static decimal Evaluate(string expression, IDictionary<string, decimal> vars)
        {
            string e = IdRx.Replace(expression, m =>
            {
                decimal v;
                if (!vars.TryGetValue(m.Value, out v)) throw new ValidationException("Công thức \"" + expression + "\": biến \"" + m.Value + "\" không tồn tại.");
                return "(" + v.ToString("0.0#####", CultureInfo.InvariantCulture) + ")";
            });
            try
            {
                var r = new DataTable().Compute(e, null);
                if (r == null || r == DBNull.Value) return 0m;
                return Math.Round(Convert.ToDecimal(r), 0, MidpointRounding.AwayFromZero);
            }
            catch (Exception)
            {
                throw new ValidationException("Công thức \"" + expression + "\" không hợp lệ hoặc bị chia cho 0.");
            }
        }

        public static Dictionary<string, decimal> SampleVariables(IEnumerable<string> extraCodes)
        {
            var d = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
            foreach (var v in BuiltInVariables) d[v] = 1m;
            d["STD_DAYS"] = 26m;
            foreach (var c in extraCodes) d[c] = 1m;
            return d;
        }
    }

    public class SalaryFormulaBUS : GenericBUS<SalaryFormulaDTO>
    {
        readonly SalaryFormulaDAL dal;
        public SalaryFormulaBUS() : this(new SalaryFormulaDAL()) { }
        SalaryFormulaBUS(SalaryFormulaDAL d) : base(Const.M_FORMULA, d) { dal = d; }

        public override List<SalaryFormulaDTO> GetAll() { Session.Require(Module, "view"); return dal.GetAll(null, "sort_order, id"); }

        protected override void Validate(SalaryFormulaDTO e)
        {
            base.Validate(e);
            e.Code = e.Code.Trim().ToUpperInvariant();
            if (!Regex.IsMatch(e.Code, @"^[A-Z][A-Z0-9_]*$")) throw new ValidationException("Mã chỉ gồm chữ HOA, số, gạch dưới và bắt đầu bằng chữ cái.");
            if (FormulaEngine.BuiltInVariables.Contains(e.Code)) throw new ValidationException("Mã trùng với tên biến hệ thống.");
            if (dal.GetAll("code=@c AND id<>@i", "id", DbHelper.P("@c", e.Code), DbHelper.P("@i", e.Id)).Count > 0)
                throw new ValidationException("Mã công thức đã tồn tại.");
            // chạy thử công thức với dữ liệu mẫu để bắt lỗi cú pháp ngay khi lưu
            var others = dal.GetAll("id<>@i", "id", DbHelper.P("@i", e.Id)).Select(f => f.Code);
            FormulaEngine.Evaluate(e.Expression, FormulaEngine.SampleVariables(others));
        }
    }

    public class PayrollBUS
    {
        readonly PayrollDAL dal = new PayrollDAL();
        readonly TimesheetDAL tsDal = new TimesheetDAL();
        static readonly CultureInfo VN = new CultureInfo("vi-VN");

        public DataTable View(int month, int year, bool selfOnly = false)
        {
            Session.Require(selfOnly ? Const.M_MYPAYSLIP : Const.M_PAYROLL, "view");
            if (selfOnly)
            {
                if (!Session.EmployeeId.HasValue) throw new ValidationException("Tài khoản chưa liên kết với hồ sơ nhân viên.");
                return dal.View(month, year, Session.EmployeeId.Value);
            }
            return dal.View(month, year);
        }

        static decimal Pit(decimal taxable)
        {
            // "ngưỡng_trên:thuế_suất;..." - ngưỡng 0 = không giới hạn
            var brackets = AppSettings.Get("PitBrackets", "10000000:0.05;30000000:0.10;60000000:0.20;100000000:0.30;0:0.35")
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Split(':'))
                .Select(a => new { Upper = decimal.Parse(a[0], CultureInfo.InvariantCulture), Rate = decimal.Parse(a[1], CultureInfo.InvariantCulture) });
            decimal tax = 0, prev = 0;
            foreach (var b in brackets)
            {
                decimal upper = b.Upper == 0 ? decimal.MaxValue : b.Upper;
                if (taxable > prev) tax += (Math.Min(taxable, upper) - prev) * b.Rate;
                if (taxable <= upper) break;
                prev = upper;
            }
            return Math.Round(tax, 0, MidpointRounding.AwayFromZero);
        }

        /// <summary>Tính lương toàn công ty cho kỳ (yêu cầu bảng công đã chốt). Bỏ qua phiếu lương đã chốt.</summary>
        public int Calculate(int month, int year)
        {
            Session.Require(Const.M_PAYROLL, "edit");
            if (!tsDal.IsLocked(month, year))
                throw new ValidationException("Bảng công tháng " + month + "/" + year + " chưa được chốt. Hãy chốt công trước khi tính lương.");

            var formulas = new SalaryFormulaDAL().GetActive();
            var positions = new GenericDAL<PositionDTO>().GetAll().ToDictionary(p => p.Id);
            decimal std = AppSettings.GetDecimal("StandardDays", 26);
            decimal cap = AppSettings.GetDecimal("InsuranceCap", 46800000);
            decimal rXh = AppSettings.GetDecimal("BhxhRate", 0.08m), rYt = AppSettings.GetDecimal("BhytRate", 0.015m), rTn = AppSettings.GetDecimal("BhtnRate", 0.01m);
            decimal rEmp = AppSettings.GetDecimal("EmpBhxhRate", 0.175m) + AppSettings.GetDecimal("EmpBhytRate", 0.03m)
                         + AppSettings.GetDecimal("EmpBhtnRate", 0.01m) + AppSettings.GetDecimal("EmpTnldRate", 0.005m);
            decimal personal = AppSettings.GetDecimal("PersonalDeduction", 15500000);
            decimal dependentDed = AppSettings.GetDecimal("DependentDeduction", 6200000);

            var empDal = new EmployeeDAL();
            int count = 0;
            foreach (var ts in tsDal.GetAll("month=@m AND year=@y", "employee_id", DbHelper.P("@m", month), DbHelper.P("@y", year)))
            {
                var emp = empDal.GetById(ts.EmployeeId);
                if (emp == null) continue;
                var existing = dal.Find(emp.Id, month, year);
                if (existing != null && existing.Status != "Nháp") continue;

                PositionDTO pos; positions.TryGetValue(emp.PositionId ?? 0, out pos);
                decimal allowance = pos == null ? 0 : pos.Allowance;

                var vars = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
                {
                    { "BASIC", emp.BasicSalary }, { "ALLOWANCE", allowance }, { "STD_DAYS", std },
                    { "WORK_DAYS", ts.WorkDays + ts.LeaveDays }, { "OT_HOURS", ts.OtHours },
                    { "LATE_COUNT", ts.LateCount }, { "LATE_MINUTES", ts.LateMinutes }, { "EARLY_MINUTES", ts.EarlyMinutes },
                    { "LEAVE_DAYS", ts.LeaveDays }, { "DEPENDENTS", emp.Dependents }
                };

                decimal gross = 0, bonus = 0, deduction = 0;
                var detail = new StringBuilder();
                foreach (var f in formulas)
                {
                    decimal v = FormulaEngine.Evaluate(f.Expression, vars);
                    vars[f.Code] = v;
                    if (f.Kind == Const.Income) { gross += v; if (f.Code.StartsWith("THUONG")) bonus += v; }
                    else deduction += v;
                    detail.AppendLine(f.Kind + "|" + f.Name + "|" + v.ToString(CultureInfo.InvariantCulture));
                }

                decimal insBase = Math.Min(emp.BasicSalary + allowance, cap);
                decimal bhxh = Math.Round(insBase * rXh), bhyt = Math.Round(insBase * rYt), bhtn = Math.Round(insBase * rTn);
                decimal taxable = Math.Max(0, gross - bhxh - bhyt - bhtn - personal - emp.Dependents * dependentDed);
                decimal pit = Pit(taxable);

                var p = existing ?? new PayrollDTO { EmployeeId = emp.Id, Month = month, Year = year };
                p.WorkDays = ts.WorkDays + ts.LeaveDays;
                p.GrossIncome = gross; p.Bonus = bonus; p.IncomeDetail = detail.ToString();
                p.Bhxh = bhxh; p.Bhyt = bhyt; p.Bhtn = bhtn; p.EmployerInsurance = Math.Round(insBase * rEmp);
                p.TaxableIncome = taxable; p.Pit = pit; p.OtherDeduction = deduction;
                p.NetSalary = gross - bhxh - bhyt - bhtn - pit - deduction;
                p.Status = "Nháp";
                if (p.Id == 0) dal.Insert(p); else dal.Update(p);
                count++;
            }
            AuditBUS.Log("Tính lương", Const.M_PAYROLL, month + "/" + year + ": " + count + " nhân viên");
            return count;
        }

        public void FinalizePeriod(int month, int year)
        {
            Session.Require(Const.M_PAYROLL, "edit");
            int n = DbHelper.Execute("UPDATE payroll SET status='Đã chốt' WHERE month=@m AND year=@y", DbHelper.P("@m", month), DbHelper.P("@y", year));
            if (n == 0) throw new ValidationException("Chưa có bảng lương kỳ này.");
            AuditBUS.Log("Chốt bảng lương", Const.M_PAYROLL, month + "/" + year);
        }

        // ---------- Phiếu lương ----------
        public string BuildPayslipHtml(int payrollId)
        {
            var p = dal.GetById(payrollId);
            if (p == null) throw new ValidationException("Không tìm thấy phiếu lương.");
            if (Session.SelfOnly(Const.M_PAYROLL) || !Session.Can(Const.M_PAYROLL, "view"))
                if (p.EmployeeId != Session.EmployeeId) throw new UnauthorizedAccessException("Bạn chỉ được xem phiếu lương của chính mình.");
            var emp = new EmployeeDAL().GetById(p.EmployeeId);
            var dept = emp.DepartmentId.HasValue ? new DepartmentDAL().GetById(emp.DepartmentId.Value) : null;
            Func<decimal, string> M = v => v.ToString("N0", VN);
            Func<string, string> H = s => WebUtility.HtmlEncode(s ?? "");

            var sb = new StringBuilder();
            sb.Append("<html><head><meta charset='utf-8'><style>body{font-family:Segoe UI,Arial;font-size:13px;color:#222}" +
                      "h2{color:#1F4E79;margin:0}table{border-collapse:collapse;width:100%;margin:8px 0}td,th{border:1px solid #ccc;padding:5px 8px}" +
                      "th{background:#DCE6F1;text-align:left}td.r{text-align:right}.net{font-size:16px;font-weight:bold;color:#1F4E79}</style></head><body>");
            sb.Append("<h2>PHIẾU LƯƠNG THÁNG " + p.Month + "/" + p.Year + "</h2><p><i>Tài liệu bảo mật - chỉ gửi cho cá nhân người nhận</i></p>");
            sb.Append("<table><tr><td>Mã NV: <b>" + H(emp.Code) + "</b></td><td>Họ tên: <b>" + H(emp.FullName) + "</b></td></tr>" +
                      "<tr><td>Phòng ban: " + H(dept == null ? "" : dept.Name) + "</td><td>Công tính lương: <b>" + p.WorkDays + "</b></td></tr></table>");
            sb.Append("<table><tr><th colspan='2'>THU NHẬP</th></tr>");
            var lines = (p.IncomeDetail ?? "").Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(l => l.Split('|')).Where(a => a.Length == 3).ToList();
            foreach (var a in lines.Where(a => a[0] == Const.Income))
                sb.Append("<tr><td>" + H(a[1]) + "</td><td class='r'>" + M(decimal.Parse(a[2], CultureInfo.InvariantCulture)) + "</td></tr>");
            sb.Append("<tr><td><b>Tổng thu nhập</b></td><td class='r'><b>" + M(p.GrossIncome) + "</b></td></tr>");
            sb.Append("<tr><th colspan='2'>KHẤU TRỪ</th></tr>");
            sb.Append("<tr><td>BHXH (người lao động)</td><td class='r'>" + M(p.Bhxh) + "</td></tr>");
            sb.Append("<tr><td>BHYT (người lao động)</td><td class='r'>" + M(p.Bhyt) + "</td></tr>");
            sb.Append("<tr><td>BHTN (người lao động)</td><td class='r'>" + M(p.Bhtn) + "</td></tr>");
            sb.Append("<tr><td>Thu nhập tính thuế</td><td class='r'>" + M(p.TaxableIncome) + "</td></tr>");
            sb.Append("<tr><td>Thuế TNCN</td><td class='r'>" + M(p.Pit) + "</td></tr>");
            foreach (var a in lines.Where(a => a[0] == Const.Deduction))
                sb.Append("<tr><td>" + H(a[1]) + "</td><td class='r'>" + M(decimal.Parse(a[2], CultureInfo.InvariantCulture)) + "</td></tr>");
            sb.Append("<tr><td class='net'>THỰC LĨNH</td><td class='r net'>" + M(p.NetSalary) + " đ</td></tr></table></body></html>");
            return sb.ToString();
        }

        /// <summary>Gửi phiếu lương qua email cho từng nhân viên (mỗi người một thư riêng).</summary>
        public string SendPayslips(IEnumerable<int> payrollIds)
        {
            Session.Require(Const.M_PAYROLL, "edit");
            if (!MailService.IsConfigured) throw new ValidationException("Chưa cấu hình SMTP trong App.config nên chưa thể gửi email.");
            int ok = 0; var errors = new List<string>();
            foreach (int id in payrollIds)
            {
                var p = dal.GetById(id);
                if (p == null) continue;
                var emp = new EmployeeDAL().GetById(p.EmployeeId);
                try
                {
                    MailService.Send(emp.Email, "Phiếu lương tháng " + p.Month + "/" + p.Year, BuildPayslipHtml(id));
                    dal.SetStatus(id, p.Status == "Nháp" ? "Đã gửi" : p.Status);
                    ok++;
                }
                catch (Exception ex) { errors.Add(emp.Code + ": " + ex.Message); }
            }
            AuditBUS.Log("Gửi phiếu lương", Const.M_PAYROLL, ok + " thành công, " + errors.Count + " lỗi");
            return "Đã gửi " + ok + " phiếu lương." + (errors.Count > 0 ? "\nLỗi:\n" + string.Join("\n", errors) : "");
        }
    }
}
