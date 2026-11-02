using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SmartHRM.GUI
{
    /// <summary>Thẻ chỉ số (tiêu đề, giá trị lớn, ghi chú, dải màu) dùng trên Dashboard.</summary>
    public partial class KpiCard : UserControl
    {
        bool clickable;

        public KpiCard()
        {
            InitializeComponent();
            foreach (Control c in Controls) c.Click += (s, e) => OnClick(e);   // bấm vào bất kỳ chỗ nào của thẻ
        }

        [Category("SmartHRM"), DefaultValue("Chỉ số")]
        public string Title { get { return lblTitle.Text; } set { lblTitle.Text = value; } }

        [Category("SmartHRM"), DefaultValue("0")]
        public string Value { get { return lblValue.Text; } set { lblValue.Text = value; } }

        [Category("SmartHRM"), DefaultValue("")]
        public string Hint { get { return lblHint.Text; } set { lblHint.Text = value; } }

        [Category("SmartHRM")]
        public Color AccentColor
        {
            get { return pnlAccent.BackColor; }
            set { pnlAccent.BackColor = value; lblValue.ForeColor = value; }
        }
        bool ShouldSerializeAccentColor() { return AccentColor != Color.FromArgb(31, 78, 121); }
        void ResetAccentColor() { AccentColor = Color.FromArgb(31, 78, 121); }

        /// <summary>Hiện con trỏ bàn tay khi thẻ có thể bấm để mở chức năng liên quan.</summary>
        [Category("SmartHRM"), DefaultValue(false)]
        public bool Clickable
        {
            get { return clickable; }
            set
            {
                clickable = value;
                var cur = value ? Cursors.Hand : Cursors.Default;
                Cursor = cur;
                foreach (Control c in Controls) c.Cursor = cur;
            }
        }
    }
}