using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SmartHRM.BUS;

namespace SmartHRM.GUI
{
    /// <summary>
    /// Xuất dữ liệu bất kỳ (DataTable) ra Excel / PDF bằng SAP Crystal Reports với MỘT mẫu chung "GenericGrid.rpt".
    /// Mẫu có 12 cột dữ liệu C1..C12 và 12 tiêu đề H1..H12; cột nào không dùng sẽ bị ẩn.
    /// Mọi chức năng gọi Ui.ExportTable(...) đều đi qua lớp này.
    /// Lưu ý: chỉ lớp này tham chiếu tới Crystal Reports; nếu thiếu thư viện hoặc mẫu .rpt, Ui.ExportTable tự quay về xuất Excel thường.
    /// </summary>
    public static class CrystalExporter
    {
        public const int MaxColumns = 12;
        static readonly CultureInfo VN = new CultureInfo("vi-VN");

        /// <summary>true: xuất .xlsx (ExcelWorkbook). Nếu Excel báo "sai định dạng", đặt false để xuất .xls (Excel 97-2003).</summary>
        static readonly bool UseXlsx = true;

        static string TemplatePath
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "GenericGrid.rpt"); }
        }

        /// <summary>Tìm lớp báo cáo do Visual Studio sinh ra (tên GenericGrid) khi bạn thêm GenericGrid.rpt vào project.</summary>
        static Type TemplateType
        {
            get
            {
                return typeof(CrystalExporter).Assembly.GetTypes()
                    .FirstOrDefault(t => t.Name == "GenericGrid" && !t.IsAbstract && typeof(ReportDocument).IsAssignableFrom(t));
            }
        }

        /// <summary>Có mẫu báo cáo để dùng không (lớp nhúng trong project hoặc file Reports\GenericGrid.rpt cạnh file .exe).</summary>
        public static bool IsAvailable
        {
            get
            {
                try { return TemplateType != null || File.Exists(TemplatePath); }
                catch { return false; }
            }
        }

        static ReportDocument CreateReport()
        {
            var type = TemplateType;
            if (type != null) return (ReportDocument)Activator.CreateInstance(type);
            if (!File.Exists(TemplatePath))
                throw new ValidationException("Chưa có mẫu báo cáo Crystal Reports (GenericGrid.rpt).");
            var rpt = new ReportDocument();
            rpt.Load(TemplatePath);
            return rpt;
        }

        static string FormatCell(object v)
        {
            if (v == null || v == DBNull.Value) return "";
            if (v is DateTime) { var d = (DateTime)v; return d.TimeOfDay == TimeSpan.Zero ? d.ToString("dd/MM/yyyy") : d.ToString("dd/MM/yyyy HH:mm"); }
            if (v is decimal) return ((decimal)v).ToString("#,##0.##", VN);
            if (v is double) return ((double)v).ToString("#,##0.##", VN);
            if (v is bool) return (bool)v ? "Có" : "Không";
            return Convert.ToString(v);
        }

        /// <summary>Chuyển DataTable bất kỳ sang bảng "Grid" đúng cấu trúc mẫu (tối đa 12 cột).</summary>
        static DataTable BuildDataSource(DataTable src, string title)
        {
            var grid = new DataTable("Grid");
            grid.Columns.Add("T"); grid.Columns.Add("S"); grid.Columns.Add("G");
            for (int i = 1; i <= MaxColumns; i++) grid.Columns.Add("H" + i);
            for (int i = 1; i <= MaxColumns; i++) grid.Columns.Add("C" + i);

            var cols = src.Columns.Cast<DataColumn>().Where(c => !c.ColumnName.StartsWith("__")).Take(MaxColumns).ToList();
            string subtitle = "Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") +
                              (Session.User == null ? "" : "   |   Người xuất: " + (Session.User.FullName ?? Session.User.Username)) +
                              "   |   Tổng số dòng: " + src.Rows.Count.ToString("N0", VN);
            string cleanTitle = (title ?? "").Replace('_', ' ').ToUpperInvariant();

            foreach (DataRow r in src.Rows)
            {
                var row = grid.NewRow();
                row["T"] = cleanTitle; row["S"] = subtitle; row["G"] = "1";
                for (int k = 0; k < cols.Count; k++)
                {
                    row["H" + (k + 1)] = cols[k].ColumnName.Trim();
                    row["C" + (k + 1)] = FormatCell(r[cols[k]]);
                }
                grid.Rows.Add(row);
            }
            return grid;
        }

        /// <summary>Xuất ra file .xlsx/.xls hoặc .pdf (theo đuôi file của path).</summary>
        public static void Export(DataTable source, string title, string path)
        {
            if (source == null || source.Rows.Count == 0) throw new ValidationException("Không có dữ liệu để xuất.");
            var data = BuildDataSource(source, title);
            bool pdf = Path.GetExtension(path).Equals(".pdf", StringComparison.OrdinalIgnoreCase);
            var format = pdf ? ExportFormatType.PortableDocFormat : (UseXlsx ? ExportFormatType.ExcelWorkbook : ExportFormatType.Excel);

            using (var rpt = CreateReport())
            {
                rpt.SetDataSource(data);
                rpt.ExportToDisk(format, path);
            }
        }
    }
}