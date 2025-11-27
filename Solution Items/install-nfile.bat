@ECHO OFF
REM --- 1. Associate .nlock extension with custom file type ---
REM Set the default value of .nlock to the ProgID "NLock.nlockfile"
REG ADD "HKCR\.nlock" /ve /t REG_SZ /d "NLock.nlockfile" /f

REM Create the ProgID key for NLock.nlockfile and set its default (optional description)
REG ADD "HKCR\NLock.nlockfile" /ve /t REG_SZ /d "NLock Encrypted File" /f

REM Set the DefaultIcon for .nlock files to the icon in NLock.Lock.exe (index 0)
REG ADD "HKCR\NLock.nlockfile\DefaultIcon" /ve /t REG_SZ ^
    /d "%ProgramFiles%\NLock\NLock.Lock.exe,0" /f

REM Set the double-click "open" command to use NLock.Unlock.exe with the file path as argument
REG ADD "HKCR\NLock.nlockfile\shell\open\command" /ve /t REG_SZ ^
    /d "\"%ProgramFiles%\NLock\NLock.Unlock.exe\" \"%%1\"" /f

REM --- 2. Add context menu entries for all files ---
REM Create the parent "NLock" submenu under the * (all files) context menu
REG ADD "HKCR\*\shell\NLock" /ve /t REG_SZ /d "" /f
REM Use MUIVerb for the submenu display name and assign the Lock icon to the submenu itself
REG ADD "HKCR\*\shell\NLock" /v "MUIVerb" /t REG_SZ /d "NLock" /f
REG ADD "HKCR\*\shell\NLock" /v "Icon"   /t REG_SZ /d "%ProgramFiles%\NLock\NLock.Lock.exe,0" /f

REM Specify the sub-commands (cascading menu items) to appear under "NLock"
REG ADD "HKCR\*\shell\NLock" /v "SubCommands" /t REG_SZ /d "NLock.Lock;NLock.Unlock" /f

REM --- 3. Define the "Lock" context menu command in CommandStore ---
REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\NLock.Lock" /ve /t REG_SZ /d "Lock" /f
REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\NLock.Lock" /v "Icon" /t REG_SZ ^
    /d "%ProgramFiles%\NLock\NLock.Lock.exe,0" /f
REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\NLock.Lock\command" /ve /t REG_SZ ^
    /d "\"%ProgramFiles%\NLock\NLock.Lock.exe\" \"%%1\"" /f

REM --- 4. Define the "Unlock" context menu command in CommandStore ---
REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\NLock.Unlock" /ve /t REG_SZ /d "Unlock" /f
REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\NLock.Unlock" /v "Icon" /t REG_SZ ^
    /d "%ProgramFiles%\NLock\NLock.Unlock.exe,0" /f
REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\NLock.Unlock\command" /ve /t REG_SZ ^
    /d "\"%ProgramFiles%\NLock\NLock.Unlock.exe\" \"%%1\"" /f

ECHO NLock integration has been installed. 
