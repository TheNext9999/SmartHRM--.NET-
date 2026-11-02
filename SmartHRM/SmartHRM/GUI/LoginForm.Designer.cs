namespace SmartHRM.GUI
{
    partial class LoginForm
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
            this.pnlBrand = new System.Windows.Forms.Panel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblTagline = new System.Windows.Forms.Label();
            this.lblBrand = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lblDb = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.tabLogin = new System.Windows.Forms.TabControl();
            this.tpPassword = new System.Windows.Forms.TabPage();
            this.lblCaps = new System.Windows.Forms.Label();
            this.chkShow = new System.Windows.Forms.CheckBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.lblPass = new System.Windows.Forms.Label();
            this.tpOtp = new System.Windows.Forms.TabPage();
            this.lblOtpHelp = new System.Windows.Forms.Label();
            this.btnOtpLogin = new System.Windows.Forms.Button();
            this.btnSendOtp = new System.Windows.Forms.Button();
            this.txtOtp = new System.Windows.Forms.TextBox();
            this.lblOtp = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblSub = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tmrTick = new System.Windows.Forms.Timer(this.components);
            this.pnlBrand.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.tabLogin.SuspendLayout();
            this.tpPassword.SuspendLayout();
            this.tpOtp.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlBrand
            //
            this.pnlBrand.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.pnlBrand.Controls.Add(this.lblVersion);
            this.pnlBrand.Controls.Add(this.lblTagline);
            this.pnlBrand.Controls.Add(this.lblBrand);
            this.pnlBrand.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBrand.Name = "pnlBrand";
            this.pnlBrand.Size = new System.Drawing.Size(270, 420);
            this.pnlBrand.TabIndex = 1;
            //
            // lblBrand
            //
            this.lblBrand.AutoSize = true;
            this.lblBrand.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblBrand.ForeColor = System.Drawing.Color.White;
            this.lblBrand.Location = new System.Drawing.Point(24, 120);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Text = "SmartHRM";
            //
            // lblTagline
            //
            this.lblTagline.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblTagline.ForeColor = System.Drawing.Color.FromArgb(200, 220, 240);
            this.lblTagline.Location = new System.Drawing.Point(28, 180);
            this.lblTagline.Name = "lblTagline";
            this.lblTagline.Size = new System.Drawing.Size(220, 90);
            this.lblTagline.Text = "Hệ thống quản lý nhân sự toàn diện:\r\nhồ sơ • chấm công • tiền lương • đánh giá";
            //
            // lblVersion
            //
            this.lblVersion.AutoSize = true;
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(150, 180, 210);
            this.lblVersion.Location = new System.Drawing.Point(26, 385);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Text = "Phiên bản 1.0";
            //
            // pnlMain
            //
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.Controls.Add(this.lblDb);
            this.pnlMain.Controls.Add(this.lblMessage);
            this.pnlMain.Controls.Add(this.tabLogin);
            this.pnlMain.Controls.Add(this.txtUser);
            this.pnlMain.Controls.Add(this.lblUser);
            this.pnlMain.Controls.Add(this.lblSub);
            this.pnlMain.Controls.Add(this.lblTitle);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.TabIndex = 0;
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.lblTitle.Location = new System.Drawing.Point(32, 24);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "Đăng nhập";
            //
            // lblSub
            //
            this.lblSub.AutoSize = true;
            this.lblSub.ForeColor = System.Drawing.Color.Gray;
            this.lblSub.Location = new System.Drawing.Point(35, 66);
            this.lblSub.Name = "lblSub";
            this.lblSub.Text = "Vui lòng đăng nhập để tiếp tục";
            //
            // lblUser
            //
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(35, 100);
            this.lblUser.Name = "lblUser";
            this.lblUser.Text = "Tên đăng nhập";
            //
            // txtUser
            //
            this.txtUser.Location = new System.Drawing.Point(35, 122);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(400, 25);
            this.txtUser.TabIndex = 0;
            //
            // tabLogin
            //
            this.tabLogin.Controls.Add(this.tpPassword);
            this.tabLogin.Controls.Add(this.tpOtp);
            this.tabLogin.Location = new System.Drawing.Point(35, 160);
            this.tabLogin.Name = "tabLogin";
            this.tabLogin.SelectedIndex = 0;
            this.tabLogin.Size = new System.Drawing.Size(400, 176);
            this.tabLogin.TabIndex = 1;
            this.tabLogin.SelectedIndexChanged += new System.EventHandler(this.tabLogin_SelectedIndexChanged);
            //
            // tpPassword
            //
            this.tpPassword.Controls.Add(this.lblCaps);
            this.tpPassword.Controls.Add(this.chkShow);
            this.tpPassword.Controls.Add(this.btnLogin);
            this.tpPassword.Controls.Add(this.txtPass);
            this.tpPassword.Controls.Add(this.lblPass);
            this.tpPassword.Name = "tpPassword";
            this.tpPassword.Padding = new System.Windows.Forms.Padding(3);
            this.tpPassword.Text = "Mật khẩu / AD";
            this.tpPassword.UseVisualStyleBackColor = true;
            //
            // lblPass
            //
            this.lblPass.AutoSize = true;
            this.lblPass.Location = new System.Drawing.Point(10, 12);
            this.lblPass.Name = "lblPass";
            this.lblPass.Text = "Mật khẩu";
            //
            // txtPass
            //
            this.txtPass.Location = new System.Drawing.Point(10, 34);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(374, 25);
            this.txtPass.TabIndex = 0;
            this.txtPass.UseSystemPasswordChar = true;
            this.txtPass.Enter += new System.EventHandler(this.txtPass_CheckCaps);
            this.txtPass.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPass_KeyUp);
            //
            // chkShow
            //
            this.chkShow.AutoSize = true;
            this.chkShow.Location = new System.Drawing.Point(10, 68);
            this.chkShow.Name = "chkShow";
            this.chkShow.TabIndex = 1;
            this.chkShow.Text = "Hiện mật khẩu";
            this.chkShow.UseVisualStyleBackColor = true;
            this.chkShow.CheckedChanged += new System.EventHandler(this.chkShow_CheckedChanged);
            //
            // lblCaps
            //
            this.lblCaps.AutoSize = true;
            this.lblCaps.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblCaps.Location = new System.Drawing.Point(190, 69);
            this.lblCaps.Name = "lblCaps";
            this.lblCaps.Text = "⚠ Caps Lock đang bật";
            this.lblCaps.Visible = false;
            //
            // btnLogin
            //
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(10, 100);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(374, 40);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            //
            // tpOtp
            //
            this.tpOtp.Controls.Add(this.lblOtpHelp);
            this.tpOtp.Controls.Add(this.btnOtpLogin);
            this.tpOtp.Controls.Add(this.btnSendOtp);
            this.tpOtp.Controls.Add(this.txtOtp);
            this.tpOtp.Controls.Add(this.lblOtp);
            this.tpOtp.Name = "tpOtp";
            this.tpOtp.Padding = new System.Windows.Forms.Padding(3);
            this.tpOtp.Text = "Mã OTP (email)";
            this.tpOtp.UseVisualStyleBackColor = true;
            //
            // lblOtp
            //
            this.lblOtp.AutoSize = true;
            this.lblOtp.Location = new System.Drawing.Point(10, 12);
            this.lblOtp.Name = "lblOtp";
            this.lblOtp.Text = "Mã OTP (6 chữ số)";
            //
            // txtOtp
            //
            this.txtOtp.Font = new System.Drawing.Font("Consolas", 12F);
            this.txtOtp.Location = new System.Drawing.Point(10, 34);
            this.txtOtp.MaxLength = 6;
            this.txtOtp.Name = "txtOtp";
            this.txtOtp.Size = new System.Drawing.Size(150, 26);
            this.txtOtp.TabIndex = 0;
            this.txtOtp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOtp_KeyPress);
            //
            // btnSendOtp
            //
            this.btnSendOtp.Location = new System.Drawing.Point(172, 32);
            this.btnSendOtp.Name = "btnSendOtp";
            this.btnSendOtp.Size = new System.Drawing.Size(212, 30);
            this.btnSendOtp.TabIndex = 1;
            this.btnSendOtp.Text = "Gửi mã OTP về email";
            this.btnSendOtp.UseVisualStyleBackColor = true;
            this.btnSendOtp.Click += new System.EventHandler(this.btnSendOtp_Click);
            //
            // lblOtpHelp
            //
            this.lblOtpHelp.ForeColor = System.Drawing.Color.Gray;
            this.lblOtpHelp.Location = new System.Drawing.Point(10, 66);
            this.lblOtpHelp.Name = "lblOtpHelp";
            this.lblOtpHelp.Size = new System.Drawing.Size(374, 30);
            this.lblOtpHelp.Text = "Nhập tên đăng nhập, bấm \"Gửi mã\". Mã có hiệu lực 5 phút.";
            //
            // btnOtpLogin
            //
            this.btnOtpLogin.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.btnOtpLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOtpLogin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnOtpLogin.ForeColor = System.Drawing.Color.White;
            this.btnOtpLogin.Location = new System.Drawing.Point(10, 100);
            this.btnOtpLogin.Name = "btnOtpLogin";
            this.btnOtpLogin.Size = new System.Drawing.Size(374, 40);
            this.btnOtpLogin.TabIndex = 2;
            this.btnOtpLogin.Text = "Đăng nhập bằng OTP";
            this.btnOtpLogin.UseVisualStyleBackColor = false;
            this.btnOtpLogin.Click += new System.EventHandler(this.btnOtpLogin_Click);
            //
            // lblMessage
            //
            this.lblMessage.ForeColor = System.Drawing.Color.Firebrick;
            this.lblMessage.Location = new System.Drawing.Point(35, 346);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(400, 22);
            this.lblMessage.Text = "";
            //
            // lblDb
            //
            this.lblDb.AutoSize = true;
            this.lblDb.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblDb.ForeColor = System.Drawing.Color.Gray;
            this.lblDb.Location = new System.Drawing.Point(35, 385);
            this.lblDb.Name = "lblDb";
            this.lblDb.Text = "Đang kiểm tra kết nối cơ sở dữ liệu...";
            //
            // tmrTick
            //
            this.tmrTick.Interval = 1000;
            this.tmrTick.Tick += new System.EventHandler(this.tmrTick_Tick);
            //
            // LoginForm
            //
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 420);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlBrand);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập - SmartHRM";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.Shown += new System.EventHandler(this.LoginForm_Shown);
            this.pnlBrand.ResumeLayout(false);
            this.pnlBrand.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.tabLogin.ResumeLayout(false);
            this.tpPassword.ResumeLayout(false);
            this.tpPassword.PerformLayout();
            this.tpOtp.ResumeLayout(false);
            this.tpOtp.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlBrand;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.Label lblTagline;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSub;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TabControl tabLogin;
        private System.Windows.Forms.TabPage tpPassword;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.CheckBox chkShow;
        private System.Windows.Forms.Label lblCaps;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TabPage tpOtp;
        private System.Windows.Forms.Label lblOtp;
        private System.Windows.Forms.TextBox txtOtp;
        private System.Windows.Forms.Button btnSendOtp;
        private System.Windows.Forms.Label lblOtpHelp;
        private System.Windows.Forms.Button btnOtpLogin;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblDb;
        private System.Windows.Forms.Timer tmrTick;
    }
}