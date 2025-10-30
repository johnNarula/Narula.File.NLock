@echo off
echo Building NLock with Native AOT Compilation...
echo.

echo Building CLI (Native AOT)...
dotnet publish Narula.File.NLock.CLI\Narula.File.NLock.CLI.csproj -c Release -r win-x64 --self-contained -p:PublishAot=true -p:TrimMode=full -p:EnableCompressionInSingleFile=true -o "bin\Release\CLI-Native"

echo.
echo Building Lock Application (Native AOT)...
dotnet publish Narula.File.NLock.Lock\Narula.File.NLock.Lock.csproj -c Release -r win-x64 --self-contained -p:PublishAot=true -p:TrimMode=none -p:EnableCompressionInSingleFile=true -o "bin\Release\Lock-Native"

echo.
echo Building Unlock Application (Native AOT)...
dotnet publish Narula.File.NLock.Unlock\Narula.File.NLock.Unlock.csproj -c Release -r win-x64 --self-contained -p:PublishAot=true -p:TrimMode=none -p:EnableCompressionInSingleFile=true -o "bin\Release\Unlock-Native"

echo.
echo Native AOT compilation complete!
echo.
echo Output files:
echo - CLI: bin\Release\CLI-Native\NLock.exe
echo - Lock: bin\Release\Lock-Native\NLock.Lock.exe  
echo - Unlock: bin\Release\Unlock-Native\NLock.Unlock.exe
echo.
echo These are native executables with no .NET runtime dependencies!
pause
