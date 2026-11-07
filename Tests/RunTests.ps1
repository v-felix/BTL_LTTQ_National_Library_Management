param([string]$MSBuildPath = 'C:\Program Files\Microsoft Visual Studio\18\Insiders\MSBuild\Current\Bin\MSBuild.exe')
$ErrorActionPreference = 'Stop'
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$work = Join-Path $PSScriptRoot 'Work'
New-Item -ItemType Directory -Path $work -Force | Out-Null
$testName = 'QLTV_Test_' + [Guid]::NewGuid().ToString('N')
$testMdf = Join-Path $work ($testName + '.mdf')
$testLdf = Join-Path $work ($testName + '_log.ldf')
$backup = Join-Path $work 'SourceForTests.bak'
$dbFile = Join-Path $projectRoot 'Database\DuLieu.mdf'
$compiler = Join-Path (Split-Path $MSBuildPath) 'Roslyn\csc.exe'
function Sql([string]$query) {
    & sqlcmd -S '(LocalDB)\MSSQLLocalDB' -E -C -b -Q $query
    if ($LASTEXITCODE -ne 0) { throw 'SQL test setup/cleanup failed.' }
}
$oldConnection = $env:QLTV_TEST_CONNECTION
$created = $false
try {
    & $MSBuildPath (Join-Path $projectRoot 'QuanLyThuVienQuocGia.csproj') /t:Build /p:Configuration=Debug /v:minimal /nologo
    if ($LASTEXITCODE -ne 0) { throw 'Build failed.' }
    $safeDb = $dbFile.Replace(']',']]')
    $safeBackup = $backup.Replace("'","''")
    Sql "BACKUP DATABASE [$safeDb] TO DISK=N'$safeBackup' WITH COPY_ONLY,INIT;"
    Sql "RESTORE DATABASE [$testName] FROM DISK=N'$safeBackup' WITH MOVE N'DuLieu' TO N'$($testMdf.Replace("'","''"))',MOVE N'DuLieu_log' TO N'$($testLdf.Replace("'","''"))';"
    $created = $true
    $env:QLTV_TEST_CONNECTION = "Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=$testName;Integrated Security=True;Connect Timeout=10"
    $integrationExe = Join-Path $work 'LibraryIntegrationTests.exe'
    $sources = @('Tests\LibraryIntegrationTests.cs','LibraryDb.cs','LocalDbStartup.cs','Catalog.cs','ProcessDataBase.cs','DatabaseMaintenance.cs') | ForEach-Object { Join-Path $projectRoot $_ }
    & $compiler /nologo /target:exe "/out:$integrationExe" /r:System.Data.dll /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Core.dll "/resource:$(Join-Path $projectRoot 'LibraryUpgrade.sql'),LibraryUpgrade.sql" $sources
    if ($LASTEXITCODE -ne 0) { throw 'Integration test compilation failed.' }
    & $integrationExe | Tee-Object -FilePath (Join-Path $work 'IntegrationResults.txt')
    if ($LASTEXITCODE -ne 0) { throw 'Integration tests failed.' }
    $smokeExe = Join-Path $work 'UiSmokeTests.exe'
    & $compiler /nologo /target:exe "/out:$smokeExe" /r:System.Data.dll /r:System.Windows.Forms.dll /r:System.Core.dll /r:System.IO.Compression.dll /r:System.IO.Compression.FileSystem.dll /r:System.Xml.dll (Join-Path $PSScriptRoot 'UiSmokeTests.cs')
    if ($LASTEXITCODE -ne 0) { throw 'UI test compilation failed.' }
    & $smokeExe (Join-Path $projectRoot 'bin\Debug\QuanLyThuVienQuocGia.exe') | Tee-Object -FilePath (Join-Path $work 'UiResults.txt')
    if ($LASTEXITCODE -ne 0) { throw 'UI/report smoke tests failed.' }
    & (Join-Path $PSScriptRoot 'RunExcelTests.ps1') -MSBuildPath $MSBuildPath
    $startupExe = Join-Path $work 'LocalDbStartupTests.exe'
    & $compiler /nologo /target:exe "/out:$startupExe" /r:System.Data.dll /r:System.Core.dll (Join-Path $PSScriptRoot 'LocalDbStartupTests.cs') (Join-Path $projectRoot 'LocalDbStartup.cs')
    if ($LASTEXITCODE -ne 0) { throw 'Startup test compilation failed.' }
    $startupName = 'QLTV_Startup_' + [Guid]::NewGuid().ToString('N')
    & SqlLocalDB create $startupName
    if ($LASTEXITCODE -ne 0) { throw 'Cannot create disposable startup-test instance.' }
    try {
        & $startupExe $startupName | Tee-Object -FilePath (Join-Path $work 'StartupResults.txt')
        if ($LASTEXITCODE -ne 0) { throw 'Cold-start tests failed.' }
    }
    finally {
        & SqlLocalDB stop $startupName
        & SqlLocalDB delete $startupName
    }
}
finally {
    $env:QLTV_TEST_CONNECTION = $oldConnection
    if ($created -and $testName.StartsWith('QLTV_Test_')) {
        Sql "ALTER DATABASE [$testName] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [$testName];"
    }
}
