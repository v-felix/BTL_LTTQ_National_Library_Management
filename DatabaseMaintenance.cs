using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace QuanLyThuVienQuocGia
{
    internal static class DatabaseMaintenance
    {
        static string Quote(string name) { return "["+name.Replace("]","]]")+"]"; }
        static SqlConnection Master()
        {
            var b=new SqlConnectionStringBuilder(LibraryDb.ConnectionString);b.AttachDBFilename="";b.InitialCatalog="master";b.Pooling=false;
            var cn=new SqlConnection(b.ConnectionString);cn.Open();return cn;
        }
        static string CurrentName()
        {using(var cn=LibraryDb.OpenConnection()){return cn.Database;}}
        public static void Backup(string path)
        {
            string name=CurrentName();
            using(var cn=Master())using(var cmd=new SqlCommand("BACKUP DATABASE "+Quote(name)+" TO DISK=@file WITH COPY_ONLY, INIT, CHECKSUM",cn))
            {cmd.CommandTimeout=120;cmd.Parameters.AddWithValue("@file",path);cmd.ExecuteNonQuery();}
        }
        public static void BackupDialog()
        {
            using(var dlg=new SaveFileDialog{Filter="SQL Server backup (*.bak)|*.bak",FileName="ThuVien_"+DateTime.Now.ToString("yyyyMMdd_HHmmss")+".bak"})
            {if(dlg.ShowDialog()!=DialogResult.OK)return;Backup(dlg.FileName);MessageBox.Show("Đã sao lưu CSDL vào:\n"+dlg.FileName,"Sao lưu");}
        }
        public static void RestoreDialog()
        {
            using(var dlg=new OpenFileDialog{Filter="SQL Server backup (*.bak)|*.bak"})
            {
                if(dlg.ShowDialog()!=DialogResult.OK)return;
                if(MessageBox.Show("Phục hồi sẽ thay dữ liệu hiện tại bằng bản sao đã chọn. Chương trình sẽ kiểm tra bản sao và tự sao lưu dữ liệu hiện tại trước khi thay. Tiếp tục?","Phục hồi CSDL",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)!=DialogResult.Yes)return;
                Restore(dlg.FileName);MessageBox.Show("Đã phục hồi CSDL. Các màn hình mở sau đây dùng dữ liệu vừa phục hồi.","Phục hồi");
            }
        }
        public static void Restore(string backup)
        {
            string target=CurrentName(),mdf=Convert.ToString(LibraryDb.Query("SELECT physical_name FROM sys.database_files WHERE type=0").Rows[0][0]);
            string folder=Path.Combine(Path.GetDirectoryName(mdf),"Backups");Directory.CreateDirectory(folder);
            string staging="LibraryRestore_"+Guid.NewGuid().ToString("N"),stageMdf=Path.Combine(folder,staging+".mdf"),stageLog=Path.Combine(folder,staging+"_log.ldf");
            string autoBackup=Path.Combine(folder,"BeforeRestore_"+DateTime.Now.ToString("yyyyMMdd_HHmmss_fff")+".bak");
            using(var cn=Master())
            {
                var files=new DataTable();using(var cmd=new SqlCommand("RESTORE FILELISTONLY FROM DISK=@file",cn))using(var da=new SqlDataAdapter(cmd)){cmd.Parameters.AddWithValue("@file",backup);da.Fill(files);}
                if(files.Rows.Count!=2)throw new InvalidOperationException("Bản sao cần có đúng một tệp dữ liệu và một tệp nhật ký.");
                string logicalData=null,logicalLog=null;
                foreach(DataRow r in files.Rows){if(Convert.ToString(r["Type"])=="D")logicalData=Convert.ToString(r["LogicalName"]);if(Convert.ToString(r["Type"])=="L")logicalLog=Convert.ToString(r["LogicalName"]);}
                if(logicalData==null || logicalLog==null)throw new InvalidOperationException("Bản sao không hợp lệ.");
                bool created=false;
                try
                {
                    RestoreTo(cn,staging,backup,logicalData,logicalLog,stageMdf,stageLog,false);created=true;
                    using(var check=new SqlCommand("SELECT COUNT(*) FROM "+Quote(staging)+".sys.tables WHERE name IN ('tblDMSach','tblTheMuon','tblHoSoMuon','tblHoSoTra','tblChiTietHSMuon','tblChiTietHSTra','tblThuThu','tblViPham','tblNXB','tblTheLoai','tblNgonNgu','tblQue','tblDanToc')",cn))
                    {if(Convert.ToInt32(check.ExecuteScalar())!=13)throw new InvalidOperationException("Bản sao không phải CSDL thư viện phù hợp.");}
                    // Migrate and validate the staging copy before touching the active database.
                    string previous=LibraryDb.ConnectionOverride;
                    try{var b=new SqlConnectionStringBuilder(LibraryDb.ConnectionString){AttachDBFilename="",InitialCatalog=staging,Pooling=false};LibraryDb.ConnectionOverride=b.ConnectionString;LibraryDb.Initialize();}
                    finally{LibraryDb.ConnectionOverride=previous;}
                    Backup(autoBackup);SqlConnection.ClearAllPools();
                    var currentFiles=LibraryDb.Query("SELECT type,name,physical_name FROM sys.database_files");
                    string log=Convert.ToString(currentFiles.Select("type=1")[0]["physical_name"]);
                    string originalData=Convert.ToString(currentFiles.Select("type=0")[0]["name"]),originalLog=Convert.ToString(currentFiles.Select("type=1")[0]["name"]);
                    using(var single=new SqlCommand("ALTER DATABASE "+Quote(target)+" SET SINGLE_USER WITH ROLLBACK IMMEDIATE",cn))single.ExecuteNonQuery();
                    try{RestoreTo(cn,target,backup,logicalData,logicalLog,mdf,log,true);}
                    catch
                    {
                        // Return to the automatic backup if SQL Server failed after replacement started.
                        RestoreTo(cn,target,autoBackup,originalData,originalLog,mdf,log,true);throw;
                    }
                    finally{using(var multi=new SqlCommand("ALTER DATABASE "+Quote(target)+" SET MULTI_USER",cn))multi.ExecuteNonQuery();}
                }
                finally
                {
                    if(created)using(var drop=new SqlCommand("ALTER DATABASE "+Quote(staging)+" SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE "+Quote(staging),cn))drop.ExecuteNonQuery();
                }
            }
            LibraryDb.Initialize();
        }
        static void RestoreTo(SqlConnection cn,string name,string backup,string data,string log,string mdf,string ldf,bool replace)
        {
            using(var cmd=new SqlCommand("RESTORE DATABASE "+Quote(name)+" FROM DISK=@file WITH MOVE @data TO @mdf, MOVE @log TO @ldf"+(replace?", REPLACE":""),cn))
            {cmd.CommandTimeout=120;cmd.Parameters.AddWithValue("@file",backup);cmd.Parameters.AddWithValue("@data",data);cmd.Parameters.AddWithValue("@log",log);cmd.Parameters.AddWithValue("@mdf",mdf);cmd.Parameters.AddWithValue("@ldf",ldf);cmd.ExecuteNonQuery();}
        }
    }
}
