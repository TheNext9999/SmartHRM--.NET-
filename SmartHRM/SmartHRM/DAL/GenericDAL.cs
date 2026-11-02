using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using MySqlConnector;
using SmartHRM.DTO;

namespace SmartHRM.DAL
{
    public static class Naming
    {
        /// <summary>DepartmentId -> department_id</summary>
        public static string ToSnake(string s)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                if (char.IsUpper(ch) && i > 0 && (char.IsLower(s[i - 1]) || char.IsDigit(s[i - 1]))) sb.Append('_');
                sb.Append(char.ToLowerInvariant(ch));
            }
            return sb.ToString();
        }
    }

    /// <summary>DAL tổng quát: ánh xạ DTO &lt;-&gt; bảng MySQL bằng reflection (tên cột = snake_case của tên thuộc tính).</summary>
    public class GenericDAL<T> where T : BaseDTO, new()
    {
        public static readonly string Table =
            ((TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute), true)[0]).Name;

        protected static readonly PropertyInfo[] Cols = typeof(T).GetProperties()
            .Where(p => p.CanRead && p.CanWrite && p.GetCustomAttribute<IgnoreAttribute>() == null).ToArray();

        public static string Col(PropertyInfo p) { return Naming.ToSnake(p.Name); }

        protected static T Map(DataRow r)
        {
            var e = new T();
            foreach (var p in Cols)
            {
                var c = Col(p);
                if (!r.Table.Columns.Contains(c)) continue;
                var v = r[c];
                if (v == DBNull.Value) continue;
                var t = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                p.SetValue(e, Convert.ChangeType(v, t));
            }
            return e;
        }

        public virtual List<T> GetAll(string where = null, string orderBy = "id DESC", params MySqlParameter[] ps)
        {
            var sql = "SELECT * FROM `" + Table + "`" + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where) + " ORDER BY " + orderBy;
            return DbHelper.Query(sql, ps).Rows.Cast<DataRow>().Select(Map).ToList();
        }

        public virtual T GetById(int id)
        {
            return GetAll("id=@id", "id", DbHelper.P("@id", id)).FirstOrDefault();
        }

        public virtual int Insert(T e)
        {
            var names = new List<string>();
            var ps = new List<MySqlParameter>();
            foreach (var p in Cols)
            {
                if (p.Name == "Id") continue;
                var v = p.GetValue(e);
                if (v == null) continue;               // bỏ qua null -> dùng DEFAULT của CSDL
                names.Add(Col(p));
                ps.Add(new MySqlParameter("@" + Col(p), v));
            }
            var sql = "INSERT INTO `" + Table + "` (" + string.Join(",", names.Select(n => "`" + n + "`")) +
                      ") VALUES (" + string.Join(",", names.Select(n => "@" + n)) + ")";
            e.Id = (int)DbHelper.Insert(sql, ps.ToArray());
            return e.Id;
        }

        public virtual bool Update(T e)
        {
            var sets = new List<string>();
            var ps = new List<MySqlParameter>();
            foreach (var p in Cols)
            {
                if (p.Name == "Id") continue;
                sets.Add("`" + Col(p) + "`=@" + Col(p));
                ps.Add(new MySqlParameter("@" + Col(p), p.GetValue(e) ?? DBNull.Value));
            }
            ps.Add(new MySqlParameter("@id", e.Id));
            return DbHelper.Execute("UPDATE `" + Table + "` SET " + string.Join(",", sets) + " WHERE id=@id", ps.ToArray()) > 0;
        }

        public virtual bool Delete(int id)
        {
            return DbHelper.Execute("DELETE FROM `" + Table + "` WHERE id=@id", DbHelper.P("@id", id)) > 0;
        }
    }
}
