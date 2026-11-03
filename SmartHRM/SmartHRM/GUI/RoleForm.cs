using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartHRM.BUS;
using SmartHRM.DTO;

namespace SmartHRM.GUI
{
    /// <summary>
    /// Quản lý nhóm quyền và ma trận quyền RBAC (Xem / Thêm / Sửa / Xóa) cho từng chức năng.
    /// Có: cột "Tất cả", bấm tiêu đề cột để chọn cả cột, tự ràng buộc (Thêm/Sửa/Xóa kéo theo Xem),
    /// sao chép quyền từ nhóm khác, cảnh báo thay đổi chưa lưu khi chuyển nhóm.
    /// </summary>
    public partial class RoleForm : Form
    {
        readonly RoleBUS bus = new RoleBUS();
        readonly Dictionary<string, int> groupIndex = new Dictionary<string, int>();
        List<RoleDTO> roles = new List<RoleDTO>();
        bool loading, dirty;
        int lastIndex = -1;

        public RoleForm()
        {
            InitializeComponent();
            tsbAddRole.Visible = Session.Can(Const.M_ROLE, "add");
            tsbEditRole.Visible = Session.Can(Const.M_ROLE, "edit");
            tsbDeleteRole.Visible = Session.Can(Const.M_ROLE, "delete");
            foreach (var g in Modules.All.Select(m => m.Group).Distinct()) groupIndex[g] = groupIndex.Count;
        }

        RoleDTO Current() { return lstRoles.SelectedItem as RoleDTO; }
        bool CanEditPerms { get { return Session.Can(Const.M_ROLE, "edit"); } }

        void RoleForm_Load(object sender, EventArgs e) { ReloadRoles(null); }

        // ------------------------------------------------------------ danh sách nhóm
        void ReloadRoles(int? select)
        {
            try { roles = bus.GetAll().OrderBy(r => r.Id).ToList(); }
            catch (Exception ex) { Ui.Error(ex); return; }
            loading = true;
            lstRoles.DataSource = null;
            lstRoles.DisplayMember = "Name";
            lstRoles.DataSource = roles;
            int idx = select.HasValue ? roles.FindIndex(r => r.Id == select.Value) : 0;
            lstRoles.SelectedIndex = idx >= 0 ? idx : (roles.Count > 0 ? 0 : -1);
            lastIndex = lstRoles.SelectedIndex;
            loading = false;
            LoadPermissions();
        }

        void lstRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            if (dirty)
            {
                var prev = lastIndex >= 0 && lastIndex < roles.Count ? roles[lastIndex] : null;
                var r = MessageBox.Show("Nhóm \"" + (prev == null ? "" : prev.Name) + "\" có thay đổi chưa lưu.\n\nCó = lưu rồi chuyển nhóm\nKhông = bỏ thay đổi\nHủy = ở lại nhóm này",
                    Ui.AppTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (r == DialogResult.Cancel) { loading = true; lstRoles.SelectedIndex = lastIndex; loading = false; return; }
                if (r == DialogResult.Yes && prev != null && !SavePermissions(prev)) { loading = true; lstRoles.SelectedIndex = lastIndex; loading = false; return; }
            }
            lastIndex = lstRoles.SelectedIndex;
            LoadPermissions();
        }

        void tsbAddRole_Click(object sender, EventArgs e) { EditRole(new RoleDTO()); }
        void tsbEditRole_Click(object sender, EventArgs e) { var r = Current(); if (r != null) EditRole(Ui.Clone(r)); }

        void EditRole(RoleDTO r)
        {
            if (dirty && !Ui.Confirm("Có thay đổi quyền chưa lưu sẽ bị bỏ khi tải lại danh sách. Tiếp tục?")) return;
            while (true)
            {
                using (var dlg = new EntityEditForm(r, "Nhóm quyền"))
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;
                try { bus.Save(r); break; } catch (Exception ex) { Ui.Error(ex); }
            }
            dirty = false;
            ReloadRoles(r.Id == 0 ? (int?)null : r.Id);
            if (r.Id != 0) return;
            // nhóm mới: chọn dòng cuối (Id lớn nhất)
            loading = true; lstRoles.SelectedIndex = roles.Count - 1; lastIndex = lstRoles.SelectedIndex; loading = false;
            LoadPermissions();
        }

        void tsbDeleteRole_Click(object sender, EventArgs e)
        {
            var r = Current();
            if (r == null || !Ui.Confirm("Xóa nhóm quyền \"" + r.Name + "\"?")) return;
            try { bus.Delete(r); dirty = false; ReloadRoles(null); } catch (Exception ex) { Ui.Error(ex); }
        }

        // ------------------------------------------------------------ ma trận quyền
        void LoadPermissions()
        {
            loading = true;
            dgvPerm.Rows.Clear();
            var r = Current();
            if (r == null) { loading = false; UpdateUi(); return; }

            bool admin = r.Id == 1;
            try
            {
                var perms = bus.GetPermissions(r.Id);
                foreach (var m in Modules.All)
                {
                    var p = perms.First(x => x.ModuleCode == m.Code);
                    dgvPerm.Rows.Add(m.Code, m.Group, m.Title, false, admin || p.CanView, admin || p.CanAdd, admin || p.CanEdit, admin || p.CanDelete);
                }
                foreach (DataGridViewRow row in dgvPerm.Rows) SyncAll(row);
            }
            catch (Exception ex) { Ui.Error(ex); }

            dgvPerm.ReadOnly = admin || !CanEditPerms;
            colCode.ReadOnly = colGroup.ReadOnly = colTitle.ReadOnly = true;
            dirty = false;
            loading = false;
            UpdateUi();
        }

        static bool B(DataGridViewRow row, DataGridViewColumn col) { return row.Cells[col.Index].Value is bool && (bool)row.Cells[col.Index].Value; }

        void SyncAll(DataGridViewRow row)
        {
            row.Cells[colAll.Index].Value = B(row, colView) && B(row, colAdd) && B(row, colEdit) && B(row, colDelete);
        }

        void dgvPerm_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvPerm.IsCurrentCellDirty) dgvPerm.CommitEdit(DataGridViewDataErrorContexts.Commit);   // đổi giá trị ngay khi tick
        }

        void dgvPerm_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (loading || e.RowIndex < 0 || e.ColumnIndex < colAll.Index) return;
            var row = dgvPerm.Rows[e.RowIndex];
            loading = true;
            try
            {
                if (e.ColumnIndex == colAll.Index)
                {
                    bool v = B(row, colAll);
                    foreach (var c in new[] { colView, colAdd, colEdit, colDelete }) row.Cells[c.Index].Value = v;
                }
                else Normalize(row, dgvPerm.Columns[e.ColumnIndex]);
            }
            finally { loading = false; }
            MarkDirty();
        }

        /// <summary>Quy tắc: có Thêm/Sửa/Xóa thì phải có Xem; bỏ Xem thì bỏ luôn Thêm/Sửa/Xóa.</summary>
        void Normalize(DataGridViewRow row, DataGridViewColumn changed)
        {
            if (changed == colView && !B(row, colView))
                foreach (var c in new[] { colAdd, colEdit, colDelete }) row.Cells[c.Index].Value = false;
            else if (changed != colView && changed != colAll && B(row, changed))
                row.Cells[colView.Index].Value = true;
            SyncAll(row);
        }

        void dgvPerm_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvPerm.ReadOnly || e.ColumnIndex < colAll.Index) return;
            var col = dgvPerm.Columns[e.ColumnIndex];
            bool value = !dgvPerm.Rows.Cast<DataGridViewRow>().All(r => B(r, col));      // nếu chưa chọn hết thì chọn hết, ngược lại bỏ hết
            loading = true;
            foreach (DataGridViewRow row in dgvPerm.Rows)
            {
                if (col == colAll) foreach (var c in new[] { colView, colAdd, colEdit, colDelete }) row.Cells[c.Index].Value = value;
                else { row.Cells[col.Index].Value = value; Normalize(row, col); }
                SyncAll(row);
            }
            loading = false;
            MarkDirty();
        }

        void dgvPerm_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var g = Convert.ToString(dgvPerm.Rows[e.RowIndex].Cells[colGroup.Index].Value);
            int gi; groupIndex.TryGetValue(g ?? "", out gi);
            e.CellStyle.BackColor = gi % 2 == 0 ? Color.White : Color.FromArgb(243, 247, 252);     // tô xen kẽ theo nhóm chức năng
            if (dgvPerm.ReadOnly) e.CellStyle.ForeColor = Color.Gray;
        }

        // ------------------------------------------------------------ công cụ nhanh
        void SetAll(bool view, bool add, bool edit, bool del)
        {
            if (dgvPerm.ReadOnly) return;
            loading = true;
            foreach (DataGridViewRow row in dgvPerm.Rows)
            {
                row.Cells[colView.Index].Value = view; row.Cells[colAdd.Index].Value = add;
                row.Cells[colEdit.Index].Value = edit; row.Cells[colDelete.Index].Value = del;
                SyncAll(row);
            }
            loading = false;
            MarkDirty();
        }

        void tsbAll_Click(object sender, EventArgs e) { SetAll(true, true, true, true); }
        void tsbNone_Click(object sender, EventArgs e) { SetAll(false, false, false, false); }
        void tsbViewOnly_Click(object sender, EventArgs e) { SetAll(true, false, false, false); }

        void tsbCopy_Click(object sender, EventArgs e)
        {
            var cur = Current();
            if (cur == null || dgvPerm.ReadOnly) return;
            var others = roles.Where(r => r.Id != cur.Id).ToList();
            if (others.Count == 0) { Ui.Warn("Chưa có nhóm quyền nào khác để sao chép."); return; }
            RoleDTO source;
            using (var f = new Form
            {
                Text = "Sao chép quyền",
                ClientSize = new Size(380, 130),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                Font = Font
            })
            {
                var cbo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Left = 15, Top = 38, Width = 350, DisplayMember = "Name", DataSource = others };
                var ok = new Button { Text = "Sao chép", Left = 265, Top = 80, Width = 100, Height = 32, DialogResult = DialogResult.OK };
                f.Controls.AddRange(new Control[] { new System.Windows.Forms.Label { Text = "Lấy toàn bộ quyền từ nhóm:", Left = 15, Top = 14, AutoSize = true }, cbo, ok });
                f.AcceptButton = ok;
                if (f.ShowDialog(this) != DialogResult.OK) return;
                source = cbo.SelectedItem as RoleDTO;
            }
            if (source == null) return;
            try
            {
                var perms = bus.GetPermissions(source.Id);
                loading = true;
                foreach (DataGridViewRow row in dgvPerm.Rows)
                {
                    var p = perms.First(x => x.ModuleCode == (string)row.Cells[colCode.Index].Value);
                    row.Cells[colView.Index].Value = source.Id == 1 || p.CanView;
                    row.Cells[colAdd.Index].Value = source.Id == 1 || p.CanAdd;
                    row.Cells[colEdit.Index].Value = source.Id == 1 || p.CanEdit;
                    row.Cells[colDelete.Index].Value = source.Id == 1 || p.CanDelete;
                    SyncAll(row);
                }
                loading = false;
                MarkDirty();
            }
            catch (Exception ex) { loading = false; Ui.Error(ex); }
        }

        // ------------------------------------------------------------ lưu / hoàn tác
        void MarkDirty() { dirty = true; UpdateUi(); }

        void tsbReload_Click(object sender, EventArgs e) { if (!dirty || Ui.Confirm("Bỏ các thay đổi chưa lưu?")) LoadPermissions(); }
        void tsbSave_Click(object sender, EventArgs e) { var r = Current(); if (r != null) SavePermissions(r); }

        bool SavePermissions(RoleDTO role)
        {
            var list = new List<RolePermissionDTO>();
            foreach (DataGridViewRow row in dgvPerm.Rows)
                list.Add(new RolePermissionDTO
                {
                    ModuleCode = (string)row.Cells[colCode.Index].Value,
                    CanView = B(row, colView),
                    CanAdd = B(row, colAdd),
                    CanEdit = B(row, colEdit),
                    CanDelete = B(row, colDelete)
                });
            try
            {
                bus.SavePermissions(role.Id, list);
                dirty = false; UpdateUi();
                Ui.Info("Đã lưu phân quyền cho nhóm \"" + role.Name + "\".\nNgười dùng đang đăng nhập cần đăng nhập lại để áp dụng quyền mới.");
                return true;
            }
            catch (Exception ex) { Ui.Error(ex); return false; }
        }

        // ------------------------------------------------------------ cập nhật giao diện
        void UpdateUi()
        {
            var r = Current();
            bool admin = r != null && r.Id == 1;
            bool editable = r != null && !admin && CanEditPerms;

            lblDirty.Visible = dirty;
            tsbSave.Enabled = editable && dirty;
            tsbReload.Enabled = editable && dirty;
            tsbAll.Enabled = tsbNone.Enabled = tsbViewOnly.Enabled = tsbCopy.Enabled = editable;
            tsbEditRole.Enabled = r != null;
            tsbDeleteRole.Enabled = r != null && r.Id > 3;

            if (r == null)
            {
                lblRoleTitle.Text = "Phân quyền"; lblBanner.Text = "Chọn một nhóm quyền ở bên trái."; lblRoleInfo.Text = ""; lblStatus.Text = "";
                return;
            }
            lblRoleTitle.Text = "Nhóm: " + r.Name;
            lblBanner.Text = admin ? "Nhóm Admin luôn có toàn quyền, không thể chỉnh sửa."
                           : !CanEditPerms ? "Bạn chỉ có quyền xem phân quyền."
                           : "Tích chọn quyền được cấp cho nhóm này ở từng chức năng.";
            lblBanner.ForeColor = admin ? Color.DarkOrange : Color.Gray;
            int users = 0;
            try { users = bus.CountUsers(r.Id); } catch { }
            lblRoleInfo.Text = (string.IsNullOrWhiteSpace(r.Description) ? "(chưa có mô tả)" : r.Description) + "\r\n\r\nSố tài khoản thuộc nhóm: " + users;

            var rows = dgvPerm.Rows.Cast<DataGridViewRow>().ToList();
            lblStatus.Text = "Xem: " + rows.Count(x => B(x, colView)) + "/" + rows.Count +
                             "   |   Thêm: " + rows.Count(x => B(x, colAdd)) + "   |   Sửa: " + rows.Count(x => B(x, colEdit)) + "   |   Xóa: " + rows.Count(x => B(x, colDelete));
        }
    }
}