using System;
using System.Data;
using System.Linq;
using ClosedXML.Excel;

namespace SmartHRM.BUS
{
    public static class ExcelHelper
    {
        /// <summary>Xuất DataTable ra file .xlsx (ẩn các cột kỹ thuật bắt đầu bằng "__").</summary>
        public static void Export(DataTable source, string path, string sheetName)
        {
            var dt = source.Copy();
            foreach (var name in dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName).Where(n => n.StartsWith("__")).ToList())
                dt.Columns.Remove(name);
            dt.TableName = "Data";
            using (var wb = new XLWorkbook())
            {
                var safe = new string((sheetName ?? "Sheet1").Where(ch => "[]:*?/\\".IndexOf(ch) < 0).ToArray());
                if (safe.Length > 31) safe = safe.Substring(0, 31);
                var ws = wb.Worksheets.Add(dt, string.IsNullOrEmpty(safe) ? "Sheet1" : safe);
                ws.Columns().AdjustToContents();
                wb.SaveAs(path);
            }
        }
    }
}
