using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

class CatalogSaveTests
{
    static Assembly app;
    static readonly BindingFlags Flags=BindingFlags.NonPublic|BindingFlags.Instance;
    static object Field(object target,string name){return target.GetType().GetField(name,Flags).GetValue(target);}
    static void Call(object target,string method){target.GetType().GetMethod(method,Flags).Invoke(target,null);Application.DoEvents();}
    static void Check(bool ok,string message){if(!ok)throw new Exception(message);Console.WriteLine("PASS: "+message);}
    static decimal Count(string cs,string sql){using(var c=new SqlConnection(cs)){c.Open();using(var cmd=new SqlCommand(sql,c))return Convert.ToDecimal(cmd.ExecuteScalar());}}
    [STAThread] static int Main(string[] args)
    {
        try
        {
            string cs=Environment.GetEnvironmentVariable("QLTV_TEST_CONNECTION");
            if(string.IsNullOrEmpty(cs)||!new SqlConnectionStringBuilder(cs).InitialCatalog.StartsWith("QLTV_Test_"))throw new Exception("A disposable test database is required.");
            Application.EnableVisualStyles();app=Assembly.LoadFrom(args[0]);
            app.GetType("QuanLyThuVienQuocGia.LibraryDb").GetField("ConnectionOverride",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,cs);
            decimal before=Count(cs,"SELECT COUNT(*) FROM tblDMSach");
            using(var form=(Form)Activator.CreateInstance(app.GetType("QuanLyThuVienQuocGia.frmCatalog"),new object[]{"sach"}))
            {
                form.ShowInTaskbar=false;form.Opacity=0;form.Show();Application.DoEvents();
                var search=(TextBox)Field(form,"search");
                var grid=(DataGridView)Field(form,"grid");
                var editors=(Dictionary<string,Control>)Field(form,"editors");
                Action<string> fill=name=>{editors["TenSach"].Text=name;editors["TenTG"].Text="Kiểm thử";editors["SoLuong"].Text="4";editors["DonGiaThue"].Text="3000";editors["MaXB"].Text="2026";};
                Call(form,"NewRecord");fill("TEST_BOOK_NO_FILTER");Call(form,"Save");
                Check(Count(cs,"SELECT COUNT(*) FROM tblDMSach")==before+1,"new book is committed to SQL");
                search.Text="NO_MATCH_FOR_NEW_BOOK_7291";Application.DoEvents();
                Check(grid.Rows.Count==0,"search excludes the next book before saving");
                Call(form,"NewRecord");fill("TEST_BOOK_WITH_FILTER");Call(form,"Save");
                Check(Count(cs,"SELECT COUNT(*) FROM tblDMSach")==before+2,"new book is committed even with an active search");
                Check(search.Text=="" && grid.Rows.Cast<DataGridViewRow>().Any(r=>Convert.ToString(r.Cells["TenSach"].Value)=="TEST_BOOK_WITH_FILTER"),"saved book is visible after the previous filter is cleared");
                Check(grid.SelectedRows.Count==1 && Convert.ToString(grid.SelectedRows[0].Cells["TenSach"].Value)=="TEST_BOOK_WITH_FILTER","newly saved book is selected and shown in the editor");
                Call(form,"Save");Check(Count(cs,"SELECT COUNT(*) FROM tblDMSach")==before+2,"saving the selected book again updates it without duplicate insertion");
                Call(form,"NewRecord");Check(editors["TenSach"].Text=="" && Field(form,"selected")==null,"New Record enters insert mode after editing a saved book");
                fill("TEST_BOOK_AFTER_EDIT");Call(form,"Save");Check(Count(cs,"SELECT COUNT(*) FROM tblDMSach")==before+3,"can insert another book after editing");
                form.Close();
            }
            Console.WriteLine("ALL CATALOG SAVE CHECKS PASSED");return 0;
        }
        catch(Exception ex){Console.Error.WriteLine(ex is TargetInvocationException ? ex.InnerException : ex);return 1;}
    }
}
