using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace QuanLyThuVienQuocGia
{
    internal sealed class CatalogField
    {
        public string Name, Label, Kind, Reference;
        public bool Required, ReadOnly;
        public int MaxLength;
        public CatalogField(string name,string label,string kind="text",bool required=false,string reference=null,int max=100)
        { Name=name;Label=label;Kind=kind;Required=required;Reference=reference;MaxLength=max; }
    }
    internal sealed class CatalogSpec
    {
        public string Table,Key,Title;
        public CatalogField[] Fields;
        public CatalogSpec(string table,string key,string title,params CatalogField[] fields)
        { Table=table;Key=key;Title=title;Fields=fields; }
    }
    internal static class Catalog
    {
        static CatalogField F(string n,string l,string k="text",bool r=false,string lookup=null,int max=100) { return new CatalogField(n,l,k,r,lookup,max); }
        public static readonly Dictionary<string,CatalogSpec> Specs=new Dictionary<string,CatalogSpec> {
            {"sach",new CatalogSpec("tblDMSach","MaSach","Quản lý sách",
                F("TenSach","Tên sách","text",true,null,200),F("TenTG","Tác giả"),F("MaNXB","Nhà xuất bản","lookup",false,"nxb"),
                F("MaXB","Năm xuất bản","int"),F("KhoSach","Khổ sách","text",false,null,50),F("GiaSach","Giá sách","money"),F("LanTaiBan","Lần tái bản","int"),
                F("MaTheLoai","Thể loại","lookup",false,"theloai"),F("MaNgonNgu","Ngôn ngữ","lookup",false,"ngonngu"),F("SoTrang","Số trang","int"),
                F("TomTatND","Tóm tắt nội dung","memo",false,null,0),F("SoLuong","Số lượng còn trong kho","int",true),F("DonGiaThue","Đơn giá thuê/ngày","money",true))},
            {"the",new CatalogSpec("tblTheMuon","MaThe","Quản lý thẻ mượn",
                F("HoTen","Họ tên","text",true),F("GioiTinh","Giới tính","text",false,null,10),F("NgaySinh","Ngày sinh","date"),
                F("MaQue","Quê quán","lookup",false,"que"),F("DiaChiHienTai","Địa chỉ hiện tại","text",false,null,200),F("MaDanToc","Dân tộc","lookup",false,"dantoc"),
                F("CMTND","CMTND/CCCD","text",false,null,20),F("NgayLamThe","Ngày làm thẻ","date",true),F("NgayHetHan","Ngày hết hạn","date",true),
                new CatalogField("SoLuongMuon","Số sách đang mượn","int"){ReadOnly=true})},
            {"thuthu",new CatalogSpec("tblThuThu","MaThuThu","Quản lý và tìm kiếm thủ thư",
                F("TenThuThu","Tên thủ thư","text",true),F("DiaChi","Địa chỉ","text",false,null,200),F("DienThoaiCD","Điện thoại cố định","text",false,null,20),
                F("DienThoaiDD","Điện thoại di động","text",false,null,20),F("MaQue","Quê quán","lookup",false,"que"))},
            {"nxb",new CatalogSpec("tblNXB","MaNXB","Nhà xuất bản",F("TenNXB","Tên nhà xuất bản","text",true),F("DiaChi","Địa chỉ","text",false,null,200),F("DienThoai","Điện thoại","text",false,null,20),F("Email","Email","text",false,null,100))},
            {"theloai",new CatalogSpec("tblTheLoai","MaTheLoai","Thể loại sách",F("TenTheLoai","Tên thể loại","text",true))},
            {"ngonngu",new CatalogSpec("tblNgonNgu","MaNgonNgu","Ngôn ngữ",F("TenNgonNgu","Tên ngôn ngữ","text",true))},
            {"que",new CatalogSpec("tblQue","MaQue","Quê quán",F("TenQue","Tên quê","text",true))},
            {"dantoc",new CatalogSpec("tblDanToc","MaDanToc","Dân tộc",F("TenDanToc","Tên dân tộc","text",true))},
            {"vipham",new CatalogSpec("tblViPham","MaViPham","Danh mục vi phạm và tiền phạt",F("TenViPham","Tên vi phạm","text",true,null,200),F("TienPhat","Tiền phạt","money",true))}
        };
        public static DataTable Lookup(string key)
        {
            var s=Specs[key]; return LibraryDb.Query("SELECT ["+s.Key+"] Id,CONVERT(nvarchar(20),["+s.Key+"])+N' — '+["+s.Fields[0].Name+"] Name FROM ["+s.Table+"] ORDER BY ["+s.Key+"]");
        }
        public static DataTable List(CatalogSpec s,string search)
        {
            var columns=new List<string>{"c.*"}; var joins=new List<string>(); var terms=new List<string>{"CONVERT(nvarchar(20),c.["+s.Key+"]) LIKE @q ESCAPE '~'"};
            foreach(var f in s.Fields)
            {
                if(f.Reference!=null)
                {
                    var r=Specs[f.Reference]; string alias="r"+f.Name;
                    joins.Add(" LEFT JOIN ["+r.Table+"] "+alias+" ON c.["+f.Name+"]="+alias+".["+r.Key+"]");
                    columns.Add(alias+".["+r.Fields[0].Name+"] AS ["+f.Name+"Text]");
                    terms.Add(alias+".["+r.Fields[0].Name+"] LIKE @q ESCAPE '~'");
                }
                else if(f.Kind=="text" || f.Kind=="memo") terms.Add("c.["+f.Name+"] LIKE @q ESCAPE '~'");
            }
            string q="%"+(search??"").Trim().Replace("~","~~").Replace("%","~%").Replace("_","~_").Replace("[","~[")+"%";
            return LibraryDb.Query("SELECT "+string.Join(",",columns)+" FROM ["+s.Table+"] c"+string.Join("",joins)+" WHERE ("+string.Join(" OR ",terms)+") ORDER BY c.["+s.Key+"]",LibraryDb.P("@q",q));
        }
        public static int Save(CatalogSpec s,int? id,Dictionary<string,object> values,int? originalStock=null)
        {
            foreach(var f in s.Fields.Where(x=>!x.ReadOnly))
            {
                object v=values[f.Name]; bool empty=v==null || v==DBNull.Value || (v is string && string.IsNullOrWhiteSpace((string)v));
                if(f.Required && empty) throw new InvalidOperationException("Vui lòng nhập "+f.Label.ToLower()+".");
                if(!empty && (f.Kind=="int" || f.Kind=="money") && Convert.ToDecimal(v)<0) throw new InvalidOperationException(f.Label+" không được âm.");
            }
            if(s.Table=="tblDMSach" && values["MaXB"]!=null && (Convert.ToInt32(values["MaXB"])<1000 || Convert.ToInt32(values["MaXB"])>9999))
                throw new InvalidOperationException("Năm xuất bản phải gồm 4 chữ số hoặc để trống.");
            if(s.Table=="tblTheMuon")
            {
                DateTime issued=Convert.ToDateTime(values["NgayLamThe"]),expiry=Convert.ToDateTime(values["NgayHetHan"]);
                if(expiry<issued) throw new InvalidOperationException("Ngày hết hạn phải từ ngày làm thẻ trở đi.");
                if(values["NgaySinh"]!=null && Convert.ToDateTime(values["NgaySinh"])>issued) throw new InvalidOperationException("Ngày sinh phải trước ngày làm thẻ.");
            }
            return LibraryDb.Transaction((cn,tx)=> {
                if(id.HasValue && s.Table=="tblDMSach" && originalStock.HasValue)
                {
                    var current=LibraryDb.Query(cn,tx,"SELECT SoLuong FROM tblDMSach WITH(UPDLOCK,HOLDLOCK) WHERE MaSach=@id",LibraryDb.P("@id",id.Value));
                    if(current.Rows.Count!=1 || Convert.ToInt32(current.Rows[0][0])!=originalStock.Value)
                        throw new InvalidOperationException("Tồn kho đã thay đổi do mượn/trả hoặc được cập nhật ở cửa sổ khác. Hãy tải lại sách trước khi lưu để tránh ghi đè tồn kho.");
                }
                int key=id??LibraryDb.NextId(cn,tx,s.Table,s.Key);
                var fields=s.Fields.Where(x=>!x.ReadOnly).ToArray();
                var ps=fields.Select(f=>LibraryDb.P("@"+f.Name,values[f.Name])).ToList(); ps.Add(LibraryDb.P("@key",key));
                string sql;
                if(id.HasValue) sql="UPDATE ["+s.Table+"] SET "+string.Join(",",fields.Select(f=>"["+f.Name+"]=@"+f.Name))+" WHERE ["+s.Key+"]=@key";
                else sql="INSERT ["+s.Table+"](["+s.Key+"],"+string.Join(",",fields.Select(f=>"["+f.Name+"]"))+") VALUES(@key,"+string.Join(",",fields.Select(f=>"@"+f.Name))+")";
                if(LibraryDb.Execute(cn,tx,sql,ps.ToArray())!=1) throw new InvalidOperationException("Bản ghi đã thay đổi hoặc bị xóa. Hãy tải lại dữ liệu.");
                return key;
            });
        }
        public static void Delete(CatalogSpec s,int id)
        { LibraryDb.Transaction((cn,tx)=> { LibraryDb.Execute(cn,tx,"DELETE FROM ["+s.Table+"] WHERE ["+s.Key+"]=@id",LibraryDb.P("@id",id));return 0; }); }
    }
}
