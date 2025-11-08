# README: install-nfile.bat Command Breakdown

This README explains each command used in the `install-nfile.bat` script that sets up `.nlock` file associations and context menus in Windows.

---

### reg
**Explanation:**  
Windows command-line tool for editing the registry. Used to add, delete, and query keys and values.

### add
**Explanation:**  
Adds a new registry key or value.

### "HKCR\.nlock"
**Explanation:**  
Specifies the registry path under HKEY_CLASSES_ROOT for the `.nlock` file extension.

### /ve
**Explanation:**  
"Value Empty" — modifies the default (unnamed) value of the key.

### /t REG_SZ
**Explanation:**  
Sets the value type to `REG_SZ` (a plain string).

### /d "NLockFile"
**Explanation:**  
Sets the data for the registry value. This maps `.nlock` files to a custom file type named `NLockFile`.

### /f
**Explanation:**  
Forces the command to execute without asking for confirmation (force overwrite).

### "HKCR\NLockFile"
**Explanation:**  
Creates a new custom file type ProgID for `.nlock` files.

### "HKCR\NLockFile\DefaultIcon"
**Explanation:**  
Sets the icon that Windows Explorer uses for `.nlock` files.

### "HKCR\NLockFile\shell\open\command"
**Explanation:**  
Defines what happens when a user double-clicks a `.nlock` file. It points to the `NLock.Unlock.exe` executable.

### "HKCR\AllFileSystemObjects\shell\NLock"
**Explanation:**  
Creates a new context menu entry named `NLock` for all file types (single or multiple selections).

### MUIVerb
**Explanation:**  
Sets the label text shown in the context menu.

### Icon
**Explanation:**  
Specifies the icon that will appear next to a context menu item. Points to an executable and icon index.

### SubCommands
**Explanation:**  
An empty string value that makes the menu a cascading submenu (i.e., a parent with child items).

### "HKCR\AllFileSystemObjects\shell\NLock\shell\Lock"
**Explanation:**  
Defines the "Lock" submenu item under the `NLock` cascading context menu.

### "HKCR\AllFileSystemObjects\shell\NLock\shell\Lock\command"
**Explanation:**  
Specifies the command that runs when the "Lock" menu item is selected.

### Command Format: "\"C:\\Program Files\\NLock\\NLock.Lock.exe\" \"%1\""
**Explanation:**  
- Entire command is wrapped in escaped quotes to support spaces in paths.
- `%1` is replaced by the selected file path.

Same format applies for the "Unlock" entry as well.

---

This README helps clarify how the registry is configured using the batch file and what each line accomplishes.