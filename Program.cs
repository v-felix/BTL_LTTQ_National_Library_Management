using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyThuVienQuocGia
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                ProcessDataBase.LayDuongDanMdf();
                LocalDbStartup.EnsureRunning();
                LibraryDb.Initialize();
                Application.Run(new frmMain());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi động thư viện: " + ex.Message,
                    "Kiểm tra CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
