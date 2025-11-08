@echo off
REM ** install-nfile.bat ** — Associate .nlock files with NLock and add context menu entries

REM 1. Create ProgID for .nlock files and set file type description
reg add "HKCR\.nlock" /ve /t REG_SZ /d "NLockFile" /f
reg add "HKCR\NLockFile" /ve /t REG_SZ /d "NLock Encrypted File" /f

REM 2. Set custom icon for .nlock files (icon from NLock.Lock.exe)
reg add "HKCR\NLockFile\DefaultIcon" /ve /t REG_SZ /d "\"C:\Program Files\NLock\NLock.Lock.exe\",0" /f

REM 3. Set double-click (open) command to use NLock.Unlock.exe
reg add "HKCR\NLockFile\shell" /f
reg add "HKCR\NLockFile\shell\open" /f
reg add "HKCR\NLockFile\shell\open\command" /ve /t REG_SZ ^
    /d "\"C:\Program Files\NLock\NLock.Unlock.exe\" \"%1\"" /f

REM 4. Add cascaded context menu "NLock" for all files (Lock/Unlock as sub-items)
reg add "HKCR\AllFileSystemObjects\shell\NLock" /ve /t REG_SZ /d "NLock" /f
reg add "HKCR\AllFileSystemObjects\shell\NLock" /v "MUIVerb" /t REG_SZ /d "NLock" /f
reg add "HKCR\AllFileSystemObjects\shell\NLock" /v "Icon" /t REG_SZ /d "\"C:\Program Files\NLock\NLock.Lock.exe\",0" /f
reg add "HKCR\AllFileSystemObjects\shell\NLock" /v "SubCommands" /t REG_SZ /d "" /f

REM 4a. "Lock" submenu item
reg add "HKCR\AllFileSystemObjects\shell\NLock\shell\Lock" /ve /t REG_SZ /d "Lock" /f
reg add "HKCR\AllFileSystemObjects\shell\NLock\shell\Lock" /v "Icon" /t REG_SZ /d "\"C:\Program Files\NLock\NLock.Lock.exe\",0" /f
reg add "HKCR\AllFileSystemObjects\shell\NLock\shell\Lock\command" /ve /t REG_SZ ^
    /d "\"C:\Program Files\NLock\NLock.Lock.exe\" \"%1\"" /f

REM 4b. "Unlock" submenu item
reg add "HKCR\AllFileSystemObjects\shell\NLock\shell\Unlock" /ve /t REG_SZ /d "Unlock" /f
reg add "HKCR\AllFileSystemObjects\shell\NLock\shell\Unlock" /v "Icon" /t REG_SZ /d "\"C:\Program Files\NLock\NLock.Unlock.exe\",0" /f
reg add "HKCR\AllFileSystemObjects\shell\NLock\shell\Unlock\command" /ve /t REG_SZ ^
    /d "\"C:\Program Files\NLock\NLock.Unlock.exe\" \"%1\"" /f

echo Registry entries for NLock installed successfully.
