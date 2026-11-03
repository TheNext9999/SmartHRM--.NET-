using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartHRM.BUS;
using SmartHRM.DTO;

namespace SmartHRM.GUI
{
    /// <summary>
    /// Nhật ký hệ thống (Audit Log): ai, làm gì, lúc nào.
    /// Lọc theo ngày / người dùng / mô-đun / thao tác / từ khóa, xem chi tiết, xuất Excel,
    /// tự làm mới và dọn nhật ký cũ (chỉ người có quyền Xóa trên mô-đun này).
    /// </summary>
    public partial class AuditLogForm : Form
    {
        const int MaxRows = 5000;
        const string AllText = "(Tất cả)";
        DataTable table;
        bool loading;

        public AuditLogForm()
        {
            InitializeComponent();
            dtpFrom.Value = DateTime.Today.AddDays(-7);
            dtpTo.Value = DateTime.Today;
            tsbPurge.Visible = tssPurge.Visible = Session.Can(Const.M_AUDIT, "delete");
        }

        // ---------------------------------------------------------------- khởi tạo
        void AuditLogForm_Load(object sender, EventArgs e)
        {
            LoadFilterLists();
            RunSearch();
        }

        static string ModuleName(string code)
        {
            if (string.IsNullOrEmpty(code)) return "";
            return code == "AUTH" ? "Đăng nhập / Đăng xuất" : Modules.TitleOf(code);
        }

        static List<KeyValuePair<string, string>> WithAll(IEnumerable<KeyValuePair<string, string>> items)
        {
            var l = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("", AllText) };
            l.AddRange(items);
            return l;
        }

        static void Bind(ComboBox cbo, List<KeyValuePair<string, string>> data)
        {
            var keep = Convert.ToString(cbo.SelectedValue) ?? "";
            cbo.DisplayMember = "Value"; cbo.ValueMember = "Key";
            cbo.DataSource = data;
            cbo.SelectedValue = data.Any(x => x.Key == keep) ? keep : "";
        }

        void LoadFilterLists()
        {
            loading = true;
            try
            {
                Bind(cboUser, WithAll(AuditBUS.DistinctValues("username").Select(v => new KeyValuePair<string, string>(v, v))));
                Bind(cboAction, WithAll(AuditBUS.DistinctValues("action").Select(v => new KeyValuePair<string, string>(v, v))));
                Bind(cboModule, WithAll(AuditBUS.DistinctValues("module")
                    .Select(v => new KeyValuePair<string, string>(v, ModuleName(v))).OrderBy(x => x.Value)));
            }
            catch (Exception ex) { Ui.Error(ex); }
            finally { loading = false; }
        }

        // ---------------------------------------------------------------- tìm kiếm
        string Sel(ComboBox c) { return Convert.ToString(c.SelectedValue) ?? ""; }

        void RunSearch()
        {
            if (loading) return;
            var from = dtpFrom.Value.Date; var to = dtpTo.Value.Date;
            if (to < from) { Ui.Warn("\"Đến ngày\" phải sau hoặc bằng \"Từ ngày\"."); return; }
            try
            {
                Cursor = Cursors.WaitCursor;
                int? keepId = SelectedId();
                string u = Sel(cboUser), m = Sel(cboModule), a = Sel(cboAction), k = txtKeyword.Text.Trim();
                table = AuditBUS.SearchTable(from, to, u, m, a, k, MaxRows);
                int total = AuditBUS.CountMatching(from, to, u, m, a, k);
                dgvLog.DataSource = table;
                lblCount.Text = "Hiển thị " + table.Rows.Count.ToString("N0") + " / " + total.ToString("N0") + " bản ghi" +
                                (total > MaxRows ? "  (chỉ hiện " + MaxRows.ToString("N0") + " bản ghi mới nhất - hãy thu hẹp bộ lọc)" : "");
                if (keepId.HasValue) ReselectById(keepId.Value);
                ShowDetail();
            }
            catch (Exception ex) { tmrAuto.Stop(); tsbAuto.Checked = false; Ui.Error(ex); }
            finally { Cursor = Cursors.Default; }
        }

        int? SelectedId()
        {
            if (dgvLog.CurrentRow == null) return null;
            var v = dgvLog.CurrentRow.Cells[colId.Index].Value;
            return v == null || v == DBNull.Value ? (int?)null : Convert.ToInt32(v);
        }

        void ReselectById(int id)
        {
            foreach (DataGridViewRow r in dgvLog.Rows)
                if (Convert.ToInt32(r.Cells[colId.Index].Value) == id) { dgvLog.CurrentCell = r.Cells[colTime.Index]; return; }
        }

        void btnSearch_Click(object sender, EventArgs e) { RunSearch(); }

        void txtKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; RunSearch(); }
        }

        void btnQuickRange_Click(object sender, EventArgs e)
        {
            dtpTo.Value = DateTime.Today;
            dtpFrom.Value = sender == btnToday ? DateTime.Today : DateTime.Today.AddDays(sender == btn7Days ? -7 : -30);
            RunSearch();
        }

        void btnReset_Click(object sender, EventArgs e)
        {
            loading = true;
            dtpFrom.Value = DateTime.Today.AddDays(-7); dtpTo.Value = DateTime.Today;
            cboUser.SelectedValue = ""; cboModule.SelectedValue = ""; cboAction.SelectedValue = "";
            txtKeyword.Clear();
            loading = false;
            RunSearch();
        }

        // ---------------------------------------------------------------- hiển thị
        void dgvLog_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvLog.Rows[e.RowIndex];

            if (e.ColumnIndex == colModule.Index && e.Value != null)
            {
                e.Value = ModuleName(Convert.ToString(e.Value));
                e.FormattingApplied = true;
            }

            var act = Convert.ToString(row.Cells[colAction.Index].Value) ?? "";
            if (act.IndexOf("thất bại", StringComparison.OrdinalIgnoreCase) >= 0)
                e.CellStyle.BackColor = Color.FromArgb(255, 220, 220);
            else if (act == "Xóa" || act == "Dọn nhật ký")
                e.CellStyle.BackColor = Color.FromArgb(255, 232, 205);
            else if (act == "Phân quyền" || act == "Đổi mật khẩu")
                e.CellStyle.BackColor = Color.FromArgb(255, 249, 210);
        }

        void dgvLog_SelectionChanged(object sender, EventArgs e) { ShowDetail(); }

        void ShowDetail()
        {
            if (dgvLog.CurrentRow == null || dgvLog.CurrentRow.Index < 0 || dgvLog.CurrentRow.DataBoundItem == null)
            {
                txtDetail.Text = ""; return;
            }
            var r = dgvLog.CurrentRow;
            var time = r.Cells[colTime.Index].Value;
            var detail = Convert.ToString(r.Cells[colDetail.Index].Value) ?? "";
            txtDetail.Text =
                "Thời gian : " + (time is DateTime ? ((DateTime)time).ToString("dddd, dd/MM/yyyy HH:mm:ss") : "") + Environment.NewLine +
                "Người dùng: " + r.Cells[colUser.Index].Value + Environment.NewLine +
                "Thao tác  : " + r.Cells[colAction.Index].Value + Environment.NewLine +
                "Mô-đun    : " + ModuleName(Convert.ToString(r.Cells[colModule.Index].Value)) + Environment.NewLine + Environment.NewLine +
                "Chi tiết:" + Environment.NewLine + detail.Replace("; ", Environment.NewLine);
        }

        // ---------------------------------------------------------------- tiện ích
        void tsbExport_Click(object sender, EventArgs e)
        {
            if (table == null || table.Rows.Count == 0) { Ui.Warn("Không có dữ liệu để xuất."); return; }
            var dt = new DataTable();
            foreach (var c in new[] { "Thời gian", "Người dùng", "Thao tác", "Mô-đun", "Chi tiết" }) dt.Columns.Add(c, c == "Thời gian" ? typeof(DateTime) : typeof(string));
            foreach (DataRow r in table.Rows)
                dt.Rows.Add(r["created_at"], r["username"], r["action"], ModuleName(Convert.ToString(r["module"])), r["detail"]);
            AuditBUS.Log("Xuất nhật ký", Const.M_AUDIT, dt.Rows.Count + " dòng (" + dtpFrom.Value.ToString("dd/MM/yyyy") + " - " + dtpTo.Value.ToString("dd/MM/yyyy") + ")");
            Ui.ExportTable(dt, "NhatKyHeThong");
        }

        void tsbAuto_CheckedChanged(object sender, EventArgs e)
        {
            tmrAuto.Enabled = tsbAuto.Checked;
            tsbAuto.BackColor = tsbAuto.Checked ? Color.FromArgb(205, 230, 255) : SystemColors.Control;
        }

        void tmrAuto_Tick(object sender, EventArgs e)
        {
            dtpTo.Value = DateTime.Today;      // luôn bao gồm hôm nay khi theo dõi trực tiếp
            RunSearch();
        }

        void tsbPurge_Click(object sender, EventArgs e)
        {
            using (var f = new Form
            {
                Text = "Dọn nhật ký cũ",
                ClientSize = new Size(400, 150),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                Font = Font
            })
            {
                var n = new NumericUpDown { Minimum = 30, Maximum = 3650, Value = 180, Left = 215, Top = 20, Width = 80 };
                var ok = new Button { Text = "Xóa nhật ký cũ", Left = 215, Top = 90, Width = 160, Height = 32 };
                f.Controls.AddRange(new Control[]
                {
                    new System.Windows.Forms.Label { Text = "Chỉ giữ lại nhật ký trong (ngày):", Left = 15, Top = 24, AutoSize = true }, n,
                    new System.Windows.Forms.Label { Text = "Tối thiểu 30 ngày. Hành động này không thể hoàn tác.", Left = 15, Top = 56, AutoSize = true, ForeColor = Color.Firebrick },
                    ok
                });
                ok.Click += (s, ev) =>
                {
                    if (!Ui.Confirm("Xóa vĩnh viễn nhật ký cũ hơn " + (int)n.Value + " ngày?")) return;
                    try
                    {
                        int removed = AuditBUS.Purge((int)n.Value);
                        f.Close();
                        Ui.Info("Đã xóa " + removed.ToString("N0") + " bản ghi.");
                    }
                    catch (Exception ex) { Ui.Error(ex); }
                };
                f.ShowDialog(this);
            }
            LoadFilterLists();
            RunSearch();
        }
    }
}