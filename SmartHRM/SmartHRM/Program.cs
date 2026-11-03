using System;
using System.Windows.Forms;
using SmartHRM.BUS;
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
            Application.ThreadException += (s, e) => Ui.Error(e.Exception);

            try { DbInitializer.Run(); }
            catch (Exception ex)
            {
                MessageBox.Show("Không kết nối được cơ sở dữ liệu MySQL.\n\n" + ex.Message +
                    "\n\nHãy chạy Database/smarthrm.sql trong MySQL Workbench và sửa chuỗi kết nối trong App.config.",
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
        }
    }
}