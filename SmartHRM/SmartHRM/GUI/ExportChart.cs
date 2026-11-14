using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;

namespace SmartHRM.GUI
{
    /// <summary>
    /// Tạo ảnh biểu đồ (PNG) để chèn vào file Excel khi xuất báo cáo.
    /// - FromChart: lấy đúng biểu đồ người dùng đang xem (màn hình Báo cáo).
    /// - RenderAuto: tự chọn biểu đồ phù hợp từ bảng dữ liệu bất kỳ (các danh mục khác).
    /// </summary>
    public static class ExportChart
    {
        static readonly string[] NumPriority = { "Thực lĩnh", "Tổng thu nhập", "Lương cơ bản", "Lương theo HĐ", "Kinh phí", "Điểm cuối cùng", "Số nhân viên", "Số lượng", "Công thực tế" };
        static readonly string[] CatPriority = { "Loại", "Trạng thái", "Phòng ban", "Giới tính", "Thao tác", "Mô-đun", "Hình thức", "Chức vụ", "Kết quả", "Nhân viên", "Người dùng" };
        static readonly Regex IdLike = new Regex(@"^(id|.*_id|__.*)$", RegexOptions.IgnoreCase);
        static readonly Regex LabelLike = new Regex("(Họ|Tên|Nhân viên|Khóa|Đơn vị|Phòng ban|Chức vụ|Tiêu chí|Kỳ)", RegexOptions.IgnoreCase);
        static readonly Regex NotLabel = new Regex("(^Mã|^Số |Email|Điện thoại|CCCD|Ghi chú|Lý do|Chi tiết|Mô tả)", RegexOptions.IgnoreCase);

        /// <summary>Chụp biểu đồ đang hiển thị trên form (null nếu chưa có dữ liệu).</summary>
        public static byte[] FromChart(Chart chart)
        {
            if (chart == null || chart.Series.Count == 0) return null;
            using (var ms = new MemoryStream())
            {
                chart.SaveImage(ms, ChartImageFormat.Png);
                return ms.ToArray();
            }
        }

        static bool IsNumeric(Type t)
        {
            return t == typeof(decimal) || t == typeof(double) || t == typeof(float) || t == typeof(int) ||
                   t == typeof(long) || t == typeof(short) || t == typeof(byte);
        }

        static int Distinct(DataTable dt, DataColumn c)
        {
            return dt.Rows.Cast<DataRow>().Select(r => Convert.ToString(r[c])).Distinct().Count();
        }

        static DataColumn PickByPriority(List<DataColumn> cols, string[] priority)
        {
            foreach (var p in priority)
            {
                var hit = cols.FirstOrDefault(c => c.ColumnName.IndexOf(p, StringComparison.OrdinalIgnoreCase) >= 0);
                if (hit != null) return hit;
            }
            return null;
        }

        static decimal Num(object v) { return v == null || v == DBNull.Value ? 0m : Convert.ToDecimal(v); }

        static string Trim(string s, int max)
        {
            s = (s ?? "").Trim();
            if (s.Length == 0) return "(trống)";
            return s.Length > max ? s.Substring(0, max - 1) + "…" : s;
        }

        /// <summary>Tự chọn biểu đồ: mỗi dòng một đối tượng (so sánh số liệu) hoặc gom nhóm theo cột phân loại.</summary>
        public static byte[] RenderAuto(DataTable src, string title)
        {
            if (src == null || src.Rows.Count == 0) return null;
            var cols = src.Columns.Cast<DataColumn>().Where(c => !IdLike.IsMatch(c.ColumnName)).ToList();
            var nums = cols.Where(c => IsNumeric(c.DataType)).ToList();
            var texts = cols.Where(c => c.DataType == typeof(string)).ToList();
            DataColumn numCol = PickByPriority(nums, NumPriority) ?? nums.FirstOrDefault();

            DataTable plot = null;
            var type = SeriesChartType.Column;
            string heading = "";

            // 1) Ít dòng và mỗi dòng một đối tượng (nhãn duy nhất): so sánh số liệu giữa các đối tượng
            if (numCol != null && src.Rows.Count <= 30)
            {
                var label = texts
                    .Where(c => !NotLabel.IsMatch(c.ColumnName) && Distinct(src, c) == src.Rows.Count &&
                                src.Rows.Cast<DataRow>().All(r => !string.IsNullOrWhiteSpace(Convert.ToString(r[c]))))
                    .OrderByDescending(c => LabelLike.IsMatch(c.ColumnName)).FirstOrDefault();
                if (label != null)
                {
                    plot = new DataTable();
                    plot.Columns.Add(label.ColumnName, typeof(string));
                    plot.Columns.Add(numCol.ColumnName, typeof(decimal));
                    foreach (var r in src.Rows.Cast<DataRow>().OrderByDescending(r => Num(r[numCol])).Take(25))
                        plot.Rows.Add(Trim(Convert.ToString(r[label]), 26), Num(r[numCol]));
                    type = plot.Rows.Count > 8 ? SeriesChartType.Bar : SeriesChartType.Column;
                    heading = numCol.ColumnName + " theo " + label.ColumnName.ToLowerInvariant();
                }
            }

            // 2) Gom nhóm theo cột phân loại: đếm số dòng hoặc cộng dồn cột số
            if (plot == null)
            {
                var cats = texts
                    .Where(c => !NotLabel.IsMatch(c.ColumnName))
                    .Select(c => new { Col = c, D = Distinct(src, c) })
                    .Where(x => x.D >= 2 && x.D <= 40 && x.D < src.Rows.Count)
                    .Select(x => new
                    {
                        x.Col, x.D,
                        Rank = Enumerable.Range(0, CatPriority.Length).Where(i => x.Col.ColumnName.IndexOf(CatPriority[i], StringComparison.OrdinalIgnoreCase) >= 0).DefaultIfEmpty(99).Min()
                    })
                    .OrderBy(x => x.Rank).ThenBy(x => x.D).ToList();
                if (cats.Count > 0)
                {
                    var cat = cats[0].Col;
                    var groups = src.Rows.Cast<DataRow>().GroupBy(r => Trim(Convert.ToString(r[cat]), 26)).ToList();
                    plot = new DataTable();
                    plot.Columns.Add(cat.ColumnName, typeof(string));
                    string valName = numCol != null ? "Tổng " + numCol.ColumnName.ToLowerInvariant() : "Số lượng";
                    plot.Columns.Add(valName, typeof(decimal));
                    foreach (var g in groups.OrderByDescending(g => numCol != null ? g.Sum(r => Num(r[numCol])) : g.Count()).Take(15))
                        plot.Rows.Add(g.Key, numCol != null ? g.Sum(r => Num(r[numCol])) : (decimal)g.Count());
                    type = numCol == null && plot.Rows.Count <= 6 ? SeriesChartType.Pie
                         : plot.Rows.Count > 8 ? SeriesChartType.Bar : SeriesChartType.Column;
                    heading = (numCol != null ? valName : "Số lượng") + " theo " + cat.ColumnName.ToLowerInvariant();
                }
            }

            if (plot == null || plot.Rows.Count == 0) return null;
            return Draw(plot, type, (title ?? "").Replace('_', ' ') + " - " + heading);
        }

        static byte[] Draw(DataTable plot, SeriesChartType type, string title)
        {
            using (var chart = new Chart())
            {
                chart.Width = 960;
                chart.Height = type == SeriesChartType.Bar ? Math.Max(420, 26 * plot.Rows.Count + 150) : 480;
                chart.BackColor = Color.White;
                chart.AntiAliasing = AntiAliasingStyles.All;
                chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
                chart.ChartAreas.Add(new ChartArea("main"));
                ChartHelper.Fill(chart, plot, type, title);
                foreach (var s in chart.Series) { s.LabelFormat = "#,##0.##"; s.Font = new Font("Segoe UI", 8.5f); }
                if (type != SeriesChartType.Pie)
                {
                    var area = chart.ChartAreas[0];
                    area.AxisY.LabelStyle.Format = "#,##0";
                    if (type == SeriesChartType.Bar) area.AxisX.IsReversed = true;   // giá trị lớn nhất nằm trên cùng
                    if (chart.Series.Count == 1) chart.Legends.Clear();
                }
                using (var ms = new MemoryStream())
                {
                    chart.SaveImage(ms, ChartImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }
    }
}
