param([string]$TargetDirectory = $PSScriptRoot)
$ErrorActionPreference = 'Stop'
New-Item -ItemType Directory -Path $TargetDirectory -Force | Out-Null
$target = (Resolve-Path -LiteralPath $TargetDirectory).Path
$mdf = Join-Path $target 'DuLieu.mdf'
$ldf = Join-Path $target 'DuLieu_log.ldf'
if ((Test-Path -LiteralPath $mdf) -or (Test-Path -LiteralPath $ldf)) {
    throw "Database files already exist in $target. Existing data will not be overwritten."
}
& SqlLocalDB start MSSQLLocalDB
if ($LASTEXITCODE -ne 0) { throw 'Cannot start MSSQLLocalDB. Install SQL Server Express LocalDB first.' }
$name = 'QLTV_Setup_' + [Guid]::NewGuid().ToString('N')
$master = New-Object System.Data.SqlClient.SqlConnection 'Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30'
$created = $false
try {
    $master.Open()
    $command = $master.CreateCommand()
    $command.CommandText = "CREATE DATABASE [$name] ON PRIMARY (NAME=N'DuLieu',FILENAME=N'$($mdf.Replace("'","''"))') LOG ON (NAME=N'DuLieu_log',FILENAME=N'$($ldf.Replace("'","''"))');"
    [void]$command.ExecuteNonQuery()
    $command.Dispose()
    $created = $true
    $database = New-Object System.Data.SqlClient.SqlConnection "Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=$name;Integrated Security=True;Connect Timeout=30;Pooling=False"
    try {
        $database.Open()
        $command = $database.CreateCommand()
        $command.CommandText = [IO.File]::ReadAllText((Join-Path $PSScriptRoot 'Schema.sql'))
        [void]$command.ExecuteNonQuery()
        $command.Dispose()
    }
    finally { $database.Dispose() }
}
finally {
    if ($created) {
        $command = $master.CreateCommand()
        $command.CommandText = 'EXEC master.dbo.sp_detach_db @dbname=@name;'
        [void]$command.Parameters.AddWithValue('@name',$name)
        [void]$command.ExecuteNonQuery()
        $command.Dispose()
    }
    $master.Dispose()
}
Write-Output "Created empty library database: $mdf"
