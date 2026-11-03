using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartHRM.BUS;
using SmartHRM.DTO;

namespace SmartHRM.GUI
{
    /// <summary>
    /// Tổng hợp và chốt bảng công cuối tháng để chuyển sang mô-đun lương.
    /// Quy trình: Tổng hợp công → Kiểm tra bất thường → Chốt công (khóa dữ liệu chấm công của tháng).
    /// </summary>
    public partial class TimesheetForm : Form
    {
        readonly TimesheetBUS bus = new TimesheetBUS();
        DataTable table;
        DataView view;
        bool locked;

        public TimesheetForm()
        {
            InitializeComponent();
            var last = DateTime.Today.AddMonths(-1);
            nudMonth.Value = last.Month; nudYear.Value = last.Year;
            bool edit = Session.Can(Const.M_TIMESHEET, "edit");
            tsbCalc.Visible = tsbLock.Visible = edit;
            tsbUnlock.Visible = Session.Can(Const.M_TIMESHEET, "delete");
            cardCount.Value = cardWork.Value = cardLate.Value = cardOt.Value = "0";
        }

        int M { get { return (int)nudMonth.Value; } }
        int Y { get { return (int)nudYear.Value; } }

        void TimesheetForm_Load(object sender, EventArgs e) { LoadData(); }

        // ------------------------------------------------------------ chọn kỳ
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
                table = bus.View(M, Y);
                view = new DataView(table);
                ApplyFilter();
                dgvSheet.DataSource = view;
                if (dgvSheet.Columns.Contains("id")) dgvSheet.Columns["id"].Visible = false;
                foreach (DataGridViewColumn c in dgvSheet.Columns)
                    if (c.ValueType == typeof(decimal)) { c.DefaultCellStyle.Format = "0.##"; c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; }
                UpdateSummary();
            }
            catch (Exception ex) { Ui.Error(ex); }
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

        void UpdateSummary()
        {
            int n = table.Rows.Count;
            locked = n > 0 && table.Rows.Cast<DataRow>().All(r => Convert.ToString(r["Trạng thái"]) == "Đã chốt");

            cardCount.Value = n.ToString("N0"); cardCount.Hint = "kỳ " + M + "/" + Y;
            cardWork.Value = Sum(table, "Công thực tế").ToString("#,##0.##"); cardWork.Hint = "ngày công từ chấm công";
            cardLate.Value = Sum(table, "Số lần muộn").ToString("N0"); cardLate.Hint = Sum(table, "Phút muộn").ToString("N0") + " phút muộn";
            cardOt.Value = Sum(table, "Giờ OT").ToString("#,##0.#"); cardOt.Hint = "từ đơn OT đã duyệt";

            if (n == 0) { lblPeriodStatus.Text = "● Chưa tổng hợp công"; lblPeriodStatus.ForeColor = Color.Gray; }
            else if (locked) { lblPeriodStatus.Text = "● Đã chốt công"; lblPeriodStatus.ForeColor = Color.SeaGreen; }
            else { lblPeriodStatus.Text = "● Chưa chốt"; lblPeriodStatus.ForeColor = Color.DarkOrange; }

            tsbCalc.Enabled = !locked;
            tsbCheck.Enabled = true;
            tsbLock.Enabled = n > 0 && !locked;
            tsbUnlock.Enabled = locked;
            lblStatus.Text = "Kỳ " + M + "/" + Y + ": " + view.Count + " / " + n + " nhân viên";
        }

        void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (view == null) return;
            ApplyFilter();
            lblStatus.Text = "Kỳ " + M + "/" + Y + ": " + view.Count + " / " + table.Rows.Count + " nhân viên";
        }

        void dgvSheet_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || !dgvSheet.Columns.Contains("Số lần muộn")) return;
            var row = dgvSheet.Rows[e.RowIndex];
            int late = Convert.ToInt32(row.Cells["Số lần muộn"].Value == DBNull.Value ? 0 : row.Cells["Số lần muộn"].Value);
            if (late >= 3) e.CellStyle.BackColor = Color.FromArgb(255, 226, 226);
            else if (Convert.ToString(row.Cells["Trạng thái"].Value) == "Đã chốt") e.CellStyle.BackColor = Color.FromArgb(226, 244, 230);
        }

        // ------------------------------------------------------------ nghiệp vụ
        bool Run(Action a)
        {
            try { Cursor = Cursors.WaitCursor; a(); LoadData(); return true; }
            catch (Exception ex) { Ui.Error(ex); return false; }
            finally { Cursor = Cursors.Default; }
        }

        void tsbCalc_Click(object sender, EventArgs e)
        {
            if (Run(() => bus.Calculate(M, Y)))
                Ui.Info("Đã tổng hợp công tháng " + M + "/" + Y + ".\nHãy bấm \"Kiểm tra bất thường\" rồi \"Chốt công\".");
        }

        void tsbCheck_Click(object sender, EventArgs e) { ShowIssues(false); }

        /// <summary>Hiển thị các bất thường; trả về số lượng.</summary>
        int ShowIssues(bool askToContinue, out bool proceed)
        {
            proceed = true;
            DataTable issues;
            try { issues = bus.GetIssues(M, Y); }
            catch (Exception ex) { Ui.Error(ex); proceed = false; return 0; }

            if (issues.Rows.Count == 0)
            {
                if (!askToContinue) Ui.Info("Không phát hiện bất thường nào trong tháng " + M + "/" + Y + ". ✔");
                return 0;
            }
            using (var f = new Form { Text = "Bất thường tháng " + M + "/" + Y, Size = new Size(820, 480), StartPosition = FormStartPosition.CenterParent, Font = Font })
            {
                var g = Ui.NewGrid(); g.DataSource = issues; Ui.FormatGrid(g);
                var bar = new Panel { Dock = DockStyle.Bottom, Height = 48, BackColor = Color.FromArgb(240, 243, 247) };
                var lbl = new System.Windows.Forms.Label { Text = issues.Rows.Count + " vấn đề cần xem lại" + (askToContinue ? " trước khi chốt." : "."), AutoSize = true, Left = 12, Top = 15, ForeColor = Color.Firebrick };
                var export = new Button { Text = "📊 Xuất Excel", Width = 110, Height = 30, Top = 9, Anchor = AnchorStyles.Top | AnchorStyles.Right };
                var close = new Button { Text = askToContinue ? "Quay lại xử lý" : "Đóng", Width = 130, Height = 30, Top = 9, DialogResult = DialogResult.Cancel, Anchor = AnchorStyles.Top | AnchorStyles.Right };
                Button go = null;
                if (askToContinue)
                {
                    go = new Button { Text = "Vẫn chốt công", Width = 120, Height = 30, Top = 9, DialogResult = DialogResult.OK, Anchor = AnchorStyles.Top | AnchorStyles.Right };
                    bar.Controls.Add(go);
                }
                bar.Controls.Add(export); bar.Controls.Add(close); bar.Controls.Add(lbl);
                int x = f.ClientSize.Width - 12;
                foreach (var b in bar.Controls.OfType<Button>().Reverse()) { x -= b.Width; b.Left = x; x -= 8; }
                export.Click += (s, ev) => Ui.ExportTable(issues, "BatThuongCong_" + M + "_" + Y);
                f.Controls.Add(g); f.Controls.Add(bar);
                f.CancelButton = close;
                var r = f.ShowDialog(this);
                proceed = !askToContinue || r == DialogResult.OK;
            }
            return issues.Rows.Count;
        }

        void ShowIssues(bool askToContinue) { bool p; ShowIssues(askToContinue, out p); }

        void tsbLock_Click(object sender, EventArgs e)
        {
            bool proceed;
            ShowIssues(true, out proceed);
            if (!proceed) return;
            if (!Ui.Confirm("Chốt bảng công tháng " + M + "/" + Y + "?\nSau khi chốt không thể sửa chấm công của tháng này (chỉ người có quyền mới mở khóa được).")) return;
            Run(() => bus.Lock(M, Y));
        }

        void tsbUnlock_Click(object sender, EventArgs e)
        {
            if (!Ui.Confirm("Mở khóa bảng công " + M + "/" + Y + " để chỉnh sửa lại?")) return;
            Run(() => bus.Unlock(M, Y));
        }

        void tsbExport_Click(object sender, EventArgs e)
        {
            if (table == null || table.Rows.Count == 0) { Ui.Warn("Chưa có dữ liệu để xuất."); return; }
            var t = table.Copy();
            if (t.Columns.Contains("id")) t.Columns.Remove("id");
            Ui.ExportTable(t, "BangCong_" + M + "_" + Y);
        }
    }
}