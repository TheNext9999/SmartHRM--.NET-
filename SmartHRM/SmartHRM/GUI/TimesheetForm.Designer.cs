namespace SmartHRM.GUI
{
    partial class TimesheetForm
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
            this.tsbCheck = new System.Windows.Forms.ToolStripButton();
            this.tsbLock = new System.Windows.Forms.ToolStripButton();
            this.tsbUnlock = new System.Windows.Forms.ToolStripButton();
            this.tss1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbExport = new System.Windows.Forms.ToolStripButton();
            this.txtSearch = new System.Windows.Forms.ToolStripTextBox();
            this.lblSearch = new System.Windows.Forms.ToolStripLabel();
            this.flpCards = new System.Windows.Forms.FlowLayoutPanel();
            this.cardCount = new SmartHRM.GUI.KpiCard();
            this.cardWork = new SmartHRM.GUI.KpiCard();
            this.cardLate = new SmartHRM.GUI.KpiCard();
            this.cardOt = new SmartHRM.GUI.KpiCard();
            this.dgvSheet = new System.Windows.Forms.DataGridView();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblHint = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMonth)).BeginInit();
            this.tsMain.SuspendLayout();
            this.flpCards.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSheet)).BeginInit();
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
            this.pnlTop.Size = new System.Drawing.Size(1100, 58);
            this.pnlTop.TabIndex = 0;
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.lblTitle.Location = new System.Drawing.Point(14, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "Chốt bảng công";
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
                this.tsbCalc, this.tsbCheck, this.tsbLock, this.tsbUnlock, this.tss1, this.tsbExport, this.txtSearch, this.lblSearch });
            this.tsMain.Name = "tsMain";
            this.tsMain.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tsMain.TabIndex = 1;
            //
            // tsbCalc
            //
            this.tsbCalc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbCalc.Name = "tsbCalc";
            this.tsbCalc.Text = "🧮 Tổng hợp công";
            this.tsbCalc.ToolTipText = "Tổng hợp từ chấm công và đơn từ đã duyệt (tính lại được khi chưa chốt)";
            this.tsbCalc.Click += new System.EventHandler(this.tsbCalc_Click);
            //
            // tsbCheck
            //
            this.tsbCheck.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbCheck.Name = "tsbCheck";
            this.tsbCheck.Text = "⚠ Kiểm tra bất thường";
            this.tsbCheck.ToolTipText = "Thiếu giờ ra, đơn chưa duyệt, nhân viên không có dữ liệu chấm công";
            this.tsbCheck.Click += new System.EventHandler(this.tsbCheck_Click);
            //
            // tsbLock
            //
            this.tsbLock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbLock.Name = "tsbLock";
            this.tsbLock.Text = "🔒 Chốt công";
            this.tsbLock.Click += new System.EventHandler(this.tsbLock_Click);
            //
            // tsbUnlock
            //
            this.tsbUnlock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbUnlock.Name = "tsbUnlock";
            this.tsbUnlock.Text = "🔓 Mở khóa";
            this.tsbUnlock.Click += new System.EventHandler(this.tsbUnlock_Click);
            //
            // tss1
            //
            this.tss1.Name = "tss1";
            //
            // tsbExport
            //
            this.tsbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbExport.Name = "tsbExport";
            this.tsbExport.Text = "📊 Xuất Excel";
            this.tsbExport.Click += new System.EventHandler(this.tsbExport_Click);
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
            this.flpCards.Controls.Add(this.cardWork);
            this.flpCards.Controls.Add(this.cardLate);
            this.flpCards.Controls.Add(this.cardOt);
            this.flpCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpCards.Name = "flpCards";
            this.flpCards.Padding = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.flpCards.Size = new System.Drawing.Size(1100, 104);
            this.flpCards.TabIndex = 2;
            //
            // cardCount
            //
            this.cardCount.AccentColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.cardCount.Margin = new System.Windows.Forms.Padding(0, 0, 12, 8);
            this.cardCount.Name = "cardCount";
            this.cardCount.Size = new System.Drawing.Size(206, 88);
            this.cardCount.TabIndex = 0;
            this.cardCount.Title = "Số nhân viên";
            //
            // cardWork
            //
            this.cardWork.AccentColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.cardWork.Margin = new System.Windows.Forms.Padding(0, 0, 12, 8);
            this.cardWork.Name = "cardWork";
            this.cardWork.Size = new System.Drawing.Size(206, 88);
            this.cardWork.TabIndex = 1;
            this.cardWork.Title = "Tổng công thực tế";
            //
            // cardLate
            //
            this.cardLate.AccentColor = System.Drawing.Color.FromArgb(230, 126, 34);
            this.cardLate.Margin = new System.Windows.Forms.Padding(0, 0, 12, 8);
            this.cardLate.Name = "cardLate";
            this.cardLate.Size = new System.Drawing.Size(206, 88);
            this.cardLate.TabIndex = 2;
            this.cardLate.Title = "Lượt đi muộn";
            //
            // cardOt
            //
            this.cardOt.AccentColor = System.Drawing.Color.FromArgb(142, 68, 173);
            this.cardOt.Margin = new System.Windows.Forms.Padding(0, 0, 12, 8);
            this.cardOt.Name = "cardOt";
            this.cardOt.Size = new System.Drawing.Size(206, 88);
            this.cardOt.TabIndex = 3;
            this.cardOt.Title = "Giờ làm thêm (OT)";
            //
            // dgvSheet
            //
            this.dgvSheet.AllowUserToAddRows = false;
            this.dgvSheet.AllowUserToDeleteRows = false;
            this.dgvSheet.AllowUserToResizeRows = false;
            altStyle.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.dgvSheet.AlternatingRowsDefaultCellStyle = altStyle;
            this.dgvSheet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvSheet.BackgroundColor = System.Drawing.Color.White;
            this.dgvSheet.BorderStyle = System.Windows.Forms.BorderStyle.None;
            headerStyle.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            headerStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            headerStyle.ForeColor = System.Drawing.Color.White;
            headerStyle.SelectionBackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.dgvSheet.ColumnHeadersDefaultCellStyle = headerStyle;
            this.dgvSheet.ColumnHeadersHeight = 32;
            this.dgvSheet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSheet.EnableHeadersVisualStyles = false;
            this.dgvSheet.MultiSelect = false;
            this.dgvSheet.Name = "dgvSheet";
            this.dgvSheet.ReadOnly = true;
            this.dgvSheet.RowHeadersVisible = false;
            this.dgvSheet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSheet.TabIndex = 3;
            this.dgvSheet.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSheet_CellFormatting);
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
            this.lblHint.Text = "Đỏ nhạt: đi muộn từ 3 lần   |   Xanh lá: đã chốt";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // TimesheetForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 640);
            this.Controls.Add(this.dgvSheet);
            this.Controls.Add(this.flpCards);
            this.Controls.Add(this.tsMain);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.statusMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.Name = "TimesheetForm";
            this.Text = "Chốt bảng công";
            this.Load += new System.EventHandler(this.TimesheetForm_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMonth)).EndInit();
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.flpCards.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSheet)).EndInit();
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
        private System.Windows.Forms.ToolStripButton tsbCheck;
        private System.Windows.Forms.ToolStripButton tsbLock;
        private System.Windows.Forms.ToolStripButton tsbUnlock;
        private System.Windows.Forms.ToolStripSeparator tss1;
        private System.Windows.Forms.ToolStripButton tsbExport;
        private System.Windows.Forms.ToolStripTextBox txtSearch;
        private System.Windows.Forms.ToolStripLabel lblSearch;
        private System.Windows.Forms.FlowLayoutPanel flpCards;
        private SmartHRM.GUI.KpiCard cardCount;
        private SmartHRM.GUI.KpiCard cardWork;
        private SmartHRM.GUI.KpiCard cardLate;
        private SmartHRM.GUI.KpiCard cardOt;
        private System.Windows.Forms.DataGridView dgvSheet;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblHint;
    }
}