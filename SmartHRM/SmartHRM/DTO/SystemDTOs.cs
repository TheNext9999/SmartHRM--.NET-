using System;

namespace SmartHRM.DTO
{
    [Table("roles")]
    public class RoleDTO : BaseDTO
    {
        [Field("Tên nhóm quyền", Required = true)] public string Name { get; set; }
        [Field("Mô tả")] public string Description { get; set; }
    }

    [Table("role_permissions")]
    public class RolePermissionDTO : BaseDTO
    {
        [Field("Nhóm quyền")] public int RoleId { get; set; }
        [Field("Mô-đun")] public string ModuleCode { get; set; }
        [Field("Xem")] public bool CanView { get; set; }
        [Field("Thêm")] public bool CanAdd { get; set; }
        [Field("Sửa")] public bool CanEdit { get; set; }
        [Field("Xóa")] public bool CanDelete { get; set; }
    }

    [Table("users")]
    public class UserDTO : BaseDTO
    {
        [Field("Tên đăng nhập", Required = true)] public string Username { get; set; }
        [Field("Họ tên")] public string FullName { get; set; }
        [Field("Email")] public string Email { get; set; }
        [Field("Nhóm quyền", Kind = FieldKind.Lookup, Source = "roles|id|name", Required = true)] public int RoleId { get; set; }
        [Field("Nhân viên liên kết", Kind = FieldKind.Lookup, Source = "employees|id|CONCAT(code,' - ',full_name)")] public int? EmployeeId { get; set; }
        [Field("Dùng tài khoản AD", Kind = FieldKind.Bool)] public bool UseAd { get; set; }
        [Field("Hoạt động", Kind = FieldKind.Bool)] public bool IsActive { get; set; } = true;
        [Field("Mật khẩu (để trống = giữ nguyên)", Kind = FieldKind.Password, InGrid = false), Ignore] public string NewPassword { get; set; }
        [Field("Hash", Hidden = true, InGrid = false)] public string PasswordHash { get; set; }
    }

    [Table("audit_logs")]
    public class AuditLogDTO : BaseDTO
    {
        [Field("Thời gian", Kind = FieldKind.DateAndTime)] public DateTime? CreatedAt { get; set; }
        [Field("Người dùng")] public string Username { get; set; }
        [Field("Thao tác")] public string Action { get; set; }
        [Field("Mô-đun")] public string Module { get; set; }
        [Field("Chi tiết", Kind = FieldKind.MultiLine)] public string Detail { get; set; }
        [Field("UserId", Hidden = true, InGrid = false)] public int? UserId { get; set; }
    }

    [Table("system_settings")]
    public class SettingDTO : BaseDTO
    {
        [Field("Khóa", Required = true)] public string SettingKey { get; set; }
        [Field("Giá trị")] public string SettingValue { get; set; }
        [Field("Mô tả")] public string Description { get; set; }
    }

    [Table("company_policies")]
    public class CompanyPolicyDTO : BaseDTO
    {
        [Field("Tên chính sách", Required = true)] public string Title { get; set; }
        [Field("Nội dung", Kind = FieldKind.MultiLine, Required = true)] public string Content { get; set; }
        [Field("Trạng thái", Kind = FieldKind.Combo, Source = Const.PolicyStatuses, Required = true)] public string Status { get; set; } = "Dự thảo";
        [Field("Ngày bắt đầu áp dụng", Kind = FieldKind.Date)] public DateTime? EffectiveDate { get; set; }
        [Field("Ngày hết hạn", Kind = FieldKind.Date)] public DateTime? ExpiryDate { get; set; }
        [Field("Ghi chú", Kind = FieldKind.MultiLine)] public string Note { get; set; }
    }
}
