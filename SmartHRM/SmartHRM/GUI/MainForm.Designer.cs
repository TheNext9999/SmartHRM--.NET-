namespace SmartHRM.GUI
{
    partial class MainForm
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
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.mnuSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChangePassword = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCloseTab = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCloseOthers = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsQuick = new System.Windows.Forms.ToolStrip();
            this.tsbDashboard = new System.Windows.Forms.ToolStripButton();
            this.tsbEmployee = new System.Windows.Forms.ToolStripButton();
            this.tsbAttendance = new System.Windows.Forms.ToolStripButton();
            this.tsbLeave = new System.Windows.Forms.ToolStripButton();
            this.tsbPayroll = new System.Windows.Forms.ToolStripButton();
            this.tsbReport = new System.Windows.Forms.ToolStripButton();
            this.tstFind = new System.Windows.Forms.ToolStripTextBox();
            this.lblFind = new System.Windows.Forms.ToolStripLabel();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.tvModules = new System.Windows.Forms.TreeView();
            this.lblMenuTitle = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.cmsTabs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsCloseTab = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsCloseOthers = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlWelcome = new System.Windows.Forms.Panel();
            this.lblWelcomeSub = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.lblUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblClock = new System.Windows.Forms.ToolStripStatusLabel();
            this.tmrClock = new System.Windows.Forms.Timer(this.components);
            this.msMain.SuspendLayout();
            this.tsQuick.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.cmsTabs.SuspendLayout();
            this.pnlWelcome.SuspendLayout();
            this.statusMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // msMain
            // 
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSystem,
            this.mnuWindow,
            this.mnuHelp});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Size = new System.Drawing.Size(1280, 24);
            this.msMain.TabIndex = 0;
            // 
            // mnuSystem
            // 
            this.mnuSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuChangePassword,
            this.mnuLogout,
            this.mnuSep1,
            this.mnuExit});
            this.mnuSystem.Name = "mnuSystem";
            this.mnuSystem.Size = new System.Drawing.Size(69, 20);
            this.mnuSystem.Text = "Hệ thống";
            // 
            // mnuChangePassword
            // 
            this.mnuChangePassword.Name = "mnuChangePassword";
            this.mnuChangePassword.Size = new System.Drawing.Size(154, 22);
            this.mnuChangePassword.Text = "Đổi mật khẩu...";
            this.mnuChangePassword.Click += new System.EventHandler(this.mnuChangePassword_Click);
            // 
            // mnuLogout
            // 
            this.mnuLogout.Name = "mnuLogout";
            this.mnuLogout.Size = new System.Drawing.Size(154, 22);
            this.mnuLogout.Text = "Đăng xuất";
            this.mnuLogout.Click += new System.EventHandler(this.mnuLogout_Click);
            // 
            // mnuSep1
            // 
            this.mnuSep1.Name = "mnuSep1";
            this.mnuSep1.Size = new System.Drawing.Size(151, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(154, 22);
            this.mnuExit.Text = "Thoát";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuWindow
            // 
            this.mnuWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCloseTab,
            this.mnuCloseOthers,
            this.mnuCloseAll});
            this.mnuWindow.Name = "mnuWindow";
            this.mnuWindow.Size = new System.Drawing.Size(55, 20);
            this.mnuWindow.Text = "Cửa sổ";
            // 
            // mnuCloseTab
            // 
            this.mnuCloseTab.Name = "mnuCloseTab";
            this.mnuCloseTab.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.mnuCloseTab.Size = new System.Drawing.Size(210, 22);
            this.mnuCloseTab.Text = "Đóng tab hiện tại";
            this.mnuCloseTab.Click += new System.EventHandler(this.CloseCurrentTab_Click);
            // 
            // mnuCloseOthers
            // 
            this.mnuCloseOthers.Name = "mnuCloseOthers";
            this.mnuCloseOthers.Size = new System.Drawing.Size(210, 22);
            this.mnuCloseOthers.Text = "Đóng các tab khác";
            this.mnuCloseOthers.Click += new System.EventHandler(this.CloseOtherTabs_Click);
            // 
            // mnuCloseAll
            // 
            this.mnuCloseAll.Name = "mnuCloseAll";
            this.mnuCloseAll.Size = new System.Drawing.Size(210, 22);
            this.mnuCloseAll.Text = "Đóng tất cả tab";
            this.mnuCloseAll.Click += new System.EventHandler(this.CloseAllTabs_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(63, 20);
            this.mnuHelp.Text = "Trợ giúp";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(186, 22);
            this.mnuAbout.Text = "Giới thiệu SmartHRM";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // tsQuick
            // 
            this.tsQuick.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsQuick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbDashboard,
            this.tsbEmployee,
            this.tsbAttendance,
            this.tsbLeave,
            this.tsbPayroll,
            this.tsbReport,
            this.tstFind,
            this.lblFind});
            this.tsQuick.Location = new System.Drawing.Point(0, 24);
            this.tsQuick.Name = "tsQuick";
            this.tsQuick.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tsQuick.Size = new System.Drawing.Size(1280, 31);
            this.tsQuick.TabIndex = 1;
            // 
            // tsbDashboard
            // 
            this.tsbDashboard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDashboard.Name = "tsbDashboard";
            this.tsbDashboard.Size = new System.Drawing.Size(83, 20);
            this.tsbDashboard.Tag = "DASHBOARD";
            this.tsbDashboard.Text = "🏠 Dashboard";
            this.tsbDashboard.Click += new System.EventHandler(this.tsbQuick_Click);
            // 
            // tsbEmployee
            // 
            this.tsbEmployee.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbEmployee.Name = "tsbEmployee";
            this.tsbEmployee.Size = new System.Drawing.Size(80, 20);
            this.tsbEmployee.Tag = "EMPLOYEE";
            this.tsbEmployee.Text = "👤 Nhân viên";
            this.tsbEmployee.Click += new System.EventHandler(this.tsbQuick_Click);
            // 
            // tsbAttendance
            // 
            this.tsbAttendance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbAttendance.Name = "tsbAttendance";
            this.tsbAttendance.Size = new System.Drawing.Size(88, 20);
            this.tsbAttendance.Tag = "ATTENDANCE";
            this.tsbAttendance.Text = "🕒 Chấm công";
            this.tsbAttendance.Click += new System.EventHandler(this.tsbQuick_Click);
            // 
            // tsbLeave
            // 
            this.tsbLeave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbLeave.Name = "tsbLeave";
            this.tsbLeave.Size = new System.Drawing.Size(62, 20);
            this.tsbLeave.Tag = "LEAVE";
            this.tsbLeave.Text = "📝 Đơn từ";
            this.tsbLeave.Click += new System.EventHandler(this.tsbQuick_Click);
            // 
            // tsbPayroll
            // 
            this.tsbPayroll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbPayroll.Name = "tsbPayroll";
            this.tsbPayroll.Size = new System.Drawing.Size(87, 20);
            this.tsbPayroll.Tag = "PAYROLL";
            this.tsbPayroll.Text = "💰 Bảng lương";
            this.tsbPayroll.Click += new System.EventHandler(this.tsbQuick_Click);
            // 
            // tsbReport
            // 
            this.tsbReport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbReport.Name = "tsbReport";
            this.tsbReport.Size = new System.Drawing.Size(68, 20);
            this.tsbReport.Tag = "REPORT";
            this.tsbReport.Text = "📊 Báo cáo";
            this.tsbReport.Click += new System.EventHandler(this.tsbQuick_Click);
            // 
            // tstFind
            // 
            this.tstFind.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tstFind.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tstFind.Name = "tstFind";
            this.tstFind.Size = new System.Drawing.Size(200, 23);
            this.tstFind.ToolTipText = "Gõ để lọc danh sách chức năng";
            this.tstFind.TextChanged += new System.EventHandler(this.tstFind_TextChanged);
            // 
            // lblFind
            // 
            this.lblFind.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(105, 20);
            this.lblFind.Text = "🔍 Tìm chức năng:";
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitMain.Location = new System.Drawing.Point(0, 55);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.tvModules);
            this.splitMain.Panel1.Controls.Add(this.lblMenuTitle);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.tabMain);
            this.splitMain.Panel2.Controls.Add(this.pnlWelcome);
            this.splitMain.Size = new System.Drawing.Size(1280, 643);
            this.splitMain.SplitterDistance = 280;
            this.splitMain.TabIndex = 2;
            // 
            // tvModules
            // 
            this.tvModules.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvModules.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tvModules.FullRowSelect = true;
            this.tvModules.HideSelection = false;
            this.tvModules.ItemHeight = 28;
            this.tvModules.Location = new System.Drawing.Point(0, 32);
            this.tvModules.Name = "tvModules";
            this.tvModules.ShowLines = false;
            this.tvModules.Size = new System.Drawing.Size(280, 611);
            this.tvModules.TabIndex = 0;
            this.tvModules.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvModules_NodeMouseClick);
            this.tvModules.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvModules_KeyDown);
            // 
            // lblMenuTitle
            // 
            this.lblMenuTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(78)))), ((int)(((byte)(121)))));
            this.lblMenuTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMenuTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMenuTitle.ForeColor = System.Drawing.Color.White;
            this.lblMenuTitle.Location = new System.Drawing.Point(0, 0);
            this.lblMenuTitle.Name = "lblMenuTitle";
            this.lblMenuTitle.Padding = new System.Windows.Forms.Padding(10, 7, 0, 0);
            this.lblMenuTitle.Size = new System.Drawing.Size(280, 32);
            this.lblMenuTitle.TabIndex = 1;
            this.lblMenuTitle.Text = "SmartHRM - Chức năng";
            // 
            // tabMain
            // 
            this.tabMain.ContextMenuStrip = this.cmsTabs;
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Drawing.Point(22, 5);
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(996, 643);
            this.tabMain.TabIndex = 0;
            this.tabMain.Visible = false;
            this.tabMain.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabMain_DrawItem);
            this.tabMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tabMain_MouseUp);
            // 
            // cmsTabs
            // 
            this.cmsTabs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsCloseTab,
            this.cmsCloseOthers,
            this.cmsCloseAll});
            this.cmsTabs.Name = "cmsTabs";
            this.cmsTabs.Size = new System.Drawing.Size(173, 70);
            // 
            // cmsCloseTab
            // 
            this.cmsCloseTab.Name = "cmsCloseTab";
            this.cmsCloseTab.Size = new System.Drawing.Size(172, 22);
            this.cmsCloseTab.Text = "Đóng tab này";
            this.cmsCloseTab.Click += new System.EventHandler(this.CloseCurrentTab_Click);
            // 
            // cmsCloseOthers
            // 
            this.cmsCloseOthers.Name = "cmsCloseOthers";
            this.cmsCloseOthers.Size = new System.Drawing.Size(172, 22);
            this.cmsCloseOthers.Text = "Đóng các tab khác";
            this.cmsCloseOthers.Click += new System.EventHandler(this.CloseOtherTabs_Click);
            // 
            // cmsCloseAll
            // 
            this.cmsCloseAll.Name = "cmsCloseAll";
            this.cmsCloseAll.Size = new System.Drawing.Size(172, 22);
            this.cmsCloseAll.Text = "Đóng tất cả tab";
            this.cmsCloseAll.Click += new System.EventHandler(this.CloseAllTabs_Click);
            // 
            // pnlWelcome
            // 
            this.pnlWelcome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))));
            this.pnlWelcome.Controls.Add(this.lblWelcomeSub);
            this.pnlWelcome.Controls.Add(this.lblWelcome);
            this.pnlWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWelcome.Location = new System.Drawing.Point(0, 0);
            this.pnlWelcome.Name = "pnlWelcome";
            this.pnlWelcome.Size = new System.Drawing.Size(996, 643);
            this.pnlWelcome.TabIndex = 1;
            // 
            // lblWelcomeSub
            // 
            this.lblWelcomeSub.AutoSize = true;
            this.lblWelcomeSub.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblWelcomeSub.ForeColor = System.Drawing.Color.Gray;
            this.lblWelcomeSub.Location = new System.Drawing.Point(44, 112);
            this.lblWelcomeSub.Name = "lblWelcomeSub";
            this.lblWelcomeSub.Size = new System.Drawing.Size(576, 20);
            this.lblWelcomeSub.TabIndex = 0;
            this.lblWelcomeSub.Text = "Chọn một chức năng ở danh sách bên trái hoặc dùng các nút truy cập nhanh phía trê" +
    "n.";
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(78)))), ((int)(((byte)(121)))));
            this.lblWelcome.Location = new System.Drawing.Point(40, 60);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(136, 37);
            this.lblWelcome.TabIndex = 1;
            this.lblWelcome.Text = "Xin chào!";
            // 
            // statusMain
            // 
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblUser,
            this.lblClock});
            this.statusMain.Location = new System.Drawing.Point(0, 698);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(1280, 22);
            this.statusMain.TabIndex = 3;
            // 
            // lblUser
            // 
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(0, 17);
            // 
            // lblClock
            // 
            this.lblClock.Name = "lblClock";
            this.lblClock.Size = new System.Drawing.Size(1265, 17);
            this.lblClock.Spring = true;
            this.lblClock.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tmrClock
            // 
            this.tmrClock.Interval = 1000;
            this.tmrClock.Tick += new System.EventHandler(this.tmrClock_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.tsQuick);
            this.Controls.Add(this.msMain);
            this.Controls.Add(this.statusMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.MainMenuStrip = this.msMain;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SmartHRM - Quản lý nhân sự";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.tsQuick.ResumeLayout(false);
            this.tsQuick.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.cmsTabs.ResumeLayout(false);
            this.pnlWelcome.ResumeLayout(false);
            this.pnlWelcome.PerformLayout();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem mnuSystem;
        private System.Windows.Forms.ToolStripMenuItem mnuChangePassword;
        private System.Windows.Forms.ToolStripMenuItem mnuLogout;
        private System.Windows.Forms.ToolStripSeparator mnuSep1;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuWindow;
        private System.Windows.Forms.ToolStripMenuItem mnuCloseTab;
        private System.Windows.Forms.ToolStripMenuItem mnuCloseOthers;
        private System.Windows.Forms.ToolStripMenuItem mnuCloseAll;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.ToolStrip tsQuick;
        private System.Windows.Forms.ToolStripButton tsbDashboard;
        private System.Windows.Forms.ToolStripButton tsbEmployee;
        private System.Windows.Forms.ToolStripButton tsbAttendance;
        private System.Windows.Forms.ToolStripButton tsbLeave;
        private System.Windows.Forms.ToolStripButton tsbPayroll;
        private System.Windows.Forms.ToolStripButton tsbReport;
        private System.Windows.Forms.ToolStripTextBox tstFind;
        private System.Windows.Forms.ToolStripLabel lblFind;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.TreeView tvModules;
        private System.Windows.Forms.Label lblMenuTitle;
        private System.Windows.Forms.Panel pnlWelcome;
        private System.Windows.Forms.Label lblWelcomeSub;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.ContextMenuStrip cmsTabs;
        private System.Windows.Forms.ToolStripMenuItem cmsCloseTab;
        private System.Windows.Forms.ToolStripMenuItem cmsCloseOthers;
        private System.Windows.Forms.ToolStripMenuItem cmsCloseAll;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel lblUser;
        private System.Windows.Forms.ToolStripStatusLabel lblClock;
        private System.Windows.Forms.Timer tmrClock;
    }
}