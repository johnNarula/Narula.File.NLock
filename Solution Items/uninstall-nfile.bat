@ECHO OFF
REM Remove .nlock file association and ProgID
REG DELETE "HKCR\.nlock" /f
REG DELETE "HKCR\NLock.nlockfile" /f

REM Remove NLock entries from all-files context menu
REG DELETE "HKCR\*\shell\NLock" /f

REM Remove NLock commands from CommandStore
REG DELETE "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\NLock.Lock" /f
REG DELETE "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\NLock.Unlock" /f

ECHO NLock integration has been removed.
