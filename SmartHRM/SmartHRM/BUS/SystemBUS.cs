using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SmartHRM.DAL;
using SmartHRM.DTO;

namespace SmartHRM.BUS
{
    public class UserBUS : GenericBUS<UserDTO>
    {
        public UserBUS() : base(Const.M_USER, new UserDAL()) { }

        protected override void Validate(UserDTO e)
        {
            base.Validate(e);
            if (!Regex.IsMatch(e.Username, @"^[A-Za-z0-9_.@-]{3,50}$"))
                throw new ValidationException("Tên đăng nhập 3-50 ký tự (chữ, số, . _ - @), không dấu, không khoảng trắng.");
            var dup = Dal.GetAll("username=@u AND id<>@i", "id", DbHelper.P("@u", e.Username), DbHelper.P("@i", e.Id));
            if (dup.Count > 0) throw new ValidationException("Tên đăng nhập đã tồn tại.");
            if (e.Id == 0 && !e.UseAd && string.IsNullOrEmpty(e.NewPassword))
                throw new ValidationException("Phải nhập mật khẩu cho tài khoản mới.");
            if (!string.IsNullOrEmpty(e.NewPassword) && e.NewPassword.Length < 6)
                throw new ValidationException("Mật khẩu tối thiểu 6 ký tự.");
            if (e.Id == Session.User.Id && !e.IsActive) throw new ValidationException("Không thể tự khóa tài khoản đang đăng nhập.");
        }

        protected override void BeforeSave(UserDTO e, bool isNew)
        {
            if (!string.IsNullOrEmpty(e.NewPassword)) e.PasswordHash = BCrypt.Net.BCrypt.HashPassword(e.NewPassword);
            else if (string.IsNullOrEmpty(e.PasswordHash)) e.PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString("N")); // tài khoản AD
            e.NewPassword = null;
        }

        protected override void BeforeDelete(UserDTO e)
        {
            if (e.Id == Session.User.Id) throw new ValidationException("Không thể xóa tài khoản đang đăng nhập.");
        }
    }

    public class RoleBUS : GenericBUS<RoleDTO>
    {
        readonly RoleDAL roleDal;
        public RoleBUS() : this(new RoleDAL()) { }
        RoleBUS(RoleDAL dal) : base(Const.M_ROLE, dal) { roleDal = dal; }

        protected override void BeforeDelete(RoleDTO e)
        {
            if (e.Id <= 3) throw new ValidationException("Không thể xóa nhóm quyền mặc định của hệ thống.");
            if (DbHelper.ScalarInt("SELECT COUNT(*) FROM users WHERE role_id=@r", DbHelper.P("@r", e.Id)) > 0)
                throw new ValidationException("Còn tài khoản thuộc nhóm quyền này, hãy chuyển họ sang nhóm khác trước.");
        }

        public int CountUsers(int roleId)
        {
            return DbHelper.ScalarInt("SELECT COUNT(*) FROM users WHERE role_id=@r", DbHelper.P("@r", roleId));
        }

        public List<RolePermissionDTO> GetPermissions(int roleId)
        {
            Session.Require(Const.M_ROLE, "view");
            var saved = roleDal.GetPermissions(roleId);
            var result = new List<RolePermissionDTO>();
            foreach (var m in Modules.All)
                result.Add(saved.FirstOrDefault(p => p.ModuleCode == m.Code) ?? new RolePermissionDTO { RoleId = roleId, ModuleCode = m.Code });
            return result;
        }

        public void SavePermissions(int roleId, IEnumerable<RolePermissionDTO> perms)
        {
            Session.Require(Const.M_ROLE, "edit");
            if (roleId == 1) throw new ValidationException("Nhóm Admin luôn có toàn quyền, không cần cấu hình.");
            foreach (var p in perms)
            {
                p.RoleId = roleId;
                // Có quyền thêm/sửa/xóa thì mặc nhiên phải được xem
                if (p.CanAdd || p.CanEdit || p.CanDelete) p.CanView = true;
                roleDal.SavePermission(p);
            }
            AuditBUS.Log("Phân quyền", Const.M_ROLE, "Cập nhật quyền cho nhóm #" + roleId);
        }
    }

    public class AuditLogBUS : GenericBUS<AuditLogDTO>
    {
        public AuditLogBUS() : base(Const.M_AUDIT, new AuditDAL()) { }
        public override List<AuditLogDTO> GetAll() { return AuditBUS.Search(DateTime.Today.AddDays(-30), DateTime.Today, ""); }
    }

    public class SettingBUS : GenericBUS<SettingDTO>
    {
        public SettingBUS() : base(Const.M_SETTING, new SettingDAL()) { }
        public override List<SettingDTO> GetAll() { Session.Require(Module, "view"); return Dal.GetAll(null, "setting_key"); }
        protected override void AfterSave(SettingDTO e, bool isNew) { AppSettings.Reload(); }
    }
}