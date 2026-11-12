using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SmartHRM.GUI
{
    /// <summary>
    /// Logo và biểu tượng ứng dụng. Logo được đọc từ tài nguyên nhúng (Build Action = Embedded Resource)
    /// hoặc từ thư mục Images cạnh file .exe (Copy to Output Directory). Không có logo thì giao diện vẫn chạy bình thường.
    /// </summary>
    public static class AppImages
    {
        static Image logo;
        static Icon icon;
        static bool loaded;

        public static Image Logo { get { Ensure(); return logo; } }
        public static Icon AppIcon { get { Ensure(); return icon; } }

        /// <summary>Đặt biểu tượng cửa sổ (góc trái thanh tiêu đề và thanh tác vụ) bằng logo SmartHRM.</summary>
        public static void Apply(Form form)
        {
            var i = AppIcon;
            if (i != null && form != null) form.Icon = i;
        }

        static void Ensure()
        {
            if (loaded) return;
            loaded = true;
            try
            {
                logo = LoadLogo();
                if (logo != null) icon = MakeIcon(logo);
            }
            catch { /* thiếu logo hoặc lỗi định dạng: bỏ qua */ }
        }

        static Image LoadLogo()
        {
            var asm = Assembly.GetExecutingAssembly();
            var name = asm.GetManifestResourceNames().FirstOrDefault(n =>
                n.EndsWith("SmartHRM_Logo.png", StringComparison.OrdinalIgnoreCase) ||
                n.EndsWith("SmartHRM Logo.png", StringComparison.OrdinalIgnoreCase));
            if (name != null)
                using (var s = asm.GetManifestResourceStream(name))
                using (var bmp = new Bitmap(s))
                    return new Bitmap(bmp);                       // sao chép để không phụ thuộc vào stream

            foreach (var file in new[] { "SmartHRM_Logo.png", "SmartHRM Logo.png" })
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", file);
                if (File.Exists(path))
                    using (var bmp = new Bitmap(path)) return new Bitmap(bmp);
            }

            // Phương án cuối: logo nhúng sẵn trong mã (LogoData.cs) - luôn có, không phụ thuộc Build Action
            var bytes = Convert.FromBase64String(string.Concat(LogoData.Parts));
            using (var ms = new MemoryStream(bytes))
            using (var bmp = new Bitmap(ms))
                return new Bitmap(bmp);
        }

        static Icon MakeIcon(Image img)
        {
            using (var canvas = new Bitmap(64, 64))
            using (var g = Graphics.FromImage(canvas))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                float k = Math.Min(60f / img.Width, 60f / img.Height);
                int w = (int)(img.Width * k), h = (int)(img.Height * k);
                g.DrawImage(img, (64 - w) / 2, (64 - h) / 2, w, h);
                return Icon.FromHandle(canvas.GetHicon());        // giữ suốt vòng đời ứng dụng
            }
        }
    }
}