using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySqlConnector;
using SmartHRM.DTO;

namespace SmartHRM.DAL
{
    public class UserDAL : GenericDAL<UserDTO>
    {
        public UserDTO GetByUsername(string username)
        {
            return GetAll("username=@u", "id", DbHelper.P("@u", username)).FirstOrDefault();
        }
        public int Count() { return DbHelper.ScalarInt("SELECT COUNT(*) FROM users"); }
        public void TouchLogin(int id) { DbHelper.Execute("UPDATE users SET last_login=NOW() WHERE id=@i", DbHelper.P("@i", id)); }
        public void SetOtp(int id, string hash, DateTime expires)
        {
            DbHelper.Execute("UPDATE users SET otp_hash=@h, otp_expires=@e WHERE id=@i",
                DbHelper.P("@h", hash), DbHelper.P("@e", expires), DbHelper.P("@i", id));
        }
        public DataRow GetOtp(int id)
        {
            var dt = DbHelper.Query("SELECT otp_hash, otp_expires FROM users WHERE id=@i", DbHelper.P("@i", id));
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
        public void ClearOtp(int id) { DbHelper.Execute("UPDATE users SET otp_hash=NULL, otp_expires=NULL WHERE id=@i", DbHelper.P("@i", id)); }
        public void SetPassword(int id, string hash) { DbHelper.Execute("UPDATE users SET password_hash=@h WHERE id=@i", DbHelper.P("@h", hash), DbHelper.P("@i", id)); }
    }

    public class RoleDAL : GenericDAL<RoleDTO>
    {
        public List<RolePermissionDTO> GetPermissions(int roleId)
        {
            return new GenericDAL<RolePermissionDTO>().GetAll("role_id=@r", "id", DbHelper.P("@r", roleId));
        }
        public void SavePermission(RolePermissionDTO p)
        {
            DbHelper.Execute(
                "INSERT INTO role_permissions (role_id,module_code,can_view,can_add,can_edit,can_delete) VALUES (@r,@m,@v,@a,@e,@d) " +
                "ON DUPLICATE KEY UPDATE can_view=@v, can_add=@a, can_edit=@e, can_delete=@d",
                DbHelper.P("@r", p.RoleId), DbHelper.P("@m", p.ModuleCode), DbHelper.P("@v", p.CanView),
                DbHelper.P("@a", p.CanAdd), DbHelper.P("@e", p.CanEdit), DbHelper.P("@d", p.CanDelete));
        }
        public int PermissionCount() { return DbHelper.ScalarInt("SELECT COUNT(*) FROM role_permissions"); }
    }

    public class AuditDAL : GenericDAL<AuditLogDTO>
    {
        public void Write(int? userId, string username, string action, string module, string detail)
        {
            DbHelper.Execute("INSERT INTO audit_logs (user_id,username,action,module,detail,created_at) VALUES (@u,@n,@a,@m,@d,NOW())",
                DbHelper.P("@u", userId), DbHelper.P("@n", username), DbHelper.P("@a", action),
                DbHelper.P("@m", module), DbHelper.P("@d", detail));
        }
        static string BuildFilter(DateTime from, DateTime to, string user, string module, string action, string keyword, List<MySqlParameter> ps)
        {
            ps.Add(DbHelper.P("@f", from.Date));
            ps.Add(DbHelper.P("@t", to.Date.AddDays(1)));
            ps.Add(DbHelper.P("@u", user ?? ""));
            ps.Add(DbHelper.P("@m", module ?? ""));
            ps.Add(DbHelper.P("@a", action ?? ""));
            ps.Add(DbHelper.P("@k", keyword ?? ""));
            return "created_at>=@f AND created_at<@t AND (@u='' OR username=@u) AND (@m='' OR module=@m) AND (@a='' OR action=@a) " +
                   "AND (@k='' OR detail LIKE CONCAT('%',@k,'%') OR username LIKE CONCAT('%',@k,'%'))";
        }

        public DataTable SearchTable(DateTime from, DateTime to, string user, string module, string action, string keyword, int limit)
        {
            var ps = new List<MySqlParameter>();
            var where = BuildFilter(from, to, user, module, action, keyword, ps);
            return DbHelper.Query("SELECT id, created_at, username, action, module, detail FROM audit_logs WHERE " + where +
                                  " ORDER BY id DESC LIMIT " + limit, ps.ToArray());
        }

        public int CountMatching(DateTime from, DateTime to, string user, string module, string action, string keyword)
        {
            var ps = new List<MySqlParameter>();
            var where = BuildFilter(from, to, user, module, action, keyword, ps);
            return DbHelper.ScalarInt("SELECT COUNT(*) FROM audit_logs WHERE " + where, ps.ToArray());
        }

        public List<string> DistinctValues(string column)
        {
            if (column != "username" && column != "module" && column != "action") throw new ArgumentException("column");
            return DbHelper.Query("SELECT DISTINCT " + column + " AS v FROM audit_logs WHERE " + column + " IS NOT NULL AND " + column + "<>'' ORDER BY v")
                .Rows.Cast<DataRow>().Select(r => Convert.ToString(r["v"])).ToList();
        }

        public int PurgeBefore(DateTime before)
        {
            return DbHelper.Execute("DELETE FROM audit_logs WHERE created_at < @b", DbHelper.P("@b", before));
        }

        public List<AuditLogDTO> Search(DateTime from, DateTime to, string keyword)
        {
            return GetAll("created_at>=@f AND created_at<@t AND (@k='' OR username LIKE CONCAT('%',@k,'%') OR detail LIKE CONCAT('%',@k,'%') OR module LIKE CONCAT('%',@k,'%'))",
                "id DESC", DbHelper.P("@f", from.Date), DbHelper.P("@t", to.Date.AddDays(1)), DbHelper.P("@k", keyword ?? ""));
        }
    }

    public class SettingDAL : GenericDAL<SettingDTO>
    {
        public string Get(string key)
        {
            var v = DbHelper.Scalar("SELECT setting_value FROM system_settings WHERE setting_key=@k", DbHelper.P("@k", key));
            return v == null ? null : Convert.ToString(v);
        }
    }
}
