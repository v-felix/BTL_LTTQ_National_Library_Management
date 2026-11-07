param([string]$MSBuildPath = 'C:\Program Files\Microsoft Visual Studio\18\Insiders\MSBuild\Current\Bin\MSBuild.exe')
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$work = Join-Path $PSScriptRoot 'Work'
New-Item -ItemType Directory -Path $work -Force | Out-Null
$compiler = Join-Path (Split-Path $MSBuildPath) 'Roslyn\csc.exe'
$interop = Get-ChildItem (Join-Path $env:windir 'assembly\GAC_MSIL\Microsoft.Office.Interop.Excel'),(Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Shared\Visual Studio Tools for Office\PIA\Office15') -Filter Microsoft.Office.Interop.Excel.dll -Recurse -ErrorAction SilentlyContinue | Select-Object -First 1 -ExpandProperty FullName
if (!$interop) { throw 'Microsoft Office Interop Excel reference is required.' }
$exe = Join-Path $work 'ExcelExportTests.exe'
& $compiler /nologo /target:exe "/out:$exe" /r:System.Data.dll /r:System.Core.dll /r:System.Drawing.dll /r:System.IO.Compression.dll /r:System.IO.Compression.FileSystem.dll /r:System.Xml.dll "/link:$interop" (Join-Path $PSScriptRoot 'ExcelExportTests.cs') (Join-Path $root 'ExcelExport.cs')
if ($LASTEXITCODE -ne 0) { throw 'Excel export test compilation failed.' }
& $exe | Tee-Object -FilePath (Join-Path $work 'ExcelResults.txt')
if ($LASTEXITCODE -ne 0) { throw 'Excel export tests failed.' }
