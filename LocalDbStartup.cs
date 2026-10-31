using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace QuanLyThuVienQuocGia
{
    internal static class LocalDbStartup
    {
        private static readonly object Sync = new object();
        public static string DataSource { get; private set; } = @"(LocalDB)\MSSQLLocalDB";

        // Start the native utility outside the application's directory/debugger, then
        // use its actual named pipe. This avoids relying on SqlClient's auto-start.
        public static void EnsureRunning(string instance = "MSSQLLocalDB")
        {
            if (!Regex.IsMatch(instance, @"^[A-Za-z][A-Za-z0-9_]{0,100}$"))
                throw new ArgumentException("Tên LocalDB không hợp lệ.");
            lock (Sync)
            {
                string tool = FindTool();
                string details;
                if (Run(tool, "info \"" + instance + "\"", out details) != 0)
                {
                    if (Run(tool, "create \"" + instance + "\"", out details) != 0)
                        throw new InvalidOperationException("Không tạo được LocalDB " + instance + ".\n" + details);
                }
                if (Run(tool, "start \"" + instance + "\"", out details) != 0)
                    throw new InvalidOperationException("Không khởi động được SQL Server LocalDB " + instance + ".\n" + details);
                if (Run(tool, "info \"" + instance + "\"", out details) != 0)
                    throw new InvalidOperationException("Không đọc được thông tin LocalDB.\n" + details);
                // Match only the local pipe address, independent of the utility's UI language.
                var pipe = Regex.Match(details, @"(?:np:)?\\\\\.\\pipe\\LOCALDB#[^\s]+");
                if (!pipe.Success) throw new InvalidOperationException("LocalDB đã chạy nhưng chưa có địa chỉ kết nối.\n" + details);
                string server = pipe.Value.StartsWith("np:", StringComparison.OrdinalIgnoreCase) ? pipe.Value : "np:" + pipe.Value;
                Exception last = null;
                for (int attempt = 0; attempt < 3; attempt++)
                {
                    try
                    {
                        using (var cn = new SqlConnection(new SqlConnectionStringBuilder {
                            DataSource = server, InitialCatalog = "master", IntegratedSecurity = true,
                            ConnectTimeout = 5, Pooling = false
                        }.ConnectionString)) cn.Open();
                        DataSource = server;
                        return;
                    }
                    catch (SqlException ex) { last = ex; if (attempt < 2) Thread.Sleep(500); }
                }
                throw new InvalidOperationException("LocalDB đã được khởi động nhưng chưa nhận kết nối.\n" + last.Message, last);
            }
        }

        internal static string FindTool()
        {
            var roots = new[] { Environment.GetEnvironmentVariable("ProgramW6432"),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                Environment.GetEnvironmentVariable("ProgramFiles(x86)") };
            foreach (string root in roots.Where(p => !string.IsNullOrWhiteSpace(p)).Distinct())
            {
                string sqlRoot = Path.Combine(root, "Microsoft SQL Server");
                if (!Directory.Exists(sqlRoot)) continue;
                foreach (string dir in Directory.GetDirectories(sqlRoot).OrderByDescending(VersionNumber))
                {
                    string path = Path.Combine(dir, "Tools", "Binn", "SqlLocalDB.exe");
                    if (File.Exists(path)) return path;
                }
            }
            foreach (string dir in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(';').Where(p => !string.IsNullOrWhiteSpace(p)))
            {
                string path = Path.Combine(dir.Trim('"'), "SqlLocalDB.exe");
                if (File.Exists(path)) return path;
            }
            throw new FileNotFoundException("Máy chưa có công cụ SQL Server LocalDB. Hãy cài LocalDB rồi chạy lại chương trình.");
        }
        private static int VersionNumber(string path)
        { int version; return int.TryParse(Path.GetFileName(path), out version) ? version : 0; }
        private static int Run(string tool, string arguments, out string details)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo(tool, arguments) {
                    UseShellExecute = false, CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden,
                    WorkingDirectory = Path.GetDirectoryName(tool), RedirectStandardOutput = true, RedirectStandardError = true
                };
                process.Start();
                var output = process.StandardOutput.ReadToEndAsync();
                var error = process.StandardError.ReadToEndAsync();
                if (!process.WaitForExit(30000))
                {
                    try { process.Kill(); } catch { }
                    throw new TimeoutException("LocalDB chưa phản hồi sau 30 giây. Vui lòng kiểm tra SQL Server LocalDB trên máy.");
                }
                details = (output.GetAwaiter().GetResult() + "\n" + error.GetAwaiter().GetResult()).Trim();
                return process.ExitCode;
            }
        }
    }
}
