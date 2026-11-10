using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.CSharp;

// Compile the actual serialized layouts using framework assemblies only.
// No production EXE, custom base form, SQL, Excel or Crystal Reports is loaded.
class ColdDesignerTests
{
    static int checks;
    static void Check(bool ok,string text)
    {if(!ok)throw new Exception(text);Console.WriteLine("PASS: "+text);checks++;}
    static IEnumerable<Control> Children(Control root)
    {foreach(Control c in root.Controls){yield return c;foreach(var child in Children(c))yield return child;}}
    [STAThread] static int Main(string[] args)
    {
        try
        {
            Application.EnableVisualStyles();
            string root=Path.GetFullPath(args[0]);
            string work=Path.Combine(root,"Tests","Work","ColdDesigner_"+Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(work);
            var files=new List<string>();
            var options=new CompilerParameters{GenerateExecutable=false,OutputAssembly=Path.Combine(work,"ColdLayouts.dll")};
            options.ReferencedAssemblies.AddRange(new[]{"System.dll","System.Core.dll","System.Drawing.dll","System.Windows.Forms.dll"});
            foreach(string name in new[]{"frmDMSach","frmCatalog","frmCirculation","frmReports","frmMain"})
            {
                string source=File.ReadAllText(Path.Combine(root,name+".cs"));
                Check(Regex.IsMatch(source,@"class\s+"+name+@"\s*:\s*(?:System\.Windows\.Forms\.)?Form\b"),"framework Form base needs no application build: "+name);
                string layout=File.ReadAllText(Path.Combine(root,name+".Designer.cs"));
                string path=Path.Combine(work,name+".Designer.cs");File.WriteAllText(path,layout,Encoding.UTF8);files.Add(path);
                var stub=new StringBuilder("namespace QuanLyThuVienQuocGia { public partial class "+name+" : System.Windows.Forms.Form { public "+name+"() { InitializeComponent(); } ");
                foreach(var handler in Regex.Matches(layout,@"new System\.EventHandler\(this\.(\w+)\)").Cast<Match>().Select(m=>m.Groups[1].Value).Distinct())
                    stub.Append("private void "+handler+"(object sender, System.EventArgs e) {} ");
                stub.Append("} }");path=Path.Combine(work,name+".cs");File.WriteAllText(path,stub.ToString(),Encoding.UTF8);files.Add(path);
                string resx=Path.Combine(root,name+".resx");
                if(File.Exists(resx))
                {
                    string resource=Path.Combine(work,"QuanLyThuVienQuocGia."+name+".resources");
                    using(var reader=new ResXResourceReader(resx))
                    using(var writer=new ResourceWriter(resource))
                    {foreach(DictionaryEntry item in reader)writer.AddResource((string)item.Key,item.Value);}
                    options.EmbeddedResources.Add(resource);
                }
            }
            using(var compiler=new CSharpCodeProvider())
            {
                var result=compiler.CompileAssemblyFromFile(options,files.ToArray());
                var errors=result.Errors.Cast<CompilerError>().Where(e=>!e.IsWarning).ToArray();
                Check(errors.Length==0,"all serialized layouts compile with framework references only: "+string.Join("; ",errors.Select(e=>e.ToString())));
            }
            var assembly=Assembly.LoadFrom(options.OutputAssembly);
            foreach(string name in new[]{"frmDMSach","frmCatalog","frmCirculation","frmReports","frmMain"})
            using(var surface=new DesignSurface(assembly.GetType("QuanLyThuVienQuocGia."+name)))
            {
                Check(surface.LoadErrors.Count==0,"isolated design surface opens: "+name);
                var host=(IDesignerHost)surface.GetService(typeof(IDesignerHost));
                var controls=Children((Form)host.RootComponent).ToArray();
                Check(name=="frmMain"?controls.OfType<MenuStrip>().Any():controls.OfType<Button>().Count()>=3,"serialized actions are visible: "+name);
                if(name!="frmMain")Check(controls.OfType<DataGridView>().All(g=>g.Columns.Cast<DataGridViewColumn>().Count(c=>c.Visible)>=3),"serialized grid headers are visible: "+name);
            }
            Console.WriteLine("ALL "+checks+" COLD DESIGNER CHECKS PASSED");return 0;
        }
        catch(Exception ex){Console.Error.WriteLine(ex);return 1;}
    }
}
