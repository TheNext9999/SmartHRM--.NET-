using System;
using System.Data;
using System.IO;
using System.Linq;
using ClosedXML.Excel;

namespace SmartHRM.BUS
{
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