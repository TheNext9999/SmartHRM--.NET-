using System;
using System.Windows.Forms;
using SmartHRM.BUS;
using SmartHRM.Common;
using SmartHRM.GUI;

namespace SmartHRM
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ----- Ghi log kỹ thuật (file .log) -----
            AppLogger.Init();
            AppLogger.UserProvider = () => Session.User == null ? "-" : Session.User.Username;
            AppLogger.LogStartup();

            // Lỗi chưa được xử lý ở bất kỳ đâu: ghi log rồi báo cho người dùng thay vì để chương trình đóng đột ngột
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) =>
            {
                AppLogger.Error("Lỗi chưa được xử lý (luồng giao diện)", e.Exception);
                Ui.Error(e.Exception);
            };
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                AppLogger.Fatal("Lỗi nghiêm trọng, ứng dụng sắp đóng (đang đóng: " + e.IsTerminating + ")", e.ExceptionObject as Exception);

            try { DbInitializer.Run(); }
            catch (Exception ex)
            {
                AppLogger.Fatal("Không kết nối được cơ sở dữ liệu khi khởi động", ex);
                MessageBox.Show("Không kết nối được cơ sở dữ liệu MySQL.\n\n" + ex.Message +
                    "\n\nHãy chạy Database/smarthrm.sql trong MySQL Workbench và sửa chuỗi kết nối trong App.config." +
                    "\n\nChi tiết lỗi đã được ghi vào file:\n" + AppLogger.CurrentFile,
                    Ui.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            while (true)
            {
                using (var login = new LoginForm())
                    if (login.ShowDialog() != DialogResult.OK) break;
                var main = new MainForm();
                Application.Run(main);
                if (!main.LoggedOut) break;       // đóng cửa sổ = thoát; "Đăng xuất" = quay lại màn hình đăng nhập
            }
            AppLogger.Info("Ứng dụng thoát bình thường");
        }
    }
}