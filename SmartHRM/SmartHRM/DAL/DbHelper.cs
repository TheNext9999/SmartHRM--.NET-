using System;
using System.Configuration;
using System.Data;
using MySqlConnector;

namespace SmartHRM.DAL
{
    /// <summary>Truy cập MySQL: mở kết nối, chạy truy vấn có tham số.</summary>
    public static class DbHelper
    {
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

        public static DataTable Query(string sql, params MySqlParameter[] ps)
        {
            using (var c = Open())
            using (var cmd = new MySqlCommand(sql, c))
            {
                if (ps != null) cmd.Parameters.AddRange(ps);
                var dt = new DataTable();
                using (var ad = new MySqlDataAdapter(cmd)) ad.Fill(dt);
                return dt;
            }
        }

        public static int Execute(string sql, params MySqlParameter[] ps)
        {
            using (var c = Open())
            using (var cmd = new MySqlCommand(sql, c))
            {
                if (ps != null) cmd.Parameters.AddRange(ps);
                return cmd.ExecuteNonQuery();
            }
        }

        public static object Scalar(string sql, params MySqlParameter[] ps)
        {
            using (var c = Open())
            using (var cmd = new MySqlCommand(sql, c))
            {
                if (ps != null) cmd.Parameters.AddRange(ps);
                var v = cmd.ExecuteScalar();
                return v == DBNull.Value ? null : v;
            }
        }

        public static long Insert(string sql, params MySqlParameter[] ps)
        {
            using (var c = Open())
            using (var cmd = new MySqlCommand(sql, c))
            {
                if (ps != null) cmd.Parameters.AddRange(ps);
                cmd.ExecuteNonQuery();
                return cmd.LastInsertedId;
            }
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
