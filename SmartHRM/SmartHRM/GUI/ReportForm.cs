using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using SmartHRM.BUS;

namespace SmartHRM.GUI
{
    /// <summary>
    /// Báo cáo nhân sự & chi phí lương: chọn báo cáo ở cây bên trái, đổi dạng biểu đồ,
    /// xem bảng số liệu kèm dòng tổng, xuất Excel và lưu ảnh biểu đồ.
    /// </summary>
    public partial class ReportForm : Form
    {
        class ReportDef
        {
            public string Group, Title, Description;
            public bool NeedsYear;
            public SeriesChartType Default;
            public SeriesChartType[] Allowed;
            public Func<int, DataTable> Load;
        }

        static readonly Dictionary<SeriesChartType, string> TypeNames = new Dictionary<SeriesChartType, string>
        {
            { SeriesChartType.Column, "Cột" }, { SeriesChartType.Bar, "Thanh ngang" }, { SeriesChartType.Line, "Đường" },
            { SeriesChartType.Area, "Vùng" }, { SeriesChartType.Pie, "Tròn" }, { SeriesChartType.Doughnut, "Vành khuyên" },
            { SeriesChartType.StackedColumn, "Cột chồng" }
        };

        readonly ReportBUS bus = new ReportBUS();
        readonly List<ReportDef> defs = new List<ReportDef>();
        DataTable table;
        ReportDef current;
        bool loading;

        public ReportForm()
        {
            InitializeComponent();
            nudYear.Value = DateTime.Today.Year;
            BuildDefinitions();
            BuildTree();
        }

        void BuildDefinitions()
        {
            var cat = new[] { SeriesChartType.Column, SeriesChartType.Bar, SeriesChartType.Pie, SeriesChartType.Doughnut };
            var cat2 = new[] { SeriesChartType.Pie, SeriesChartType.Doughnut, SeriesChartType.Column, SeriesChartType.Bar };
            var time = new[] { SeriesChartType.Column, SeriesChartType.Line, SeriesChartType.Area, SeriesChartType.StackedColumn };
            const string hr = "Báo cáo nhân sự", cost = "Báo cáo chi phí";

            defs.Add(new ReportDef
            {
                Group = hr,
                Title = "Nhân sự theo phòng ban",
                Description = "Số nhân viên đang làm việc của từng phòng ban",
                Default = SeriesChartType.Column,
                Allowed = cat,
                Load = y => bus.HeadcountByDepartment()
            });
            defs.Add(new ReportDef
            {
                Group = hr,
                Title = "Cơ cấu giới tính",
                Description = "Tỷ lệ nam / nữ trong toàn công ty",
                Default = SeriesChartType.Pie,
                Allowed = cat2,
                Load = y => bus.GenderRatio()
            });
            defs.Add(new ReportDef
            {
                Group = hr,
                Title = "Cơ cấu độ tuổi",
                Description = "Phân bố nhân viên theo nhóm tuổi",
                Default = SeriesChartType.Column,
                Allowed = cat,
                Load = y => bus.AgeGroups()
            });
            defs.Add(new ReportDef
            {
                Group = hr,
                Title = "Thâm niên làm việc",
                Description = "Phân bố nhân viên theo số năm gắn bó",
                Default = SeriesChartType.Column,
                Allowed = cat,
                Load = y => bus.Seniority()
            });
            defs.Add(new ReportDef
            {
                Group = hr,
                Title = "Biến động nhân sự theo tháng",
                NeedsYear = true,
                Description = "Nhân viên mới, nghỉ việc và tỷ lệ nghỉ việc từng tháng trong năm",
                Default = SeriesChartType.Column,
                Allowed = time,
                Load = y => bus.Turnover(y)
            });
            defs.Add(new ReportDef
            {
                Group = cost,
                Title = "Chi phí lương - thưởng - bảo hiểm",
                NeedsYear = true,
                Description = "Chi phí doanh nghiệp chi trả theo tháng (bảo hiểm tính phần doanh nghiệp đóng)",
                Default = SeriesChartType.StackedColumn,
                Allowed = new[] { SeriesChartType.StackedColumn, SeriesChartType.Column, SeriesChartType.Line, SeriesChartType.Area },
                Load = y => bus.PayrollCost(y)
            });
        }

        void BuildTree()
        {
            foreach (var g in defs.GroupBy(d => d.Group))
            {
                var gn = new TreeNode(g.Key) { NodeFont = new Font(tvReports.Font, FontStyle.Bold), ForeColor = Color.FromArgb(31, 78, 121) };
                foreach (var d in g) gn.Nodes.Add(new TreeNode(d.Title) { Tag = d });
                tvReports.Nodes.Add(gn);
            }
            tvReports.ExpandAll();
        }

        void ReportForm_Load(object sender, EventArgs e)
        {
            if (tvReports.Nodes.Count > 0 && tvReports.Nodes[0].Nodes.Count > 0) tvReports.SelectedNode = tvReports.Nodes[0].Nodes[0];
        }

        // ------------------------------------------------------------ chọn báo cáo
        void tvReports_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var def = e.Node.Tag as ReportDef;
            if (def == null) return;
            current = def;
            lblReportTitle.Text = def.Title;
            lblReportDesc.Text = def.Description;
            lblYear.Enabled = nudYear.Enabled = def.NeedsYear;

            loading = true;
            cboChartType.DisplayMember = "Value"; cboChartType.ValueMember = "Key";
            cboChartType.DataSource = def.Allowed.Select(t => new KeyValuePair<SeriesChartType, string>(t, TypeNames[t])).ToList();
            cboChartType.SelectedValue = def.Default;
            loading = false;
            Run();
        }

        void btnView_Click(object sender, EventArgs e) { Run(); }

        void cboChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading && table != null) DrawChart();       // đổi dạng biểu đồ không cần truy vấn lại
        }

        // ------------------------------------------------------------ tải & vẽ
        void Run()
        {
            if (current == null) return;
            try
            {
                Cursor = Cursors.WaitCursor;
                table = current.Load((int)nudYear.Value);
                dgvData.DataSource = table;
                Ui.FormatGrid(dgvData);
                lblTotals.Text = BuildTotals(table);
                DrawChart();
                lblStatus.Text = current.Title + (current.NeedsYear ? " - năm " + nudYear.Value : "") + "  |  " + table.Rows.Count + " dòng  |  tạo lúc " + DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
            }
            catch (Exception ex) { Ui.Error(ex); }
            finally { Cursor = Cursors.Default; }
        }

        void DrawChart()
        {
            var type = cboChartType.SelectedValue is SeriesChartType ? (SeriesChartType)cboChartType.SelectedValue : current.Default;
            var plot = table.Copy();
            foreach (var name in plot.Columns.Cast<DataColumn>().Select(c => c.ColumnName).Where(n => n.Contains("%")).ToList())
                plot.Columns.Remove(name);                         // cột tỷ lệ % khác thang đo, chỉ hiện ở bảng số liệu
            ChartHelper.Fill(chartMain, plot, type, current.Title + (current.NeedsYear ? " - năm " + nudYear.Value : ""));
            foreach (var s in chartMain.Series)
            {
                s.IsValueShownAsLabel = chkLabels.Checked;
                if (type == SeriesChartType.Pie || type == SeriesChartType.Doughnut) s.Label = chkLabels.Checked ? "#VALX: #VAL (#PERCENT{P0})" : "";
            }
        }

        static string BuildTotals(DataTable dt)
        {
            if (dt.Rows.Count == 0) return "Không có dữ liệu";
            var parts = new List<string>();
            foreach (DataColumn c in dt.Columns)
            {
                if (c.DataType == typeof(string) || c.DataType == typeof(DateTime)) continue;
                var vals = dt.Rows.Cast<DataRow>().Where(r => r[c] != DBNull.Value).Select(r => Convert.ToDecimal(r[c])).ToList();
                if (vals.Count == 0) continue;
                bool percent = c.ColumnName.Contains("%");
                parts.Add(c.ColumnName + (percent ? " (trung bình)" : "") + ": " + (percent ? vals.Average() : vals.Sum()).ToString("#,##0.##", new CultureInfo("vi-VN")));
            }
            return "Tổng cộng  —  " + string.Join("   |   ", parts);
        }

        // ------------------------------------------------------------ xuất
        void btnExport_Click(object sender, EventArgs e)
        {
            if (table == null || table.Rows.Count == 0) { Ui.Warn("Không có dữ liệu để xuất."); return; }
            Ui.ExportTable(table, current.Title);
        }

        void btnSaveImage_Click(object sender, EventArgs e)
        {
            if (table == null || chartMain.Series.Count == 0) { Ui.Warn("Chưa có biểu đồ để lưu."); return; }
            using (var d = new SaveFileDialog { Filter = "Ảnh PNG (*.png)|*.png", FileName = "BieuDo_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".png" })
            {
                if (d.ShowDialog() != DialogResult.OK) return;
                try { chartMain.SaveImage(d.FileName, ChartImageFormat.Png); Ui.Info("Đã lưu ảnh biểu đồ:\n" + d.FileName); }
                catch (Exception ex) { Ui.Error(ex); }
            }
        }
    }
}