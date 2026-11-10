param(
    [string]$MSBuildPath='C:\Program Files\Microsoft Visual Studio\18\Insiders\MSBuild\Current\Bin\MSBuild.exe',
    [switch]$SkipBuild
)
$ErrorActionPreference='Stop'
$root=(Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$work=Join-Path $PSScriptRoot 'Work'
New-Item -ItemType Directory -Path $work -Force | Out-Null
$compiler=Join-Path (Split-Path $MSBuildPath) 'Roslyn\csc.exe'
# This check intentionally runs before building the application.
& $compiler /nologo /target:exe "/out:$work\ColdDesignerTests.exe" /r:System.Core.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll /r:System.Design.dll /r:Microsoft.CSharp.dll (Join-Path $PSScriptRoot 'ColdDesignerTests.cs')
if($LASTEXITCODE -ne 0){throw 'Cold Designer test compilation failed.'}
& "$work\ColdDesignerTests.exe" $root | Tee-Object -FilePath "$work\ColdDesignerResults.txt"
if($LASTEXITCODE -ne 0){throw 'Cold Designer tests failed.'}
if(!$SkipBuild) {
    & $MSBuildPath (Join-Path $root 'QuanLyThuVienQuocGia.csproj') /t:Build /p:Configuration=Debug /v:minimal /nologo
    if($LASTEXITCODE -ne 0){throw 'Build failed.'}
}
& $compiler /nologo /target:exe "/out:$work\DesignerFormsTests.exe" /r:System.Data.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll /r:System.Design.dll /r:System.Core.dll (Join-Path $PSScriptRoot 'DesignerFormsTests.cs')
if($LASTEXITCODE -ne 0){throw 'Designer test compilation failed.'}
& "$work\DesignerFormsTests.exe" (Join-Path $root 'bin\Debug\QuanLyThuVienQuocGia.exe') | Tee-Object -FilePath "$work\DesignerResults.txt"
if($LASTEXITCODE -ne 0){throw 'Designer tests failed.'}
& $compiler /nologo /target:exe "/out:$work\CatalogSearchFocusTests.exe" /r:System.Data.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll /r:System.Core.dll (Join-Path $PSScriptRoot 'CatalogSearchFocusTests.cs') (Join-Path $root 'CatalogController.cs') (Join-Path $root 'frmCatalog.cs') (Join-Path $root 'frmCatalog.Designer.cs')
if($LASTEXITCODE -ne 0){throw 'Search focus test compilation failed.'}
& "$work\CatalogSearchFocusTests.exe" | Tee-Object -FilePath "$work\FocusResults.txt"
if($LASTEXITCODE -ne 0){throw 'Search focus tests failed.'}
