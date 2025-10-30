# Build NLock with Native AOT Compilation (Windows Forms Compatible)
Write-Host "Building NLock with Native AOT Compilation..." -ForegroundColor Green
Write-Host ""

# Build CLI (Native AOT with full trimming)
Write-Host "Building CLI (Native AOT with full trimming)..." -ForegroundColor Yellow
dotnet publish "Narula.File.NLock.CLI\Narula.File.NLock.CLI.csproj" -c Release -r win-x64 --self-contained -p:PublishAot=true -p:TrimMode=full -p:EnableCompressionInSingleFile=true -o "bin\Release\CLI-Native"

# Build Lock Application (Native AOT without trimming - Windows Forms limitation)
Write-Host "Building Lock Application (Native AOT without trimming)..." -ForegroundColor Yellow
dotnet publish "Narula.File.NLock.Lock\Narula.File.NLock.Lock.csproj" -c Release -r win-x64 --self-contained -p:PublishAot=true -p:TrimMode=none -p:EnableCompressionInSingleFile=true -p:SuppressTrimAnalysisWarnings=true -p:EnableTrimAnalyzer=false -o "bin\Release\Lock-Native"

# Build Unlock Application (Native AOT without trimming - Windows Forms limitation)
Write-Host "Building Unlock Application (Native AOT without trimming)..." -ForegroundColor Yellow
dotnet publish "Narula.File.NLock.Unlock\Narula.File.NLock.Unlock.csproj" -c Release -r win-x64 --self-contained -p:PublishAot=true -p:TrimMode=none -p:EnableCompressionInSingleFile=true -p:SuppressTrimAnalysisWarnings=true -p:EnableTrimAnalyzer=false -o "bin\Release\Unlock-Native"

Write-Host ""
Write-Host "Native AOT compilation complete!" -ForegroundColor Green
Write-Host ""
Write-Host "Output files:" -ForegroundColor Cyan
Write-Host "- CLI: bin\Release\CLI-Native\NLock.exe (Fully trimmed)" -ForegroundColor White
Write-Host "- Lock: bin\Release\Lock-Native\NLock.Lock.exe (Not trimmed due to Windows Forms)" -ForegroundColor White
Write-Host "- Unlock: bin\Release\Unlock-Native\NLock.Unlock.exe (Not trimmed due to Windows Forms)" -ForegroundColor White
Write-Host ""
Write-Host "Note: Windows Forms applications cannot be fully trimmed due to framework limitations." -ForegroundColor Yellow
Write-Host "However, they are still compiled to native code for better performance and security." -ForegroundColor Green
