# Build NLock with Native AOT Compilation
Write-Host "Building NLock with Native AOT Compilation..." -ForegroundColor Green
Write-Host ""

# Build CLI (Native AOT)
Write-Host "Building CLI (Native AOT)..." -ForegroundColor Yellow
dotnet publish "Narula.File.NLock.CLI\Narula.File.NLock.CLI.csproj" -c Release -r win-x64 --self-contained -p:PublishAot=true -p:TrimMode=full -p:EnableCompressionInSingleFile=true -o "bin\Release\CLI-Native"

# Build Lock Application (Native AOT)
Write-Host "Building Lock Application (Native AOT)..." -ForegroundColor Yellow
dotnet publish "Narula.File.NLock.Lock\Narula.File.NLock.Lock.csproj" -c Release -r win-x64 --self-contained -p:PublishAot=true -p:TrimMode=none -p:EnableCompressionInSingleFile=true -o "bin\Release\Lock-Native"

# Build Unlock Application (Native AOT)
Write-Host "Building Unlock Application (Native AOT)..." -ForegroundColor Yellow
dotnet publish "Narula.File.NLock.Unlock\Narula.File.NLock.Unlock.csproj" -c Release -r win-x64 --self-contained -p:PublishAot=true -p:TrimMode=none -p:EnableCompressionInSingleFile=true -o "bin\Release\Unlock-Native"

Write-Host ""
Write-Host "Native AOT compilation complete!" -ForegroundColor Green
Write-Host ""
Write-Host "Output files:" -ForegroundColor Cyan
Write-Host "- CLI: bin\Release\CLI-Native\NLock.exe" -ForegroundColor White
Write-Host "- Lock: bin\Release\Lock-Native\NLock.Lock.exe" -ForegroundColor White
Write-Host "- Unlock: bin\Release\Unlock-Native\NLock.Unlock.exe" -ForegroundColor White
Write-Host ""
Write-Host "These are native executables with no .NET runtime dependencies!" -ForegroundColor Green
