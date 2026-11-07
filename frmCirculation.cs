using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyThuVienQuocGia
{
    internal sealed class frmCirculation : Form
    {
        readonly bool returning;
        readonly DataGridView grid=LibraryUi.Grid();
        readonly FlowLayoutPanel header=LibraryUi.Bar();
        readonly ComboBox cardOrLoan=new ComboBox(),librarian=new ComboBox();
        readonly DateTimePicker date=new DateTimePicker{Format=DateTimePickerFormat.Custom,CustomFormat="dd/MM/yyyy",Width=130,Margin=new Padding(4,7,4,4),MaxDate=DateTime.Today};
        readonly NumericUpDown deposit=new NumericUpDown{Maximum=999999999999m,DecimalPlaces=2,ThousandsSeparator=true,Width=145,Margin=new Padding(4,7,4,4)};
        readonly Label summary=new Label{Dock=DockStyle.Bottom,Height=76,Padding=new Padding(12),ForeColor=UiTheme.Navy};
        readonly TextBox search=new TextBox{Width=250,Margin=new Padding(4,7,4,4)};
        bool loading;
        public frmCirculation(bool isReturn)
        {
            returning=isReturn;LibraryUi.Setup(this,returning?"Trả sách và thanh toán":"Lập hồ sơ mượn sách",1250,740);
            ConfigureCombo(cardOrLoan,310);ConfigureCombo(librarian,245);
            header.Controls.Add(LibraryUi.Label(returning?"Hồ sơ mượn":"Thẻ mượn"));header.Controls.Add(cardOrLoan);header.Controls.Add(LibraryUi.Label("Thủ thư"));header.Controls.Add(librarian);header.Controls.Add(LibraryUi.Label(returning?"Ngày trả":"Ngày mượn"));header.Controls.Add(date);
            var actions=LibraryUi.Bar();
            if(!returning){actions.Controls.Add(LibraryUi.Label("Tạm ứng"));actions.Controls.Add(deposit);actions.Controls.Add(LibraryUi.Label("Tìm sách"));actions.Controls.Add(search);}
            actions.Controls.Add(LibraryUi.Button("Tải lại",Reload));actions.Controls.Add(LibraryUi.Button(returning?"Lưu phiếu trả":"Lưu phiếu mượn",Save));actions.Controls.Add(LibraryUi.Button("Xem hồ sơ",()=>new frmReports("loans").ShowDialog(this)));
            var note=new Label{Dock=DockStyle.Top,AutoSize=true,Padding=new Padding(12),Text=returning?"Chỉ hiển thị sách chưa trả của hồ sơ. Tiền thuê = số ngày × đơn giá lúc mượn + phạt. Tạm ứng được trừ một lần ở lần trả cuối; số âm là hoàn lại.":"Đánh dấu các sách cần mượn (tối đa 3 cuốn đang mượn/thẻ). Thẻ phải còn hạn và sách phải còn trong kho."};
            grid.ReadOnly=false;grid.AutoGenerateColumns=false;
            grid.Columns.Add(new DataGridViewCheckBoxColumn{Name="Choose",HeaderText="Chọn",Width=65,ValueType=typeof(bool),TrueValue=true,FalseValue=false,
                DefaultCellStyle=new DataGridViewCellStyle{NullValue=false,DataSourceNullValue=false,Alignment=DataGridViewContentAlignment.MiddleCenter,Padding=new Padding(0)}});
            Add("MaSach","Mã sách");Add("TenSach","Tên sách");
            if(returning)Add("NgayMuon","Ngày mượn");else{Add("TenTheLoai","Thể loại");Add("TenNXB","Nhà xuất bản");Add("SoLuong","Còn trong kho");}
            Add("DonGiaThue","Đơn giá/ngày");
            if(returning)
            {
                var vp=Catalog.Lookup("vipham");var none=vp.NewRow();none["Id"]=DBNull.Value;none["Name"]="Không vi phạm";vp.Rows.InsertAt(none,0);
                grid.Columns.Add(new DataGridViewComboBoxColumn{Name="Violation",HeaderText="Vi phạm",DataSource=vp,DisplayMember="Name",ValueMember="Id",MinimumWidth=210,DefaultCellStyle=new DataGridViewCellStyle{NullValue="Không vi phạm"}});
                grid.Columns.Add(new DataGridViewTextBoxColumn{Name="Amount",HeaderText="Thành tiền",ReadOnly=true,DefaultCellStyle=new DataGridViewCellStyle{Format="N0"}});
            }
            Controls.Add(grid);Controls.Add(summary);Controls.Add(note);Controls.Add(actions);Controls.Add(header);
            grid.CurrentCellDirtyStateChanged+=(s,e)=>{if(grid.IsCurrentCellDirty)grid.CommitEdit(DataGridViewDataErrorContexts.Commit);};
            grid.CellValueChanged+=(s,e)=>{if(!loading && e.ColumnIndex>=0 && (grid.Columns[e.ColumnIndex].Name=="Choose" || grid.Columns[e.ColumnIndex].Name=="Violation"))LibraryUi.Run(UpdateSummary);};
            grid.DataError+=(s,e)=>{e.ThrowException=false;};
            cardOrLoan.SelectedIndexChanged+=(s,e)=>{if(!loading)LibraryUi.Run(()=>{if(returning)LoadBooks();UpdateSummary();});};
            date.ValueChanged+=(s,e)=>{if(!loading)LibraryUi.Run(UpdateSummary);};deposit.ValueChanged+=(s,e)=>{if(!loading)LibraryUi.Run(UpdateSummary);};
            search.TextChanged+=(s,e)=>LibraryUi.Run(LoadBooks);
            Shown+=(s,e)=>LibraryUi.Run(Reload);
        }
        void ConfigureCombo(ComboBox cb,int width){cb.Width=width;cb.DropDownWidth=500;cb.DropDownStyle=ComboBoxStyle.DropDownList;cb.DisplayMember="Name";cb.ValueMember="Id";cb.Margin=new Padding(4,7,4,4);}
        void Add(string name,string title){grid.Columns.Add(new DataGridViewTextBoxColumn{Name=name,DataPropertyName=name,HeaderText=title,ReadOnly=true});}
        void Reload()
        {
            loading=true;try{
                librarian.DataSource=Catalog.Lookup("thuthu");librarian.SelectedIndex=-1;
                cardOrLoan.DataSource=returning?LibraryDb.Query(@"SELECT h.MaHSM Id,CONVERT(nvarchar(20),h.MaHSM)+N' — '+c.HoTen+N' — '+CONVERT(nvarchar(10),h.NgayMuon,103) Name
                    FROM tblHoSoMuon h JOIN tblTheMuon c ON c.MaThe=h.MaThe WHERE EXISTS(SELECT 1 FROM tblChiTietHSMuon d WHERE d.MaHSM=h.MaHSM AND d.ChuaTra=1) ORDER BY h.MaHSM DESC"):Catalog.Lookup("the");
                cardOrLoan.SelectedIndex=-1;
            }finally{loading=false;}LoadBooks();
        }
        void LoadBooks()
        {
            loading=true;try{
                DataTable dt;
                if(returning){dt=cardOrLoan.SelectedValue==null || cardOrLoan.SelectedValue==DBNull.Value ? new DataTable():LibraryDb.Outstanding(LibraryUi.Selected(cardOrLoan,"hồ sơ mượn"));}
                else dt=LibraryDb.Query(@"SELECT s.MaSach,s.TenSach,s.SoLuong,s.DonGiaThue,t.TenTheLoai,n.TenNXB FROM tblDMSach s
                    LEFT JOIN tblTheLoai t ON t.MaTheLoai=s.MaTheLoai LEFT JOIN tblNXB n ON n.MaNXB=s.MaNXB
                    WHERE s.TenSach LIKE @q OR t.TenTheLoai LIKE @q OR n.TenNXB LIKE @q ORDER BY s.MaSach",LibraryDb.P("@q","%"+search.Text.Trim()+"%"));
                grid.DataSource=dt;LibraryUi.Format(grid);
                foreach(DataGridViewRow r in grid.Rows){r.Cells["Choose"].Value=false;if(returning)r.Cells["Violation"].Value=DBNull.Value;}
            }finally{loading=false;}UpdateSummary();
        }
        DataGridViewRow[] Chosen(){grid.EndEdit();return grid.Rows.Cast<DataGridViewRow>().Where(r=>Convert.ToBoolean(r.Cells["Choose"].Value??false)).ToArray();}
        int? Violation(DataGridViewRow r){var v=r.Cells["Violation"].Value;return v==null || v==DBNull.Value?(int?)null:Convert.ToInt32(v);}
        void UpdateSummary()
        {
            var chosen=Chosen();
            if(!returning)
            {
                string extra="";
                if(cardOrLoan.SelectedValue!=null && !(cardOrLoan.SelectedValue is DataRowView))
                {
                    var c=LibraryDb.Query("SELECT NgayHetHan,SoLuongMuon FROM tblTheMuon WHERE MaThe=@c",LibraryDb.P("@c",cardOrLoan.SelectedValue));
                    if(c.Rows.Count>0)extra=" • Đang mượn: "+c.Rows[0]["SoLuongMuon"]+"/3 • Hết hạn: "+(c.Rows[0]["NgayHetHan"]==DBNull.Value?"chưa khai báo":Convert.ToDateTime(c.Rows[0]["NgayHetHan"]).ToString("dd/MM/yyyy"));
                }
                summary.Text="Đã chọn "+chosen.Length+" cuốn • Tạm ứng: "+deposit.Value.ToString("N0")+" đ"+extra;return;
            }
            if(grid.Rows.Count==0){summary.Text="Chọn hồ sơ còn sách chưa trả.";return;}
            decimal total=0;bool invalid=false;
            foreach(DataGridViewRow r in grid.Rows)
            {
                var drv=(DataRowView)r.DataBoundItem;int days=(date.Value.Date-Convert.ToDateTime(drv["NgayMuon"]).Date).Days;
                decimal fine=0;int? v=Violation(r);
                if(v.HasValue){var f=LibraryDb.Query("SELECT TienPhat FROM tblViPham WHERE MaViPham=@v",LibraryDb.P("@v",v.Value));if(f.Rows.Count>0 && f.Rows[0][0]!=DBNull.Value)fine=Convert.ToDecimal(f.Rows[0][0]);else invalid=true;}
                if(days<0 || drv["DonGiaThue"]==DBNull.Value){r.Cells["Amount"].Value=null;invalid=true;continue;}
                decimal amount=days*Convert.ToDecimal(drv["DonGiaThue"])+fine;r.Cells["Amount"].Value=amount;if(chosen.Contains(r))total+=amount;
            }
            decimal advance=0;
            if(chosen.Length==grid.Rows.Count && chosen.Length>0)
            {var a=LibraryDb.Query("SELECT ISNULL(h.TamUng,0)-ISNULL((SELECT SUM(r.TamUngApDung) FROM tblHoSoTra r WHERE r.MaHSM=h.MaHSM),0) A FROM tblHoSoMuon h WHERE h.MaHSM=@h",LibraryDb.P("@h",LibraryUi.Selected(cardOrLoan,"hồ sơ")));if(a.Rows.Count>0)advance=Convert.ToDecimal(a.Rows[0][0]);}
            summary.Text=invalid?"Cần kiểm tra ngày trả, đơn giá hoặc tiền phạt trước khi lưu.":string.Format("Đã chọn {0}/{1} sách chưa trả • Tổng tiền: {2:N0} đ • Trừ tạm ứng: {3:N0} đ\n{4}: {5:N0} đ",chosen.Length,grid.Rows.Count,total,advance,total-advance<0?"Hoàn lại bạn đọc":"Phải thu thêm",Math.Abs(total-advance));
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmCirculation
            // 
            this.ClientSize = new System.Drawing.Size(536, 405);
            this.Name = "frmCirculation";
            this.ResumeLayout(false);

        }

        void Save()
        {
            int staff=LibraryUi.Selected(librarian,"thủ thư"),owner=LibraryUi.Selected(cardOrLoan,returning?"hồ sơ mượn":"thẻ mượn");var chosen=Chosen();
            if(chosen.Length==0)throw new InvalidOperationException("Chọn ít nhất một sách.");
            if(MessageBox.Show(summary.Text+"\n\nXác nhận lưu?","Xác nhận",MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes)return;
            int id=returning?LibraryDb.Return(owner,staff,date.Value,chosen.ToDictionary(r=>Convert.ToInt32(r.Cells["MaSach"].Value),r=>Violation(r))):LibraryDb.Borrow(owner,staff,date.Value,deposit.Value,chosen.Select(r=>Convert.ToInt32(r.Cells["MaSach"].Value)).ToArray());
            MessageBox.Show("Đã lưu hồ sơ "+(returning?"trả":"mượn")+" mã "+id+".","Thành công");deposit.Value=0;Reload();
        }
    }
}
