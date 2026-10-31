using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Xml;

class UiSmokeTests
{
    static Assembly app;
    static int count;
    static object Field(object o,string name){return o.GetType().GetField(name,BindingFlags.Instance|BindingFlags.NonPublic).GetValue(o);}
    static void Call(object o,string method){o.GetType().GetMethod(method,BindingFlags.Instance|BindingFlags.NonPublic).Invoke(o,null);}
    static Form Create(string type,params object[] args)
    {
        var form=(Form)Activator.CreateInstance(app.GetType("QuanLyThuVienQuocGia."+type),args);
        form.BindingContext=new BindingContext();form.CreateControl();return form;
    }
    static void Pass(bool ok,string name){if(!ok)throw new Exception(name);Console.WriteLine("PASS: "+name);count++;}
    [STAThread] static int Main(string[] args)
    {
        try
        {
            string connection=Environment.GetEnvironmentVariable("QLTV_TEST_CONNECTION");
            if(string.IsNullOrEmpty(connection)||!new SqlConnectionStringBuilder(connection).InitialCatalog.StartsWith("QLTV_Test_"))throw new Exception("Disposable test database required.");
            Application.EnableVisualStyles();app=Assembly.LoadFrom(args[0]);
            app.GetType("QuanLyThuVienQuocGia.LibraryDb").GetField("ConnectionOverride",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,connection);
            foreach(string key in new[]{"sach","the","thuthu","nxb","theloai","ngonngu","que","dantoc","vipham"})
            using(var form=Create("frmCatalog",key))
            {Call(form,"LoadLookups");Call(form,"LoadRows");var g=(DataGridView)Field(form,"grid");Pass(g.DataSource is DataTable && g.Columns.Count>0,"catalog form loads: "+key);}
            foreach(string mode in new[]{"revenue","outstanding","loans","returns","violations"})
            using(var form=Create("frmReports",mode,null,null))
            {
                Call(form,"LoadReport");var g=(DataGridView)Field(form,"detail");var dt=(DataTable)g.DataSource;
                Pass(dt!=null && dt.Columns.Count>0,"report SQL and binding: "+mode);
                if(mode=="revenue")
                {
                    var sums=(DataTable)((DataGridView)Field(form,"totals")).DataSource;
                    Pass(dt.Rows.Count>0 && sums.Rows.Count>=3,"revenue produces detail and month/quarter/year totals");
                    decimal sum=dt.AsEnumerable().Sum(r=>r.Field<decimal>("ThucThu"));
                    using(var cn=new SqlConnection(connection))using(var cmd=new SqlCommand("SELECT SUM(SoTienThanhToan) FROM tblHoSoTra WHERE NgayTra>=@from AND NgayTra<=@to",cn))
                    {cmd.Parameters.AddWithValue("@from",new DateTime(DateTime.Today.Year,1,1));cmd.Parameters.AddWithValue("@to",DateTime.Today);cn.Open();Pass(sum==Convert.ToDecimal(cmd.ExecuteScalar()),"revenue detail matches stored settlement totals");}
                }
            }
            foreach(bool returning in new[]{false,true})
            using(var form=Create("frmCirculation",returning))
            {
                Call(form,"Reload");var g=(DataGridView)Field(form,"grid");var cb=(ComboBox)Field(form,"cardOrLoan");
                if(cb.Items.Count>0)cb.SelectedIndex=0;
                Call(form,"LoadBooks");Pass(g.DataSource is DataTable,"circulation form binds: "+returning);
                if(g.Rows.Count>0)
                {
                    g.Rows[0].Cells["Choose"].Value=true;Call(form,"UpdateSummary");
                    Pass(((Label)Field(form,"summary")).Text.Contains(returning?"Đã chọn 1/":"Đã chọn 1 cuốn"),"checkbox drives live preview: "+returning);
                    Pass(g.Rows[0].Cells["Choose"].FormattedValue is bool,"checkbox has valid boolean display: "+returning);
                }
            }
            var export=new DataTable();export.Columns.Add("Ngày trả",typeof(DateTime));export.Columns.Add("Tiền",typeof(decimal));export.Columns.Add("Tên sách");export.Rows.Add(DateTime.Today,12500.5m,"=Tên sách không phải công thức");
            string exportPath=Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"ExportSmoke.xlsx");
            app.GetType("QuanLyThuVienQuocGia.LibraryUi").GetMethod("WriteExcel",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{export,exportPath});
            using(var zip=ZipFile.OpenRead(exportPath))using(var stream=zip.GetEntry("xl/worksheets/sheet1.xml").Open())
            {
                var xml=new XmlDocument();xml.Load(stream);var ns=new XmlNamespaceManager(xml.NameTable);ns.AddNamespace("s",xml.DocumentElement.NamespaceURI);
                Pass(xml.SelectSingleNode("//s:c[@r='A2']/s:v",ns)!=null && xml.SelectSingleNode("//s:c[@r='B2']/s:v",ns).InnerText=="12500.5" && xml.SelectNodes("//s:f",ns).Count==0,"Excel export preserves typed dates/numbers and literal book names");
            }
            Console.WriteLine("ALL "+count+" UI/REPORT CHECKS PASSED");return 0;
        }
        catch(Exception ex){Console.Error.WriteLine(ex);return 1;}
    }
}
