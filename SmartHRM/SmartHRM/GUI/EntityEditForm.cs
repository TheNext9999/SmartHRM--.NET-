using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using SmartHRM.DTO;

namespace SmartHRM.GUI
{
    /// <summary>
    /// Hộp thoại thêm / sửa. Khung (tiêu đề, nút Lưu/Hủy, vùng cuộn) thiết kế trong Designer;
    /// các ô nhập được dựng tự động từ thuộc tính [Field] của DTO (text, số, ngày, danh mục, tick...).
    /// </summary>
    public partial class EntityEditForm : Form
    {
        class Item { public PropertyInfo Prop; public FieldAttribute Attr; public Control Ctl; }

        readonly List<Item> items = new List<Item>();
        BaseDTO entity;

        public EntityEditForm() { InitializeComponent(); }   // dành cho Designer

        public EntityEditForm(BaseDTO entity, string title) : this()
        {
            this.entity = entity;
            bool isNew = entity.Id == 0;
            Text = (isNew ? "Thêm mới - " : "Cập nhật - ") + title;
            lblHeader.Text = (isNew ? "Thêm mới" : "Cập nhật") + ": " + title;
            lblSub.Text = isNew ? "Nhập thông tin rồi bấm Lưu" : "Mã bản ghi: " + entity.Id;

            AcceptButton = null;     // Enter không tự lưu (tránh lưu nhầm khi đang nhập)
            BuildFields();

            int h = Math.Min(items.Count * 38 + 58 + 56 + 36, Screen.PrimaryScreen.WorkingArea.Height - 80);
            ClientSize = new Size(580, Math.Max(h, 240));
        }

        // ------------------------------------------------------------ dựng ô nhập
        void BuildFields()
        {
            tblFields.SuspendLayout();
            foreach (var p in entity.GetType().GetProperties())
            {
                var f = p.GetCustomAttribute<FieldAttribute>();
                if (f == null || f.Hidden) continue;

                var ctl = CreateControl(f);
                var lbl = new Label
                {
                    Text = f.Label + (f.Required ? " *" : ""),
                    AutoSize = true,
                    Anchor = AnchorStyles.Left | AnchorStyles.Top,
                    Margin = new Padding(0, 8, 6, 0)
                };
                if (f.Required) lbl.ForeColor = Color.FromArgb(160, 30, 30);

                Control cell = ctl;
                if (f.Kind == FieldKind.File)
                {
                    var pnl = new Panel { Height = 27 };
                    var btn = new Button { Text = "...", Width = 34, Dock = DockStyle.Right };
                    var tb = (TextBox)ctl; tb.Dock = DockStyle.Fill;
                    btn.Click += (s, e) => { using (var d = new OpenFileDialog()) if (d.ShowDialog() == DialogResult.OK) tb.Text = d.FileName; };
                    pnl.Controls.Add(tb); pnl.Controls.Add(btn);
                    cell = pnl;
                }
                ctl.Enabled = !f.ReadOnly;
                cell.Dock = DockStyle.Fill; cell.Margin = new Padding(0, 3, 0, 3);

                tblFields.RowCount++;
                tblFields.Controls.Add(lbl, 0, tblFields.RowCount - 1);
                tblFields.Controls.Add(cell, 1, tblFields.RowCount - 1);

                var it = new Item { Prop = p, Attr = f, Ctl = ctl };
                items.Add(it);
                SetValue(it, p.GetValue(entity));
            }
            tblFields.ResumeLayout();
            lblRequired.Visible = items.Any(i => i.Attr.Required);
        }

        static Control CreateControl(FieldAttribute f)
        {
            switch (f.Kind)
            {
                case FieldKind.MultiLine: return new TextBox { Multiline = true, AcceptsReturn = true, Height = 66, ScrollBars = ScrollBars.Vertical };
                case FieldKind.Password: return new TextBox { UseSystemPasswordChar = true };
                case FieldKind.Integer: return new NumericUpDown { Maximum = 1000000000, Minimum = -1000000000, ThousandsSeparator = true };
                case FieldKind.Decimal: return new NumericUpDown { Maximum = 100000000000m, Minimum = -100000000000m, DecimalPlaces = 2, ThousandsSeparator = true };
                case FieldKind.Date: return new DateTimePicker { Format = DateTimePickerFormat.Short, ShowCheckBox = !f.Required };
                case FieldKind.DateAndTime: return new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm", ShowCheckBox = true };
                case FieldKind.Time: return new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "HH:mm", ShowUpDown = true };
                case FieldKind.Bool: return new CheckBox();
                case FieldKind.Combo:
                    {
                        var cb = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
                        if (!f.Required) cb.Items.Add("");
                        cb.Items.AddRange(f.Source.Split('|'));
                        return cb;
                    }
                case FieldKind.Lookup:
                    {
                        var cb = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
                        cb.DataSource = Ui.Lookup(f.Source, true);
                        cb.DisplayMember = "text"; cb.ValueMember = "id";
                        return cb;
                    }
                default: return new TextBox();   // Text, File
            }
        }

        // ------------------------------------------------------------ đọc / ghi giá trị
        void SetValue(Item it, object v)
        {
            var c = it.Ctl;
            switch (it.Attr.Kind)
            {
                case FieldKind.Integer:
                case FieldKind.Decimal:
                    {
                        var n = (NumericUpDown)c;
                        n.Value = v == null ? 0 : Math.Max(n.Minimum, Math.Min(n.Maximum, Convert.ToDecimal(v))); break;
                    }
                case FieldKind.Date:
                case FieldKind.DateAndTime:
                    {
                        var d = (DateTimePicker)c;
                        var has = v != null && (DateTime)v != DateTime.MinValue;
                        d.Value = has ? (DateTime)v : (it.Attr.Kind == FieldKind.Date ? DateTime.Today : DateTime.Now);
                        if (d.ShowCheckBox) d.Checked = has;
                        break;
                    }
                case FieldKind.Time:
                    {
                        TimeSpan t; TimeSpan.TryParse(Convert.ToString(v), out t);
                        ((DateTimePicker)c).Value = DateTime.Today + t; break;
                    }
                case FieldKind.Bool: ((CheckBox)c).Checked = v != null && (bool)v; break;
                case FieldKind.Combo:
                    {
                        var cb = (ComboBox)c; var s = Convert.ToString(v) ?? "";
                        int i = cb.Items.IndexOf(s); cb.SelectedIndex = i >= 0 ? i : (cb.Items.Count > 0 ? 0 : -1); break;
                    }
                case FieldKind.Lookup:
                    {
                        var cb = (ComboBox)c;
                        if (v == null || (v is int && (int)v == 0)) cb.SelectedIndex = 0; else cb.SelectedValue = Convert.ToInt32(v);
                        break;
                    }
                default: c.Text = Convert.ToString(v) ?? ""; break;
            }
        }

        static bool IsEmpty(Item it)
        {
            var c = it.Ctl;
            switch (it.Attr.Kind)
            {
                case FieldKind.Lookup: { var sv = ((ComboBox)c).SelectedValue; return sv == null || sv == DBNull.Value; }
                case FieldKind.Combo: return string.IsNullOrEmpty(Convert.ToString(((ComboBox)c).SelectedItem));
                case FieldKind.Date:
                case FieldKind.DateAndTime: { var d = (DateTimePicker)c; return d.ShowCheckBox && !d.Checked; }
                case FieldKind.Integer:
                case FieldKind.Decimal:
                case FieldKind.Bool:
                case FieldKind.Time: return false;
                default: return string.IsNullOrWhiteSpace(c.Text);
            }
        }

        void Collect()
        {
            foreach (var it in items)
            {
                if (it.Attr.ReadOnly) continue;
                object v;
                var c = it.Ctl;
                var type = Nullable.GetUnderlyingType(it.Prop.PropertyType) ?? it.Prop.PropertyType;
                switch (it.Attr.Kind)
                {
                    case FieldKind.Integer:
                    case FieldKind.Decimal: v = Convert.ChangeType(((NumericUpDown)c).Value, type); break;
                    case FieldKind.Date:
                    case FieldKind.DateAndTime:
                        {
                            var d = (DateTimePicker)c;
                            v = d.ShowCheckBox && !d.Checked ? null :
                                (object)(it.Attr.Kind == FieldKind.Date ? d.Value.Date : new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, d.Value.Hour, d.Value.Minute, 0));
                            break;
                        }
                    case FieldKind.Time: v = ((DateTimePicker)c).Value.ToString("HH:mm"); break;
                    case FieldKind.Bool: v = ((CheckBox)c).Checked; break;
                    case FieldKind.Combo: { var s = Convert.ToString(((ComboBox)c).SelectedItem); v = string.IsNullOrEmpty(s) ? null : s; break; }
                    case FieldKind.Lookup:
                        {
                            var sv = ((ComboBox)c).SelectedValue;
                            v = sv == null || sv == DBNull.Value ? null : (object)Convert.ToInt32(sv); break;
                        }
                    case FieldKind.Password: v = string.IsNullOrEmpty(c.Text) ? null : c.Text; break;
                    default: v = string.IsNullOrWhiteSpace(c.Text) ? null : c.Text.Trim(); break;
                }
                if (v == null && it.Prop.PropertyType.IsValueType && Nullable.GetUnderlyingType(it.Prop.PropertyType) == null)
                    v = Activator.CreateInstance(it.Prop.PropertyType);
                it.Prop.SetValue(entity, v);
            }
        }

        // ------------------------------------------------------------ lưu
        void btnSave_Click(object sender, EventArgs e)
        {
            Item first = null;
            foreach (var it in items)
            {
                bool bad = it.Attr.Required && !it.Attr.ReadOnly && IsEmpty(it);
                epMain.SetError(it.Ctl, bad ? "\"" + it.Attr.Label + "\" là bắt buộc" : "");
                if (bad && first == null) first = it;
            }
            if (first != null) { first.Ctl.Focus(); return; }
            try { Collect(); }
            catch (Exception ex) { Ui.Error(ex); return; }
            DialogResult = DialogResult.OK;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S)) { btnSave_Click(this, EventArgs.Empty); return true; }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}