using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using SmartHRM.BUS;
using SmartHRM.Common;
using SmartHRM.DTO;

namespace SmartHRM.GUI
{
    /// <summary>
    /// Dashboard cho Ban giám đốc: 9 thẻ chỉ số + 4 biểu đồ (phòng ban, chi phí lương, giới tính, biến động nhân sự).
    /// Tự làm mới mỗi 30 giây; bấm vào thẻ để mở chức năng liên quan.
    /// </summary>
    public partial class DashboardForm : Form
    {
        readonly ReportBUS bus = new ReportBUS();

        // Dữ liệu của lần làm mới gần nhất - dùng để xuất báo cáo đúng như đang hiển thị
        Dictionary<string, string> lastKpis;
        DataTable tDept, tCost, tGender, tTurnover;
        int lastYear;

        /// <summary>Yêu cầu mở một mô-đun (mã mô-đun) - MainForm xử lý.</summary>
        public event Action<string> NavigateRequested;

        public DashboardForm()
        {
            InitializeComponent();
            foreach (var c in flpCards.Controls.OfType<KpiCard>())
                if (!Session.Can(Convert.ToString(c.Tag), "view")) c.Clickable = false;   // không có quyền thì thẻ chỉ để xem
        }

        void DashboardForm_Load(object sender, EventArgs e)
        {
            Reload();
            tmrRefresh.Enabled = chkAuto.Checked;
        }

        // ------------------------------------------------------------ làm mới
        void Reload()
        {
            try
            {
                int expiring = new ContractBUS().GetExpiring().Rows.Count;
                var k = bus.Kpis(expiring);
                int active = ToInt(k["Nhân viên đang làm"]), present = ToInt(k["Có mặt hôm nay"]);
                int pendingLeave = ToInt(k["Đơn chờ duyệt"]);

                Set(cardActive, k["Nhân viên đang làm"], "đang làm việc tại công ty");
                Set(cardPresent, k["Có mặt hôm nay"], active == 0 ? "" : (present * 100 / active) + "% quân số");
                Set(cardNew, k["Tuyển mới tháng này"], "nhân viên vào làm");
                Set(cardLeft, k["Nghỉ việc tháng này"], "nhân viên nghỉ việc");
                Set(cardProbation, k["Đang thử việc"], "cần theo dõi đánh giá");
                Set(cardContracts, k["HĐ sắp hết hạn"], expiring > 0 ? "cần gia hạn / tái ký" : "không có cảnh báo");
                cardContracts.AccentColor = expiring > 0 ? Color.FromArgb(230, 126, 34) : Color.FromArgb(46, 139, 87);
                Set(cardLeave, k["Đơn chờ duyệt"], pendingLeave > 0 ? "đang chờ bạn xử lý" : "không có đơn tồn");
                cardLeave.AccentColor = pendingLeave > 0 ? Color.FromArgb(230, 126, 34) : Color.FromArgb(46, 139, 87);
                Set(cardCandidates, k["Ứng viên đang xử lý"], "mới / phỏng vấn / đạt");
                Set(cardNet, k["Thực lĩnh kỳ gần nhất"], "đồng - tổng thực lĩnh");

                lastKpis = k; lastYear = DateTime.Today.Year;
                tDept = bus.Headcount();
                tCost = Reverse(bus.CostLastMonths());
                tGender = bus.DashGender();
                tTurnover = bus.DashTurnover(lastYear);
                ChartHelper.Fill(chartDept, tDept, SeriesChartType.Bar, "Nhân sự theo phòng ban");
                ChartHelper.Fill(chartCost, tCost, SeriesChartType.StackedColumn, "Chi phí lương - thưởng - bảo hiểm (6 kỳ gần nhất)");
                ChartHelper.Fill(chartGender, tGender, SeriesChartType.Pie, "Cơ cấu giới tính");
                ChartHelper.Fill(chartTurnover, tTurnover, SeriesChartType.Column, "Biến động nhân sự năm " + lastYear);
                lblUpdated.Text = "Cập nhật lúc " + DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
            }
            catch (Exception ex)
            {
                tmrRefresh.Stop(); chkAuto.Checked = false;
                Ui.Error(ex);
            }
        }

        static void Set(KpiCard card, string value, string hint) { card.Value = value; card.Hint = hint; }
        static int ToInt(string s) { int n; return int.TryParse(s, out n) ? n : 0; }

        /// <summary>Truy vấn trả về kỳ mới nhất trước; đảo lại để biểu đồ chạy theo thời gian.</summary>
        static DataTable Reverse(DataTable src)
        {
            var dt = src.Clone();
            for (int i = src.Rows.Count - 1; i >= 0; i--) dt.ImportRow(src.Rows[i]);
            return dt;
        }

        // ------------------------------------------------------------ sự kiện
        void btnRefresh_Click(object sender, EventArgs e) { Reload(); }

        /// <summary>Xuất báo cáo tổng quan ra Excel: các chỉ số, 4 biểu đồ và bảng số liệu của từng biểu đồ.</summary>
        void btnExport_Click(object sender, EventArgs e)
        {
            if (lastKpis == null) { Ui.Warn("Chưa có dữ liệu để xuất, hãy bấm Làm mới."); return; }
            string path;
            using (var d = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = "BaoCao_TongQuan_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx"
            })
            {
                if (d.ShowDialog() != DialogResult.OK) return;
                path = d.FileName;
            }
            try
            {
                Cursor = Cursors.WaitCursor;
                var sections = new List<DashboardSection>
                {
                    new DashboardSection { Title = "Nhân sự theo phòng ban", Data = tDept, ChartPng = ExportChart.FromChart(chartDept) },
                    new DashboardSection { Title = "Chi phí lương 6 kỳ", Data = tCost, ChartPng = ExportChart.FromChart(chartCost) },
                    new DashboardSection { Title = "Cơ cấu giới tính", Data = tGender, ChartPng = ExportChart.FromChart(chartGender) },
                    new DashboardSection { Title = "Biến động nhân sự " + lastYear, Data = tTurnover, ChartPng = ExportChart.FromChart(chartTurnover) }
                };
                var who = Session.User == null ? "" : (Session.User.FullName ?? Session.User.Username);
                ExcelHelper.ExportDashboard(path, "BÁO CÁO TỔNG QUAN NHÂN SỰ", who, lastKpis.ToList(), sections);
                AuditBUS.Log("Xuất báo cáo", Const.M_DASHBOARD, "Báo cáo tổng quan: " + path);
                if (Ui.Confirm("Đã xuất báo cáo tổng quan (gồm 4 biểu đồ):\n" + path + "\n\nMở file ngay?"))
                    System.Diagnostics.Process.Start(path);
            }
            catch (Exception ex) { Ui.Error(ex); }
            finally { Cursor = Cursors.Default; }
        }
        void tmrRefresh_Tick(object sender, EventArgs e) { Reload(); }
        void chkAuto_CheckedChanged(object sender, EventArgs e) { tmrRefresh.Enabled = chkAuto.Checked; }

        void card_Click(object sender, EventArgs e)
        {
            var card = sender as KpiCard;
            if (card == null || !card.Clickable) return;
            var handler = NavigateRequested;
            if (handler != null) handler(Convert.ToString(card.Tag));
        }
    }
}