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
    /// Sơ đồ tổ chức dạng cây (phòng ban - tổ đội): thêm/sửa/xóa đơn vị, kéo thả để đổi cấp trên,
    /// xem thông tin đơn vị và danh sách nhân viên (có thể gồm cả đơn vị con), tìm đơn vị, xuất Excel.
    /// </summary>
    public partial class OrgChartForm : Form
    {
        readonly DepartmentBUS bus = new DepartmentBUS();
        List<DepartmentDTO> all = new List<DepartmentDTO>();
        Dictionary<int, DepartmentDTO> byId = new Dictionary<int, DepartmentDTO>();
        Dictionary<int, int> direct = new Dictionary<int, int>();
        DataTable empTable;
        int findIndex = -1;

        public OrgChartForm()
        {
            InitializeComponent();
            bool add = Session.Can(Const.M_ORG, "add"), edit = Session.Can(Const.M_ORG, "edit"), del = Session.Can(Const.M_ORG, "delete");
            tsbAddRoot.Visible = add; tsbAddChild.Visible = cmsAddChild.Visible = add;
            tsbEdit.Visible = cmsEdit.Visible = edit;
            tsbDelete.Visible = cmsDelete.Visible = del;
            tvOrg.AllowDrop = edit;
            lblDragHint.Visible = edit;
        }

        void OrgChartForm_Load(object sender, EventArgs e) { Reload(); }

        // ------------------------------------------------------------ nạp cây
        DepartmentDTO Selected() { return tvOrg.SelectedNode == null ? null : tvOrg.SelectedNode.Tag as DepartmentDTO; }

        void Reload()
        {
            int? keep = Selected() == null ? (int?)null : Selected().Id;
            try
            {
                all = bus.GetAll();
                direct = bus.CountActive();
            }
            catch (Exception ex) { Ui.Error(ex); return; }
            byId = all.ToDictionary(d => d.Id);

            tvOrg.BeginUpdate();
            tvOrg.Nodes.Clear();
            foreach (var root in all.Where(d => !d.ParentId.HasValue || !byId.ContainsKey(d.ParentId.Value)).OrderBy(d => d.Code))
                tvOrg.Nodes.Add(Build(root, 0));
            tvOrg.ExpandAll();
            tvOrg.EndUpdate();

            var node = keep.HasValue ? FindNode(tvOrg.Nodes, keep.Value) : null;
            if (node != null) tvOrg.SelectedNode = node;
            else if (tvOrg.Nodes.Count > 0) tvOrg.SelectedNode = tvOrg.Nodes[0];
            else ShowInfo();

            lblStatus.Text = all.Count + " đơn vị  |  " + direct.Values.Sum() + " nhân viên đang làm";
        }

        TreeNode Build(DepartmentDTO d, int depth)
        {
            int dir; direct.TryGetValue(d.Id, out dir);
            var n = new TreeNode("[" + d.Code + "] " + d.Name + "   (" + Total(d.Id, 0) + ")") { Tag = d };
            if (!d.ParentId.HasValue) n.NodeFont = new Font(tvOrg.Font, FontStyle.Bold);
            if (dir == 0 && Total(d.Id, 0) == 0) n.ForeColor = Color.Gray;
            if (depth < 30)
                foreach (var c in all.Where(x => x.ParentId == d.Id).OrderBy(x => x.Code)) n.Nodes.Add(Build(c, depth + 1));
            return n;
        }

        /// <summary>Tổng nhân viên của đơn vị và các đơn vị con cháu.</summary>
        int Total(int deptId, int depth)
        {
            int dir; direct.TryGetValue(deptId, out dir);
            if (depth > 30) return dir;
            return dir + all.Where(x => x.ParentId == deptId).Sum(c => Total(c.Id, depth + 1));
        }

        static TreeNode FindNode(TreeNodeCollection nodes, int id)
        {
            foreach (TreeNode n in nodes)
            {
                if (((DepartmentDTO)n.Tag).Id == id) return n;
                var f = FindNode(n.Nodes, id);
                if (f != null) return f;
            }
            return null;
        }

        // ------------------------------------------------------------ thông tin đơn vị
        void tvOrg_AfterSelect(object sender, TreeViewEventArgs e) { ShowInfo(); }
        void chkSub_CheckedChanged(object sender, EventArgs e) { ShowInfo(); }

        void ShowInfo()
        {
            var d = Selected();
            if (d == null)
            {
                lblInfoName.Text = "(chọn một đơn vị)";
                lblInfoCode.Text = lblInfoParent.Text = lblInfoManager.Text = lblInfoCounts.Text = lblInfoDesc.Text = "";
                dgvEmployees.DataSource = null; empTable = null;
                return;
            }
            DepartmentDTO parent = d.ParentId.HasValue && byId.ContainsKey(d.ParentId.Value) ? byId[d.ParentId.Value] : null;
            int dir; direct.TryGetValue(d.Id, out dir);
            int children = all.Count(x => x.ParentId == d.Id);

            lblInfoName.Text = d.Name;
            lblInfoCode.Text = "Mã đơn vị: " + d.Code;
            lblInfoParent.Text = "Trực thuộc: " + (parent == null ? "(đơn vị gốc)" : parent.Name);
            try { lblInfoManager.Text = "Trưởng đơn vị: " + (bus.ManagerName(d) is string m && m.Length > 0 ? m : "(chưa có)"); }
            catch { lblInfoManager.Text = "Trưởng đơn vị: "; }
            lblInfoCounts.Text = "Nhân viên trực tiếp: " + dir + "   |   Gồm đơn vị con: " + Total(d.Id, 0) + "   |   Đơn vị con: " + children;
            lblInfoDesc.Text = string.IsNullOrWhiteSpace(d.Description) ? "" : "Mô tả: " + d.Description;

            try
            {
                empTable = bus.GetEmployeeTable(d.Id, chkSub.Checked);
                dgvEmployees.DataSource = empTable;
                lblGridTitle.Text = "Danh sách nhân viên (" + empTable.Rows.Count + ")";
            }
            catch (Exception ex) { Ui.Error(ex); }
        }

        // ------------------------------------------------------------ thêm / sửa / xóa
        void tsbAddRoot_Click(object sender, EventArgs e) { Edit(new DepartmentDTO()); }

        void tsbAddChild_Click(object sender, EventArgs e)
        {
            var p = Selected();
            Edit(new DepartmentDTO { ParentId = p == null ? (int?)null : p.Id });
        }

        void tsbEdit_Click(object sender, EventArgs e)
        {
            var d = Selected();
            if (d == null) { Ui.Warn("Hãy chọn một đơn vị."); return; }
            Edit(Ui.Clone(d));
        }

        void tsbDelete_Click(object sender, EventArgs e)
        {
            var d = Selected();
            if (d == null) { Ui.Warn("Hãy chọn một đơn vị."); return; }
            if (!Ui.Confirm("Xóa đơn vị \"" + d.Name + "\"?")) return;
            try { bus.Delete(d); Reload(); } catch (Exception ex) { Ui.Error(ex); }
        }

        void Edit(DepartmentDTO d)
        {
            while (true)
            {
                using (var dlg = new EntityEditForm(d, "Đơn vị tổ chức"))
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;
                try { bus.Save(d); break; } catch (Exception ex) { Ui.Error(ex); }
            }
            Reload();
        }

        void tsbRefresh_Click(object sender, EventArgs e) { Reload(); }
        void tsbExpand_Click(object sender, EventArgs e) { tvOrg.ExpandAll(); }
        void tsbCollapse_Click(object sender, EventArgs e) { tvOrg.CollapseAll(); if (tvOrg.Nodes.Count > 0) tvOrg.Nodes[0].Expand(); }
        void cmsExpand_Click(object sender, EventArgs e) { if (tvOrg.SelectedNode != null) tvOrg.SelectedNode.ExpandAll(); }

        void tsbExport_Click(object sender, EventArgs e)
        {
            var d = Selected();
            if (d == null || empTable == null || empTable.Rows.Count == 0) { Ui.Warn("Không có nhân viên để xuất."); return; }
            Ui.ExportTable(empTable, "NhanVien_" + d.Code);
        }

        void tvOrg_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right) tvOrg.SelectedNode = e.Node;     // chuột phải chọn luôn nút
        }

        // ------------------------------------------------------------ tìm đơn vị
        void txtFind_TextChanged(object sender, EventArgs e)
        {
            findIndex = -1;
            Highlight(txtFind.Text.Trim());
            if (txtFind.Text.Trim().Length > 0) NextMatch();
        }

        void txtFind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; NextMatch(); }
        }

        IEnumerable<TreeNode> AllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode n in nodes)
            {
                yield return n;
                foreach (var c in AllNodes(n.Nodes)) yield return c;
            }
        }

        void Highlight(string q)
        {
            foreach (var n in AllNodes(tvOrg.Nodes))
                n.BackColor = q.Length > 0 && n.Text.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0 ? Color.Khaki : Color.Empty;
        }

        void NextMatch()
        {
            var hits = AllNodes(tvOrg.Nodes).Where(n => n.BackColor == Color.Khaki).ToList();
            if (hits.Count == 0) return;
            findIndex = (findIndex + 1) % hits.Count;
            tvOrg.SelectedNode = hits[findIndex];
            hits[findIndex].EnsureVisible();
        }

        // ------------------------------------------------------------ kéo thả đổi cấp trên
        void tvOrg_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (Session.Can(Const.M_ORG, "edit")) DoDragDrop(e.Item, DragDropEffects.Move);
        }

        void tvOrg_DragOver(object sender, DragEventArgs e)
        {
            var dragged = e.Data.GetData(typeof(TreeNode)) as TreeNode;
            if (dragged == null) { e.Effect = DragDropEffects.None; return; }
            var target = tvOrg.GetNodeAt(tvOrg.PointToClient(new Point(e.X, e.Y)));
            var d = (DepartmentDTO)dragged.Tag;
            bool ok = target == null ? d.ParentId.HasValue : target != dragged && ((DepartmentDTO)target.Tag).Id != d.ParentId;
            e.Effect = ok ? DragDropEffects.Move : DragDropEffects.None;
        }

        void tvOrg_DragDrop(object sender, DragEventArgs e)
        {
            var dragged = e.Data.GetData(typeof(TreeNode)) as TreeNode;
            if (dragged == null) return;
            var target = tvOrg.GetNodeAt(tvOrg.PointToClient(new Point(e.X, e.Y)));
            var d = Ui.Clone((DepartmentDTO)dragged.Tag);
            string msg = target == null
                ? "Chuyển \"" + d.Name + "\" thành đơn vị gốc?"
                : "Chuyển \"" + d.Name + "\" về trực thuộc \"" + ((DepartmentDTO)target.Tag).Name + "\"?";
            if (!Ui.Confirm(msg)) return;
            d.ParentId = target == null ? (int?)null : ((DepartmentDTO)target.Tag).Id;
            try { bus.Save(d); Reload(); }
            catch (Exception ex) { Ui.Error(ex); }       // vd: kéo đơn vị vào chính đơn vị con của nó
        }
    }
}