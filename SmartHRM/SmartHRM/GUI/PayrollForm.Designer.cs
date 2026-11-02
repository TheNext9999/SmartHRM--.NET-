namespace SmartHRM.GUI
{
    partial class PayrollForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle headerStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle altStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblPeriodStatus = new System.Windows.Forms.Label();
            this.btnView = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.nudYear = new System.Windows.Forms.NumericUpDown();
            this.lblYear = new System.Windows.Forms.Label();
            this.nudMonth = new System.Windows.Forms.NumericUpDown();
            this.lblMonth = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbCalc = new System.Windows.Forms.ToolStripButton();
            this.tsbFinalize = new System.Windows.Forms.ToolStripButton();
            this.tsbSend = new System.Windows.Forms.ToolStripButton();
            this.tss1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbPrint = new System.Windows.Forms.ToolStripButton();
            this.tsbExport = new System.Windows.Forms.ToolStripButton();
            this.tsbPreview = new System.Windows.Forms.ToolStripButton();
            this.txtSearch = new System.Windows.Forms.ToolStripTextBox();
            this.lblSearch = new System.Windows.Forms.ToolStripLabel();
            this.flpCards = new System.Windows.Forms.FlowLayoutPanel();
            this.cardCount = new SmartHRM.GUI.KpiCard();
            this.cardGross = new SmartHRM.GUI.KpiCard();
            this.cardInsurance = new SmartHRM.GUI.KpiCard();
            this.cardPit = new SmartHRM.GUI.KpiCard();
            this.cardNet = new SmartHRM.GUI.KpiCard();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.dgvPayroll = new System.Windows.Forms.DataGridView();
            this.wbPreview = new System.Windows.Forms.WebBrowser();
            this.lblPreviewTitle = new System.Windows.Forms.Label();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblHint = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMonth)).BeginInit();
            this.tsMain.SuspendLayout();
            this.flpCards.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayroll)).BeginInit();
            this.statusMain.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.lblPeriodStatus);
            this.pnlTop.Controls.Add(this.btnView);
            this.pnlTop.Controls.Add(this.btnNext);
            this.pnlTop.Controls.Add(this.nudYear);
            this.pnlTop.Controls.Add(this.lblYear);
            this.pnlTop.Controls.Add(this.nudMonth);
            this.pnlTop.Controls.Add(this.lblMonth);
            this.pnlTop.Controls.Add(this.btnPrev);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1200, 58);
            this.pnlTop.TabIndex = 0;
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.lblTitle.Location = new System.Drawing.Point(14, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "Bảng lương";
            //
            // btnPrev
            //
            this.btnPrev.Location = new System.Drawing.Point(300, 14);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(34, 30);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.Text = "◀";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            //
            // lblMonth
            //
            this.lblMonth.AutoSize = true;
            this.lblMonth.Location = new System.Drawing.Point(344, 20);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Text = "Tháng";
            //
            // nudMonth
            //
            this.nudMonth.Location = new System.Drawing.Point(392, 16);
            this.nudMonth.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            this.nudMonth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudMonth.Name = "nudMonth";
            this.nudMonth.Size = new System.Drawing.Size(52, 25);
            this.nudMonth.TabIndex = 1;
            this.nudMonth.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // lblYear
            //
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(456, 20);
            this.lblYear.Name = "lblYear";
            this.lblYear.Text = "Năm";
            //
            // nudYear
            //
            this.nudYear.Location = new System.Drawing.Point(494, 16);
            this.nudYear.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
            this.nudYear.Minimum = new decimal(new int[] { 2000, 0, 0, 0 });
            this.nudYear.Name = "nudYear";
            this.nudYear.Size = new System.Drawing.Size(70, 25);
            this.nudYear.TabIndex = 2;
            this.nudYear.Value = new decimal(new int[] { 2026, 0, 0, 0 });
            //
            // btnNext
            //
            this.btnNext.Location = new System.Drawing.Point(574, 14);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(34, 30);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = "▶";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            //
            // btnView
            //
            this.btnView.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnView.ForeColor = System.Drawing.Color.White;
            this.btnView.Location = new System.Drawing.Point(620, 14);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(80, 30);
            this.btnView.TabIndex = 4;
            this.btnView.Text = "Xem";
            this.btnView.UseVisualStyleBackColor = false;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            //
            // lblPeriodStatus
            //
            this.lblPeriodStatus.AutoSize = true;
            this.lblPeriodStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPeriodStatus.Location = new System.Drawing.Point(720, 20);
            this.lblPeriodStatus.Name = "lblPeriodStatus";
            this.lblPeriodStatus.Text = "";
            //
            // tsMain
            //
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsbCalc, this.tsbFinalize, this.tsbSend, this.tss1, this.tsbPrint, this.tsbExport, this.tsbPreview,
                this.txtSearch, this.lblSearch });
            this.tsMain.Name = "tsMain";
            this.tsMain.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tsMain.TabIndex = 1;
            //
            // tsbCalc
            //
            this.tsbCalc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbCalc.Name = "tsbCalc";
            this.tsbCalc.Text = "🧮 Tính lương";
            this.tsbCalc.ToolTipText = "Tính lương toàn công ty cho kỳ đang chọn (cần chốt bảng công trước)";
            this.tsbCalc.Click += new System.EventHandler(this.tsbCalc_Click);
            //
            // tsbFinalize
            //
            this.tsbFinalize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbFinalize.Name = "tsbFinalize";
            this.tsbFinalize.Text = "🔒 Chốt bảng lương";
            this.tsbFinalize.Click += new System.EventHandler(this.tsbFinalize_Click);
            //
            // tsbSend
            //
            this.tsbSend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbSend.Name = "tsbSend";
            this.tsbSend.Text = "✉ Gửi phiếu lương";
            this.tsbSend.Click += new System.EventHandler(this.tsbSend_Click);
            //
            // tss1
            //
            this.tss1.Name = "tss1";
            //
            // tsbPrint
            //
            this.tsbPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbPrint.Name = "tsbPrint";
            this.tsbPrint.Text = "🖨 In phiếu";
            this.tsbPrint.Click += new System.EventHandler(this.tsbPrint_Click);
            //
            // tsbExport
            //
            this.tsbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbExport.Name = "tsbExport";
            this.tsbExport.Text = "📊 Xuất Excel";
            this.tsbExport.Click += new System.EventHandler(this.tsbExport_Click);
            //
            // tsbPreview
            //
            this.tsbPreview.Checked = true;
            this.tsbPreview.CheckOnClick = true;
            this.tsbPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbPreview.Name = "tsbPreview";
            this.tsbPreview.Text = "👁 Xem trước phiếu";
            this.tsbPreview.CheckedChanged += new System.EventHandler(this.tsbPreview_CheckedChanged);
            //
            // txtSearch
            //
            this.txtSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 27);
            this.txtSearch.ToolTipText = "Lọc theo mã NV, họ tên hoặc phòng ban";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            //
            // lblSearch
            //
            this.lblSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Text = "🔍 Tìm:";
            //
            // flpCards
            //
            this.flpCards.Controls.Add(this.cardCount);
            this.flpCards.Controls.Add(this.cardGross);
            this.flpCards.Controls.Add(this.cardInsurance);
            this.flpCards.Controls.Add(this.cardPit);
            this.flpCards.Controls.Add(this.cardNet);
            this.flpCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpCards.Name = "flpCards";
            this.flpCards.Padding = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.flpCards.Size = new System.Drawing.Size(1200, 104);
            this.flpCards.TabIndex = 2;
            //
            // cardCount
            //
            this.cardCount.AccentColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.cardCount.Margin = new System.Windows.Forms.Padding(0, 0, 12, 8);
            this.cardCount.Name = "cardCount";
            this.cardCount.Size = new System.Drawing.Size(206, 88);
            this.cardCount.TabIndex = 0;
            this.cardCount.Title = "Số phiếu lương";
            //
            // cardGross
            //
            this.cardGross.AccentColor = System.Drawing.Color.FromArgb(0, 150, 170);
            this.cardGross.Margin = new System.Windows.Forms.Padding(0, 0, 12, 8);
            this.cardGross.Name = "cardGross";
            this.cardGross.Size = new System.Drawing.Size(206, 88);
            this.cardGross.TabIndex = 1;
            this.cardGross.Title = "Tổng thu nhập";
            //
            // cardInsurance
            //
            this.cardInsurance.AccentColor = System.Drawing.Color.FromArgb(142, 68, 173);
            this.cardInsurance.Margin = new System.Windows.Forms.Padding(0, 0, 12, 8);
            this.cardInsurance.Name = "cardInsurance";
            this.cardInsurance.Size = new System.Drawing.Size(206, 88);
            this.cardInsurance.TabIndex = 2;
            this.cardInsurance.Title = "Bảo hiểm (NLĐ đóng)";
            //
            // cardPit
            //
            this.cardPit.AccentColor = System.Drawing.Color.FromArgb(230, 126, 34);
            this.cardPit.Margin = new System.Windows.Forms.Padding(0, 0, 12, 8);
            this.cardPit.Name = "cardPit";
            this.cardPit.Size = new System.Drawing.Size(206, 88);
            this.cardPit.TabIndex = 3;
            this.cardPit.Title = "Thuế TNCN";
            //
            // cardNet
            //
            this.cardNet.AccentColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.cardNet.Margin = new System.Windows.Forms.Padding(0, 0, 12, 8);
            this.cardNet.Name = "cardNet";
            this.cardNet.Size = new System.Drawing.Size(206, 88);
            this.cardNet.TabIndex = 4;
            this.cardNet.Title = "Tổng thực lĩnh";
            //
            // splitMain
            //
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Name = "splitMain";
            this.splitMain.Panel1.Controls.Add(this.dgvPayroll);
            this.splitMain.Panel2.Controls.Add(this.wbPreview);
            this.splitMain.Panel2.Controls.Add(this.lblPreviewTitle);
            this.splitMain.Size = new System.Drawing.Size(1200, 440);
            this.splitMain.SplitterDistance = 760;
            this.splitMain.TabIndex = 3;
            //
            // dgvPayroll
            //
            this.dgvPayroll.AllowUserToAddRows = false;
            this.dgvPayroll.AllowUserToDeleteRows = false;
            this.dgvPayroll.AllowUserToResizeRows = false;
            altStyle.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.dgvPayroll.AlternatingRowsDefaultCellStyle = altStyle;
            this.dgvPayroll.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvPayroll.BackgroundColor = System.Drawing.Color.White;
            this.dgvPayroll.BorderStyle = System.Windows.Forms.BorderStyle.None;
            headerStyle.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            headerStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            headerStyle.ForeColor = System.Drawing.Color.White;
            headerStyle.SelectionBackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.dgvPayroll.ColumnHeadersDefaultCellStyle = headerStyle;
            this.dgvPayroll.ColumnHeadersHeight = 32;
            this.dgvPayroll.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPayroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPayroll.EnableHeadersVisualStyles = false;
            this.dgvPayroll.Name = "dgvPayroll";
            this.dgvPayroll.ReadOnly = true;
            this.dgvPayroll.RowHeadersVisible = false;
            this.dgvPayroll.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPayroll.TabIndex = 0;
            this.dgvPayroll.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayroll_CellDoubleClick);
            this.dgvPayroll.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvPayroll_CellFormatting);
            this.dgvPayroll.SelectionChanged += new System.EventHandler(this.dgvPayroll_SelectionChanged);
            //
            // lblPreviewTitle
            //
            this.lblPreviewTitle.BackColor = System.Drawing.Color.FromArgb(236, 242, 249);
            this.lblPreviewTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPreviewTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPreviewTitle.Name = "lblPreviewTitle";
            this.lblPreviewTitle.Padding = new System.Windows.Forms.Padding(8, 6, 0, 0);
            this.lblPreviewTitle.Size = new System.Drawing.Size(436, 28);
            this.lblPreviewTitle.TabIndex = 1;
            this.lblPreviewTitle.Text = "Xem trước phiếu lương";
            //
            // wbPreview
            //
            this.wbPreview.AllowWebBrowserDrop = false;
            this.wbPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbPreview.IsWebBrowserContextMenuEnabled = false;
            this.wbPreview.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbPreview.Name = "wbPreview";
            this.wbPreview.ScriptErrorsSuppressed = true;
            this.wbPreview.TabIndex = 0;
            //
            // statusMain
            //
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.lblStatus, this.lblHint });
            this.statusMain.Name = "statusMain";
            this.statusMain.TabIndex = 4;
            //
            // lblStatus
            //
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Text = "";
            //
            // lblHint
            //
            this.lblHint.ForeColor = System.Drawing.Color.Gray;
            this.lblHint.Name = "lblHint";
            this.lblHint.Spring = true;
            this.lblHint.Text = "Xanh lá: đã chốt   |   Xanh dương: đã gửi email   |   Trắng: nháp";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // PayrollForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.flpCards);
            this.Controls.Add(this.tsMain);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.statusMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.Name = "PayrollForm";
            this.Text = "Bảng lương";
            this.Load += new System.EventHandler(this.PayrollForm_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMonth)).EndInit();
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.flpCards.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayroll)).EndInit();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.NumericUpDown nudMonth;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.NumericUpDown nudYear;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Label lblPeriodStatus;
        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripButton tsbCalc;
        private System.Windows.Forms.ToolStripButton tsbFinalize;
        private System.Windows.Forms.ToolStripButton tsbSend;
        private System.Windows.Forms.ToolStripSeparator tss1;
        private System.Windows.Forms.ToolStripButton tsbPrint;
        private System.Windows.Forms.ToolStripButton tsbExport;
        private System.Windows.Forms.ToolStripButton tsbPreview;
        private System.Windows.Forms.ToolStripTextBox txtSearch;
        private System.Windows.Forms.ToolStripLabel lblSearch;
        private System.Windows.Forms.FlowLayoutPanel flpCards;
        private SmartHRM.GUI.KpiCard cardCount;
        private SmartHRM.GUI.KpiCard cardGross;
        private SmartHRM.GUI.KpiCard cardInsurance;
        private SmartHRM.GUI.KpiCard cardPit;
        private SmartHRM.GUI.KpiCard cardNet;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.DataGridView dgvPayroll;
        private System.Windows.Forms.WebBrowser wbPreview;
        private System.Windows.Forms.Label lblPreviewTitle;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblHint;
    }
}