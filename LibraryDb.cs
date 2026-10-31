using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace QuanLyThuVienQuocGia
{
    // All circulation writes run in one serializable transaction. A failed validation
    // rolls back the header, details, inventory, card count and payment together.
    internal static class LibraryDb
    {
        internal static string ConnectionOverride;
        public static string ConnectionString
        {
            get
            {
                if (!string.IsNullOrEmpty(ConnectionOverride)) return ConnectionOverride;
                return new SqlConnectionStringBuilder {
                    DataSource = LocalDbStartup.DataSource, AttachDBFilename = ProcessDataBase.LayDuongDanMdf(),
                    IntegratedSecurity = true, ConnectTimeout = 30
                }.ConnectionString;
            }
        }
        public static SqlParameter P(string name, object value) { return new SqlParameter(name, value ?? DBNull.Value); }
        public static SqlConnection OpenConnection()
        {
            var cn = new SqlConnection(ConnectionString);
            try { cn.Open(); return cn; }
            catch (SqlException ex)
            {
                cn.Dispose();
                // Retry only a failed connection, never a write/transaction which may
                // already have committed. Also covers LocalDB stopping after inactivity.
                int[] transient = { -2, 0, 2, 20, 26, 40, 50, 53, 64, 233, 258, 10053, 10054, 10060 };
                if (!string.IsNullOrEmpty(ConnectionOverride) || !transient.Contains(ex.Number)) throw;
                SqlConnection.ClearAllPools();
                LocalDbStartup.EnsureRunning();
                cn = new SqlConnection(ConnectionString);
                try { cn.Open(); return cn; } catch { cn.Dispose(); throw; }
            }
            catch { cn.Dispose(); throw; }
        }
        internal static SqlCommand Command(SqlConnection cn, SqlTransaction tx, string sql, params SqlParameter[] ps)
        {
            var cmd = new SqlCommand(sql, cn, tx) { CommandTimeout = 30 };
            cmd.Parameters.AddRange(ps); return cmd;
        }
        public static DataTable Query(string sql, params SqlParameter[] ps)
        {
            using (var cn = OpenConnection())
            using (var cmd = Command(cn, null, sql, ps))
            using (var da = new SqlDataAdapter(cmd)) { var dt = new DataTable(); da.Fill(dt); return dt; }
        }
        internal static object Scalar(SqlConnection cn, SqlTransaction tx, string sql, params SqlParameter[] ps)
        { using (var cmd = Command(cn, tx, sql, ps)) return cmd.ExecuteScalar(); }
        internal static int Execute(SqlConnection cn, SqlTransaction tx, string sql, params SqlParameter[] ps)
        { using (var cmd = Command(cn, tx, sql, ps)) return cmd.ExecuteNonQuery(); }
        internal static DataTable Query(SqlConnection cn, SqlTransaction tx, string sql, params SqlParameter[] ps)
        {
            using (var cmd = Command(cn, tx, sql, ps))
            using (var da = new SqlDataAdapter(cmd)) { var dt = new DataTable(); da.Fill(dt); return dt; }
        }
        public static T Transaction<T>(Func<SqlConnection, SqlTransaction, T> work)
        {
            using (var cn = OpenConnection())
            {
                using (var tx = cn.BeginTransaction(IsolationLevel.Serializable))
                { try { T result = work(cn, tx); tx.Commit(); return result; } catch { try { tx.Rollback(); } catch { } throw; } }
            }
        }
        internal static int NextId(SqlConnection cn, SqlTransaction tx, string table, string key)
        { return Convert.ToInt32(Scalar(cn, tx, "SELECT ISNULL(MAX([" + key + "]),0)+1 FROM [" + table + "] WITH(UPDLOCK,HOLDLOCK)")); }
        public static void Initialize()
        {
            string sql;
            using (var s = typeof(LibraryDb).Assembly.GetManifestResourceStream("LibraryUpgrade.sql"))
            using (var r = new StreamReader(s)) sql = r.ReadToEnd();
            Transaction((cn, tx) => { Execute(cn, tx, sql); return 0; });
        }
        public static int Borrow(int card, int librarian, DateTime date, decimal deposit, int[] books)
        {
            date = date.Date;
            if (date > DateTime.Today) throw new InvalidOperationException("Ngày mượn không được ở tương lai.");
            if (deposit < 0) throw new InvalidOperationException("Tạm ứng không được âm.");
            if (books == null || books.Length == 0) throw new InvalidOperationException("Chọn ít nhất một sách để mượn.");
            if (books.Distinct().Count() != books.Length) throw new InvalidOperationException("Một sách chỉ được chọn một lần trong hồ sơ.");
            return Transaction((cn, tx) => {
                var c = Query(cn, tx, "SELECT NgayLamThe,NgayHetHan FROM tblTheMuon WITH(UPDLOCK,HOLDLOCK) WHERE MaThe=@c", P("@c",card));
                if (c.Rows.Count == 0) throw new InvalidOperationException("Thẻ mượn không tồn tại.");
                if (c.Rows[0]["NgayHetHan"] == DBNull.Value || Convert.ToDateTime(c.Rows[0]["NgayHetHan"]).Date < DateTime.Today || Convert.ToDateTime(c.Rows[0]["NgayHetHan"]).Date < date)
                    throw new InvalidOperationException("Thẻ đã hết hạn hoặc chưa khai báo ngày hết hạn.");
                if (c.Rows[0]["NgayLamThe"] != DBNull.Value && Convert.ToDateTime(c.Rows[0]["NgayLamThe"]).Date > date)
                    throw new InvalidOperationException("Ngày mượn phải từ ngày làm thẻ trở đi.");
                int outstanding = Convert.ToInt32(Scalar(cn, tx, @"SELECT COUNT(*) FROM tblChiTietHSMuon d WITH(UPDLOCK,HOLDLOCK)
                    JOIN tblHoSoMuon h ON h.MaHSM=d.MaHSM WHERE h.MaThe=@c AND d.ChuaTra=1", P("@c",card)));
                if (outstanding >= 3 || outstanding + books.Length > 3)
                    throw new InvalidOperationException("Mỗi thẻ chỉ được mượn tối đa 3 cuốn cùng lúc. Đang mượn: " + outstanding + ".");
                if (Convert.ToInt32(Scalar(cn, tx, "SELECT COUNT(*) FROM tblThuThu WHERE MaThuThu=@t", P("@t",librarian))) != 1)
                    throw new InvalidOperationException("Vui lòng chọn thủ thư hợp lệ.");
                int id = NextId(cn, tx, "tblHoSoMuon", "MaHSM");
                Execute(cn, tx, "INSERT tblHoSoMuon(MaHSM,MaThe,MaThuThu,NgayMuon,TamUng) VALUES(@id,@c,@t,@d,@a)", P("@id",id),P("@c",card),P("@t",librarian),P("@d",date),P("@a",deposit));
                foreach (int book in books.OrderBy(x=>x))
                {
                    var s = Query(cn, tx, "SELECT TenSach,SoLuong,DonGiaThue FROM tblDMSach WITH(UPDLOCK,HOLDLOCK) WHERE MaSach=@s", P("@s",book));
                    if (s.Rows.Count == 0 || Convert.ToInt32(s.Rows[0]["SoLuong"]) <= 0)
                        throw new InvalidOperationException("Sách mã " + book + " đã hết hoặc không tồn tại.");
                    if (s.Rows[0]["DonGiaThue"] == DBNull.Value || Convert.ToDecimal(s.Rows[0]["DonGiaThue"]) < 0)
                        throw new InvalidOperationException("Chưa khai báo đơn giá thuê hợp lệ cho sách " + s.Rows[0]["TenSach"] + ".");
                    Execute(cn, tx, "UPDATE tblDMSach SET SoLuong=SoLuong-1 WHERE MaSach=@s; INSERT tblChiTietHSMuon(MaHSM,MaSach,ChuaTra,DonGiaThueLucMuon) VALUES(@id,@s,1,@p)",
                        P("@s",book),P("@id",id),P("@p",s.Rows[0]["DonGiaThue"]));
                }
                Execute(cn, tx, "UPDATE tblTheMuon SET SoLuongMuon=@n WHERE MaThe=@c",P("@n",outstanding+books.Length),P("@c",card));
                return id;
            });
        }
        public static DataTable Outstanding(int loan)
        {
            return Query(@"SELECT d.MaSach,s.TenSach,h.NgayMuon,ISNULL(d.DonGiaThueLucMuon,s.DonGiaThue) AS DonGiaThue
                FROM tblChiTietHSMuon d JOIN tblHoSoMuon h ON h.MaHSM=d.MaHSM JOIN tblDMSach s ON s.MaSach=d.MaSach
                WHERE d.MaHSM=@h AND d.ChuaTra=1 ORDER BY d.MaSach",P("@h",loan));
        }
        public static int Return(int loan, int librarian, DateTime date, IDictionary<int,int?> books)
        {
            date = date.Date;
            if (date > DateTime.Today) throw new InvalidOperationException("Ngày trả không được ở tương lai.");
            if (books == null || books.Count == 0) throw new InvalidOperationException("Chọn ít nhất một sách chưa trả.");
            return Transaction((cn,tx) => {
                var h = Query(cn,tx,"SELECT MaThe,NgayMuon,ISNULL(TamUng,0) TamUng FROM tblHoSoMuon WITH(UPDLOCK,HOLDLOCK) WHERE MaHSM=@h",P("@h",loan));
                if (h.Rows.Count==0) throw new InvalidOperationException("Hồ sơ mượn không tồn tại.");
                DateTime borrowed=Convert.ToDateTime(h.Rows[0]["NgayMuon"]).Date;
                if (date < borrowed) throw new InvalidOperationException("Ngày trả không được trước ngày mượn.");
                if (Convert.ToInt32(Scalar(cn,tx,"SELECT COUNT(*) FROM tblThuThu WHERE MaThuThu=@t",P("@t",librarian)))!=1) throw new InvalidOperationException("Vui lòng chọn thủ thư hợp lệ.");
                int card=Convert.ToInt32(h.Rows[0]["MaThe"]);
                Scalar(cn,tx,"SELECT MaThe FROM tblTheMuon WITH(UPDLOCK,HOLDLOCK) WHERE MaThe=@c",P("@c",card));
                int id=NextId(cn,tx,"tblHoSoTra","MaHSTra");
                Execute(cn,tx,"INSERT tblHoSoTra(MaHSTra,MaHSM,NgayTra,MaThuThu,TongTien,SoTienThanhToan,TamUngApDung) VALUES(@id,@h,@d,@t,0,0,0)",P("@id",id),P("@h",loan),P("@d",date),P("@t",librarian));
                decimal total=0; int days=(date-borrowed).Days;
                foreach(var item in books.OrderBy(x=>x.Key))
                {
                    var d=Query(cn,tx,@"SELECT ISNULL(d.DonGiaThueLucMuon,s.DonGiaThue) Price FROM tblChiTietHSMuon d WITH(UPDLOCK,HOLDLOCK)
                        JOIN tblDMSach s ON s.MaSach=d.MaSach WHERE d.MaHSM=@h AND d.MaSach=@s AND d.ChuaTra=1",P("@h",loan),P("@s",item.Key));
                    if(d.Rows.Count!=1) throw new InvalidOperationException("Sách " + item.Key + " không thuộc hồ sơ hoặc đã trả.");
                    if(d.Rows[0]["Price"]==DBNull.Value) throw new InvalidOperationException("Sách chưa có đơn giá thuê. Cần cập nhật danh mục trước khi trả.");
                    decimal price=Convert.ToDecimal(d.Rows[0]["Price"]), fine=0;
                    if(item.Value.HasValue)
                    {
                        var vp=Query(cn,tx,"SELECT TienPhat FROM tblViPham WHERE MaViPham=@v",P("@v",item.Value.Value));
                        if(vp.Rows.Count!=1 || vp.Rows[0][0]==DBNull.Value) throw new InvalidOperationException("Vi phạm chưa có mức tiền phạt.");
                        fine=Convert.ToDecimal(vp.Rows[0][0]);
                    }
                    if(price<0 || fine<0) throw new InvalidOperationException("Đơn giá và tiền phạt không được âm.");
                    decimal amount=decimal.Round(days*price+fine,2); total+=amount;
                    Execute(cn,tx,@"INSERT tblChiTietHSTra(MaHSTra,MaSach,MaViPham,ThanhTien,SoNgayThue,DonGiaThue,TienPhat)
                        VALUES(@id,@s,@v,@a,@n,@p,@f); UPDATE tblChiTietHSMuon SET ChuaTra=0 WHERE MaHSM=@h AND MaSach=@s;
                        UPDATE tblDMSach SET SoLuong=SoLuong+1 WHERE MaSach=@s",P("@id",id),P("@s",item.Key),P("@v",item.Value),P("@a",amount),P("@n",days),P("@p",price),P("@f",fine),P("@h",loan));
                }
                int remain=Convert.ToInt32(Scalar(cn,tx,"SELECT COUNT(*) FROM tblChiTietHSMuon WHERE MaHSM=@h AND ChuaTra=1",P("@h",loan)));
                decimal used=Convert.ToDecimal(Scalar(cn,tx,"SELECT ISNULL(SUM(TamUngApDung),0) FROM tblHoSoTra WHERE MaHSM=@h",P("@h",loan)));
                // The advance is settled exactly once, on the final return, rather than
                // being deducted again on every partial return. Negative payable = refund.
                decimal applied=remain==0 ? Convert.ToDecimal(h.Rows[0]["TamUng"])-used : 0;
                if(applied<0) throw new InvalidOperationException("Dữ liệu tạm ứng không nhất quán.");
                Execute(cn,tx,"UPDATE tblHoSoTra SET TongTien=@a,TamUngApDung=@u,SoTienThanhToan=@a-@u WHERE MaHSTra=@id",P("@a",total),P("@u",applied),P("@id",id));
                Execute(cn,tx,@"UPDATE tblTheMuon SET SoLuongMuon=(SELECT COUNT(*) FROM tblChiTietHSMuon d JOIN tblHoSoMuon h ON h.MaHSM=d.MaHSM WHERE h.MaThe=@c AND d.ChuaTra=1) WHERE MaThe=@c",P("@c",card));
                return id;
            });
        }
    }
}
