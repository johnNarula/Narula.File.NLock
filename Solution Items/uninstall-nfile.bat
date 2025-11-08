@echo off
REM ** uninstall-nfile.bat ** — Remove NLock file association and context menu entries

REM 1. Remove .nlock extension association
reg delete "HKCR\.nlock" /f

REM 2. Remove NLockFile ProgID and all its subkeys (icon and shell commands)
reg delete "HKCR\NLockFile" /f

REM 3. Remove NLock cascaded context menu (Lock/Unlock) from all file types
reg delete "HKCR\AllFileSystemObjects\shell\NLock" /f

echo NLock registry entries have been removed.
