using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using MySqlConnector;
using SmartHRM.Common;

namespace SmartHRM.DAL
{
    /// <summary>Truy cập MySQL: mở kết nối, chạy truy vấn có tham số; ghi log lỗi và truy vấn chậm (không ghi giá trị tham số).</summary>
    public static class DbHelper
    {
        const int SlowMs = 1500;

        public static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["SmartHRM"].ConnectionString;

        public static MySqlConnection Open()
        {
            var c = new MySqlConnection(ConnectionString);
            c.Open();
            return c;
        }

        public static MySqlParameter P(string name, object value)
        {
            return new MySqlParameter(name, value ?? DBNull.Value);
        }

        static string Short(string sql)
        {
            var s = Regex.Replace(sql ?? "", @"\s+", " ").Trim();
            return s.Length > 300 ? s.Substring(0, 300) + "..." : s;
        }

        static T Track<T>(string sql, Func<T> action)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var result = action();
                sw.Stop();
                if (sw.ElapsedMilliseconds >= SlowMs) AppLogger.Warn("Truy vấn chậm (" + sw.ElapsedMilliseconds + " ms): " + Short(sql));
                else if (AppLogger.IsDebug) AppLogger.Debug("SQL (" + sw.ElapsedMilliseconds + " ms): " + Short(sql));
                return result;
            }
            catch (Exception ex)
            {
                AppLogger.Error("Lỗi cơ sở dữ liệu khi chạy: " + Short(sql), ex);
                throw;
            }
        }

        public static DataTable Query(string sql, params MySqlParameter[] ps)
        {
            return Track(sql, () =>
            {
                using (var c = Open())
                using (var cmd = new MySqlCommand(sql, c))
                {
                    if (ps != null) cmd.Parameters.AddRange(ps);
                    var dt = new DataTable();
                    using (var ad = new MySqlDataAdapter(cmd)) ad.Fill(dt);
                    return dt;
                }
            });
        }

        public static int Execute(string sql, params MySqlParameter[] ps)
        {
            return Track(sql, () =>
            {
                using (var c = Open())
                using (var cmd = new MySqlCommand(sql, c))
                {
                    if (ps != null) cmd.Parameters.AddRange(ps);
                    return cmd.ExecuteNonQuery();
                }
            });
        }

        public static object Scalar(string sql, params MySqlParameter[] ps)
        {
            return Track(sql, () =>
            {
                using (var c = Open())
                using (var cmd = new MySqlCommand(sql, c))
                {
                    if (ps != null) cmd.Parameters.AddRange(ps);
                    var v = cmd.ExecuteScalar();
                    return v == DBNull.Value ? null : v;
                }
            });
        }

        public static long Insert(string sql, params MySqlParameter[] ps)
        {
            return Track(sql, () =>
            {
                using (var c = Open())
                using (var cmd = new MySqlCommand(sql, c))
                {
                    if (ps != null) cmd.Parameters.AddRange(ps);
                    cmd.ExecuteNonQuery();
                    return cmd.LastInsertedId;
                }
            });
        }

        public static int ScalarInt(string sql, params MySqlParameter[] ps)
        {
            var v = Scalar(sql, ps);
            return v == null ? 0 : Convert.ToInt32(v);
        }

        public static decimal ScalarDecimal(string sql, params MySqlParameter[] ps)
        {
            var v = Scalar(sql, ps);
            return v == null ? 0m : Convert.ToDecimal(v);
        }
    }
}