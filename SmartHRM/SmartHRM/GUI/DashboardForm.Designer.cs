namespace SmartHRM.GUI
{
    partial class DashboardForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea area0 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea area1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea area2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea area3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.chkAuto = new System.Windows.Forms.CheckBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblUpdated = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.flpCards = new System.Windows.Forms.FlowLayoutPanel();
            this.cardActive = new SmartHRM.GUI.KpiCard();
            this.cardPresent = new SmartHRM.GUI.KpiCard();
            this.cardNew = new SmartHRM.GUI.KpiCard();
            this.cardLeft = new SmartHRM.GUI.KpiCard();
            this.cardProbation = new SmartHRM.GUI.KpiCard();
            this.cardContracts = new SmartHRM.GUI.KpiCard();
            this.cardLeave = new SmartHRM.GUI.KpiCard();
            this.cardCandidates = new SmartHRM.GUI.KpiCard();
            this.cardNet = new SmartHRM.GUI.KpiCard();
            this.tlpCharts = new System.Windows.Forms.TableLayoutPanel();
            this.chartDept = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartCost = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartGender = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartTurnover = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.pnlHeader.SuspendLayout();
            this.flpCards.SuspendLayout();
            this.tlpCharts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartGender)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTurnover)).BeginInit();
            this.SuspendLayout();
            //
            // pnlHeader
            //
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.chkAuto);
            this.pnlHeader.Controls.Add(this.btnRefresh);
            this.pnlHeader.Controls.Add(this.lblUpdated);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1180, 60);
            this.pnlHeader.TabIndex = 0;
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.lblTitle.Location = new System.Drawing.Point(14, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "Tổng quan nhân sự";
            //
            // lblUpdated
            //
            this.lblUpdated.AutoSize = true;
            this.lblUpdated.ForeColor = System.Drawing.Color.Gray;
            this.lblUpdated.Location = new System.Drawing.Point(17, 38);
            this.lblUpdated.Name = "lblUpdated";
            this.lblUpdated.Text = "Chưa cập nhật";
            //
            // btnRefresh
            //
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(1060, 14);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(108, 32);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "🔄 Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // chkAuto
            //
            this.chkAuto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAuto.AutoSize = true;
            this.chkAuto.Checked = true;
            this.chkAuto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAuto.Location = new System.Drawing.Point(880, 20);
            this.chkAuto.Name = "chkAuto";
            this.chkAuto.TabIndex = 2;
            this.chkAuto.Text = "Tự làm mới (30 giây)";
            this.chkAuto.UseVisualStyleBackColor = true;
            this.chkAuto.CheckedChanged += new System.EventHandler(this.chkAuto_CheckedChanged);
            //
            // flpCards
            //
            this.flpCards.AutoScroll = true;
            this.flpCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpCards.Name = "flpCards";
            this.flpCards.Padding = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.flpCards.Size = new System.Drawing.Size(1180, 212);
            this.flpCards.TabIndex = 1;
            this.flpCards.Controls.Add(this.cardActive);
            this.flpCards.Controls.Add(this.cardPresent);
            this.flpCards.Controls.Add(this.cardNew);
            this.flpCards.Controls.Add(this.cardLeft);
            this.flpCards.Controls.Add(this.cardProbation);
            this.flpCards.Controls.Add(this.cardContracts);
            this.flpCards.Controls.Add(this.cardLeave);
            this.flpCards.Controls.Add(this.cardCandidates);
            this.flpCards.Controls.Add(this.cardNet);
            //
            // cardActive
            //
            this.cardActive.AccentColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.cardActive.Clickable = true;
            this.cardActive.Margin = new System.Windows.Forms.Padding(0, 0, 12, 12);
            this.cardActive.Name = "cardActive";
            this.cardActive.Size = new System.Drawing.Size(206, 88);
            this.cardActive.TabIndex = 0;
            this.cardActive.Tag = "EMPLOYEE";
            this.cardActive.Title = "Nhân viên đang làm";
            this.cardActive.Click += new System.EventHandler(this.card_Click);
            //
            // cardPresent
            //
            this.cardPresent.AccentColor = System.Drawing.Color.FromArgb(46, 139, 87);
            this.cardPresent.Clickable = true;
            this.cardPresent.Margin = new System.Windows.Forms.Padding(0, 0, 12, 12);
            this.cardPresent.Name = "cardPresent";
            this.cardPresent.Size = new System.Drawing.Size(206, 88);
            this.cardPresent.TabIndex = 1;
            this.cardPresent.Tag = "ATTENDANCE";
            this.cardPresent.Title = "Có mặt hôm nay";
            this.cardPresent.Click += new System.EventHandler(this.card_Click);
            //
            // cardNew
            //
            this.cardNew.AccentColor = System.Drawing.Color.FromArgb(0, 150, 170);
            this.cardNew.Clickable = true;
            this.cardNew.Margin = new System.Windows.Forms.Padding(0, 0, 12, 12);
            this.cardNew.Name = "cardNew";
            this.cardNew.Size = new System.Drawing.Size(206, 88);
            this.cardNew.TabIndex = 2;
            this.cardNew.Tag = "REPORT";
            this.cardNew.Title = "Tuyển mới tháng này";
            this.cardNew.Click += new System.EventHandler(this.card_Click);
            //
            // cardLeft
            //
            this.cardLeft.AccentColor = System.Drawing.Color.FromArgb(192, 57, 43);
            this.cardLeft.Clickable = true;
            this.cardLeft.Margin = new System.Windows.Forms.Padding(0, 0, 12, 12);
            this.cardLeft.Name = "cardLeft";
            this.cardLeft.Size = new System.Drawing.Size(206, 88);
            this.cardLeft.TabIndex = 3;
            this.cardLeft.Tag = "REPORT";
            this.cardLeft.Title = "Nghỉ việc tháng này";
            this.cardLeft.Click += new System.EventHandler(this.card_Click);
            //
            // cardProbation
            //
            this.cardProbation.AccentColor = System.Drawing.Color.FromArgb(142, 68, 173);
            this.cardProbation.Clickable = true;
            this.cardProbation.Margin = new System.Windows.Forms.Padding(0, 0, 12, 12);
            this.cardProbation.Name = "cardProbation";
            this.cardProbation.Size = new System.Drawing.Size(206, 88);
            this.cardProbation.TabIndex = 4;
            this.cardProbation.Tag = "EMPLOYEE";
            this.cardProbation.Title = "Đang thử việc";
            this.cardProbation.Click += new System.EventHandler(this.card_Click);
            //
            // cardContracts
            //
            this.cardContracts.AccentColor = System.Drawing.Color.FromArgb(230, 126, 34);
            this.cardContracts.Clickable = true;
            this.cardContracts.Margin = new System.Windows.Forms.Padding(0, 0, 12, 12);
            this.cardContracts.Name = "cardContracts";
            this.cardContracts.Size = new System.Drawing.Size(206, 88);
            this.cardContracts.TabIndex = 5;
            this.cardContracts.Tag = "CONTRACT";
            this.cardContracts.Title = "HĐ sắp hết hạn";
            this.cardContracts.Click += new System.EventHandler(this.card_Click);
            //
            // cardLeave
            //
            this.cardLeave.AccentColor = System.Drawing.Color.FromArgb(230, 126, 34);
            this.cardLeave.Clickable = true;
            this.cardLeave.Margin = new System.Windows.Forms.Padding(0, 0, 12, 12);
            this.cardLeave.Name = "cardLeave";
            this.cardLeave.Size = new System.Drawing.Size(206, 88);
            this.cardLeave.TabIndex = 6;
            this.cardLeave.Tag = "LEAVE";
            this.cardLeave.Title = "Đơn chờ duyệt";
            this.cardLeave.Click += new System.EventHandler(this.card_Click);
            //
            // cardCandidates
            //
            this.cardCandidates.AccentColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.cardCandidates.Clickable = true;
            this.cardCandidates.Margin = new System.Windows.Forms.Padding(0, 0, 12, 12);
            this.cardCandidates.Name = "cardCandidates";
            this.cardCandidates.Size = new System.Drawing.Size(206, 88);
            this.cardCandidates.TabIndex = 7;
            this.cardCandidates.Tag = "CANDIDATE";
            this.cardCandidates.Title = "Ứng viên đang xử lý";
            this.cardCandidates.Click += new System.EventHandler(this.card_Click);
            //
            // cardNet
            //
            this.cardNet.AccentColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.cardNet.Clickable = true;
            this.cardNet.Margin = new System.Windows.Forms.Padding(0, 0, 12, 12);
            this.cardNet.Name = "cardNet";
            this.cardNet.Size = new System.Drawing.Size(206, 88);
            this.cardNet.TabIndex = 8;
            this.cardNet.Tag = "PAYROLL";
            this.cardNet.Title = "Thực lĩnh kỳ gần nhất";
            this.cardNet.Click += new System.EventHandler(this.card_Click);
            //
            // tlpCharts
            //
            this.tlpCharts.ColumnCount = 2;
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCharts.Controls.Add(this.chartDept, 0, 0);
            this.tlpCharts.Controls.Add(this.chartCost, 1, 0);
            this.tlpCharts.Controls.Add(this.chartGender, 0, 1);
            this.tlpCharts.Controls.Add(this.chartTurnover, 1, 1);
            this.tlpCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCharts.Name = "tlpCharts";
            this.tlpCharts.Padding = new System.Windows.Forms.Padding(8, 0, 8, 8);
            this.tlpCharts.RowCount = 2;
            this.tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCharts.TabIndex = 2;
            //
            // chartDept
            //
            area0.Name = "ChartArea0";
            this.chartDept.ChartAreas.Add(area0);
            this.chartDept.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartDept.Name = "chartDept";
            this.chartDept.TabIndex = 0;
            this.chartDept.Text = "chartDept";
            //
            // chartCost
            //
            area1.Name = "ChartArea1";
            this.chartCost.ChartAreas.Add(area1);
            this.chartCost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartCost.Name = "chartCost";
            this.chartCost.TabIndex = 1;
            this.chartCost.Text = "chartCost";
            //
            // chartGender
            //
            area2.Name = "ChartArea2";
            this.chartGender.ChartAreas.Add(area2);
            this.chartGender.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartGender.Name = "chartGender";
            this.chartGender.TabIndex = 2;
            this.chartGender.Text = "chartGender";
            //
            // chartTurnover
            //
            area3.Name = "ChartArea3";
            this.chartTurnover.ChartAreas.Add(area3);
            this.chartTurnover.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartTurnover.Name = "chartTurnover";
            this.chartTurnover.TabIndex = 3;
            this.chartTurnover.Text = "chartTurnover";
            //
            // tmrRefresh
            //
            this.tmrRefresh.Interval = 30000;
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            //
            // DashboardForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(244, 247, 251);
            this.ClientSize = new System.Drawing.Size(1180, 720);
            this.Controls.Add(this.tlpCharts);
            this.Controls.Add(this.flpCards);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.Name = "DashboardForm";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.DashboardForm_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.flpCards.ResumeLayout(false);
            this.tlpCharts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartGender)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTurnover)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUpdated;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.CheckBox chkAuto;
        private System.Windows.Forms.FlowLayoutPanel flpCards;
        private SmartHRM.GUI.KpiCard cardActive;
        private SmartHRM.GUI.KpiCard cardPresent;
        private SmartHRM.GUI.KpiCard cardNew;
        private SmartHRM.GUI.KpiCard cardLeft;
        private SmartHRM.GUI.KpiCard cardProbation;
        private SmartHRM.GUI.KpiCard cardContracts;
        private SmartHRM.GUI.KpiCard cardLeave;
        private SmartHRM.GUI.KpiCard cardCandidates;
        private SmartHRM.GUI.KpiCard cardNet;
        private System.Windows.Forms.TableLayoutPanel tlpCharts;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDept;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCost;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartGender;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTurnover;
        private System.Windows.Forms.Timer tmrRefresh;
    }
}