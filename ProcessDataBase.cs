using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyThuVienQuocGia
{
    internal class ProcessDataBase
    {
        private SqlConnection Conn;
        private static string _duongDanMdf;

        public static string LayDuongDanMdf()
        {
            if (!string.IsNullOrEmpty(_duongDanMdf) && File.Exists(_duongDanMdf))
            {
                return _duongDanMdf;
            }

            string asmDir = Path.GetDirectoryName(typeof(ProcessDataBase).Assembly.Location);
            var cacViTri = new List<string>();
            // Debug and Release use the same project Database, never stale output copies.
            var parent = new DirectoryInfo(asmDir);
            for (int depth = 0; parent != null && depth < 4; depth++, parent = parent.Parent)
                if (File.Exists(Path.Combine(parent.FullName, "QuanLyThuVienQuocGia.csproj")))
                    cacViTri.Add(Path.Combine(parent.FullName, "Database", "DuLieu.mdf"));
            // A standalone published application keeps its database next to the executable.
            cacViTri.Add(Path.Combine(asmDir, "Database", "DuLieu.mdf"));

            foreach (string p in cacViTri)
            {
                try
                {
                    string full = Path.GetFullPath(p);
                    if (File.Exists(full))
                    {
                        _duongDanMdf = full;
                        AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetDirectoryName(full));
                        return _duongDanMdf;
                    }
                }
                catch { }
            }

            throw new FileNotFoundException("Không tìm thấy Database\\DuLieu.mdf. Hãy sao chép đầy đủ thư mục chương trình.");
        }

        public void KetNoiCSDL()
        {
            if (Conn == null || Conn.State != ConnectionState.Open)
            {
                if (Conn != null) Conn.Dispose();
                Conn = LibraryDb.OpenConnection();
            }
        }

        public void DongKetNoiCSDL()
        {
            if (Conn != null && Conn.State != ConnectionState.Closed)
            {
                Conn.Close();
                Conn.Dispose();
                Conn = null;
            }
        }

        public DataTable DocBang(string strSQL)
        {
            DataTable dt = new DataTable();
            try
            {
                KetNoiCSDL();
                using (SqlDataAdapter DA = new SqlDataAdapter(strSQL, Conn))
                {
                    DA.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đọc CSDL: " + ex.Message, "Lỗi truy vấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DongKetNoiCSDL();
            }
            return dt;
        }

        public bool CapNhatDuLieu(string strSQL)
        {
            try
            {
                KetNoiCSDL();
                using (SqlCommand cmd = new SqlCommand(strSQL, Conn))
                {
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật CSDL: " + ex.Message, "Lỗi thực thi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                DongKetNoiCSDL();
            }
        }

        public DataSet DocDataSet(string strSQL)
        {
            DataSet ds = new DataSet();
            try
            {
                KetNoiCSDL();
                using (SqlDataAdapter DA = new SqlDataAdapter(strSQL, Conn))
                {
                    DA.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đọc CSDL (DataSet): " + ex.Message, "Lỗi truy vấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DongKetNoiCSDL();
            }
            return ds;
        }
    }
}
