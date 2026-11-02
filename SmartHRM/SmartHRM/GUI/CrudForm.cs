using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using SmartHRM.BUS;
using SmartHRM.DTO;

namespace SmartHRM.GUI
{
    /// <summary>
    /// Form danh sách dùng chung cho mọi danh mục: bảng dữ liệu, thêm/sửa/xóa, tìm nhanh, xuất Excel,
    /// menu chuột phải và các nút nghiệp vụ bổ sung. Cột lấy từ thuộc tính [Field] của DTO.
    /// Dùng: CrudForm.For(new EmployeeBUS(), "Hồ sơ nhân viên")
    /// </summary>
    public partial class CrudForm : Form
    {
        readonly List<Tuple<PropertyInfo, FieldAttribute>> cols = new List<Tuple<PropertyInfo, FieldAttribute>>();
        readonly Dictionary<PropertyInfo, Dictionary<int, string>> lookups = new Dictionary<PropertyInfo, Dictionary<int, string>>();
        ICrudService svc;
        List<BaseDTO> items = new List<BaseDTO>();
        Dictionary<int, BaseDTO> byId = new Dictionary<int, BaseDTO>();
        DataTable table;

        public bool AllowAdd = true, AllowEdit = true, AllowDelete = true, AllowExport = true;

        public static CrudForm For<T>(GenericBUS<T> bus, string title) where T : BaseDTO, new()
        {
            return new CrudForm(new CrudService<T>(bus, title));
        }

        public CrudForm() { InitializeComponent(); }   // dành cho Designer

        public CrudForm(ICrudService service) : this()
        {
            svc = service;
            Text = service.Title;
            foreach (var p in service.EntityType.GetProperties())
            {
                var f = p.GetCustomAttribute<FieldAttribute>();
                if (f != null && f.InGrid && !f.Hidden) cols.Add(Tuple.Create(p, f));
            }
        }

        // ------------------------------------------------------------ nạp & hiển thị
        void CrudForm_Load(object sender, EventArgs e)
        {
            if (svc == null) return;
            ApplyPermissions();
            Reload();
        }

        void ApplyPermissions()
        {
            bool add = AllowAdd && Session.Can(svc.Module, "add");
            bool edit = AllowEdit && Session.Can(svc.Module, "edit");
            bool del = AllowDelete && Session.Can(svc.Module, "delete");
            tsbAdd.Visible = cmsAdd.Visible = add;
            tsbEdit.Visible = cmsEdit.Visible = edit;
            tsbDelete.Visible = cmsDelete.Visible = del;
            tsbExport.Visible = AllowExport;
            lblHint.Text = (edit ? "Nhấp đúp hoặc Enter để sửa  |  " : "") + (del ? "Delete để xóa  |  " : "") + "Chuột phải để mở menu";
        }

        public void Reload()
        {
            if (svc == null) return;
            try
            {
                items = svc.GetAll();
                lookups.Clear();
                foreach (var c in cols.Where(c => c.Item2.Kind == FieldKind.Lookup))
                    lookups[c.Item1] = Ui.Lookup(c.Item2.Source, false).Rows.Cast<DataRow>()
                        .ToDictionary(r => Convert.ToInt32(r["id"]), r => Convert.ToString(r["text"]));
            }
            catch (Exception ex) { Ui.Error(ex); items = new List<BaseDTO>(); }
            byId = items.ToDictionary(i => i.Id);
            Rebind();
        }

        void Rebind()
        {
            int? keep = Selected() == null ? (int?)null : Selected().Id;
            var dt = new DataTable();
            dt.Columns.Add("__id", typeof(int));
            foreach (var c in cols)
            {
                var t = Nullable.GetUnderlyingType(c.Item1.PropertyType) ?? c.Item1.PropertyType;
                if (c.Item2.Kind == FieldKind.Lookup || t == typeof(string)) t = typeof(string);
                else if (t == typeof(int) || t == typeof(long)) t = typeof(decimal);
                var name = c.Item2.Label;
                while (dt.Columns.Contains(name)) name += " ";
                dt.Columns.Add(name, t);
            }
            string q = (tstSearch.Text ?? "").Trim().ToLowerInvariant();
            foreach (var it in items)
            {
                var row = new object[cols.Count + 1];
                row[0] = it.Id;
                var text = new System.Text.StringBuilder();
                for (int i = 0; i < cols.Count; i++)
                {
                    var p = cols[i].Item1; var v = p.GetValue(it);
                    object cell = DBNull.Value;
                    if (v != null)
                    {
                        if (cols[i].Item2.Kind == FieldKind.Lookup) { string s; cell = lookups[p].TryGetValue(Convert.ToInt32(v), out s) ? s : ""; }
                        else if (v is int || v is long) cell = Convert.ToDecimal(v);
                        else cell = v;
                    }
                    row[i + 1] = cell;
                    if (q.Length > 0 && cell != DBNull.Value) text.Append(Convert.ToString(cell)).Append('\t');
                }
                if (q.Length == 0 || text.ToString().ToLowerInvariant().Contains(q)) dt.Rows.Add(row);
            }
            table = dt;
            dgvMain.DataSource = dt;
            dgvMain.Columns["__id"].Visible = false;
            Ui.FormatGrid(dgvMain);
            lblCount.Text = dt.Rows.Count == items.Count ? items.Count.ToString("N0") + " bản ghi"
                                                         : dt.Rows.Count.ToString("N0") + " / " + items.Count.ToString("N0") + " bản ghi (đang lọc)";
            if (keep.HasValue)
                foreach (DataGridViewRow r in dgvMain.Rows)
                    if (Convert.ToInt32(r.Cells["__id"].Value) == keep.Value) { SelectRow(r.Index); break; }
        }

        void SelectRow(int index)
        {
            if (index < 0 || index >= dgvMain.Rows.Count) return;
            var first = dgvMain.Columns.Cast<DataGridViewColumn>().First(c => c.Visible);
            dgvMain.CurrentCell = dgvMain.Rows[index].Cells[first.Index];
            dgvMain.Rows[index].Selected = true;
        }

        BaseDTO Selected()
        {
            if (dgvMain.CurrentRow == null || table == null) return null;
            var v = dgvMain.CurrentRow.Cells["__id"].Value;
            BaseDTO it;
            return v != null && v != DBNull.Value && byId.TryGetValue(Convert.ToInt32(v), out it) ? it : null;
        }

        // ------------------------------------------------------------ nút nghiệp vụ bổ sung
        ToolStripButton AddToolButton(string text, EventHandler click)
        {
            var b = new ToolStripButton(text) { DisplayStyle = ToolStripItemDisplayStyle.Text };
            b.Click += click;
            tsMain.Items.Add(b);
            return b;
        }

        /// <summary>Nút thao tác trên dòng đang chọn (Duyệt, Từ chối, Onboard...).</summary>
        public void AddAction<T>(string text, Action<T> handler, string permission = "edit") where T : BaseDTO
        {
            var b = AddToolButton(text, (s, e) =>
            {
                var item = Selected() as T;
                if (item == null) { Ui.Warn("Hãy chọn một dòng trong danh sách."); return; }
                try { handler(item); } catch (Exception ex) { Ui.Error(ex); }
                Reload();
            });
            b.Visible = Session.Can(svc.Module, permission);
        }

        /// <summary>Nút thao tác không cần chọn dòng (nhập Excel, chấm công...).</summary>
        public void AddToolAction(string text, Action handler, string permission = "view")
        {
            var b = AddToolButton(text, (s, e) =>
            {
                try { handler(); } catch (Exception ex) { Ui.Error(ex); }
                Reload();
            });
            b.Visible = Session.Can(svc.Module, permission);
        }

        // ------------------------------------------------------------ sự kiện giao diện
        void tsbAdd_Click(object sender, EventArgs e)
        {
            try { Editor(svc.CreateNew()); } catch (Exception ex) { Ui.Error(ex); }
        }

        void tsbEdit_Click(object sender, EventArgs e)
        {
            if (!tsbEdit.Visible) return;
            var it = Selected();
            if (it == null) { Ui.Warn("Hãy chọn một dòng cần sửa."); return; }
            Editor((BaseDTO)Ui.CloneObject(it));
        }

        void tsbDelete_Click(object sender, EventArgs e)
        {
            if (!tsbDelete.Visible) return;
            var it = Selected();
            if (it == null) { Ui.Warn("Hãy chọn một dòng cần xóa."); return; }
            if (!Ui.Confirm("Bạn có chắc muốn xóa bản ghi đã chọn?")) return;
            try { svc.Delete(it); Reload(); } catch (Exception ex) { Ui.Error(ex); }
        }

        void tsbRefresh_Click(object sender, EventArgs e) { Reload(); }

        void tsbExport_Click(object sender, EventArgs e)
        {
            if (table == null || table.Rows.Count == 0) { Ui.Warn("Không có dữ liệu để xuất."); return; }
            Ui.ExportTable(table, svc.Title);
        }

        void tstSearch_TextChanged(object sender, EventArgs e) { if (svc != null) Rebind(); }

        void dgvMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) tsbEdit_Click(sender, e);
        }

        void dgvMain_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0) SelectRow(e.RowIndex);   // chuột phải chọn luôn dòng
        }

        void dgvMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) { e.Handled = true; tsbDelete_Click(sender, e); }
            else if (e.KeyCode == Keys.Enter) { e.Handled = true; e.SuppressKeyPress = true; tsbEdit_Click(sender, e); }
        }

        void Editor(BaseDTO work)
        {
            while (true)
            {
                using (var dlg = new EntityEditForm(work, svc.Title))
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;
                try { svc.Save(work); break; }
                catch (Exception ex) { Ui.Error(ex); }       // mở lại hộp thoại để người dùng sửa tiếp
            }
            Reload();
        }
    }
}