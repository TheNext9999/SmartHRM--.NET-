namespace SmartHRM.GUI
{
    partial class ReportForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.tvReports = new System.Windows.Forms.TreeView();
            this.lblReportsTitle = new System.Windows.Forms.Label();
            this.splitResult = new System.Windows.Forms.SplitContainer();
            this.chartMain = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.lblTotals = new System.Windows.Forms.Label();
            this.pnlParams = new System.Windows.Forms.Panel();
            this.btnSaveImage = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.chkLabels = new System.Windows.Forms.CheckBox();
            this.cboChartType = new System.Windows.Forms.ComboBox();
            this.lblChartType = new System.Windows.Forms.Label();
            this.nudYear = new System.Windows.Forms.NumericUpDown();
            this.lblYear = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblReportDesc = new System.Windows.Forms.Label();
            this.lblReportTitle = new System.Windows.Forms.Label();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitResult)).BeginInit();
            this.splitResult.Panel1.SuspendLayout();
            this.splitResult.Panel2.SuspendLayout();
            this.splitResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.pnlParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudYear)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.statusMain.SuspendLayout();
            this.SuspendLayout();
            //
            // splitMain
            //
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Name = "splitMain";
            this.splitMain.Panel1.Controls.Add(this.tvReports);
            this.splitMain.Panel1.Controls.Add(this.lblReportsTitle);
            this.splitMain.Panel2.Controls.Add(this.splitResult);
            this.splitMain.Panel2.Controls.Add(this.pnlParams);
            this.splitMain.Panel2.Controls.Add(this.pnlHeader);
            this.splitMain.Size = new System.Drawing.Size(1180, 678);
            this.splitMain.SplitterDistance = 290;
            this.splitMain.TabIndex = 0;
            //
            // lblReportsTitle
            //
            this.lblReportsTitle.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.lblReportsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblReportsTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblReportsTitle.ForeColor = System.Drawing.Color.White;
            this.lblReportsTitle.Name = "lblReportsTitle";
            this.lblReportsTitle.Padding = new System.Windows.Forms.Padding(8, 6, 0, 0);
            this.lblReportsTitle.Size = new System.Drawing.Size(290, 30);
            this.lblReportsTitle.TabIndex = 1;
            this.lblReportsTitle.Text = "Danh mục báo cáo";
            //
            // tvReports
            //
            this.tvReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvReports.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tvReports.FullRowSelect = true;
            this.tvReports.HideSelection = false;
            this.tvReports.ItemHeight = 28;
            this.tvReports.Name = "tvReports";
            this.tvReports.ShowLines = false;
            this.tvReports.ShowRootLines = false;
            this.tvReports.TabIndex = 0;
            this.tvReports.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvReports_AfterSelect);
            //
            // pnlHeader
            //
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.lblReportDesc);
            this.pnlHeader.Controls.Add(this.lblReportTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(886, 64);
            this.pnlHeader.TabIndex = 2;
            //
            // lblReportTitle
            //
            this.lblReportTitle.AutoSize = true;
            this.lblReportTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblReportTitle.ForeColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.lblReportTitle.Location = new System.Drawing.Point(12, 6);
            this.lblReportTitle.Name = "lblReportTitle";
            this.lblReportTitle.Text = "Báo cáo";
            //
            // lblReportDesc
            //
            this.lblReportDesc.AutoSize = true;
            this.lblReportDesc.ForeColor = System.Drawing.Color.Gray;
            this.lblReportDesc.Location = new System.Drawing.Point(14, 38);
            this.lblReportDesc.Name = "lblReportDesc";
            this.lblReportDesc.Text = "";
            //
            // pnlParams
            //
            this.pnlParams.BackColor = System.Drawing.Color.FromArgb(236, 242, 249);
            this.pnlParams.Controls.Add(this.btnSaveImage);
            this.pnlParams.Controls.Add(this.btnExport);
            this.pnlParams.Controls.Add(this.btnView);
            this.pnlParams.Controls.Add(this.chkLabels);
            this.pnlParams.Controls.Add(this.cboChartType);
            this.pnlParams.Controls.Add(this.lblChartType);
            this.pnlParams.Controls.Add(this.nudYear);
            this.pnlParams.Controls.Add(this.lblYear);
            this.pnlParams.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(886, 48);
            this.pnlParams.TabIndex = 1;
            //
            // lblYear
            //
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(12, 14);
            this.lblYear.Name = "lblYear";
            this.lblYear.Text = "Năm";
            //
            // nudYear
            //
            this.nudYear.Location = new System.Drawing.Point(50, 10);
            this.nudYear.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
            this.nudYear.Minimum = new decimal(new int[] { 2000, 0, 0, 0 });
            this.nudYear.Name = "nudYear";
            this.nudYear.Size = new System.Drawing.Size(70, 25);
            this.nudYear.TabIndex = 0;
            this.nudYear.Value = new decimal(new int[] { 2026, 0, 0, 0 });
            //
            // lblChartType
            //
            this.lblChartType.AutoSize = true;
            this.lblChartType.Location = new System.Drawing.Point(136, 14);
            this.lblChartType.Name = "lblChartType";
            this.lblChartType.Text = "Dạng biểu đồ";
            //
            // cboChartType
            //
            this.cboChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboChartType.Location = new System.Drawing.Point(222, 10);
            this.cboChartType.Name = "cboChartType";
            this.cboChartType.Size = new System.Drawing.Size(140, 25);
            this.cboChartType.TabIndex = 1;
            this.cboChartType.SelectedIndexChanged += new System.EventHandler(this.cboChartType_SelectedIndexChanged);
            //
            // chkLabels
            //
            this.chkLabels.AutoSize = true;
            this.chkLabels.Checked = true;
            this.chkLabels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLabels.Location = new System.Drawing.Point(378, 13);
            this.chkLabels.Name = "chkLabels";
            this.chkLabels.TabIndex = 2;
            this.chkLabels.Text = "Hiện nhãn giá trị";
            this.chkLabels.UseVisualStyleBackColor = true;
            this.chkLabels.CheckedChanged += new System.EventHandler(this.cboChartType_SelectedIndexChanged);
            //
            // btnView
            //
            this.btnView.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnView.ForeColor = System.Drawing.Color.White;
            this.btnView.Location = new System.Drawing.Point(520, 8);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(100, 30);
            this.btnView.TabIndex = 3;
            this.btnView.Text = "Xem báo cáo";
            this.btnView.UseVisualStyleBackColor = false;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            //
            // btnExport
            //
            this.btnExport.Location = new System.Drawing.Point(628, 8);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(110, 30);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "📊 Xuất Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            //
            // btnSaveImage
            //
            this.btnSaveImage.Location = new System.Drawing.Point(746, 8);
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(120, 30);
            this.btnSaveImage.TabIndex = 5;
            this.btnSaveImage.Text = "🖼 Lưu ảnh biểu đồ";
            this.btnSaveImage.UseVisualStyleBackColor = true;
            this.btnSaveImage.Click += new System.EventHandler(this.btnSaveImage_Click);
            //
            // splitResult
            //
            this.splitResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitResult.Name = "splitResult";
            this.splitResult.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitResult.Panel1.Controls.Add(this.chartMain);
            this.splitResult.Panel2.Controls.Add(this.dgvData);
            this.splitResult.Panel2.Controls.Add(this.lblTotals);
            this.splitResult.Size = new System.Drawing.Size(886, 566);
            this.splitResult.SplitterDistance = 330;
            this.splitResult.TabIndex = 0;
            //
            // chartMain
            //
            chartArea1.Name = "ChartArea1";
            this.chartMain.ChartAreas.Add(chartArea1);
            this.chartMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartMain.Name = "chartMain";
            this.chartMain.TabIndex = 0;
            this.chartMain.Text = "chartMain";
            //
            // lblTotals
            //
            this.lblTotals.BackColor = System.Drawing.Color.FromArgb(255, 249, 220);
            this.lblTotals.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTotals.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTotals.Name = "lblTotals";
            this.lblTotals.Padding = new System.Windows.Forms.Padding(8, 5, 0, 0);
            this.lblTotals.Size = new System.Drawing.Size(886, 28);
            this.lblTotals.TabIndex = 1;
            this.lblTotals.Text = "";
            //
            // dgvData
            //
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.AllowUserToResizeRows = false;
            altStyle.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.dgvData.AlternatingRowsDefaultCellStyle = altStyle;
            this.dgvData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvData.BackgroundColor = System.Drawing.Color.White;
            this.dgvData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            headerStyle.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            headerStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            headerStyle.ForeColor = System.Drawing.Color.White;
            headerStyle.SelectionBackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.dgvData.ColumnHeadersDefaultCellStyle = headerStyle;
            this.dgvData.ColumnHeadersHeight = 30;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.EnableHeadersVisualStyles = false;
            this.dgvData.MultiSelect = false;
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.RowHeadersVisible = false;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData.TabIndex = 0;
            //
            // statusMain
            //
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.lblStatus });
            this.statusMain.Name = "statusMain";
            this.statusMain.TabIndex = 1;
            //
            // lblStatus
            //
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Text = "";
            //
            // ReportForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 700);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.statusMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.Name = "ReportForm";
            this.Text = "Báo cáo";
            this.Load += new System.EventHandler(this.ReportForm_Load);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.splitResult.Panel1.ResumeLayout(false);
            this.splitResult.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitResult)).EndInit();
            this.splitResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.pnlParams.ResumeLayout(false);
            this.pnlParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudYear)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.TreeView tvReports;
        private System.Windows.Forms.Label lblReportsTitle;
        private System.Windows.Forms.SplitContainer splitResult;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMain;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Label lblTotals;
        private System.Windows.Forms.Panel pnlParams;
        private System.Windows.Forms.Button btnSaveImage;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.CheckBox chkLabels;
        private System.Windows.Forms.ComboBox cboChartType;
        private System.Windows.Forms.Label lblChartType;
        private System.Windows.Forms.NumericUpDown nudYear;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblReportDesc;
        private System.Windows.Forms.Label lblReportTitle;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}