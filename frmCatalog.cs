using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyThuVienQuocGia
{
    internal sealed class frmCatalog : Form
    {
        readonly CatalogSpec spec;
        readonly DataGridView grid=LibraryUi.Grid();
        readonly Dictionary<string,Control> editors=new Dictionary<string,Control>();
        readonly TextBox search=new TextBox{Width=330,Margin=new Padding(4,8,4,4)};
        readonly Label status=new Label{Dock=DockStyle.Bottom,Height=35,Padding=new Padding(10),ForeColor=UiTheme.Navy};
        readonly Label code=new Label{Text="Mã: tự động",AutoSize=true,Padding=new Padding(8)};
        readonly TableLayoutPanel fields=new TableLayoutPanel{Dock=DockStyle.Top,AutoSize=true,ColumnCount=4,Padding=new Padding(12)};
        int? selected,originalStock; bool binding;
        public frmCatalog(string key)
        {
            spec=Catalog.Specs[key];LibraryUi.Setup(this,spec.Title);
            var bar=LibraryUi.Bar();bar.Controls.Add(LibraryUi.Label("Tìm kiếm"));bar.Controls.Add(search);
            bar.Controls.Add(LibraryUi.Button("Tìm / tải lại",LoadRows));bar.Controls.Add(LibraryUi.Button("Xuất Excel",()=>LibraryUi.Export(grid,spec.Title)));
            if(key=="thuthu")bar.Controls.Add(LibraryUi.Button("Hồ sơ đã cho mượn",()=> {if(!selected.HasValue)throw new InvalidOperationException("Chọn thủ thư trước.");new frmReports("loans",null,selected).ShowDialog(this);}));
            var buttons=LibraryUi.Bar();buttons.Controls.Add(LibraryUi.Button("Thêm mới",NewRecord));buttons.Controls.Add(LibraryUi.Button("Lưu",Save));buttons.Controls.Add(LibraryUi.Button("Xóa",Delete));buttons.Controls.Add(LibraryUi.Button("Làm mới",()=>{LoadLookups();LoadRows();NewRecord();}));buttons.Controls.Add(code);
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,185));fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,185));fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
            int index=0;
            foreach(var f in spec.Fields)
            {
                int row=index/2,col=(index%2)*2;
                var label=new Label{Text=f.Label+(f.Required ? " *" : ""),AutoSize=true,Anchor=AnchorStyles.Left,Margin=new Padding(4,8,4,8)};
                Control edit;
                if(f.Kind=="date")edit=new DateTimePicker{Format=DateTimePickerFormat.Custom,CustomFormat="dd/MM/yyyy",ShowCheckBox=!f.Required};
                else if(f.Kind=="lookup")edit=new ComboBox{DropDownStyle=ComboBoxStyle.DropDownList,DisplayMember="Name",ValueMember="Id",DropDownWidth=400};
                else edit=new TextBox{ReadOnly=f.ReadOnly,MaxLength=f.MaxLength>0?f.MaxLength:32767};
                edit.Dock=DockStyle.Fill;edit.Margin=new Padding(4,6,12,6);edit.Name=f.Name;
                fields.Controls.Add(label,col,row);fields.Controls.Add(edit,col+1,row);editors.Add(f.Name,edit);index++;
            }
            var editorPanel=new Panel{Dock=DockStyle.Top,Height=Math.Min(365,55+((spec.Fields.Length+1)/2)*40),AutoScroll=true};editorPanel.Controls.Add(fields);
            var note=new Label{Dock=DockStyle.Top,AutoSize=true,Padding=new Padding(12,4,12,8),ForeColor=Color.DimGray,Text=key=="sach" ? "Số lượng = số cuốn hiện còn trong kho. Dữ liệu cũ chưa kiểm kê được đặt bằng 0; hãy nhập tồn kho và đơn giá trước khi cho mượn." : "Ô có dấu * bắt buộc nhập. Chọn một dòng để sửa; nhấn Thêm mới để tạo bản ghi."};
            Controls.Add(grid);Controls.Add(status);Controls.Add(buttons);Controls.Add(editorPanel);Controls.Add(note);Controls.Add(bar);
            grid.SelectionChanged+=(s,e)=>SelectRecord();search.TextChanged+=(s,e)=>LibraryUi.Run(LoadRows);
            Shown+=(s,e)=>LibraryUi.Run(()=>{LoadLookups();LoadRows();NewRecord();});
        }
        void LoadLookups()
        {
            foreach(var f in spec.Fields.Where(x=>x.Reference!=null))
            {
                var cb=(ComboBox)editors[f.Name];var dt=Catalog.Lookup(f.Reference);var blank=dt.NewRow();blank["Id"]=DBNull.Value;blank["Name"]="— Không chọn —";dt.Rows.InsertAt(blank,0);cb.DataSource=dt;
            }
        }
        void LoadRows()
        {
            binding=true;try{
                grid.DataSource=Catalog.List(spec,search.Text);grid.Columns[spec.Key].HeaderText="Mã";
                foreach(var f in spec.Fields)
                {if(f.Reference!=null){grid.Columns[f.Name].Visible=false;grid.Columns[f.Name+"Text"].HeaderText=f.Label;}else grid.Columns[f.Name].HeaderText=f.Label;}
                LibraryUi.Format(grid);status.Text=grid.Rows.Count+" bản ghi • "+spec.Title;grid.ClearSelection();selected=null;
            }finally{binding=false;}
            NewRecord();
        }
        void NewRecord()
        {
            selected=null;originalStock=null;grid.ClearSelection();code.Text="Mã: tự động khi lưu";
            foreach(var f in spec.Fields)
            {
                Control c=editors[f.Name];
                if(c is ComboBox)((ComboBox)c).SelectedIndex=0;
                else if(c is DateTimePicker){var d=(DateTimePicker)c;d.Value=f.Name=="NgayHetHan"?DateTime.Today.AddYears(1):DateTime.Today;d.Checked=f.Required;}
                else c.Text=f.ReadOnly || f.Name=="SoLuong" || f.Kind=="money" ? "0" : "";
            }
            editors[spec.Fields[0].Name].Focus();
        }
        void SelectRecord()
        {
            if(binding || grid.SelectedRows.Count==0)return;
            var row=((DataRowView)grid.SelectedRows[0].DataBoundItem).Row;selected=Convert.ToInt32(row[spec.Key]);code.Text="Mã: "+selected;
            originalStock=spec.Table=="tblDMSach"?(int?)Convert.ToInt32(row["SoLuong"]):null;
            foreach(var f in spec.Fields)
            {
                object v=row[f.Name];Control c=editors[f.Name];
                if(c is ComboBox){var cb=(ComboBox)c;cb.SelectedValue=v;if(cb.SelectedIndex<0)cb.SelectedIndex=0;}
                else if(c is DateTimePicker){var d=(DateTimePicker)c;d.Value=v==DBNull.Value?DateTime.Today:Convert.ToDateTime(v);d.Checked=v!=DBNull.Value;}
                else c.Text=v==DBNull.Value?"":Convert.ToString(v,CultureInfo.CurrentCulture);
            }
        }
        void Save()
        {
            var values=new Dictionary<string,object>();
            foreach(var f in spec.Fields.Where(x=>!x.ReadOnly))
            {
                Control c=editors[f.Name];object v;
                if(c is ComboBox){v=((ComboBox)c).SelectedValue;if(v is DataRowView || v==DBNull.Value)v=null;}
                else if(c is DateTimePicker){var d=(DateTimePicker)c;v=d.Checked?(object)d.Value.Date:null;}
                else if(string.IsNullOrWhiteSpace(c.Text))v=null;
                else if(f.Kind=="int"){int n;if(!int.TryParse(c.Text.Trim(),out n))throw new InvalidOperationException(f.Label+" phải là số nguyên.");v=n;}
                else if(f.Kind=="money"){decimal n;if(!decimal.TryParse(c.Text.Trim(),NumberStyles.Number,CultureInfo.CurrentCulture,out n))throw new InvalidOperationException(f.Label+" phải là số hợp lệ.");if(n>9999999999999999.99m)throw new InvalidOperationException(f.Label+" quá lớn.");v=decimal.Round(n,2);}
                else v=c.Text.Trim();
                values[f.Name]=v;
            }
            int id=Catalog.Save(spec,selected,values,originalStock);LoadRows();NewRecord();status.Text="Đã lưu bản ghi mã "+id+". "+grid.Rows.Count+" bản ghi.";
        }
        void Delete()
        {
            if(!selected.HasValue)throw new InvalidOperationException("Chọn bản ghi cần xóa.");
            if(MessageBox.Show("Xóa bản ghi mã "+selected+"?",spec.Title,MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes)return;
            Catalog.Delete(spec,selected.Value);LoadRows();NewRecord();
        }
    }
}
