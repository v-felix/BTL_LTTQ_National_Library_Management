using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyThuVienQuocGia
{
    internal sealed class frmReports : Form
    {
        readonly ComboBox mode=new ComboBox{DropDownStyle=ComboBoxStyle.DropDownList,Width=310,Margin=new Padding(4,7,4,4)};
        readonly ComboBox card,librarian;
        readonly DateTimePicker from=new DateTimePicker{Format=DateTimePickerFormat.Custom,CustomFormat="dd/MM/yyyy",Width=125,Value=new DateTime(DateTime.Today.Year,1,1),Margin=new Padding(4,7,4,4)};
        readonly DateTimePicker to=new DateTimePicker{Format=DateTimePickerFormat.Custom,CustomFormat="dd/MM/yyyy",Width=125,Margin=new Padding(4,7,4,4)};
        readonly CheckBox filterDate=new CheckBox{Text="Lọc ngày",AutoSize=true,Margin=new Padding(4,10,4,4)};
        readonly DataGridView detail=LibraryUi.Grid(),totals=LibraryUi.Grid();
        readonly Label status=new Label{Dock=DockStyle.Bottom,Height=56,Padding=new Padding(10),ForeColor=UiTheme.Navy};
        readonly TabControl tabs=new TabControl{Dock=DockStyle.Fill};
        readonly string[] keys={"revenue","outstanding","loans","returns","violations"};
        readonly Dictionary<string,string> captions=new Dictionary<string,string>{
            {"MaHSM","Mã HS mượn"},{"MaHSTra","Mã HS trả"},{"MaThe","Mã thẻ"},{"HoTen","Bạn đọc"},{"TenThuThu","Thủ thư"},{"NgayMuon","Ngày mượn"},{"NgayTra","Ngày trả"},
            {"MaSach","Mã sách"},{"TenSach","Tên sách"},{"DonGiaThue","Đơn giá/ngày"},{"SoNgayThue","Số ngày thuê"},{"TienPhat","Tiền phạt"},{"ThanhTien","Thành tiền"},
            {"TamUng","Tạm ứng"},{"TamUngApDung","Tạm ứng đã trừ"},{"SoTienThanhToan","Thu thêm / hoàn (-)"},{"TongTien","Tổng tiền thuê và phạt"},{"SoSach","Số sách"},{"ChuaTra","Chưa trả"},
            {"SoChuaTra","Số sách chưa trả"},{"TenViPham","Vi phạm"},{"Thang","Tháng"},{"Quy","Quý"},{"Nam","Năm"},{"ThucThu","Thu thêm / hoàn (-)"},{"SoPhieuTra","Số phiếu trả"}};
        public frmReports(string initial="revenue",int? cardId=null,int? staffId=null)
        {
            LibraryUi.Setup(this,"Tra cứu và báo cáo thư viện",1280,780);
            card=LibraryUi.Combo(Catalog.Lookup("the"),true);librarian=LibraryUi.Combo(Catalog.Lookup("thuthu"),true);
            mode.Items.AddRange(new object[]{"Tiền thu theo tháng, quý, năm","Sách đang mượn chưa trả","Hồ sơ mượn theo thẻ / thủ thư","Danh sách hồ sơ trả","Sách vi phạm khi trả"});mode.SelectedIndex=Math.Max(0,Array.IndexOf(keys,initial));
            var top=LibraryUi.Bar();top.Controls.Add(LibraryUi.Label("Báo cáo"));top.Controls.Add(mode);top.Controls.Add(LibraryUi.Label("Thẻ"));top.Controls.Add(card);top.Controls.Add(LibraryUi.Label("Thủ thư"));top.Controls.Add(librarian);
            var bar=LibraryUi.Bar();bar.Controls.Add(filterDate);bar.Controls.Add(from);bar.Controls.Add(LibraryUi.Label("đến"));bar.Controls.Add(to);
            bar.Controls.Add(LibraryUi.Button("Tháng này",()=>SetPeriod(0)));bar.Controls.Add(LibraryUi.Button("Quý này",()=>SetPeriod(1)));bar.Controls.Add(LibraryUi.Button("Năm nay",()=>SetPeriod(2)));
            bar.Controls.Add(LibraryUi.Button("Xem báo cáo",LoadReport));bar.Controls.Add(LibraryUi.Button("Xuất Excel",()=>LibraryUi.Export(CurrentGrid(),mode.Text)));bar.Controls.Add(LibraryUi.Button("In / PDF",()=>LibraryUi.Print(CurrentGrid(),mode.Text,status.Text)));
            var t1=new TabPage("Chi tiết");t1.Controls.Add(detail);var t2=new TabPage("Tổng hợp tháng / quý / năm");t2.Controls.Add(totals);tabs.TabPages.Add(t1);tabs.TabPages.Add(t2);
            Controls.Add(tabs);Controls.Add(status);Controls.Add(bar);Controls.Add(top);
            filterDate.Checked=initial=="revenue";if(cardId.HasValue)card.SelectedValue=cardId.Value;if(staffId.HasValue)librarian.SelectedValue=staffId.Value;
            mode.SelectedIndexChanged+=(s,e)=>LibraryUi.Run(LoadReport);
            detail.CellDoubleClick+=(s,e)=>{if(e.RowIndex>=0)LibraryUi.Run(ShowDetails);};
            Shown+=(s,e)=>LibraryUi.Run(LoadReport);
        }
        DataGridView CurrentGrid(){return tabs.SelectedIndex==1?totals:detail;}
        void SetPeriod(int kind)
        {
            DateTime now=DateTime.Today,start=kind==0?new DateTime(now.Year,now.Month,1):kind==1?new DateTime(now.Year,((now.Month-1)/3)*3+1,1):new DateTime(now.Year,1,1);
            from.Value=start;to.Value=kind==0?start.AddMonths(1).AddDays(-1):kind==1?start.AddMonths(3).AddDays(-1):start.AddYears(1).AddDays(-1);filterDate.Checked=true;LoadReport();
        }
        int? Id(ComboBox cb){return cb.SelectedValue==null || cb.SelectedValue==DBNull.Value || cb.SelectedValue is DataRowView?(int?)null:Convert.ToInt32(cb.SelectedValue);}
        void Bind(DataGridView g,DataTable dt)
        {g.DataSource=dt;foreach(DataGridViewColumn c in g.Columns)if(captions.ContainsKey(c.Name))c.HeaderText=captions[c.Name];LibraryUi.Format(g);}
        void LoadReport()
        {
            if(from.Value.Date>to.Value.Date)throw new InvalidOperationException("Ngày bắt đầu phải trước ngày kết thúc.");
            string key=keys[mode.SelectedIndex],sql;
            string dates=filterDate.Checked?" AND {0}>=@from AND {0}<@to ":"";
            string baseFilter=" AND (@card IS NULL OR h.MaThe=@card) AND (@staff IS NULL OR h.MaThuThu=@staff) ";
            if(key=="loans")sql=@"SELECT h.MaHSM,h.MaThe,c.HoTen,t.TenThuThu,h.NgayMuon,ISNULL(h.TamUng,0) TamUng,
                COUNT(d.MaSach) SoSach,SUM(CASE WHEN d.ChuaTra=1 THEN 1 ELSE 0 END) SoChuaTra FROM tblHoSoMuon h
                JOIN tblTheMuon c ON c.MaThe=h.MaThe JOIN tblThuThu t ON t.MaThuThu=h.MaThuThu LEFT JOIN tblChiTietHSMuon d ON d.MaHSM=h.MaHSM
                WHERE 1=1 "+baseFilter+string.Format(dates,"h.NgayMuon")+" GROUP BY h.MaHSM,h.MaThe,c.HoTen,t.TenThuThu,h.NgayMuon,h.TamUng ORDER BY h.MaHSM DESC";
            else if(key=="outstanding")sql=@"SELECT h.MaHSM,h.MaThe,c.HoTen,t.TenThuThu,d.MaSach,s.TenSach,h.NgayMuon,ISNULL(d.DonGiaThueLucMuon,s.DonGiaThue) DonGiaThue
                FROM tblChiTietHSMuon d JOIN tblHoSoMuon h ON h.MaHSM=d.MaHSM JOIN tblTheMuon c ON c.MaThe=h.MaThe
                JOIN tblThuThu t ON t.MaThuThu=h.MaThuThu JOIN tblDMSach s ON s.MaSach=d.MaSach WHERE d.ChuaTra=1 "+baseFilter+string.Format(dates,"h.NgayMuon")+" ORDER BY h.NgayMuon,h.MaHSM,d.MaSach";
            else if(key=="returns")sql=@"SELECT r.MaHSTra,r.MaHSM,h.MaThe,c.HoTen,t.TenThuThu,h.NgayMuon,r.NgayTra,r.TongTien,r.TamUngApDung,r.SoTienThanhToan
                FROM tblHoSoTra r JOIN tblHoSoMuon h ON h.MaHSM=r.MaHSM JOIN tblTheMuon c ON c.MaThe=h.MaThe
                LEFT JOIN tblThuThu t ON t.MaThuThu=r.MaThuThu WHERE 1=1 "+baseFilter+string.Format(dates,"r.NgayTra")+" ORDER BY r.MaHSTra DESC";
            else sql=@"SELECT r.MaHSTra,h.MaHSM,h.MaThe,c.HoTen,d.MaSach,s.TenSach,h.NgayMuon,r.NgayTra,
                d.SoNgayThue,d.DonGiaThue,ISNULL(v.TenViPham,N'Không vi phạm') TenViPham,d.TienPhat,d.ThanhTien,
                CASE WHEN ROW_NUMBER() OVER(PARTITION BY r.MaHSTra ORDER BY d.MaSach)=1 THEN r.TamUngApDung ELSE 0 END TamUngApDung,
                d.ThanhTien-CASE WHEN ROW_NUMBER() OVER(PARTITION BY r.MaHSTra ORDER BY d.MaSach)=1 THEN r.TamUngApDung ELSE 0 END ThucThu
                FROM tblChiTietHSTra d JOIN tblHoSoTra r ON r.MaHSTra=d.MaHSTra JOIN tblHoSoMuon h ON h.MaHSM=r.MaHSM
                JOIN tblTheMuon c ON c.MaThe=h.MaThe JOIN tblDMSach s ON s.MaSach=d.MaSach LEFT JOIN tblViPham v ON v.MaViPham=d.MaViPham
                WHERE 1=1 "+baseFilter+string.Format(dates,"r.NgayTra")+(key=="violations"?" AND d.MaViPham IS NOT NULL":"")+" ORDER BY r.NgayTra,r.MaHSTra,d.MaSach";
            var dt=LibraryDb.Query(sql,LibraryDb.P("@card",Id(card)),LibraryDb.P("@staff",Id(librarian)),LibraryDb.P("@from",from.Value.Date),LibraryDb.P("@to",to.Value.Date.AddDays(1)));
            if(key=="violations"){dt.Columns.Remove("TamUngApDung");dt.Columns.Remove("ThucThu");}
            Bind(detail,dt);tabs.TabPages[1].Enabled=key=="revenue";if(key!="revenue")tabs.SelectedIndex=0;
            status.Text=dt.Rows.Count+" dòng. "+(filterDate.Checked?from.Value.ToString("dd/MM/yyyy")+" – "+to.Value.ToString("dd/MM/yyyy"):"Tất cả thời gian")+". Nhấp đúp hồ sơ để xem chi tiết.";
            if(key=="revenue")
            {
                Bind(totals,Aggregate(dt));decimal fees=dt.AsEnumerable().Sum(r=>r.IsNull("ThanhTien")?0:r.Field<decimal>("ThanhTien")),cash=dt.AsEnumerable().Sum(r=>r.IsNull("ThucThu")?0:r.Field<decimal>("ThucThu"));
                status.Text+="\nTổng tiền thuê và phạt: "+fees.ToString("N0")+" đ • Thu thêm / hoàn sau tạm ứng: "+cash.ToString("N0")+" đ. Tạm ứng được quyết toán ở lần trả cuối.";
            }
            else totals.DataSource=null;
        }
        DataTable Aggregate(DataTable dt)
        {
            var result=new DataTable();result.Columns.Add("Kỳ");result.Columns.Add("TongTien",typeof(decimal));result.Columns.Add("TamUngApDung",typeof(decimal));result.Columns.Add("ThucThu",typeof(decimal));result.Columns.Add("SoPhieuTra",typeof(int));
            var rows=dt.AsEnumerable().Where(r=>!r.IsNull("NgayTra")).ToArray();
            for(int type=0;type<3;type++)
            {
                int groupType=type;
                foreach(var g in rows.GroupBy(r=> {var d=r.Field<DateTime>("NgayTra");return groupType==0?d.ToString("yyyy-MM"):groupType==1?d.Year+" / Q"+((d.Month-1)/3+1):d.Year.ToString();}).OrderBy(g=>g.Key))
                    result.Rows.Add((type==0?"Tháng ":type==1?"Quý ":"Năm ")+g.Key,g.Sum(r=>r.IsNull("ThanhTien")?0:r.Field<decimal>("ThanhTien")),g.Sum(r=>r.Field<decimal>("TamUngApDung")),g.Sum(r=>r.IsNull("ThucThu")?0:r.Field<decimal>("ThucThu")),g.Select(r=>r.Field<int>("MaHSTra")).Distinct().Count());
            }
            return result;
        }
        void ShowDetails()
        {
            if(detail.CurrentRow==null)return;var row=((DataRowView)detail.CurrentRow.DataBoundItem).Row;
            bool isReturn=row.Table.Columns.Contains("MaHSTra");int id=Convert.ToInt32(row[isReturn?"MaHSTra":"MaHSM"]);
            var dt=isReturn?LibraryDb.Query(@"SELECT d.MaSach,s.TenSach,d.SoNgayThue,d.DonGiaThue,v.TenViPham,d.TienPhat,d.ThanhTien
                FROM tblChiTietHSTra d JOIN tblDMSach s ON s.MaSach=d.MaSach LEFT JOIN tblViPham v ON v.MaViPham=d.MaViPham WHERE d.MaHSTra=@id",LibraryDb.P("@id",id)):
                LibraryDb.Query(@"SELECT d.MaSach,s.TenSach,d.ChuaTra,ISNULL(d.DonGiaThueLucMuon,s.DonGiaThue) DonGiaThue FROM tblChiTietHSMuon d JOIN tblDMSach s ON s.MaSach=d.MaSach WHERE d.MaHSM=@id",LibraryDb.P("@id",id));
            using(var f=new Form())
            {LibraryUi.Setup(f,"Chi tiết hồ sơ "+(isReturn?"trả ":"mượn ")+id,1000,650);var g=LibraryUi.Grid();Bind(g,dt);f.Controls.Add(g);var bar=LibraryUi.Bar();bar.Controls.Add(LibraryUi.Button("Xuất Excel",()=>LibraryUi.Export(g,f.Text)));bar.Controls.Add(LibraryUi.Button("In / PDF",()=>LibraryUi.Print(g,f.Text,"Ngày in: "+DateTime.Now.ToString("dd/MM/yyyy HH:mm"))));f.Controls.Add(bar);f.ShowDialog(this);}
        }
    }
}
