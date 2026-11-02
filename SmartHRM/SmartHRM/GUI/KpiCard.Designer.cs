namespace SmartHRM.GUI
{
    partial class KpiCard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.pnlAccent = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblValue = new System.Windows.Forms.Label();
            this.lblHint = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // pnlAccent
            //
            this.pnlAccent.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.pnlAccent.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlAccent.Name = "pnlAccent";
            this.pnlAccent.Size = new System.Drawing.Size(6, 86);
            this.pnlAccent.TabIndex = 3;
            //
            // lblTitle
            //
            this.lblTitle.AutoEllipsis = true;
            this.lblTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblTitle.Location = new System.Drawing.Point(14, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(182, 20);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Chỉ số";
            //
            // lblValue
            //
            this.lblValue.AutoEllipsis = true;
            this.lblValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblValue.ForeColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.lblValue.Location = new System.Drawing.Point(12, 26);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(184, 34);
            this.lblValue.TabIndex = 1;
            this.lblValue.Text = "0";
            //
            // lblHint
            //
            this.lblHint.AutoEllipsis = true;
            this.lblHint.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblHint.ForeColor = System.Drawing.Color.Gray;
            this.lblHint.Location = new System.Drawing.Point(14, 62);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(182, 18);
            this.lblHint.TabIndex = 2;
            this.lblHint.Text = "";
            //
            // KpiCard
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlAccent);
            this.Name = "KpiCard";
            this.Size = new System.Drawing.Size(206, 88);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlAccent;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Label lblHint;
    }
}