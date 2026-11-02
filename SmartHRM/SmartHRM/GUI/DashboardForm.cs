using SmartHRM.BUS;
using SmartHRM.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace SmartHRM.GUI
{
    /// <summary>
    /// Dashboard cho Ban giám đốc: 9 thẻ chỉ số + 4 biểu đồ (phòng ban, chi phí lương, giới tính, biến động nhân sự).
    /// Tự làm mới mỗi 30 giây; bấm vào thẻ để mở chức năng liên quan.
    /// </summary>
    public partial class DashboardForm : Form
    {
        readonly ReportBUS bus = new ReportBUS();

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

                ChartHelper.Fill(chartDept, bus.Headcount(), SeriesChartType.Bar, "Nhân sự theo phòng ban");
                ChartHelper.Fill(chartCost, Reverse(bus.CostLastMonths()), SeriesChartType.StackedColumn, "Chi phí lương - thưởng - bảo hiểm (6 kỳ gần nhất)");
                ChartHelper.Fill(chartGender, bus.DashGender(), SeriesChartType.Pie, "Cơ cấu giới tính");
                ChartHelper.Fill(chartTurnover, bus.DashTurnover(DateTime.Today.Year), SeriesChartType.Column, "Biến động nhân sự năm " + DateTime.Today.Year);
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