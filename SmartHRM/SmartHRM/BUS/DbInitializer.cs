using System;
using System.Collections.Generic;
using System.Linq;
using SmartHRM.DAL;
using SmartHRM.DTO;

namespace SmartHRM.BUS
{
    /// <summary>Khởi tạo dữ liệu lần đầu: tài khoản admin và quyền mặc định cho các nhóm.</summary>
    public static class DbInitializer
    {
        public static void Run()
        {
            var users = new UserDAL();
            if (users.Count() == 0)
            {
                users.Insert(new UserDTO
                {
                    Username = "admin",
                    FullName = "Quản trị hệ thống",
                    Email = "admin@congty.vn",
                    RoleId = 1,
                    IsActive = true,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
                });
            }

            var roles = new RoleDAL();
            if (roles.PermissionCount() == 0)
            {
                string[] sys = { Const.M_USER, Const.M_ROLE, Const.M_AUDIT, Const.M_SETTING };
                foreach (var m in Modules.All)
                {
                    // Nhóm 2 - Nhân sự: toàn quyền nghiệp vụ, không đụng tới quản trị hệ thống
                    if (!sys.Contains(m.Code))
                        roles.SavePermission(new RolePermissionDTO { RoleId = 2, ModuleCode = m.Code, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true });
                }
                // Nhóm 3 - Nhân viên: tự phục vụ
                Action<string, bool, bool, bool> emp = (code, v, a, e) =>
                    roles.SavePermission(new RolePermissionDTO { RoleId = 3, ModuleCode = code, CanView = v, CanAdd = a, CanEdit = e });
                emp(Const.M_ATTENDANCE, true, false, false);
                emp(Const.M_LEAVE, true, true, false);
                emp(Const.M_MYPAYSLIP, true, false, false);
                emp(Const.M_KPIEVAL, true, true, true);
                emp(Const.M_COURSE, true, false, false);
                emp(Const.M_TRAINING, true, false, false);
            }
            try { new ContractDAL().MarkExpired(); } catch { }
        }
    }
}
