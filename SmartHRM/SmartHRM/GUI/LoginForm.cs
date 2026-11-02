using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SmartHRM.BUS;
using SmartHRM.DAL;

namespace SmartHRM.GUI
{
    /// <summary>
    /// Đăng nhập bằng mật khẩu / AD hoặc mã OTP gửi qua email.
    /// Có: hiện/ẩn mật khẩu, cảnh báo Caps Lock, nhớ tên đăng nhập gần nhất,
    /// tạm khóa 30 giây sau 5 lần sai, đếm ngược 60 giây khi gửi lại OTP, hiển thị trạng thái kết nối MySQL.
    /// </summary>
    public partial class LoginForm : Form
    {
        const int MaxFails = 5, LockSeconds = 30, OtpCooldownSeconds = 60;
        readonly AuthBUS auth = new AuthBUS();
        int fails, lockLeft, otpLeft;

        static string LastUserFile
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SmartHRM", "last_user.txt"); }
        }

        public LoginForm()
        {
            InitializeComponent();
            lblVersion.Text = "Phiên bản " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
        }

        // ------------------------------------------------------------ khởi động
        void LoginForm_Load(object sender, EventArgs e)
        {
            try { if (File.Exists(LastUserFile)) txtUser.Text = File.ReadAllText(LastUserFile).Trim(); } catch { }
        }

        void LoginForm_Shown(object sender, EventArgs e)
        {
            (txtUser.Text.Length == 0 ? txtUser : txtPass).Focus();
            try
            {
                DbHelper.ScalarInt("SELECT 1");
                lblDb.Text = "● Đã kết nối cơ sở dữ liệu MySQL"; lblDb.ForeColor = Color.SeaGreen;
            }
            catch
            {
                lblDb.Text = "● Không kết nối được MySQL - kiểm tra App.config"; lblDb.ForeColor = Color.Firebrick;
            }
        }

        // ------------------------------------------------------------ đăng nhập
        void tabLogin_SelectedIndexChanged(object sender, EventArgs e)
        {
            AcceptButton = tabLogin.SelectedTab == tpOtp ? btnOtpLogin : btnLogin;
            lblMessage.Text = lockLeft > 0 ? lblMessage.Text : "";
        }

        void btnLogin_Click(object sender, EventArgs e)
        {
            if (!CheckInput(txtPass, "Hãy nhập mật khẩu.")) return;
            Run(() => auth.Login(txtUser.Text, txtPass.Text));
        }

        void btnOtpLogin_Click(object sender, EventArgs e)
        {
            if (txtOtp.Text.Trim().Length != 6) { Fail("Mã OTP gồm 6 chữ số.", false); txtOtp.Focus(); return; }
            Run(() => auth.LoginWithOtp(txtUser.Text, txtOtp.Text));
        }

        bool CheckInput(TextBox secret, string secretMsg)
        {
            if (lockLeft > 0) return false;
            if (txtUser.Text.Trim().Length == 0) { Fail("Hãy nhập tên đăng nhập.", false); txtUser.Focus(); return false; }
            if (secret.Text.Length == 0) { Fail(secretMsg, false); secret.Focus(); return false; }
            return true;
        }

        void Run(Func<AuthResult> login)
        {
            if (lockLeft > 0) return;
            try
            {
                Cursor = Cursors.WaitCursor;
                var r = login();
                if (r.Success)
                {
                    try
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(LastUserFile));
                        File.WriteAllText(LastUserFile, txtUser.Text.Trim());
                    }
                    catch { }
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else Fail(r.Message, true);
            }
            catch (Exception ex) { Ui.Error(ex); }
            finally { Cursor = Cursors.Default; }
        }

        void Fail(string message, bool countAsAttempt)
        {
            lblMessage.Text = message;
            if (!countAsAttempt) return;
            txtPass.Clear(); txtOtp.Clear();
            if (++fails >= MaxFails) StartLock();
            else lblMessage.Text = message + " (còn " + (MaxFails - fails) + " lần thử)";
        }

        // ------------------------------------------------------------ OTP
        void btnSendOtp_Click(object sender, EventArgs e)
        {
            if (txtUser.Text.Trim().Length == 0) { Fail("Hãy nhập tên đăng nhập trước.", false); txtUser.Focus(); return; }
            try
            {
                Cursor = Cursors.WaitCursor;
                var demo = auth.SendOtp(txtUser.Text);
                lblMessage.Text = "";
                if (demo == null) Ui.Info("Đã gửi mã OTP tới email của tài khoản. Mã có hiệu lực 5 phút.");
                else Ui.Info("Chế độ DEMO (chưa cấu hình SMTP trong App.config).\nMã OTP của bạn là: " + demo);
                otpLeft = OtpCooldownSeconds; btnSendOtp.Enabled = false; tmrTick.Start();
                txtOtp.Focus();
            }
            catch (Exception ex) { Ui.Error(ex); }
            finally { Cursor = Cursors.Default; }
        }

        void txtOtp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;   // chỉ cho nhập số
        }

        // ------------------------------------------------------------ tiện ích giao diện
        void chkShow_CheckedChanged(object sender, EventArgs e) { txtPass.UseSystemPasswordChar = !chkShow.Checked; }

        void txtPass_CheckCaps(object sender, EventArgs e) { lblCaps.Visible = Control.IsKeyLocked(Keys.CapsLock); }
        void txtPass_KeyUp(object sender, KeyEventArgs e) { lblCaps.Visible = Control.IsKeyLocked(Keys.CapsLock); }

        void StartLock()
        {
            lockLeft = LockSeconds; fails = 0;
            btnLogin.Enabled = btnOtpLogin.Enabled = false;
            lblMessage.Text = "Sai quá nhiều lần. Thử lại sau " + lockLeft + " giây.";
            tmrTick.Start();
        }

        void tmrTick_Tick(object sender, EventArgs e)
        {
            if (lockLeft > 0)
            {
                lockLeft--;
                if (lockLeft == 0) { btnLogin.Enabled = btnOtpLogin.Enabled = true; lblMessage.Text = ""; }
                else lblMessage.Text = "Sai quá nhiều lần. Thử lại sau " + lockLeft + " giây.";
            }
            if (otpLeft > 0)
            {
                otpLeft--;
                if (otpLeft == 0) { btnSendOtp.Enabled = true; btnSendOtp.Text = "Gửi mã OTP về email"; }
                else btnSendOtp.Text = "Gửi lại sau " + otpLeft + " giây";
            }
            if (lockLeft == 0 && otpLeft == 0) tmrTick.Stop();
        }
    }
}