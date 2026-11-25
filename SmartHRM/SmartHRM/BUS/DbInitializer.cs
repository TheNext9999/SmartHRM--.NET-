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
            DbHelper.Execute("CREATE TABLE IF NOT EXISTS company_policies (" +
                "id INT NOT NULL AUTO_INCREMENT, title VARCHAR(200) NOT NULL, content TEXT NOT NULL, " +
                "status VARCHAR(30) NOT NULL DEFAULT 'Dự thảo', effective_date DATE NULL, expiry_date DATE NULL, " +
                "note VARCHAR(500) NULL, PRIMARY KEY (id)) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4");
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
            DbHelper.Execute("UPDATE employees SET status='Đang làm việc' WHERE status IN ('Chính thức','Thử việc')");
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
            if (DbHelper.ScalarInt("SELECT COUNT(*) FROM roles WHERE id=2") > 0)
                roles.SavePermission(new RolePermissionDTO { RoleId = 2, ModuleCode = Const.M_POLICY, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true });
            try { new ContractDAL().MarkExpired(); } catch { }
        }
    }
}
