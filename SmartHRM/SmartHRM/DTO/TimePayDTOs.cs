using System;

namespace SmartHRM.DTO
{
    [Table("shifts")]
    public class ShiftDTO : BaseDTO
    {
        [Field("Tên ca", Required = true)] public string Name { get; set; }
        [Field("Giờ bắt đầu", Kind = FieldKind.Time, Required = true)] public string StartTime { get; set; } = "08:00";
        [Field("Giờ kết thúc", Kind = FieldKind.Time, Required = true)] public string EndTime { get; set; } = "17:00";
        [Field("Nghỉ giữa ca (phút)", Kind = FieldKind.Integer)] public int BreakMinutes { get; set; } = 60;
        [Field("Ca đêm", Kind = FieldKind.Bool)] public bool IsNight { get; set; }
        [Field("Ghi chú")] public string Note { get; set; }
    }

    [Table("work_schedules")]
    public class WorkScheduleDTO : BaseDTO
    {
        [Field("Nhân viên", Kind = FieldKind.Lookup, Source = "employees|id|CONCAT(code,' - ',full_name)", Required = true)] public int? EmployeeId { get; set; }
        [Field("Ca làm việc", Kind = FieldKind.Lookup, Source = "shifts|id|name", Required = true)] public int? ShiftId { get; set; }
        [Field("Từ ngày", Kind = FieldKind.Date, Required = true)] public DateTime? FromDate { get; set; }
        [Field("Đến ngày (trống = vô thời hạn)", Kind = FieldKind.Date)] public DateTime? ToDate { get; set; }
    }

    [Table("attendance")]
    public class AttendanceDTO : BaseDTO
    {
        [Field("Nhân viên", Kind = FieldKind.Lookup, Source = "employees|id|CONCAT(code,' - ',full_name)", Required = true)] public int? EmployeeId { get; set; }
        [Field("Ngày", Kind = FieldKind.Date, Required = true)] public DateTime? WorkDate { get; set; }
        [Field("Giờ vào", Kind = FieldKind.DateAndTime)] public DateTime? CheckIn { get; set; }
        [Field("Giờ ra", Kind = FieldKind.DateAndTime)] public DateTime? CheckOut { get; set; }
        [Field("Hình thức", Kind = FieldKind.Combo, Source = Const.AttMethods)] public string Method { get; set; }
        [Field("Đi muộn (phút)", Kind = FieldKind.Integer, ReadOnly = true)] public int LateMinutes { get; set; }
        [Field("Về sớm (phút)", Kind = FieldKind.Integer, ReadOnly = true)] public int EarlyMinutes { get; set; }
        [Field("Công", Kind = FieldKind.Decimal, ReadOnly = true)] public decimal WorkDays { get; set; }
        [Field("Ghi chú")] public string Note { get; set; }
    }

    [Table("leave_requests")]
    public class LeaveRequestDTO : BaseDTO
    {
        [Field("Nhân viên", Kind = FieldKind.Lookup, Source = "employees|id|CONCAT(code,' - ',full_name)", Required = true)] public int? EmployeeId { get; set; }
        [Field("Loại đơn", Kind = FieldKind.Combo, Source = Const.LeaveTypes, Required = true)] public string Type { get; set; } = Const.LeaveAnnual;
        [Field("Từ ngày", Kind = FieldKind.Date, Required = true)] public DateTime? FromDate { get; set; }
        [Field("Đến ngày", Kind = FieldKind.Date, Required = true)] public DateTime? ToDate { get; set; }
        [Field("Số ngày (0 = tự tính)", Kind = FieldKind.Decimal)] public decimal Days { get; set; }
        [Field("Số giờ (OT / muộn)", Kind = FieldKind.Decimal)] public decimal Hours { get; set; }
        [Field("Lý do", Kind = FieldKind.MultiLine)] public string Reason { get; set; }
        [Field("Trạng thái", Kind = FieldKind.Combo, Source = Const.ApprovalStatuses, ReadOnly = true)] public string Status { get; set; } = Const.Pending;
        [Field("Người duyệt", Hidden = true, InGrid = false)] public int? ApprovedBy { get; set; }
    }

    [Table("monthly_timesheets")]
    public class TimesheetDTO : BaseDTO
    {
        public int EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal WorkDays { get; set; }
        public decimal LeaveDays { get; set; }
        public int LateCount { get; set; }
        public int LateMinutes { get; set; }
        public int EarlyMinutes { get; set; }
        public decimal OtHours { get; set; }
        public bool IsLocked { get; set; }
    }

    [Table("salary_formulas")]
    public class SalaryFormulaDTO : BaseDTO
    {
        [Field("Mã (viết HOA, không dấu)", Required = true)] public string Code { get; set; }
        [Field("Tên khoản", Required = true)] public string Name { get; set; }
        [Field("Loại", Kind = FieldKind.Combo, Source = Const.FormulaKinds, Required = true)] public string Kind { get; set; } = Const.Income;
        [Field("Công thức", Kind = FieldKind.MultiLine, Required = true)] public string Expression { get; set; }
        [Field("Thứ tự tính", Kind = FieldKind.Integer)] public int SortOrder { get; set; } = 1;
        [Field("Áp dụng", Kind = FieldKind.Bool)] public bool IsActive { get; set; } = true;
        [Field("Diễn giải", Kind = FieldKind.MultiLine)] public string Description { get; set; }
    }

    [Table("payroll")]
    public class PayrollDTO : BaseDTO
    {
        public int EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal WorkDays { get; set; }
        public decimal GrossIncome { get; set; }
        public decimal Bonus { get; set; }
        public string IncomeDetail { get; set; }
        public decimal Bhxh { get; set; }
        public decimal Bhyt { get; set; }
        public decimal Bhtn { get; set; }
        public decimal EmployerInsurance { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal Pit { get; set; }
        public decimal OtherDeduction { get; set; }
        public decimal NetSalary { get; set; }
        public string Status { get; set; } = "Nháp";
    }
}
