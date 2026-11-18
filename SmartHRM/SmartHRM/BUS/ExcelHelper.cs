using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using ClosedXML.Excel;

namespace SmartHRM.BUS
{
    /// <summary>Một phần của báo cáo tổng quan: tên, bảng số liệu và ảnh biểu đồ tương ứng.</summary>
    public class DashboardSection
    {
        public string Title;
        public DataTable Data;
        public byte[] ChartPng;
    }

    public static class ExcelHelper
    {
        const string ChartSheetName = "Biểu đồ";

        /// <summary>Xuất DataTable ra file .xlsx (ẩn các cột kỹ thuật bắt đầu bằng "__"), có thể kèm sheet "Biểu đồ" chứa ảnh biểu đồ.</summary>
        public static void Export(DataTable source, string path, string sheetName, byte[] chartPng = null, string chartTitle = null)
        {
            var dt = source.Copy();
            foreach (var name in dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName).Where(n => n.StartsWith("__")).ToList())
                dt.Columns.Remove(name);
            dt.TableName = "Data";
            using (var chartStream = chartPng == null ? null : new MemoryStream(chartPng))     // giữ stream mở đến khi lưu xong
            using (var wb = new XLWorkbook())
            {
                var safe = new string((sheetName ?? "Sheet1").Where(ch => "[]:*?/\\".IndexOf(ch) < 0).ToArray());
                if (safe.Length > 31) safe = safe.Substring(0, 31);
                var ws = wb.Worksheets.Add(dt, string.IsNullOrEmpty(safe) ? "Sheet1" : safe);
                ws.Columns().AdjustToContents();
                if (chartStream != null) AddChartSheet(wb, chartStream, chartTitle ?? sheetName);
                wb.SaveAs(path);
            }
        }

        /// <summary>Mở file .xlsx đã có (ví dụ do Crystal Reports tạo) và thêm sheet "Biểu đồ".</summary>
        public static void AppendChartSheet(string path, byte[] chartPng, string chartTitle)
        {
            using (var chartStream = new MemoryStream(chartPng))
            using (var wb = new XLWorkbook(path))
            {
                AddChartSheet(wb, chartStream, chartTitle);
                wb.Save();
            }
        }

        /// <summary>
        /// Xuất báo cáo tổng quan: sheet "Tổng quan" (bảng chỉ số + lưới 2x2 biểu đồ) và mỗi biểu đồ một sheet số liệu.
        /// </summary>
        public static void ExportDashboard(string path, string title, string exportedBy,
                                           IList<KeyValuePair<string, string>> kpis, IList<DashboardSection> sections)
        {
            var streams = new List<MemoryStream>();            // phải giữ mở đến khi lưu xong
            try
            {
                using (var wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("Tổng quan");
                    ws.ShowGridLines = false;
                    ws.Cell(1, 1).Value = title;
                    ws.Cell(1, 1).Style.Font.Bold = true;
                    ws.Cell(1, 1).Style.Font.FontSize = 16;
                    ws.Cell(1, 1).Style.Font.FontColor = XLColor.FromHtml("#1F4E79");
                    ws.Cell(2, 1).Value = "Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "   |   Người xuất: " + exportedBy;
                    ws.Cell(2, 1).Style.Font.Italic = true;
                    ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;

                    ws.Cell(4, 1).Value = "Chỉ số";
                    ws.Cell(4, 2).Value = "Giá trị";
                    var head = ws.Range(4, 1, 4, 2);
                    head.Style.Font.Bold = true;
                    head.Style.Font.FontColor = XLColor.White;
                    head.Style.Fill.BackgroundColor = XLColor.FromHtml("#1F4E79");
                    int r = 5;
                    foreach (var kv in kpis)
                    {
                        ws.Cell(r, 1).Value = kv.Key;
                        ws.Cell(r, 2).Value = kv.Value;
                        ws.Cell(r, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        r++;
                    }
                    var box = ws.Range(4, 1, r - 1, 2);
                    box.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    box.Style.Border.InsideBorder = XLBorderStyleValues.Hair;
                    ws.Column(1).Width = 30;
                    ws.Column(2).Width = 20;

                    // Lưới biểu đồ 2 cột x 2 hàng, mỗi ảnh tối đa 560 x 340 px (giữ nguyên tỷ lệ)
                    int top = r + 2, i = 0;
                    foreach (var sec in sections)
                    {
                        if (sec.ChartPng == null) continue;
                        var ms = new MemoryStream(sec.ChartPng);
                        streams.Add(ms);
                        int w, h;
                        using (var img = Image.FromStream(ms, false, false)) { w = img.Width; h = img.Height; }
                        ms.Position = 0;
                        double k = Math.Min(560.0 / w, 340.0 / h);
                        var pic = ws.AddPicture(ms);
                        pic.MoveTo(ws.Cell(top + (i / 2) * 19, i % 2 == 0 ? 1 : 8));
                        pic.WithSize((int)(w * k), (int)(h * k));
                        i++;
                    }

                    int idx = 0;
                    foreach (var sec in sections)
                    {
                        if (sec.Data == null || sec.Data.Rows.Count == 0) continue;
                        var dt = sec.Data.Copy();
                        dt.TableName = "Bang" + (++idx);
                        var name = new string((sec.Title ?? "Du lieu").Where(ch => "[]:*?/\\".IndexOf(ch) < 0).ToArray());
                        if (name.Length > 28) name = name.Substring(0, 28);
                        string unique = name; int n = 1;
                        while (wb.Worksheets.Any(w2 => string.Equals(w2.Name, unique, StringComparison.OrdinalIgnoreCase))) unique = name + " " + (++n);
                        var sheet = wb.Worksheets.Add(dt, unique);
                        sheet.Columns().AdjustToContents();
                    }
                    wb.SaveAs(path);
                }
            }
            finally { foreach (var m in streams) m.Dispose(); }
        }

        static void AddChartSheet(XLWorkbook wb, Stream png, string title)
        {
            string name = ChartSheetName; int n = 1;
            while (wb.Worksheets.Any(w => string.Equals(w.Name, name, StringComparison.OrdinalIgnoreCase))) name = ChartSheetName + " " + (++n);
            var ws = wb.Worksheets.Add(name);
            ws.ShowGridLines = false;
            ws.Cell(1, 1).Value = string.IsNullOrWhiteSpace(title) ? "Biểu đồ" : title.Replace('_', ' ').ToUpperInvariant();
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 14;
            var pic = ws.AddPicture(png);
            pic.MoveTo(ws.Cell(3, 1));
        }
    }
}