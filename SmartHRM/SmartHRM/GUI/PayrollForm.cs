using SmartHRM.BUS;
using SmartHRM.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace SmartHRM.GUI
{
    /// <summary>
    /// Bảng lương theo kỳ: tính lương, chốt, xem trước/in phiếu lương, gửi phiếu qua email, xuất Excel.
    /// selfOnly = true: màn hình "Phiếu lương của tôi" cho nhân viên (chỉ xem phiếu của chính mình).
    /// </summary>
    public partial class PayrollForm : Form
    {
        static readonly CultureInfo VN = new CultureInfo("vi-VN");
        readonly PayrollBUS bus = new PayrollBUS();
        readonly bool selfOnly;
        DataTable table;
        DataView view;
        bool loading;

        public PayrollForm() : this(false) { }

        public PayrollForm(bool selfOnly)
        {
            this.selfOnly = selfOnly;
            InitializeComponent();

            Text = lblTitle.Text = selfOnly ? "Phiếu lương của tôi" : "Bảng lương";
            var last = DateTime.Today.AddMonths(-1);              // thường tính lương cho tháng trước
            nudMonth.Value = last.Month; nudYear.Value = last.Year;

            bool canEdit = !selfOnly && Session.Can(Const.M_PAYROLL, "edit");
            tsbCalc.Visible = tsbFinalize.Visible = tsbSend.Visible = canEdit;
            tss1.Visible = canEdit;
            tsbExport.Visible = !selfOnly;
            flpCards.Visible = !selfOnly;
            lblSearch.Visible = txtSearch.Visible = !selfOnly;
            dgvPayroll.MultiSelect = !selfOnly;                    // chọn nhiều dòng để gửi email
            cardCount.Value = cardGross.Value = cardInsurance.Value = cardPit.Value = cardNet.Value = "0";
        }

        int M { get { return (int)nudMonth.Value; } }
        int Y { get { return (int)nudYear.Value; } }

        void PayrollForm_Load(object sender, EventArgs e) { LoadData(); }

        // ------------------------------------------------------------ chọn kỳ lương
        void btnView_Click(object sender, EventArgs e) { LoadData(); }

        void btnPrev_Click(object sender, EventArgs e) { Shift(-1); }
        void btnNext_Click(object sender, EventArgs e) { Shift(1); }

        void Shift(int months)
        {
            var d = new DateTime(Y, M, 1).AddMonths(months);
            nudMonth.Value = d.Month; nudYear.Value = Math.Max(nudYear.Minimum, Math.Min(nudYear.Maximum, d.Year));
            LoadData();
        }

        // ------------------------------------------------------------ nạp dữ liệu
        void LoadData()
        {
            try
            {
                loading = true;
                table = bus.View(M, Y, selfOnly);
                view = new DataView(table);
                ApplyFilter();
                dgvPayroll.DataSource = view;
                FormatGrid();
                UpdateSummary();
            }
            catch (Exception ex) { Ui.Error(ex); }
            finally { loading = false; }
            ShowPreview(SelectedId());
        }

        void FormatGrid()
        {
            foreach (var n in new[] { "id", "employee_id" }) if (dgvPayroll.Columns.Contains(n)) dgvPayroll.Columns[n].Visible = false;
            if (selfOnly && dgvPayroll.Columns.Contains("Email")) dgvPayroll.Columns["Email"].Visible = false;
            foreach (DataGridViewColumn c in dgvPayroll.Columns)
                if (c.ValueType == typeof(decimal))
                {
                    c.DefaultCellStyle.Format = c.Name == "Công" ? "0.##" : "#,##0";
                    c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            if (dgvPayroll.Columns.Contains("Thực lĩnh")) dgvPayroll.Columns["Thực lĩnh"].DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        }

        void ApplyFilter()
        {
            var q = (txtSearch.Text ?? "").Trim();
            foreach (var ch in new[] { '[', ']', '*', '%', '\'' }) q = q.Replace(ch.ToString(), "");
            view.RowFilter = q.Length == 0 ? "" :
                "[Mã NV] LIKE '%" + q + "%' OR [Họ tên] LIKE '%" + q + "%' OR [Phòng ban] LIKE '%" + q + "%'";
        }

        static decimal Sum(DataTable t, string col)
        {
            return t.Rows.Cast<DataRow>().Sum(r => r[col] == DBNull.Value ? 0m : Convert.ToDecimal(r[col]));
        }

        static string Money(decimal v) { return v.ToString("N0", VN); }

        void UpdateSummary()
        {
            int n = table.Rows.Count;
            cardCount.Value = n.ToString("N0");
            cardCount.Hint = n == 0 ? "chưa tính lương kỳ này" : "nhân viên trong kỳ " + M + "/" + Y;
            decimal gross = Sum(table, "Tổng thu nhập"), ins = Sum(table, "BHXH") + Sum(table, "BHYT") + Sum(table, "BHTN");
            decimal pit = Sum(table, "Thuế TNCN"), net = Sum(table, "Thực lĩnh");
            cardGross.Value = Money(gross); cardGross.Hint = "đồng";
            cardInsurance.Value = Money(ins); cardInsurance.Hint = "BHXH + BHYT + BHTN";
            cardPit.Value = Money(pit); cardPit.Hint = "tạm khấu trừ";
            cardNet.Value = Money(net); cardNet.Hint = "đồng - sau thuế & bảo hiểm";

            var statuses = table.Rows.Cast<DataRow>().Select(r => Convert.ToString(r["Trạng thái"])).Distinct().ToList();
            if (n == 0) { lblPeriodStatus.Text = "● Chưa có bảng lương"; lblPeriodStatus.ForeColor = Color.Gray; }
            else if (statuses.Count == 1 && statuses[0] == "Đã chốt") { lblPeriodStatus.Text = "● Đã chốt"; lblPeriodStatus.ForeColor = Color.SeaGreen; }
            else if (statuses.Contains("Đã chốt")) { lblPeriodStatus.Text = "● Chốt một phần"; lblPeriodStatus.ForeColor = Color.DarkOrange; }
            else { lblPeriodStatus.Text = "● Bản nháp - chưa chốt"; lblPeriodStatus.ForeColor = Color.DarkOrange; }

            lblStatus.Text = "Kỳ " + M + "/" + Y + ": " + view.Count + " / " + n + " phiếu lương";
            tsbFinalize.Enabled = n > 0 && !(statuses.Count == 1 && statuses[0] == "Đã chốt");
        }

        void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (view == null) return;
            ApplyFilter();
            lblStatus.Text = "Kỳ " + M + "/" + Y + ": " + view.Count + " / " + table.Rows.Count + " phiếu lương";
        }

        void dgvPayroll_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || !dgvPayroll.Columns.Contains("Trạng thái")) return;
            var st = Convert.ToString(dgvPayroll.Rows[e.RowIndex].Cells["Trạng thái"].Value);
            if (st == "Đã chốt") e.CellStyle.BackColor = Color.FromArgb(226, 244, 230);
            else if (st == "Đã gửi") e.CellStyle.BackColor = Color.FromArgb(221, 238, 255);
        }

        // ------------------------------------------------------------ xem trước / in phiếu
        int? SelectedId()
        {
            if (dgvPayroll.CurrentRow == null || !dgvPayroll.Columns.Contains("id")) return null;
            var v = dgvPayroll.CurrentRow.Cells["id"].Value;
            return v == null || v == DBNull.Value ? (int?)null : Convert.ToInt32(v);
        }

        List<int> SelectedIds()
        {
            return dgvPayroll.SelectedRows.Cast<DataGridViewRow>().Select(r => Convert.ToInt32(r.Cells["id"].Value)).ToList();
        }

        void dgvPayroll_SelectionChanged(object sender, EventArgs e)
        {
            if (!loading) ShowPreview(SelectedId());
        }

        void dgvPayroll_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !tsbPreview.Checked) tsbPreview.Checked = true;   // nhấp đúp = mở khung xem trước
        }

        void ShowPreview(int? id)
        {
            if (splitMain.Panel2Collapsed) return;
            if (!id.HasValue)
            {
                wbPreview.DocumentText = "<html><body style='font-family:Segoe UI;color:#888;padding:30px'>Chọn một dòng để xem phiếu lương.</body></html>";
                return;
            }
            try { wbPreview.DocumentText = bus.BuildPayslipHtml(id.Value); }
            catch (Exception ex) { wbPreview.DocumentText = "<html><body style='font-family:Segoe UI;color:#b00;padding:30px'>" + System.Net.WebUtility.HtmlEncode(Ui.Describe(ex)) + "</body></html>"; }
        }

        void tsbPreview_CheckedChanged(object sender, EventArgs e)
        {
            splitMain.Panel2Collapsed = !tsbPreview.Checked;
            if (tsbPreview.Checked) ShowPreview(SelectedId());
        }

        void tsbPrint_Click(object sender, EventArgs e)
        {
            if (!SelectedId().HasValue) { Ui.Warn("Hãy chọn một phiếu lương để in."); return; }
            if (splitMain.Panel2Collapsed) tsbPreview.Checked = true;
            ShowPreview(SelectedId());
            try { wbPreview.ShowPrintPreviewDialog(); }
            catch (Exception ex) { Ui.Error(ex); }
        }

        // ------------------------------------------------------------ nghiệp vụ lương
        void Run(Action a)
        {
            try { Cursor = Cursors.WaitCursor; a(); LoadData(); }
            catch (Exception ex) { Ui.Error(ex); }
            finally { Cursor = Cursors.Default; }
        }

        void tsbCalc_Click(object sender, EventArgs e)
        {
            if (table != null && table.Rows.Count > 0 && !Ui.Confirm("Kỳ " + M + "/" + Y + " đã có bảng lương. Tính lại sẽ ghi đè các phiếu chưa chốt. Tiếp tục?")) return;
            Run(() => Ui.Info("Đã tính lương cho " + bus.Calculate(M, Y) + " nhân viên (phiếu đã chốt được giữ nguyên)."));
        }

        void tsbFinalize_Click(object sender, EventArgs e)
        {
            if (!Ui.Confirm("Chốt bảng lương " + M + "/" + Y + "?\nSau khi chốt sẽ không tính lại được các phiếu này.")) return;
            Run(() => bus.FinalizePeriod(M, Y));
        }

        void tsbSend_Click(object sender, EventArgs e)
        {
            if (view == null || view.Count == 0) { Ui.Warn("Chưa có phiếu lương để gửi."); return; }
            var selected = SelectedIds();
            var all = view.Cast<DataRowView>().Select(r => Convert.ToInt32(r["id"])).ToList();
            var ids = selected;
            if (selected.Count <= 1)
            {
                var r = MessageBox.Show("Gửi phiếu lương qua email cho TẤT CẢ " + all.Count + " nhân viên đang hiển thị?\n\n" +
                    "Có = gửi tất cả\nKhông = chỉ gửi dòng đang chọn\nHủy = không gửi", Ui.AppTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (r == DialogResult.Cancel) return;
                ids = r == DialogResult.Yes ? all : selected;
            }
            else if (!Ui.Confirm("Gửi phiếu lương qua email cho " + ids.Count + " nhân viên đã chọn?")) return;
            if (ids.Count == 0) { Ui.Warn("Chưa chọn dòng nào."); return; }
            try { Cursor = Cursors.WaitCursor; var msg = bus.SendPayslips(ids); LoadData(); Ui.Info(msg); }
            catch (Exception ex) { Ui.Error(ex); }
            finally { Cursor = Cursors.Default; }
        }

        void tsbExport_Click(object sender, EventArgs e)
        {
            if (table == null || table.Rows.Count == 0) { Ui.Warn("Chưa có dữ liệu để xuất."); return; }
            var t = table.Copy();
            foreach (var n in new[] { "id", "employee_id" }) if (t.Columns.Contains(n)) t.Columns.Remove(n);
            Ui.ExportTable(t, "BangLuong_" + M + "_" + Y);
        }
    }
}