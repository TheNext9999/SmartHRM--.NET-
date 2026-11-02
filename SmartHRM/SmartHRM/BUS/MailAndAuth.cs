using System;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using SmartHRM.DAL;
using SmartHRM.DTO;

namespace SmartHRM.BUS
{
    public static class MailService
    {
        static string Cfg(string k) { return ConfigurationManager.AppSettings[k] ?? ""; }
        public static bool IsConfigured { get { return !string.IsNullOrWhiteSpace(Cfg("SmtpHost")); } }

        public static void Send(string to, string subject, string htmlBody)
        {
            if (!IsConfigured) throw new ValidationException("Chưa cấu hình SMTP trong App.config (SmtpHost...).");
            if (string.IsNullOrWhiteSpace(to)) throw new ValidationException("Người nhận chưa có địa chỉ email.");
            int port; if (!int.TryParse(Cfg("SmtpPort"), out port)) port = 587;
            using (var client = new SmtpClient(Cfg("SmtpHost"), port))
            {
                client.EnableSsl = !string.Equals(Cfg("SmtpSsl"), "false", StringComparison.OrdinalIgnoreCase);
                if (!string.IsNullOrWhiteSpace(Cfg("SmtpUser")))
                    client.Credentials = new NetworkCredential(Cfg("SmtpUser"), Cfg("SmtpPass"));
                using (var msg = new MailMessage(Cfg("SmtpFrom"), to, subject, htmlBody))
                {
                    msg.IsBodyHtml = true;
                    client.Send(msg);
                }
            }
        }
    }

    public class AuthResult
    {
        public bool Success;
        public string Message;
    }

    /// <summary>Xác thực: mật khẩu (BCrypt), OTP qua email, tài khoản doanh nghiệp AD. Ứng dụng desktop dùng phiên (Session) thay cho JWT/cookie.</summary>
    public class AuthBUS
    {
        readonly UserDAL dal = new UserDAL();

        public AuthResult Login(string username, string password)
        {
            username = (username ?? "").Trim();
            var u = dal.GetByUsername(username);
            if (u == null || !u.IsActive) return Fail(username, "Tài khoản không tồn tại hoặc đã bị khóa.");
            bool ok;
            try { ok = u.UseAd ? ValidateAd(username, password) : BCrypt.Net.BCrypt.Verify(password ?? "", u.PasswordHash); }
            catch (ValidationException ex) { return Fail(username, ex.Message); }
            if (!ok) return Fail(username, "Sai tên đăng nhập hoặc mật khẩu.");
            return Succeed(u, "mật khẩu" + (u.UseAd ? " (AD)" : ""));
        }

        static bool ValidateAd(string username, string password)
        {
            var domain = ConfigurationManager.AppSettings["AdDomain"];
            if (string.IsNullOrWhiteSpace(domain)) throw new ValidationException("Chưa cấu hình AdDomain trong App.config.");
            try
            {
                using (var ctx = new PrincipalContext(ContextType.Domain, domain))
                    return ctx.ValidateCredentials(username, password);
            }
            catch (Exception ex) { throw new ValidationException("Không kết nối được AD: " + ex.Message); }
        }

        /// <summary>Gửi OTP 6 số (hiệu lực 5 phút). Trả về mã nếu chạy chế độ demo (chưa cấu hình SMTP), ngược lại trả null.</summary>
        public string SendOtp(string username)
        {
            var u = dal.GetByUsername((username ?? "").Trim());
            if (u == null || !u.IsActive) throw new ValidationException("Tài khoản không tồn tại hoặc đã bị khóa.");
            string code;
            using (var rng = RandomNumberGenerator.Create())
            {
                var b = new byte[4]; rng.GetBytes(b);
                code = (BitConverter.ToUInt32(b, 0) % 1000000u).ToString("D6");
            }
            dal.SetOtp(u.Id, BCrypt.Net.BCrypt.HashPassword(code), DateTime.Now.AddMinutes(5));
            if (MailService.IsConfigured)
            {
                MailService.Send(u.Email, "Mã OTP đăng nhập SmartHRM",
                    "<p>Mã OTP của bạn là: <b style='font-size:20px'>" + code + "</b></p><p>Mã có hiệu lực trong 5 phút.</p>");
                return null;
            }
            return code;
        }

        public AuthResult LoginWithOtp(string username, string otp)
        {
            username = (username ?? "").Trim();
            var u = dal.GetByUsername(username);
            if (u == null || !u.IsActive) return Fail(username, "Tài khoản không tồn tại hoặc đã bị khóa.");
            var row = dal.GetOtp(u.Id);
            if (row == null || row["otp_hash"] == DBNull.Value || row["otp_expires"] == DBNull.Value)
                return Fail(username, "Chưa yêu cầu mã OTP.");
            if ((DateTime)row["otp_expires"] < DateTime.Now) return Fail(username, "Mã OTP đã hết hạn.");
            if (!BCrypt.Net.BCrypt.Verify((otp ?? "").Trim(), Convert.ToString(row["otp_hash"]))) return Fail(username, "Mã OTP không đúng.");
            dal.ClearOtp(u.Id);
            return Succeed(u, "OTP");
        }

        AuthResult Succeed(UserDTO u, string how)
        {
            Session.Start(u);
            dal.TouchLogin(u.Id);
            AuditBUS.Log("Đăng nhập", "AUTH", "Đăng nhập bằng " + how);
            return new AuthResult { Success = true };
        }

        AuthResult Fail(string username, string msg)
        {
            try { new AuditDAL().Write(null, username, "Đăng nhập thất bại", "AUTH", msg); } catch { }
            return new AuthResult { Success = false, Message = msg };
        }

        public void Logout()
        {
            AuditBUS.Log("Đăng xuất", "AUTH", "");
            Session.End();
        }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            var u = Session.User;
            if (u.UseAd) throw new ValidationException("Tài khoản AD đổi mật khẩu theo hệ thống của doanh nghiệp.");
            if (!BCrypt.Net.BCrypt.Verify(oldPassword ?? "", u.PasswordHash)) throw new ValidationException("Mật khẩu hiện tại không đúng.");
            if ((newPassword ?? "").Length < 6) throw new ValidationException("Mật khẩu mới tối thiểu 6 ký tự.");
            u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            dal.SetPassword(u.Id, u.PasswordHash);
            AuditBUS.Log("Đổi mật khẩu", "AUTH", "");
        }
    }
}
