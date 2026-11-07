using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using QuanLyThuVienQuocGia;

class ExcelExportTests
{
    static int count;
    static void Check(bool value,string name){if(!value)throw new Exception(name);Console.WriteLine("PASS: "+name);count++;}
    [STAThread] static int Main()
    {
        try
        {
            var before=Process.GetProcessesByName("EXCEL").Select(p=>p.Id).ToArray();
            var data=new DataTable();data.Columns.Add("Ngày trả",typeof(DateTime));data.Columns.Add("Tiền",typeof(decimal));data.Columns.Add("Tên sách");
            data.Rows.Add(new DateTime(2026,11,8),12500.5m,"=Tên sách không phải công thức");data.Rows.Add(DBNull.Value,0m,"Sách tiếng Việt");
            string path=Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"TeacherExcelSmoke.xlsx");
            ExcelExport.Write(data,path,"Danh sách sách");
            using(var zip=ZipFile.OpenRead(path))
            {
                var xml=new XmlDocument();using(var stream=zip.GetEntry("xl/worksheets/sheet1.xml").Open())xml.Load(stream);
                var ns=new XmlNamespaceManager(xml.NameTable);ns.AddNamespace("s",xml.DocumentElement.NamespaceURI);
                Check(xml.SelectSingleNode("//s:mergeCell[@ref='A5:D5']",ns)!=null,"merged title matches teacher layout");
                Check(xml.SelectSingleNode("//s:c[@r='A8']/s:v",ns).InnerText=="1" && xml.SelectSingleNode("//s:c[@r='A9']/s:v",ns).InnerText=="2","STT starts at one and covers all records");
                Check(xml.SelectSingleNode("//s:c[@r='C8']/s:v",ns).InnerText=="12500.5","money remains numeric and exact");
                Check(xml.SelectSingleNode("//s:c[@r='B8']/s:v",ns).InnerText==new DateTime(2026,11,8).ToOADate().ToString(System.Globalization.CultureInfo.InvariantCulture),"dates retain their Excel numeric value");
                Check(xml.SelectNodes("//s:f",ns).Count==0,"book names beginning with equals remain literal text");
                var styles=new XmlDocument();using(var stream=zip.GetEntry("xl/styles.xml").Open())styles.Load(stream);
                var styleNs=new XmlNamespaceManager(styles.NameTable);styleNs.AddNamespace("s",styles.DocumentElement.NamespaceURI);
                Check(styles.SelectSingleNode("//s:font/s:name[@val='Times New Roman']",styleNs)!=null,"Times New Roman is included");
                var strings=new XmlDocument();using(var stream=zip.GetEntry("xl/sharedStrings.xml").Open())strings.Load(stream);
                Check(strings.InnerText.Contains("DANH SÁCH SÁCH") && strings.InnerText.Contains("STT") && strings.InnerText.Contains("=Tên sách không phải công thức"),"Vietnamese title, headers and book text survive saving");
            }
            var wide=new DataTable();for(int i=0;i<28;i++)wide.Columns.Add("Cột "+i);wide.Rows.Add(Enumerable.Range(0,28).Select(i=>(object)("Giá trị "+i)).ToArray());
            ExcelExport.Write(wide,Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"TeacherExcelWide.xlsx"),"Báo cáo nhiều cột");
            Check(File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"TeacherExcelWide.xlsx")),"export supports columns beyond Z");
            foreach(var process in Process.GetProcessesByName("EXCEL").Where(p=>!before.Contains(p.Id)))process.WaitForExit(5000);
            Check(!Process.GetProcessesByName("EXCEL").Any(p=>!before.Contains(p.Id)),"owned Excel instances exit after export");
            Console.WriteLine("ALL "+count+" EXCEL CHECKS PASSED");return 0;
        }
        catch(Exception ex){Console.Error.WriteLine(ex);return 1;}
    }
}
