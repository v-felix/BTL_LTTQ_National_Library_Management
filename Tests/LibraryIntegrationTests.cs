using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using QuanLyThuVienQuocGia;

class LibraryIntegrationTests
{
    static int checks;
    static void Assert(bool ok,string name){if(!ok)throw new Exception("FAIL: "+name);Console.WriteLine("PASS: "+name);checks++;}
    static decimal Value(string sql){return Convert.ToDecimal(LibraryDb.Query(sql).Rows[0][0]);}
    static void Reject(Action action,string name){bool rejected=false;try{action();}catch(InvalidOperationException){rejected=true;}Assert(rejected,name);}
    static int Save(string kind,params object[] pairs)
    {
        var spec=Catalog.Specs[kind];var values=spec.Fields.Where(f=>!f.ReadOnly).ToDictionary(f=>f.Name,f=>(object)null);
        for(int i=0;i<pairs.Length;i+=2)values[(string)pairs[i]]=pairs[i+1];return Catalog.Save(spec,null,values);
    }
    static int Card(string name,DateTime expiry){return Save("the","HoTen",name,"NgayLamThe",DateTime.Today.AddYears(-1),"NgayHetHan",expiry);}
    static int Book(string name,int quantity,decimal price,int publisher,int category)
    {return Save("sach","TenSach",name,"SoLuong",quantity,"DonGiaThue",price,"MaNXB",publisher,"MaTheLoai",category,"MaXB",2026,"KhoSach","16x24","GiaSach",50000m,"SoTrang",200,"TomTatND","Kiểm thử đầy đủ trường","LanTaiBan",1);}
    static int Main()
    {
        try
        {
            string connection=Environment.GetEnvironmentVariable("QLTV_TEST_CONNECTION");
            if(string.IsNullOrEmpty(connection) || !new SqlConnectionStringBuilder(connection).InitialCatalog.StartsWith("QLTV_Test_"))throw new Exception("Only a disposable QLTV_Test_ database is allowed.");
            LibraryDb.ConnectionOverride=connection;LibraryDb.Initialize();LibraryDb.Initialize();
            Assert(Value("SELECT COUNT(*) FROM sys.columns WHERE object_id=OBJECT_ID('tblDMSach') AND name='SoLuong'")==1,"idempotent migration");
            int publisher=Save("nxb","TenNXB","TEST_Publisher"),category=Save("theloai","TenTheLoai","TEST_Category");
            int staff=Save("thuthu","TenThuThu","TEST_Staff"),card=Card("TEST_Card",DateTime.Today.AddDays(30));
            int expired=Card("TEST_Expired",DateTime.Today.AddDays(-1)),other=Card("TEST_Other",DateTime.Today.AddDays(30));
            int a=Book("TEST_BookA",2,1000m,publisher,category),b=Book("TEST_BookB",1,2000m,publisher,category),empty=Book("TEST_Empty",0,3000m,publisher,category),d=Book("TEST_BookD",1,3000m,publisher,category);
            int fine=Save("vipham","TenViPham","TEST_Fine","TienPhat",500m);
            Assert(Catalog.List(Catalog.Specs["sach"],"TEST_Publisher").Rows.Count==4,"search by publisher");
            Assert(Catalog.List(Catalog.Specs["sach"],"TEST_Category").Rows.Count==4,"search by category");
            Assert(Catalog.List(Catalog.Specs["sach"],"TEST_BookA").Rows.Count==1,"search by book name");
            decimal originalLoans=Value("SELECT COUNT(*) FROM tblHoSoMuon");
            Reject(()=>LibraryDb.Borrow(expired,staff,DateTime.Today,0,new[]{a}),"reject expired card");
            Reject(()=>LibraryDb.Borrow(card,staff,DateTime.Today,0,new int[0]),"reject no books");
            Reject(()=>LibraryDb.Borrow(card,staff,DateTime.Today,-1,new[]{a}),"reject negative advance");
            Reject(()=>LibraryDb.Borrow(card,staff,DateTime.Today,0,new[]{a,a}),"reject duplicate books");
            Reject(()=>LibraryDb.Borrow(card,staff,DateTime.Today.AddDays(1),0,new[]{a}),"reject future loan");
            Reject(()=>LibraryDb.Borrow(card,staff,DateTime.Today,0,new[]{a,empty}),"out of stock rolls back whole loan");
            Assert(Value("SELECT COUNT(*) FROM tblHoSoMuon")==originalLoans && Value("SELECT SoLuong FROM tblDMSach WHERE MaSach="+a)==2,"rollback preserves header and inventory");
            int loan=LibraryDb.Borrow(card,staff,DateTime.Today.AddDays(-5),10000m,new[]{a,b});
            var stale=Catalog.Specs["sach"].Fields.Where(f=>!f.ReadOnly).ToDictionary(f=>f.Name,f=>(object)null);
            stale["TenSach"]="TEST_BookA";stale["SoLuong"]=2;stale["DonGiaThue"]=1000m;
            Reject(()=>Catalog.Save(Catalog.Specs["sach"],a,stale,2),"stale catalog edit cannot overwrite stock changed by borrowing");
            Assert(Value("SELECT SoLuong FROM tblDMSach WHERE MaSach="+a)==1,"rejected stale edit preserves inventory");
            Assert(Value("SELECT SoLuongMuon FROM tblTheMuon WHERE MaThe="+card)==2,"loan updates card count");
            Assert(Value("SELECT SoLuong FROM tblDMSach WHERE MaSach="+a)==1 && Value("SELECT SoLuong FROM tblDMSach WHERE MaSach="+b)==0,"loan decrements both stocks");
            int loanD=LibraryDb.Borrow(card,staff,DateTime.Today,20000m,new[]{d});
            Assert(Value("SELECT SoLuongMuon FROM tblTheMuon WHERE MaThe="+card)==3,"allow third outstanding book");
            Reject(()=>LibraryDb.Borrow(card,staff,DateTime.Today,0,new[]{a}),"reject fourth outstanding book");
            LibraryDb.Transaction((cn,tx)=>LibraryDb.Execute(cn,tx,"UPDATE tblDMSach SET DonGiaThue=9999 WHERE MaSach=@a",LibraryDb.P("@a",a)));
            Reject(()=>LibraryDb.Return(loan,staff,DateTime.Today.AddDays(-6),new Dictionary<int,int?>{{a,null}}),"reject return before loan date");
            Reject(()=>LibraryDb.Return(loan,staff,DateTime.Today,new Dictionary<int,int?>{{d,null}}),"reject a book from another loan");
            Reject(()=>LibraryDb.Return(loan,staff,DateTime.Today,new Dictionary<int,int?>{{a,int.MaxValue}}),"reject nonexistent violation");
            int first=LibraryDb.Return(loan,staff,DateTime.Today.AddDays(-2),new Dictionary<int,int?>{{a,fine}});
            Assert(Value("SELECT TongTien FROM tblHoSoTra WHERE MaHSTra="+first)==3500,"fee uses saved tariff plus penalty");
            Assert(Value("SELECT TamUngApDung FROM tblHoSoTra WHERE MaHSTra="+first)==0,"partial return keeps advance unsettled");
            Assert(LibraryDb.Outstanding(loan).Rows.Count==1 && Convert.ToInt32(LibraryDb.Outstanding(loan).Rows[0]["MaSach"])==b,"only remaining book can be returned");
            decimal returns=Value("SELECT COUNT(*) FROM tblHoSoTra");
            Reject(()=>LibraryDb.Return(loan,staff,DateTime.Today,new Dictionary<int,int?>{{a,null}}),"reject duplicate return");
            Assert(Value("SELECT COUNT(*) FROM tblHoSoTra")==returns && Value("SELECT SoLuong FROM tblDMSach WHERE MaSach="+a)==2,"duplicate return does not change stock or header");
            int last=LibraryDb.Return(loan,staff,DateTime.Today,new Dictionary<int,int?>{{b,null}});
            Assert(Value("SELECT TongTien FROM tblHoSoTra WHERE MaHSTra="+last)==10000,"final return computes five-day rental");
            Assert(Value("SELECT TamUngApDung FROM tblHoSoTra WHERE MaHSTra="+last)==10000 && Value("SELECT SoTienThanhToan FROM tblHoSoTra WHERE MaHSTra="+last)==0,"advance deducted exactly once on final return");
            Assert(Value("SELECT SUM(SoTienThanhToan) FROM tblHoSoTra WHERE MaHSM="+loan)==3500,"partial payments reconcile with total fee minus advance");
            Assert(Value("SELECT SoLuongMuon FROM tblTheMuon WHERE MaThe="+card)==1,"card still includes book from other loan");
            int refund=LibraryDb.Return(loanD,staff,DateTime.Today,new Dictionary<int,int?>{{d,null}});
            Assert(Value("SELECT SoTienThanhToan FROM tblHoSoTra WHERE MaHSTra="+refund)==-20000,"same-day zero rental refunds advance");
            Assert(Value("SELECT SoLuongMuon FROM tblTheMuon WHERE MaThe="+card)==0,"all returns clear card count");
            Assert(Value("SELECT COUNT(*) FROM tblHoSoMuon WHERE MaThe="+card+" AND MaThuThu="+staff)==2,"loan search by card and librarian");
            int raceBook=Book("TEST_LastCopy",1,100m,publisher,category);int successes=0;
            Parallel.ForEach(new[]{card,other},c=>{try{LibraryDb.Borrow(c,staff,DateTime.Today,0,new[]{raceBook});System.Threading.Interlocked.Increment(ref successes);}catch(InvalidOperationException){}catch(SqlException ex){if(ex.Number!=1205)throw;}});
            Assert(successes==1 && Value("SELECT SoLuong FROM tblDMSach WHERE MaSach="+raceBook)==0,"concurrent loans cannot oversell last copy");
            int raceLoan=Convert.ToInt32(Value("SELECT MAX(MaHSM) FROM tblChiTietHSMuon WHERE MaSach="+raceBook));successes=0;
            Parallel.For(0,2,i=>{try{LibraryDb.Return(raceLoan,staff,DateTime.Today,new Dictionary<int,int?>{{raceBook,null}});System.Threading.Interlocked.Increment(ref successes);}catch(InvalidOperationException){}catch(SqlException ex){if(ex.Number!=1205)throw;}});
            Assert(successes==1 && Value("SELECT SoLuong FROM tblDMSach WHERE MaSach="+raceBook)==1,"concurrent returns cannot double-credit stock");
            Assert(Value("SELECT COUNT(*) FROM (SELECT YEAR(NgayTra) y,DATEPART(quarter,NgayTra) q,MONTH(NgayTra) m,SUM(TongTien) fees,SUM(SoTienThanhToan) paid FROM tblHoSoTra GROUP BY YEAR(NgayTra),DATEPART(quarter,NgayTra),MONTH(NgayTra)) x")>0,"revenue groups by month quarter and year");
            string backup=Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"MaintenanceTest.bak");
            decimal beforeBackup=Value("SELECT COUNT(*) FROM tblDMSach");DatabaseMaintenance.Backup(backup);
            Book("TEST_ShouldDisappearAfterRestore",1,10m,publisher,category);
            Assert(Value("SELECT COUNT(*) FROM tblDMSach")==beforeBackup+1,"backup test mutation stays in disposable database");
            DatabaseMaintenance.Restore(backup);
            Assert(Value("SELECT COUNT(*) FROM tblDMSach")==beforeBackup,"backup and validated restore recover exact data");
            Assert(Value("SELECT COUNT(*) FROM sys.columns WHERE object_id=OBJECT_ID('tblDMSach') AND name='SoLuong'")==1,"restored database remains upgraded");
            Console.WriteLine("ALL "+checks+" CHECKS PASSED");return 0;
        }
        catch(Exception ex){Console.Error.WriteLine(ex);return 1;}
    }
}
