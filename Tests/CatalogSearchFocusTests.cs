// UI regression test: compile with CatalogController.cs, frmCatalog.cs and its Designer.
// The in-memory catalog
// replaces SQL so typing/focus can be tested even when LocalDB is unavailable.
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

class CatalogSearchFocusTests
{
    [STAThread] static int Main()
    {
        try
        {
            Application.EnableVisualStyles();
            using(var form=new QuanLyThuVienQuocGia.frmCatalog("sach"))
            {
                form.ShowInTaskbar=false;form.Opacity=0;form.Show();Application.DoEvents();
                var flags=BindingFlags.Instance|BindingFlags.NonPublic;
                var search=(TextBox)form.GetType().GetField("search",flags).GetValue(form);
                var controller=form.GetType().GetField("controller",flags).GetValue(form);
                var editors=(Dictionary<string,Control>)controller.GetType().GetField("editors",flags).GetValue(controller);
                search.Focus();
                foreach(char c in "Lập trình")
                {
                    search.SelectedText=c.ToString();Application.DoEvents();
                    if(!search.Focused)throw new Exception("Search lost focus after typing: "+search.Text);
                }
                Check(search.Text=="Lập trình" && editors["TenSach"].Text=="","all characters remain in search, not book name");
                Check(search.SelectionStart==search.Text.Length,"caret stays at the end while typing");
                search.Select(4,0);search.SelectedText="X";Application.DoEvents();
                Check(search.Focused && search.Text=="Lập Xtrình" && search.SelectionStart==5,"inserting in the middle preserves focus and caret");
                search.SelectAll();search.SelectedText="";Application.DoEvents();
                Check(search.Focused && search.Text=="" && QuanLyThuVienQuocGia.Catalog.LastSearch=="","clearing search refreshes results without changing focus");
                form.GetType().GetMethod("NewRecord",flags).Invoke(form,null);Application.DoEvents();
                Check(editors["TenSach"].Focused,"explicit New Record still focuses the book name");
                form.Close();
            }
            return 0;
        }
        catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;}
    }
    static void Check(bool value,string name){if(!value)throw new Exception(name);Console.WriteLine("PASS: "+name);}
}

// Minimal dependencies for the production form. No database or Excel is opened.
namespace QuanLyThuVienQuocGia
{
    internal class CatalogField
    {
        public string Name,Label,Kind,Reference=null;
        public bool Required=true,ReadOnly=false;
        public int MaxLength=200;
    }
    internal class CatalogSpec
    {
        public string Table="tblDMSach",Key="MaSach",Title="Search regression test";
        public CatalogField[] Fields={new CatalogField{Name="TenSach",Label="Tên sách",Kind="text"},new CatalogField{Name="SoLuong",Label="Số lượng",Kind="int"}};
    }
    internal static class Catalog
    {
        public static string LastSearch;
        public static Dictionary<string,CatalogSpec> Specs=new Dictionary<string,CatalogSpec>{{"sach",new CatalogSpec()}};
        public static DataTable Lookup(string key){return new DataTable();}
        public static DataTable List(CatalogSpec spec,string text)
        {
            LastSearch=text;var dt=new DataTable();dt.Columns.Add("MaSach",typeof(int));dt.Columns.Add("TenSach");dt.Columns.Add("SoLuong",typeof(int));
            if("Lập trình C#".Contains(text))dt.Rows.Add(1,"Lập trình C#",1);return dt;
        }
        public static int Save(CatalogSpec s,int? id,Dictionary<string,object> v,int? stock){throw new NotSupportedException();}
        public static void Delete(CatalogSpec s,int id){throw new NotSupportedException();}
    }
    internal static class UiTheme { public static Color Navy=Color.Navy; }
    internal static class LibraryUi
    {
        public static void Setup(Form f,string title){f.Text=title;f.Size=new Size(1180,760);}
        public static FlowLayoutPanel Bar(){return new FlowLayoutPanel{Dock=DockStyle.Top,Height=45};}
        public static Label Label(string text){return new Label{Text=text};}
        public static Button Button(string text,Action action){var b=new Button{Text=text};b.Click+=(s,e)=>action();return b;}
        public static DataGridView Grid(){return new DataGridView{Dock=DockStyle.Fill,AllowUserToAddRows=false,SelectionMode=DataGridViewSelectionMode.FullRowSelect};}
        public static void Run(Action action){action();}
        public static void Format(DataGridView g){}
        public static void Export(DataGridView g,string title){throw new NotSupportedException();}
    }
    internal class frmReports:Form { public frmReports(string kind,int? card,int? staff){} }
}
