using System;

namespace SmartHRM.DTO
{
    [Table("departments")]
    public class DepartmentDTO : BaseDTO
    {
        [Field("Mã phòng ban", Required = true)] public string Code { get; set; }
        [Field("Tên phòng ban / tổ đội", Required = true)] public string Name { get; set; }
        [Field("Trực thuộc", Kind = FieldKind.Lookup, Source = "departments|id|name")] public int? ParentId { get; set; }
        [Field("Trưởng đơn vị", Kind = FieldKind.Lookup, Source = "employees|id|CONCAT(code,' - ',full_name)")] public int? ManagerId { get; set; }
        [Field("Mô tả", Kind = FieldKind.MultiLine, InGrid = false)] public string Description { get; set; }
    }

    [Table("positions")]
    public class PositionDTO : BaseDTO
    {
        [Field("Tên chức vụ", Required = true)] public string Name { get; set; }
        [Field("Phụ cấp chức vụ", Kind = FieldKind.Decimal)] public decimal Allowance { get; set; }
        [Field("Mô tả", Kind = FieldKind.MultiLine)] public string Description { get; set; }
    }

    [Table("employees")]
    public class EmployeeDTO : BaseDTO
    {
        [Field("Mã NV (tự động)", ReadOnly = true)] public string Code { get; set; }
        [Field("Họ và tên", Required = true)] public string FullName { get; set; }
        [Field("Giới tính", Kind = FieldKind.Combo, Source = Const.Genders)] public string Gender { get; set; }
        [Field("Ngày sinh", Kind = FieldKind.Date)] public DateTime? BirthDate { get; set; }
        [Field("CCCD")] public string IdCard { get; set; }
        [Field("Điện thoại")] public string Phone { get; set; }
        [Field("Email")] public string Email { get; set; }
        [Field("Địa chỉ", InGrid = false)] public string Address { get; set; }
        [Field("Phòng ban", Kind = FieldKind.Lookup, Source = "departments|id|name")] public int? DepartmentId { get; set; }
        [Field("Chức vụ", Kind = FieldKind.Lookup, Source = "positions|id|name")] public int? PositionId { get; set; }
        [Field("Trạng thái", Kind = FieldKind.Combo, Source = Const.EmployeeStatuses, Required = true)] public string Status { get; set; } = Const.Probation;
        [Field("Ngày vào làm", Kind = FieldKind.Date)] public DateTime? JoinDate { get; set; }
        [Field("Ngày nghỉ việc", Kind = FieldKind.Date, InGrid = false)] public DateTime? LeaveDate { get; set; }
        [Field("Lương cơ bản", Kind = FieldKind.Decimal)] public decimal BasicSalary { get; set; }
        [Field("Số tài khoản NH", InGrid = false)] public string BankAccount { get; set; }
        [Field("Ngân hàng", InGrid = false)] public string BankName { get; set; }
        [Field("Mã số thuế", InGrid = false)] public string TaxCode { get; set; }
        [Field("Số sổ BHXH", InGrid = false)] public string SocialInsuranceNo { get; set; }
        [Field("Người phụ thuộc", Kind = FieldKind.Integer, InGrid = false)] public int Dependents { get; set; }
        [Field("Phép năm còn lại (ngày)", Kind = FieldKind.Decimal, InGrid = false)] public decimal AnnualLeaveBalance { get; set; } = 12;
    }

    [Table("contracts")]
    public class ContractDTO : BaseDTO
    {
        [Field("Nhân viên", Kind = FieldKind.Lookup, Source = "employees|id|CONCAT(code,' - ',full_name)", Required = true)] public int? EmployeeId { get; set; }
        [Field("Số hợp đồng", Required = true)] public string ContractNo { get; set; }
        [Field("Loại hợp đồng", Kind = FieldKind.Combo, Source = Const.ContractTypes, Required = true)] public string Type { get; set; }
        [Field("Ngày bắt đầu", Kind = FieldKind.Date, Required = true)] public DateTime? StartDate { get; set; }
        [Field("Ngày kết thúc", Kind = FieldKind.Date)] public DateTime? EndDate { get; set; }
        [Field("Lương theo HĐ", Kind = FieldKind.Decimal)] public decimal Salary { get; set; }
        [Field("Trạng thái", Kind = FieldKind.Combo, Source = Const.ContractStatuses, Required = true)] public string Status { get; set; } = "Hiệu lực";
        [Field("Ghi chú", Kind = FieldKind.MultiLine)] public string Note { get; set; }
    }

    [Table("recruitment_requests")]
    public class RecruitmentRequestDTO : BaseDTO
    {
        [Field("Phòng ban", Kind = FieldKind.Lookup, Source = "departments|id|name", Required = true)] public int? DepartmentId { get; set; }
        [Field("Vị trí cần tuyển", Kind = FieldKind.Lookup, Source = "positions|id|name", Required = true)] public int? PositionId { get; set; }
        [Field("Số lượng", Kind = FieldKind.Integer)] public int Quantity { get; set; } = 1;
        [Field("Lý do / yêu cầu", Kind = FieldKind.MultiLine)] public string Reason { get; set; }
        [Field("Ngày đề xuất", Kind = FieldKind.Date)] public DateTime? RequestDate { get; set; }
        [Field("Trạng thái", Kind = FieldKind.Combo, Source = Const.ApprovalStatuses, ReadOnly = true)] public string Status { get; set; } = Const.Pending;
        [Field("Ghi chú")] public string Note { get; set; }
    }

    [Table("candidates")]
    public class CandidateDTO : BaseDTO
    {
        [Field("Chiến dịch / đề xuất", Kind = FieldKind.Lookup, Source = "recruitment_requests|id|CONCAT('YC#',id,' (',quantity,' người)')")] public int? RequestId { get; set; }
        [Field("Họ và tên", Required = true)] public string FullName { get; set; }
        [Field("Email")] public string Email { get; set; }
        [Field("Điện thoại")] public string Phone { get; set; }
        [Field("Ngày sinh", Kind = FieldKind.Date, InGrid = false)] public DateTime? BirthDate { get; set; }
        [Field("File CV", Kind = FieldKind.File, InGrid = false)] public string CvPath { get; set; }
        [Field("Ngày nộp hồ sơ", Kind = FieldKind.Date)] public DateTime? AppliedDate { get; set; }
        [Field("Trạng thái", Kind = FieldKind.Combo, Source = Const.CandidateStatuses, Required = true)] public string Status { get; set; } = Const.CandNew;
        [Field("Lịch phỏng vấn", Kind = FieldKind.DateAndTime)] public DateTime? InterviewDate { get; set; }
        [Field("Kết quả phỏng vấn", Kind = FieldKind.MultiLine, InGrid = false)] public string InterviewResult { get; set; }
        [Field("Ghi chú", InGrid = false)] public string Note { get; set; }
        [Field("NV tương ứng", Hidden = true, InGrid = false)] public int? EmployeeId { get; set; }
    }
}
