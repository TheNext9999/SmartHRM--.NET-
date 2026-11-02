using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MySqlConnector;
using SmartHRM.BUS;
using SmartHRM.DAL;
using SmartHRM.DTO;

namespace SmartHRM.GUI
{
    public static class Ui
    {
        public const string AppTitle = "SmartHRM - Quản lý nhân sự";

        public static void Info(string msg) { MessageBox.Show(msg, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information); }
        public static void Warn(string msg) { MessageBox.Show(msg, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        public static void Error(Exception ex) { MessageBox.Show(Describe(ex), AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        public static bool Confirm(string msg) { return MessageBox.Show(msg, AppTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes; }

        public static string Describe(Exception ex)
        {
            if (ex is ValidationException || ex is UnauthorizedAccessException) return ex.Message;
            var my = ex as MySqlException;
            if (my != null)
            {
                if (my.Number == 1451) return "Dữ liệu đang được sử dụng ở chức năng khác nên không thể xóa.";
                if (my.Number == 1062) return "Dữ liệu bị trùng với bản ghi đã có.";
                if (my.Number == 1452) return "Giá trị tham chiếu không tồn tại (kiểm tra các ô chọn danh mục).";
                return "Lỗi cơ sở dữ liệu: " + my.Message;
            }
            return "Đã xảy ra lỗi: " + ex.Message;
        }

        /// <summary>Nạp danh mục cho combobox. source = "bảng|cột_id|biểu_thức_hiển_thị". Trả về bảng (id, text).</summary>
        public static DataTable Lookup(string source, bool withBlank)
        {
            var p = source.Split('|');
            var dt = DbHelper.Query("SELECT `" + p[1] + "` AS id, " + p[2] + " AS text FROM `" + p[0] + "` ORDER BY text");
            if (withBlank)
            {
                var blank = dt.NewRow(); blank["id"] = DBNull.Value; blank["text"] = "";
                dt.Rows.InsertAt(blank, 0);
            }
            return dt;
        }

        public static T Clone<T>(T src) where T : new()
        {
            var c = new T();
            foreach (var p in typeof(T).GetProperties().Where(x => x.CanRead && x.CanWrite))
                p.SetValue(c, p.GetValue(src));
            return c;
        }

        /// <summary>Sao chép nông một đối tượng DTO theo kiểu thực tế của nó.</summary>
        public static object CloneObject(object src)
        {
            var c = Activator.CreateInstance(src.GetType());
            foreach (var p in src.GetType().GetProperties().Where(x => x.CanRead && x.CanWrite))
                p.SetValue(c, p.GetValue(src));
            return c;
        }

        public static string ExportDialog(string defaultName)
        {
            using (var d = new SaveFileDialog { Filter = "Excel (*.xlsx)|*.xlsx", FileName = defaultName + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx" })
                return d.ShowDialog() == DialogResult.OK ? d.FileName : null;
        }

        public static void ExportTable(DataTable dt, string name)
        {
            var path = ExportDialog(name);
            if (path == null) return;
            try
            {
                ExcelHelper.Export(dt, path, name);
                if (Confirm("Đã xuất file:\n" + path + "\n\nMở file ngay?")) System.Diagnostics.Process.Start(path);
            }
            catch (Exception ex) { Error(ex); }
        }

        /// <summary>Định dạng số/ngày cho DataGridView theo kiểu dữ liệu cột.</summary>
        public static void FormatGrid(DataGridView grid)
        {
            foreach (DataGridViewColumn c in grid.Columns)
            {
                if (c.ValueType == typeof(decimal) || c.ValueType == typeof(double)) { c.DefaultCellStyle.Format = "#,##0.##"; c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; }
                else if (c.ValueType == typeof(DateTime)) c.DefaultCellStyle.Format = c.Name.Contains("Giờ") || c.Name.Contains("Lịch") || c.Name.Contains("Thời gian") ? "dd/MM/yyyy HH:mm" : "dd/MM/yyyy";
            }
        }

        public static DataGridView NewGrid()
        {
            var g = new DataGridView
            {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells, BackgroundColor = System.Drawing.Color.White,
                RowHeadersVisible = false, BorderStyle = BorderStyle.None, AlternatingRowsDefaultCellStyle = { BackColor = System.Drawing.Color.FromArgb(245, 248, 252) }
            };
            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            g.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            g.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5f, System.Drawing.FontStyle.Bold);
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            return g;
        }
    }

    public static class ChartHelper
    {
        public static void Fill(Chart chart, DataTable dt, SeriesChartType type, string title = null)
        {
            chart.Series.Clear(); chart.Titles.Clear(); chart.Legends.Clear();
            if (chart.ChartAreas.Count == 0) chart.ChartAreas.Add(new ChartArea("main"));
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart.Legends.Add(new Legend("legend") { Docking = Docking.Bottom });
            if (!string.IsNullOrEmpty(title)) chart.Titles.Add(new Title(title, Docking.Top, new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold), System.Drawing.Color.FromArgb(31, 78, 121)));
            for (int c = 1; c < dt.Columns.Count; c++)
            {
                if (dt.Columns[c].DataType == typeof(string)) continue;
                var s = new Series(dt.Columns[c].ColumnName) { ChartType = type, IsValueShownAsLabel = true };
                foreach (DataRow r in dt.Rows)
                    s.Points.AddXY(Convert.ToString(r[0]), r[c] == DBNull.Value ? 0d : Convert.ToDouble(r[c]));
                if (type == SeriesChartType.Pie || type == SeriesChartType.Doughnut) s.Label = "#VALX: #VAL (#PERCENT{P0})";
                chart.Series.Add(s);
            }
        }
    }
}
