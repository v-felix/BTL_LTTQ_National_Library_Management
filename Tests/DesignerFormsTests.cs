using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

// No SQL Server is needed: opening a design surface must never query the database.
class DesignerFormsTests
{
    static int checks;
    static void Check(bool ok,string name)
    { if(!ok)throw new Exception(name);Console.WriteLine("PASS: "+name);checks++; }
    static IEnumerable<Control> Descendants(Control root)
    { foreach(Control c in root.Controls){yield return c;foreach(var nested in Descendants(c))yield return nested;} }
    [STAThread] static int Main(string[] args)
    {
        try
        {
            Application.EnableVisualStyles();
            var app=Assembly.LoadFrom(args[0]);
            app.GetType("QuanLyThuVienQuocGia.LibraryDb").GetField("ConnectionOverride",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,"Data Source=unavailable;Initial Catalog=QLTV_Test_Designer;Integrated Security=True;Connect Timeout=1");
            foreach(string name in new[]{"frmCatalog","frmDMSach","frmCirculation","frmReports","frmMain"})
            {
                var type=app.GetType("QuanLyThuVienQuocGia."+name);
                Check(type.BaseType==typeof(Form),"Form has no compiled custom base dependency: "+name);
                using(var surface=new DesignSurface(type))
                {
                    Check(surface.LoadErrors.Count==0,"design surface opens without database: "+name);
                    var host=(IDesignerHost)surface.GetService(typeof(IDesignerHost));
                    var form=(Form)host.RootComponent;
                    var controls=Descendants(form).ToArray();
                    if(name=="frmMain")
                    {
                        Check(controls.Any(c=>c.Name=="lblTieuDe" && c.Text.Contains("QUỐC GIA")),"main title exists in Designer");
                        var menus=controls.OfType<MenuStrip>().Single();
                        var items=menus.Items.OfType<ToolStripMenuItem>().SelectMany(m=>m.DropDownItems.OfType<ToolStripMenuItem>()).ToArray();
                        Check(items.Any(m=>m.Name=="mnuThuThu") && items.Any(m=>m.Name=="mnuTienThu"),"staff and revenue menu items exist in Designer");
                    }
                    else
                    {
                        Check(controls.OfType<Button>().Count()>=3,"action buttons are present: "+name);
                        Check(controls.OfType<DataGridView>().All(g=>g.Columns.Cast<DataGridViewColumn>().Count(c=>c.Visible)>=3),"grid column headers are present: "+name);
                        var names=controls.Select(c=>c.Name).Concat(controls.OfType<DataGridView>().SelectMany(g=>g.Columns.Cast<DataGridViewColumn>().Select(c=>c.Name))).Where(n=>!string.IsNullOrEmpty(n)).ToArray();
                        Check(names.Distinct(StringComparer.OrdinalIgnoreCase).Count()==names.Length,"Designer component names are unique: "+name);
                    }
                    if(name=="frmDMSach")Check(controls.Any(c=>c.Name=="editDonGiaThue") && host.GetDesigner(controls.First(c=>c.Name=="fields"))!=null,"book form exposes an editable layout with the rental rate field");
                    if(name=="frmCirculation")
                    {
                        type.GetProperty("ReturnMode").SetValue(form,true,null);
                        Check(controls.OfType<Button>().Any(c=>c.Text=="Lưu phiếu trả") && controls.OfType<DataGridView>().Single().Columns.Cast<DataGridViewColumn>().Single(c=>c.HeaderText=="Vi phạm").Visible,"return view can be previewed without SQL");
                    }
                }
            }
            Console.WriteLine("ALL "+checks+" DESIGNER CHECKS PASSED");return 0;
        }
        catch(Exception ex){Console.Error.WriteLine(ex);return 1;}
    }
}
