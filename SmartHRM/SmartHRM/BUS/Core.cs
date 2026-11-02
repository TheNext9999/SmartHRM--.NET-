using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SmartHRM.DAL;
using SmartHRM.DTO;

namespace SmartHRM.BUS
{
    /// <summary>Lỗi nghiệp vụ / dữ liệu không hợp lệ - hiển thị trực tiếp cho người dùng.</summary>
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }

    public class ModuleInfo
    {
        public string Code, Title, Group;
        public ModuleInfo(string group, string code, string title) { Group = group; Code = code; Title = title; }
    }

    /// <summary>Danh mục mô-đun chức năng (dùng cho menu và phân quyền RBAC).</summary>
    public static class Modules
    {
        public const string G1 = "1. Hệ thống & Bảo mật", G2 = "2. Tổ chức & Hồ sơ nhân sự", G3 = "3. Tuyển dụng & Onboarding",
            G4 = "4. Chấm công & Ca làm việc", G5 = "5. Tiền lương & Phúc lợi", G6 = "6. Đánh giá & Đào tạo", G7 = "7. Báo cáo & Thống kê";

        public static readonly List<ModuleInfo> All = new List<ModuleInfo>
        {
            new ModuleInfo(G1, Const.M_USER, "Tài khoản người dùng"),
            new ModuleInfo(G1, Const.M_ROLE, "Phân quyền (RBAC)"),
            new ModuleInfo(G1, Const.M_AUDIT, "Nhật ký hệ thống"),
            new ModuleInfo(G1, Const.M_SETTING, "Tham số hệ thống"),
            new ModuleInfo(G2, Const.M_ORG, "Sơ đồ tổ chức"),
            new ModuleInfo(G2, Const.M_POSITION, "Danh mục chức vụ"),
            new ModuleInfo(G2, Const.M_EMPLOYEE, "Hồ sơ nhân viên"),
            new ModuleInfo(G2, Const.M_CONTRACT, "Hợp đồng lao động"),
            new ModuleInfo(G3, Const.M_RECREQ, "Kế hoạch / đề xuất tuyển dụng"),
            new ModuleInfo(G3, Const.M_CANDIDATE, "Ứng viên & phỏng vấn"),
            new ModuleInfo(G4, Const.M_SHIFT, "Cấu hình ca làm việc"),
            new ModuleInfo(G4, Const.M_SCHEDULE, "Lịch làm việc / phân ca"),
            new ModuleInfo(G4, Const.M_ATTENDANCE, "Chấm công"),
            new ModuleInfo(G4, Const.M_LEAVE, "Đơn từ điện tử"),
            new ModuleInfo(G4, Const.M_TIMESHEET, "Chốt bảng công"),
            new ModuleInfo(G5, Const.M_FORMULA, "Công thức lương"),
            new ModuleInfo(G5, Const.M_PAYROLL, "Bảng lương"),
            new ModuleInfo(G5, Const.M_MYPAYSLIP, "Phiếu lương của tôi"),
            new ModuleInfo(G6, Const.M_KPIPERIOD, "Kỳ đánh giá KPI/OKR"),
            new ModuleInfo(G6, Const.M_KPICRITERIA, "Tiêu chí KPI theo vị trí"),
            new ModuleInfo(G6, Const.M_KPIEVAL, "Đánh giá nhân viên"),
            new ModuleInfo(G6, Const.M_COURSE, "Khóa đào tạo"),
            new ModuleInfo(G6, Const.M_TRAINING, "Học viên & chứng chỉ"),
            new ModuleInfo(G7, Const.M_DASHBOARD, "Dashboard tổng quan"),
            new ModuleInfo(G7, Const.M_REPORT, "Báo cáo nhân sự & chi phí lương"),
        };

        public static string TitleOf(string code)
        {
            var m = All.FirstOrDefault(x => x.Code == code);
            return m == null ? code : m.Title;
        }
    }

    /// <summary>Phiên đăng nhập hiện tại + kiểm tra quyền RBAC.</summary>
    public static class Session
    {
        public static UserDTO User { get; private set; }
        public static RoleDTO Role { get; private set; }
        static Dictionary<string, RolePermissionDTO> perms = new Dictionary<string, RolePermissionDTO>();

        public static bool IsAdmin { get { return User != null && User.RoleId == 1; } }
        public static int? EmployeeId { get { return User == null ? null : User.EmployeeId; } }

        public static void Start(UserDTO u)
        {
            var dal = new RoleDAL();
            User = u;
            Role = dal.GetById(u.RoleId);
            perms = dal.GetPermissions(u.RoleId).GroupBy(p => p.ModuleCode).ToDictionary(g => g.Key, g => g.First());
        }

        public static void End() { User = null; Role = null; perms = new Dictionary<string, RolePermissionDTO>(); }

        /// <param name="action">view | add | edit | delete</param>
        public static bool Can(string module, string action)
        {
            if (User == null) return false;
            if (IsAdmin) return true;
            RolePermissionDTO p;
            if (!perms.TryGetValue(module, out p)) return false;
            switch (action)
            {
                case "view": return p.CanView;
                case "add": return p.CanAdd;
                case "edit": return p.CanEdit;
                case "delete": return p.CanDelete;
            }
            return false;
        }

        public static void Require(string module, string action)
        {
            if (!Can(module, action))
                throw new UnauthorizedAccessException("Bạn không có quyền thực hiện thao tác này (" + Modules.TitleOf(module) + " / " + action + ").");
        }

        /// <summary>Người dùng chỉ được thấy dữ liệu của chính mình (không có quyền sửa mô-đun và đã liên kết nhân viên).</summary>
        public static bool SelfOnly(string module)
        {
            return !Can(module, "edit") && EmployeeId.HasValue;
        }
    }

    /// <summary>Tham số hệ thống (bảng system_settings) có cache.</summary>
    public static class AppSettings
    {
        static Dictionary<string, string> cache;
        static readonly object sync = new object();

        static void Load()
        {
            lock (sync)
            {
                if (cache != null) return;
                cache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var s in new SettingDAL().GetAll(null, "id"))
                    cache[s.SettingKey] = s.SettingValue;
            }
        }
        public static void Reload() { lock (sync) { cache = null; } Load(); }
        public static string Get(string key, string def = "")
        {
            Load();
            string v;
            return cache.TryGetValue(key, out v) && !string.IsNullOrWhiteSpace(v) ? v : def;
        }
        public static decimal GetDecimal(string key, decimal def)
        {
            decimal d;
            return decimal.TryParse(Get(key, ""), NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : def;
        }
        public static int GetInt(string key, int def) { return (int)GetDecimal(key, def); }
    }

    public static class AuditBUS
    {
        static readonly AuditDAL dal = new AuditDAL();
        public static void Log(string action, string module, string detail)
        {
            try { dal.Write(Session.User == null ? (int?)null : Session.User.Id, Session.User == null ? "(hệ thống)" : Session.User.Username, action, module, detail); }
            catch { /* không để lỗi ghi nhật ký làm hỏng nghiệp vụ */ }
        }
        public static System.Data.DataTable SearchTable(DateTime from, DateTime to, string user, string module, string action, string keyword, int limit)
        {
            Session.Require(Const.M_AUDIT, "view");
            return dal.SearchTable(from, to, user, module, action, keyword, limit);
        }
        public static int CountMatching(DateTime from, DateTime to, string user, string module, string action, string keyword)
        {
            Session.Require(Const.M_AUDIT, "view");
            return dal.CountMatching(from, to, user, module, action, keyword);
        }
        public static List<string> DistinctValues(string column)
        {
            Session.Require(Const.M_AUDIT, "view");
            return dal.DistinctValues(column);
        }
        /// <summary>Xóa nhật ký cũ hơn keepDays ngày (tối thiểu 30 ngày). Việc dọn dẹp cũng được ghi lại.</summary>
        public static int Purge(int keepDays)
        {
            Session.Require(Const.M_AUDIT, "delete");
            if (keepDays < 30) throw new ValidationException("Phải giữ lại nhật ký tối thiểu 30 ngày gần nhất.");
            int n = dal.PurgeBefore(DateTime.Today.AddDays(-keepDays));
            Log("Dọn nhật ký", Const.M_AUDIT, "Xóa " + n + " bản ghi cũ hơn " + keepDays + " ngày");
            return n;
        }
        public static List<AuditLogDTO> Search(DateTime from, DateTime to, string keyword)
        {
            Session.Require(Const.M_AUDIT, "view");
            return dal.Search(from, to, keyword);
        }
    }
}
