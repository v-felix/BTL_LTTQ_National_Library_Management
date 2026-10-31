using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using OfficeOpenXml;

namespace QuanLyThuVienQuocGia
{
    internal static class LibraryUi
    {
        public static void Setup(Form f,string title,int width=1180,int height=760)
        { f.Text=title;f.Size=new Size(width,height);f.MinimumSize=new Size(900,620);f.StartPosition=FormStartPosition.CenterParent;f.Font=new Font("Segoe UI",10);f.BackColor=Color.FromArgb(241,245,250);f.AutoScaleMode=AutoScaleMode.Font; }
        public static FlowLayoutPanel Bar() { return new FlowLayoutPanel{Dock=DockStyle.Top,AutoSize=true,WrapContents=true,Padding=new Padding(8),BackColor=Color.White}; }
        public static Button Button(string text,Action click)
        {
            var b=new Button{Text=text,AutoSize=true,MinimumSize=new Size(100,34),Padding=new Padding(8,2,8,2),BackColor=UiTheme.Primary,ForeColor=Color.White,FlatStyle=FlatStyle.Flat,Margin=new Padding(4),Cursor=Cursors.Hand};
            b.FlatAppearance.BorderSize=0; b.Click+=(s,e)=>Run(click);return b;
        }
        public static Label Label(string text) { return new Label{Text=text,AutoSize=true,Margin=new Padding(5,11,5,5)}; }
        public static DataGridView Grid()
        {
            return new DataGridView{Dock=DockStyle.Fill,ReadOnly=true,AllowUserToAddRows=false,AllowUserToDeleteRows=false,AutoGenerateColumns=true,
                SelectionMode=DataGridViewSelectionMode.FullRowSelect,MultiSelect=false,BackgroundColor=Color.White,BorderStyle=BorderStyle.None,
                AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.DisplayedCells,AutoSizeRowsMode=DataGridViewAutoSizeRowsMode.AllCells,RowHeadersVisible=false,ColumnHeadersHeightSizeMode=DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                AlternatingRowsDefaultCellStyle=new DataGridViewCellStyle{BackColor=Color.FromArgb(242,247,253)},DefaultCellStyle=new DataGridViewCellStyle{Padding=new Padding(4),NullValue=""}};
        }
        public static void Run(Action a)
        {
            try{a();}catch(SqlException ex){MessageBox.Show(ex.Number==547 ? "Không thể xóa hoặc lưu: dữ liệu đang được tham chiếu bởi hồ sơ khác. Hãy kiểm tra danh mục liên quan." : "Không thể thực hiện thao tác với CSDL: "+ex.Message,"Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);}
            catch(Exception ex){MessageBox.Show(ex.Message,"Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);}
        }
        public static ComboBox Combo(DataTable table,bool blank=false)
        {
            if(blank) { var row=table.NewRow();row["Id"]=DBNull.Value;row["Name"]="— Không chọn —";table.Rows.InsertAt(row,0); }
            var cb=new ComboBox{Width=245,DropDownStyle=ComboBoxStyle.DropDownList,DisplayMember="Name",ValueMember="Id",DataSource=table,Margin=new Padding(4,7,4,4),DropDownWidth=450};
            if(!blank) cb.SelectedIndex=-1;return cb;
        }
        public static int Selected(ComboBox cb,string label)
        { if(cb.SelectedValue==null || cb.SelectedValue==DBNull.Value || cb.SelectedValue is DataRowView) throw new InvalidOperationException("Vui lòng chọn "+label+".");return Convert.ToInt32(cb.SelectedValue); }
        public static void Format(DataGridView g)
        {
            foreach(DataGridViewColumn c in g.Columns)
            {
                if(c.ValueType==typeof(decimal)) { c.DefaultCellStyle.Format="N0";c.DefaultCellStyle.Alignment=DataGridViewContentAlignment.MiddleRight; }
                if(c.ValueType==typeof(DateTime)) c.DefaultCellStyle.Format="dd/MM/yyyy";
                c.MinimumWidth=75;
            }
        }
        public static DataTable DisplayTable(DataGridView g)
        {
            var dt=new DataTable();foreach(DataGridViewColumn c in g.Columns) if(c.Visible) dt.Columns.Add(c.HeaderText,c.ValueType??typeof(string));
            foreach(DataGridViewRow r in g.Rows) { var row=dt.NewRow();int i=0;foreach(DataGridViewColumn c in g.Columns) if(c.Visible) row[i++]=r.Cells[c.Index].Value??DBNull.Value;dt.Rows.Add(row); }return dt;
        }
        public static void Export(DataGridView g,string title)
        {
            var dt=DisplayTable(g);if(dt.Rows.Count==0) throw new InvalidOperationException("Không có dữ liệu để xuất.");
            using(var dlg=new SaveFileDialog{Filter="Excel (*.xlsx)|*.xlsx",FileName=title+".xlsx"})
            {
                if(dlg.ShowDialog()!=DialogResult.OK)return;
                WriteExcel(dt,dlg.FileName);
                MessageBox.Show("Đã xuất dữ liệu.","Xuất Excel");
            }
        }
        internal static void WriteExcel(DataTable dt,string path)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Phan Huy Vinh");
            using(var p=new ExcelPackage())
            {
                var w=p.Workbook.Worksheets.Add("DuLieu");w.Cells[1,1].LoadFromDataTable(dt,true);w.Cells[1,1,1,dt.Columns.Count].Style.Font.Bold=true;
                for(int c=0;c<dt.Columns.Count;c++) { if(dt.Columns[c].DataType==typeof(DateTime))w.Column(c+1).Style.Numberformat.Format="dd/mm/yyyy";if(dt.Columns[c].DataType==typeof(decimal))w.Column(c+1).Style.Numberformat.Format="#,##0.00"; }
                w.Cells[w.Dimension.Address].AutoFitColumns(12,55);w.View.FreezePanes(2,1);w.Cells[w.Dimension.Address].AutoFilter=true;p.SaveAs(new FileInfo(path));
            }
        }
        public static void Print(DataGridView g,string title,string summary)
        {
            var dt=DisplayTable(g);var b=new StringBuilder("<html><head><meta charset='utf-8'><style>@page{size:A4 landscape;margin:12mm}body{font:11pt 'Segoe UI'}table{border-collapse:collapse;width:100%}td,th{border:1px solid #bbb;padding:5px}th{background:#edf2fa}thead{display:table-header-group}tr{page-break-inside:avoid}</style></head><body>");
            b.Append("<h2>").Append(WebUtility.HtmlEncode(title)).Append("</h2><p>").Append(WebUtility.HtmlEncode(summary)).Append("</p><table><thead><tr>");
            foreach(DataColumn c in dt.Columns)b.Append("<th>").Append(WebUtility.HtmlEncode(c.ColumnName)).Append("</th>");b.Append("</tr></thead><tbody>");
            foreach(DataRow r in dt.Rows){b.Append("<tr>");foreach(var v in r.ItemArray)b.Append("<td>").Append(WebUtility.HtmlEncode(v is DateTime ? ((DateTime)v).ToString("dd/MM/yyyy") : v is decimal ? ((decimal)v).ToString("N0") : Convert.ToString(v))).Append("</td>");b.Append("</tr>");}b.Append("</tbody></table></body></html>");
            using(var f=new Form{Text="Xem trước và in — "+title,Width=1100,Height=750,StartPosition=FormStartPosition.CenterParent})
            using(var browser=new WebBrowser{Dock=DockStyle.Fill,DocumentText=b.ToString()})
            {var bar=Bar();bar.Controls.Add(Button("In / lưu PDF",()=>browser.ShowPrintDialog()));f.Controls.Add(browser);f.Controls.Add(bar);f.ShowDialog();}
        }
    }
}
