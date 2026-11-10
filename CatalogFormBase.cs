using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyThuVienQuocGia
{
    public class CatalogFormBase : Form
    {
        CatalogSpec spec;
        protected readonly Dictionary<string,Control> editors=new Dictionary<string,Control>();
        protected int? selected;
        int? originalStock; bool binding;

        DataGridView grid;
        TextBox search;
        TableLayoutPanel fields;
        Panel editorPanel;
        Label note,status,code;
        Button btnStaffLoans;

        // This base form contains behavior only. Each concrete form owns its
        // Designer controls, so TableLayoutPanel and buttons stay editable.
        protected void ConfigureCatalog(string key)
        {
            grid=Find<DataGridView>("grid");search=Find<TextBox>("search");
            fields=Find<TableLayoutPanel>("fields");editorPanel=Find<Panel>("editorPanel");
            note=Find<Label>("note");status=Find<Label>("status");code=Find<Label>("code");
            btnStaffLoans=Find<Button>("btnStaffLoans");
            spec=Catalog.Specs[key];
            Text=spec.Title;
            btnStaffLoans.Visible=key=="thuthu";
            if(key=="sach")
            {
                foreach(var f in spec.Fields)
                {
                    Control edit=fields.Controls["edit"+f.Name];
                    editors.Add(f.Name,edit);
                }
            }
            else BuildFields();
            foreach(var f in spec.Fields)
                if(editors[f.Name] is TextBox)
                    ((TextBox)editors[f.Name]).MaxLength=f.MaxLength>0?f.MaxLength:32767;
            if(key!="sach")note.Text="Ô có dấu * bắt buộc nhập. Chọn một dòng để sửa; nhấn Thêm mới rồi Lưu để thêm bản ghi.";
        }

        T Find<T>(string name) where T:Control
        {return (T)Controls.Find(name,true).Single();}
        protected void BuildFields()
        {
            fields.SuspendLayout();
            try
            {
                while(fields.Controls.Count>0)fields.Controls[0].Dispose();
                fields.RowStyles.Clear();
                fields.RowCount=(spec.Fields.Length+1)/2;
                for(int i=0;i<fields.RowCount;i++)fields.RowStyles.Add(new RowStyle(SizeType.Absolute,40));
                int index=0;
                foreach(var f in spec.Fields)
                {
                    int row=index/2,col=(index%2)*2;
                    var label=new Label{Text=f.Label+(f.Required?" *":""),AutoSize=true,Anchor=AnchorStyles.Left,Margin=new Padding(4,8,4,8)};
                    Control edit;
                    if(f.Kind=="date")edit=new DateTimePicker{Format=DateTimePickerFormat.Custom,CustomFormat="dd/MM/yyyy",ShowCheckBox=!f.Required};
                    else if(f.Kind=="lookup")edit=new ComboBox{DropDownStyle=ComboBoxStyle.DropDownList,DisplayMember="Name",ValueMember="Id",DropDownWidth=400};
                    else edit=new TextBox{ReadOnly=f.ReadOnly,MaxLength=f.MaxLength>0?f.MaxLength:32767};
                    edit.Dock=DockStyle.Fill;edit.Margin=new Padding(4,6,12,6);edit.Name=f.Name;
                    fields.Controls.Add(label,col,row);fields.Controls.Add(edit,col+1,row);editors.Add(f.Name,edit);index++;
                }
                editorPanel.Height=Math.Min(365,55+fields.RowCount*40);
            }
            finally{fields.ResumeLayout(true);}
        }
        protected void HandleCatalog_Shown(object sender,EventArgs e)
        { if(!IsDesigner)LibraryUi.Run(()=>{LoadLookups();LoadRows();NewRecord();}); }
        bool IsDesigner {get{return System.ComponentModel.LicenseManager.UsageMode==System.ComponentModel.LicenseUsageMode.Designtime || DesignMode;}}
        protected void HandleSearch_Click(object sender,EventArgs e){LibraryUi.Run(LoadRows);}
        protected void HandleExport_Click(object sender,EventArgs e){LibraryUi.Run(()=>LibraryUi.Export(grid,spec.Title));}
        protected void HandleStaffLoans_Click(object sender,EventArgs e)
        { LibraryUi.Run(()=>{if(!selected.HasValue)throw new InvalidOperationException("Chọn thủ thư trước.");using(var f=new frmReports("loans",null,selected))f.ShowDialog(this);}); }
        protected void HandleNew_Click(object sender,EventArgs e){LibraryUi.Run(NewRecord);}
        protected void HandleSave_Click(object sender,EventArgs e){LibraryUi.Run(Save);}
        protected void HandleDelete_Click(object sender,EventArgs e){LibraryUi.Run(Delete);}
        protected void HandleRefresh_Click(object sender,EventArgs e){LibraryUi.Run(()=>{LoadLookups();LoadRows();NewRecord();});}
        protected void HandleGrid_SelectionChanged(object sender,EventArgs e){SelectRecord();}
        protected void HandleSearch_TextChanged(object sender,EventArgs e){if(!IsDesigner)LibraryUi.Run(LoadRows);}
        protected void LoadLookups()
        {
            foreach(var f in spec.Fields.Where(x=>x.Reference!=null))
            {
                var cb=(ComboBox)editors[f.Name];var dt=Catalog.Lookup(f.Reference);var blank=dt.NewRow();blank["Id"]=DBNull.Value;blank["Name"]="— Không chọn —";dt.Rows.InsertAt(blank,0);cb.DataSource=dt;
            }
        }
        protected void LoadRows()
        {
            binding=true;try{
                if(grid.DataSource==null)grid.Columns.Clear();
                grid.AutoGenerateColumns=true;
                grid.DataSource=Catalog.List(spec,search.Text);grid.Columns[spec.Key].HeaderText="Mã";
                foreach(var f in spec.Fields)
                {if(f.Reference!=null){grid.Columns[f.Name].Visible=false;grid.Columns[f.Name+"Text"].HeaderText=f.Label;}else grid.Columns[f.Name].HeaderText=f.Label;}
                LibraryUi.Format(grid);status.Text=grid.Rows.Count+" bản ghi • "+spec.Title;grid.ClearSelection();selected=null;
            }finally{binding=false;}
            ResetEditor();
        }
        protected void NewRecord()
        {
            ResetEditor();
            editors[spec.Fields[0].Name].Focus();
        }
        // Refreshing search results must not move the caret out of the search box.
        protected void ResetEditor()
        {
            selected=null;originalStock=null;grid.ClearSelection();code.Text="Đang thêm mới • Mã tự động khi lưu";
            foreach(var f in spec.Fields)
            {
                Control c=editors[f.Name];
                if(c is ComboBox)((ComboBox)c).SelectedIndex=0;
                else if(c is DateTimePicker){var d=(DateTimePicker)c;d.Value=f.Name=="NgayHetHan"?DateTime.Today.AddYears(1):DateTime.Today;d.Checked=f.Required;}
                else c.Text=f.ReadOnly || f.Name=="SoLuong" || f.Kind=="money" ? "0" : "";
            }
        }
        protected void SelectRecord()
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
        protected void Save()
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
            bool adding=!selected.HasValue;
            int id=Catalog.Save(spec,selected,values,originalStock);
            // A saved record must remain visible even when it does not match the
            // previous search. Clearing Text triggers LoadRows through TextChanged.
            if(search.Text.Length>0)search.Clear();else LoadRows();
            foreach(DataGridViewRow row in grid.Rows)
            {
                if(Convert.ToInt32(row.Cells[spec.Key].Value)!=id)continue;
                grid.CurrentCell=row.Cells[spec.Key];
                row.Selected=true;
                grid.FirstDisplayedScrollingRowIndex=row.Index;
                SelectRecord();
                break;
            }
            status.Text=(adding?"Đã thêm mới":"Đã cập nhật")+" bản ghi mã "+id+". "+grid.Rows.Count+" bản ghi.";
        }
        protected void Delete()
        {
            if(!selected.HasValue)throw new InvalidOperationException("Chọn bản ghi cần xóa.");
            if(MessageBox.Show("Xóa bản ghi mã "+selected+"?",spec.Title,MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes)return;
            Catalog.Delete(spec,selected.Value);LoadRows();NewRecord();
        }
    }
}
