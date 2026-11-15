using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace SmartHRM.Common
{
    public enum LogLevel { Debug = 0, Info = 1, Warn = 2, Error = 3, Fatal = 4 }

    /// <summary>
    /// Ghi nhật ký kỹ thuật ra file .log (khác với "Nhật ký hệ thống" lưu trong MySQL).
    /// - Mỗi ngày một file: SmartHRM_yyyyMMdd.log; quá 10 MB thì sang file _2, _3...
    /// - Mặc định lưu tại %LocalAppData%\SmartHRM\Logs (người dùng thường luôn có quyền ghi), có thể đổi trong App.config.
    /// - Tự xóa file cũ hơn LogRetentionDays ngày. Mật khẩu (Pwd=...) tự bị che. Không bao giờ ném lỗi ra ngoài.
    /// </summary>
    public static class AppLogger
    {
        public const string LoggedKey = "SmartHRM.Logged";
        const long MaxFileBytes = 10L * 1024 * 1024;

        static readonly object sync = new object();
        static readonly Regex SecretRx = new Regex(@"(?i)\b(pwd|password|passwd)\s*=\s*[^;""\s]+", RegexOptions.Compiled);
        static bool initialized;
        static string folder;
        static LogLevel minLevel = LogLevel.Info;
        static int retentionDays = 30;

        /// <summary>Cho biết tên người đang đăng nhập (đặt ở Program.cs).</summary>
        public static Func<string> UserProvider { get; set; }

        public static string Folder { get { Init(); return folder; } }
        public static string CurrentFile { get { Init(); return PathForToday(); } }
        public static bool IsDebug { get { Init(); return minLevel <= LogLevel.Debug; } }

        public static void Init()
        {
            lock (sync)
            {
                if (initialized) return;
                initialized = true;
                try
                {
                    LogLevel parsed;
                    if (Enum.TryParse<LogLevel>(ConfigurationManager.AppSettings["LogLevel"], true, out parsed)) minLevel = parsed;
                    int days;
                    if (int.TryParse(ConfigurationManager.AppSettings["LogRetentionDays"], out days) && days > 0) retentionDays = days;
                    var custom = ConfigurationManager.AppSettings["LogFolder"];
                    folder = string.IsNullOrWhiteSpace(custom)
                        ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SmartHRM", "Logs")
                        : Environment.ExpandEnvironmentVariables(custom.Trim());
                }
                catch { folder = null; }

                if (string.IsNullOrEmpty(folder))
                    folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SmartHRM", "Logs");
                try { Directory.CreateDirectory(folder); }
                catch
                {
                    folder = Path.Combine(Path.GetTempPath(), "SmartHRM", "Logs");        // không ghi được thư mục cấu hình: dùng thư mục tạm
                    try { Directory.CreateDirectory(folder); } catch { }
                }
                CleanOld();
            }
        }

        // ---------------------------------------------------------------- API ghi log
        public static void Debug(string message) { Write(LogLevel.Debug, message, null); }
        public static void Info(string message) { Write(LogLevel.Info, message, null); }
        public static void Warn(string message) { Write(LogLevel.Warn, message, null); }
        public static void Warn(string message, Exception ex) { Write(LogLevel.Warn, message, ex); }
        public static void Error(string message) { Write(LogLevel.Error, message, null); }
        public static void Error(string message, Exception ex) { Write(LogLevel.Error, message, ex); }
        public static void Fatal(string message, Exception ex) { Write(LogLevel.Fatal, message, ex); }

        /// <summary>Lỗi này đã được ghi log rồi (tránh ghi trùng khi lỗi đi qua nhiều tầng).</summary>
        public static bool WasLogged(Exception ex)
        {
            try { return ex != null && ex.Data.Contains(LoggedKey); } catch { return false; }
        }

        /// <summary>Ghi tiêu đề phiên làm việc: phiên bản, hệ điều hành, đường dẫn... để hỗ trợ khách hàng.</summary>
        public static void LogStartup()
        {
            try
            {
                var ver = Assembly.GetExecutingAssembly().GetName().Version;
                Info("================ KHỞI ĐỘNG SmartHRM v" + ver + " ================");
                Info("Hệ điều hành: " + Environment.OSVersion + (Environment.Is64BitOperatingSystem ? " (64-bit)" : " (32-bit)") +
                     " | Tiến trình: " + (Environment.Is64BitProcess ? "64-bit" : "32-bit") + " | .NET " + Environment.Version);
                Info("Máy: " + Environment.MachineName + " | Tài khoản Windows: " + Environment.UserName);
                Info("Thư mục chạy: " + AppDomain.CurrentDomain.BaseDirectory);
                Info("Cấp độ log: " + minLevel + " | Thư mục log: " + Folder + " | Giữ " + retentionDays + " ngày");
            }
            catch { }
        }

        public static void OpenFolder()
        {
            try { Process.Start("explorer.exe", "\"" + Folder + "\""); } catch { }
        }

        public static void OpenCurrentFile()
        {
            try
            {
                var f = CurrentFile;
                if (!File.Exists(f)) Info("Mở file log để xem.");
                Process.Start("notepad.exe", "\"" + f + "\"");
            }
            catch { }
        }

        // ---------------------------------------------------------------- nội bộ
        static string Tag(LogLevel l)
        {
            switch (l)
            {
                case LogLevel.Debug: return "DEBUG";
                case LogLevel.Info: return "INFO ";
                case LogLevel.Warn: return "WARN ";
                case LogLevel.Error: return "ERROR";
                default: return "FATAL";
            }
        }

        static string User()
        {
            try
            {
                var f = UserProvider;
                var u = f == null ? null : f();
                return string.IsNullOrEmpty(u) ? "-" : u;
            }
            catch { return "-"; }
        }

        static string Scrub(string s)
        {
            return string.IsNullOrEmpty(s) ? s : SecretRx.Replace(s, "$1=***");
        }

        static void Write(LogLevel level, string message, Exception ex)
        {
            try
            {
                Init();
                if (level < minLevel) return;
                var sb = new StringBuilder();
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")).Append(" [").Append(Tag(level)).Append("] [")
                  .Append(User()).Append("] [T").Append(Thread.CurrentThread.ManagedThreadId.ToString("00")).Append("] ")
                  .Append(Scrub(message));
                if (ex != null) AppendException(sb, ex);
                lock (sync)
                {
                    File.AppendAllText(PathForToday(), sb.ToString() + Environment.NewLine, new UTF8Encoding(true));
                }
                if (ex != null) { try { ex.Data[LoggedKey] = true; } catch { } }
            }
            catch { /* ghi log lỗi thì bỏ qua, không làm hỏng chương trình */ }
        }

        static void AppendException(StringBuilder sb, Exception ex)
        {
            int depth = 0;
            for (var e = ex; e != null && depth < 5; e = e.InnerException, depth++)
            {
                sb.AppendLine().Append(depth == 0 ? "    >> " : "    >> (nội bộ) ").Append(e.GetType().FullName).Append(": ").Append(Scrub(e.Message));
                if (!string.IsNullOrEmpty(e.StackTrace))
                    foreach (var line in e.StackTrace.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                        sb.AppendLine().Append("          ").Append(line.Trim());
            }
        }

        static string PathForToday()
        {
            var baseName = "SmartHRM_" + DateTime.Now.ToString("yyyyMMdd");
            for (int n = 1; n < 50; n++)
            {
                var path = Path.Combine(folder, baseName + (n == 1 ? "" : "_" + n) + ".log");
                try { if (!File.Exists(path) || new FileInfo(path).Length < MaxFileBytes) return path; }
                catch { return path; }
            }
            return Path.Combine(folder, baseName + "_overflow.log");
        }

        static void CleanOld()
        {
            try
            {
                var limit = DateTime.Now.AddDays(-retentionDays);
                foreach (var f in Directory.GetFiles(folder, "SmartHRM_*.log"))
                    try { if (File.GetLastWriteTime(f) < limit) File.Delete(f); } catch { }
            }
            catch { }
        }
    }
}
