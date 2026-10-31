using System;
using System.Data.SqlClient;
using QuanLyThuVienQuocGia;

class LocalDbStartupTests
{
    static int Main(string[] args)
    {
        try
        {
            if(args.Length!=1 || !args[0].StartsWith("QLTV_Startup_"))throw new Exception("A disposable startup-test instance is required.");
            LocalDbStartup.EnsureRunning(args[0]);
            using(var cn=new SqlConnection(new SqlConnectionStringBuilder{DataSource=LocalDbStartup.DataSource,InitialCatalog="master",IntegratedSecurity=true,ConnectTimeout=5,Pooling=false}.ConnectionString))
            {cn.Open();using(var cmd=new SqlCommand("SELECT 1",cn))if(Convert.ToInt32(cmd.ExecuteScalar())!=1)throw new Exception("Probe failed.");}
            Console.WriteLine("PASS: stopped LocalDB instance starts and accepts a direct pipe connection");
            LocalDbStartup.EnsureRunning(args[0]);
            Console.WriteLine("PASS: repeated startup is safe when LocalDB is already running");
            return 0;
        }
        catch(Exception ex){Console.Error.WriteLine(ex);return 1;}
    }
}
