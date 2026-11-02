namespace SmartHRM.GUI
{
    partial class AuditLogForm
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
            System.Windows.Forms.DataGridViewCellStyle timeStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.btnToday = new System.Windows.Forms.Button();
            this.btn7Days = new System.Windows.Forms.Button();
            this.btn30Days = new System.Windows.Forms.Button();
            this.lblUser = new System.Windows.Forms.Label();
            this.cboUser = new System.Windows.Forms.ComboBox();
            this.lblModule = new System.Windows.Forms.Label();
            this.cboModule = new System.Windows.Forms.ComboBox();
            this.lblAction = new System.Windows.Forms.Label();
            this.cboAction = new System.Windows.Forms.ComboBox();
            this.lblKeyword = new System.Windows.Forms.Label();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsbExport = new System.Windows.Forms.ToolStripButton();
            this.tsbAuto = new System.Windows.Forms.ToolStripButton();
            this.tssPurge = new System.Windows.Forms.ToolStripSeparator();
            this.tsbPurge = new System.Windows.Forms.ToolStripButton();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.dgvLog = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtDetail = new System.Windows.Forms.TextBox();
            this.lblDetailTitle = new System.Windows.Forms.Label();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.lblCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblLegend = new System.Windows.Forms.ToolStripStatusLabel();
            this.tmrAuto = new System.Windows.Forms.Timer(this.components);
            this.pnlFilter.SuspendLayout();
            this.tsMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).BeginInit();
            this.statusMain.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlFilter
            //
            this.pnlFilter.AutoScroll = true;
            this.pnlFilter.BackColor = System.Drawing.Color.FromArgb(236, 242, 249);
            this.pnlFilter.Controls.Add(this.lblFrom);
            this.pnlFilter.Controls.Add(this.dtpFrom);
            this.pnlFilter.Controls.Add(this.lblTo);
            this.pnlFilter.Controls.Add(this.dtpTo);
            this.pnlFilter.Controls.Add(this.btnToday);
            this.pnlFilter.Controls.Add(this.btn7Days);
            this.pnlFilter.Controls.Add(this.btn30Days);
            this.pnlFilter.Controls.Add(this.lblUser);
            this.pnlFilter.Controls.Add(this.cboUser);
            this.pnlFilter.Controls.Add(this.lblModule);
            this.pnlFilter.Controls.Add(this.cboModule);
            this.pnlFilter.Controls.Add(this.lblAction);
            this.pnlFilter.Controls.Add(this.cboAction);
            this.pnlFilter.Controls.Add(this.lblKeyword);
            this.pnlFilter.Controls.Add(this.txtKeyword);
            this.pnlFilter.Controls.Add(this.btnSearch);
            this.pnlFilter.Controls.Add(this.btnReset);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(0, 27);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(1200, 96);
            this.pnlFilter.TabIndex = 1;
            //
            // lblFrom
            //
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(14, 17);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Text = "Từ ngày";
            //
            // dtpFrom
            //
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(74, 12);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(112, 25);
            this.dtpFrom.TabIndex = 0;
            //
            // lblTo
            //
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(200, 17);
            this.lblTo.Name = "lblTo";
            this.lblTo.Text = "Đến ngày";
            //
            // dtpTo
            //
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(272, 12);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(112, 25);
            this.dtpTo.TabIndex = 1;
            //
            // btnToday
            //
            this.btnToday.Location = new System.Drawing.Point(402, 10);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(84, 29);
            this.btnToday.TabIndex = 2;
            this.btnToday.Text = "Hôm nay";
            this.btnToday.UseVisualStyleBackColor = true;
            this.btnToday.Click += new System.EventHandler(this.btnQuickRange_Click);
            //
            // btn7Days
            //
            this.btn7Days.Location = new System.Drawing.Point(492, 10);
            this.btn7Days.Name = "btn7Days";
            this.btn7Days.Size = new System.Drawing.Size(84, 29);
            this.btn7Days.TabIndex = 3;
            this.btn7Days.Text = "7 ngày";
            this.btn7Days.UseVisualStyleBackColor = true;
            this.btn7Days.Click += new System.EventHandler(this.btnQuickRange_Click);
            //
            // btn30Days
            //
            this.btn30Days.Location = new System.Drawing.Point(582, 10);
            this.btn30Days.Name = "btn30Days";
            this.btn30Days.Size = new System.Drawing.Size(84, 29);
            this.btn30Days.TabIndex = 4;
            this.btn30Days.Text = "30 ngày";
            this.btn30Days.UseVisualStyleBackColor = true;
            this.btn30Days.Click += new System.EventHandler(this.btnQuickRange_Click);
            //
            // lblUser
            //
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(14, 59);
            this.lblUser.Name = "lblUser";
            this.lblUser.Text = "Người dùng";
            //
            // cboUser
            //
            this.cboUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUser.Location = new System.Drawing.Point(96, 54);
            this.cboUser.Name = "cboUser";
            this.cboUser.Size = new System.Drawing.Size(150, 25);
            this.cboUser.TabIndex = 5;
            //
            // lblModule
            //
            this.lblModule.AutoSize = true;
            this.lblModule.Location = new System.Drawing.Point(262, 59);
            this.lblModule.Name = "lblModule";
            this.lblModule.Text = "Mô-đun";
            //
            // cboModule
            //
            this.cboModule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModule.Location = new System.Drawing.Point(326, 54);
            this.cboModule.Name = "cboModule";
            this.cboModule.Size = new System.Drawing.Size(210, 25);
            this.cboModule.TabIndex = 6;
            //
            // lblAction
            //
            this.lblAction.AutoSize = true;
            this.lblAction.Location = new System.Drawing.Point(552, 59);
            this.lblAction.Name = "lblAction";
            this.lblAction.Text = "Thao tác";
            //
            // cboAction
            //
            this.cboAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAction.Location = new System.Drawing.Point(618, 54);
            this.cboAction.Name = "cboAction";
            this.cboAction.Size = new System.Drawing.Size(160, 25);
            this.cboAction.TabIndex = 7;
            //
            // lblKeyword
            //
            this.lblKeyword.AutoSize = true;
            this.lblKeyword.Location = new System.Drawing.Point(794, 59);
            this.lblKeyword.Name = "lblKeyword";
            this.lblKeyword.Text = "Từ khóa";
            //
            // txtKeyword
            //
            this.txtKeyword.Location = new System.Drawing.Point(858, 54);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(170, 25);
            this.txtKeyword.TabIndex = 8;
            this.txtKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKeyword_KeyDown);
            //
            // btnSearch
            //
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(1042, 51);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(70, 31);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "Lọc";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            //
            // btnReset
            //
            this.btnReset.Location = new System.Drawing.Point(1118, 51);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(76, 31);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "Đặt lại";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            //
            // tsMain
            //
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsbRefresh, this.tsbExport, this.tsbAuto, this.tssPurge, this.tsbPurge });
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Padding = new System.Windows.Forms.Padding(6, 2, 6, 2);
            this.tsMain.Size = new System.Drawing.Size(1200, 27);
            this.tsMain.TabIndex = 0;
            //
            // tsbRefresh
            //
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Text = "🔄 Làm mới";
            this.tsbRefresh.Click += new System.EventHandler(this.btnSearch_Click);
            //
            // tsbExport
            //
            this.tsbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbExport.Name = "tsbExport";
            this.tsbExport.Text = "📊 Xuất Excel";
            this.tsbExport.Click += new System.EventHandler(this.tsbExport_Click);
            //
            // tsbAuto
            //
            this.tsbAuto.CheckOnClick = true;
            this.tsbAuto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbAuto.Name = "tsbAuto";
            this.tsbAuto.Text = "⏱ Tự làm mới (10 giây)";
            this.tsbAuto.CheckedChanged += new System.EventHandler(this.tsbAuto_CheckedChanged);
            //
            // tssPurge
            //
            this.tssPurge.Name = "tssPurge";
            //
            // tsbPurge
            //
            this.tsbPurge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbPurge.Name = "tsbPurge";
            this.tsbPurge.Text = "🧹 Dọn nhật ký cũ";
            this.tsbPurge.Click += new System.EventHandler(this.tsbPurge_Click);
            //
            // splitMain
            //
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 123);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitMain.Panel1.Controls.Add(this.dgvLog);
            this.splitMain.Panel2.Controls.Add(this.txtDetail);
            this.splitMain.Panel2.Controls.Add(this.lblDetailTitle);
            this.splitMain.Size = new System.Drawing.Size(1200, 475);
            this.splitMain.SplitterDistance = 340;
            this.splitMain.TabIndex = 2;
            //
            // dgvLog
            //
            this.dgvLog.AllowUserToAddRows = false;
            this.dgvLog.AllowUserToDeleteRows = false;
            this.dgvLog.AllowUserToResizeRows = false;
            altStyle.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.dgvLog.AlternatingRowsDefaultCellStyle = altStyle;
            this.dgvLog.AutoGenerateColumns = false;
            this.dgvLog.BackgroundColor = System.Drawing.Color.White;
            this.dgvLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLog.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            headerStyle.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            headerStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            headerStyle.ForeColor = System.Drawing.Color.White;
            headerStyle.SelectionBackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.dgvLog.ColumnHeadersDefaultCellStyle = headerStyle;
            this.dgvLog.ColumnHeadersHeight = 32;
            this.dgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colId, this.colTime, this.colUser, this.colAction, this.colModule, this.colDetail });
            this.dgvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLog.EnableHeadersVisualStyles = false;
            this.dgvLog.MultiSelect = false;
            this.dgvLog.Name = "dgvLog";
            this.dgvLog.ReadOnly = true;
            this.dgvLog.RowHeadersVisible = false;
            this.dgvLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLog.TabIndex = 0;
            this.dgvLog.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvLog_CellFormatting);
            this.dgvLog.SelectionChanged += new System.EventHandler(this.dgvLog_SelectionChanged);
            //
            // colId
            //
            this.colId.DataPropertyName = "id";
            this.colId.HeaderText = "ID";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.Visible = false;
            //
            // colTime
            //
            this.colTime.DataPropertyName = "created_at";
            timeStyle.Format = "dd/MM/yyyy HH:mm:ss";
            this.colTime.DefaultCellStyle = timeStyle;
            this.colTime.HeaderText = "Thời gian";
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            this.colTime.Width = 150;
            //
            // colUser
            //
            this.colUser.DataPropertyName = "username";
            this.colUser.HeaderText = "Người dùng";
            this.colUser.Name = "colUser";
            this.colUser.ReadOnly = true;
            this.colUser.Width = 130;
            //
            // colAction
            //
            this.colAction.DataPropertyName = "action";
            this.colAction.HeaderText = "Thao tác";
            this.colAction.Name = "colAction";
            this.colAction.ReadOnly = true;
            this.colAction.Width = 150;
            //
            // colModule
            //
            this.colModule.DataPropertyName = "module";
            this.colModule.HeaderText = "Mô-đun";
            this.colModule.Name = "colModule";
            this.colModule.ReadOnly = true;
            this.colModule.Width = 200;
            //
            // colDetail
            //
            this.colDetail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDetail.DataPropertyName = "detail";
            this.colDetail.HeaderText = "Chi tiết";
            this.colDetail.Name = "colDetail";
            this.colDetail.ReadOnly = true;
            //
            // txtDetail
            //
            this.txtDetail.BackColor = System.Drawing.Color.White;
            this.txtDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDetail.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtDetail.Multiline = true;
            this.txtDetail.Name = "txtDetail";
            this.txtDetail.ReadOnly = true;
            this.txtDetail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetail.TabIndex = 1;
            //
            // lblDetailTitle
            //
            this.lblDetailTitle.BackColor = System.Drawing.Color.FromArgb(236, 242, 249);
            this.lblDetailTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDetailTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblDetailTitle.Name = "lblDetailTitle";
            this.lblDetailTitle.Padding = new System.Windows.Forms.Padding(6, 4, 0, 0);
            this.lblDetailTitle.Size = new System.Drawing.Size(1200, 26);
            this.lblDetailTitle.TabIndex = 0;
            this.lblDetailTitle.Text = "Chi tiết thao tác đang chọn";
            //
            // statusMain
            //
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.lblCount, this.lblLegend });
            this.statusMain.Location = new System.Drawing.Point(0, 598);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(1200, 22);
            this.statusMain.TabIndex = 3;
            //
            // lblCount
            //
            this.lblCount.Name = "lblCount";
            this.lblCount.Text = "Chưa có dữ liệu";
            //
            // lblLegend
            //
            this.lblLegend.ForeColor = System.Drawing.Color.Gray;
            this.lblLegend.Name = "lblLegend";
            this.lblLegend.Spring = true;
            this.lblLegend.Text = "Đỏ: đăng nhập thất bại   |   Cam: thao tác xóa / dọn nhật ký   |   Vàng: phân quyền / đổi mật khẩu";
            this.lblLegend.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // tmrAuto
            //
            this.tmrAuto.Interval = 10000;
            this.tmrAuto.Tick += new System.EventHandler(this.tmrAuto_Tick);
            //
            // AuditLogForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 620);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.tsMain);
            this.Controls.Add(this.statusMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.Name = "AuditLogForm";
            this.Text = "Nhật ký hệ thống";
            this.Load += new System.EventHandler(this.AuditLogForm_Load);
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            this.splitMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).EndInit();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Button btn7Days;
        private System.Windows.Forms.Button btn30Days;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.ComboBox cboUser;
        private System.Windows.Forms.Label lblModule;
        private System.Windows.Forms.ComboBox cboModule;
        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.ComboBox cboAction;
        private System.Windows.Forms.Label lblKeyword;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripButton tsbExport;
        private System.Windows.Forms.ToolStripButton tsbAuto;
        private System.Windows.Forms.ToolStripSeparator tssPurge;
        private System.Windows.Forms.ToolStripButton tsbPurge;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.DataGridView dgvLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModule;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetail;
        private System.Windows.Forms.TextBox txtDetail;
        private System.Windows.Forms.Label lblDetailTitle;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel lblCount;
        private System.Windows.Forms.ToolStripStatusLabel lblLegend;
        private System.Windows.Forms.Timer tmrAuto;
    }
}