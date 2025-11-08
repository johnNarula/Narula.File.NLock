Building the MSI (WiX)

Prerequisites:
- WiX Toolset (v3.11 or WiX v4) installed and available in PATH.
- If using WiX v3, use candle/light. For WiX v4, use dotnet-wix or the WiX build tasks.

Example (WiX v3):
1. From the `Narula.File.NLock.Installer` directory, run:
   `candle.exe Installer.wxs`
   This produces `Installer.wixobj`.
2. Run:
   `light.exe -ext WixUIExtension Installer.wixobj -out NLockInstaller.msi`

Notes:
- Ensure the `Source` paths in `Installer.wxs` point to the correct build output (Debug/Release). You can update the `$(var.Configuration)` definition or change paths.
- MSI will attempt to write registry entries under HKCR and install files into `Program Files` by default. Installing per-user requires extra authoring (e.g., changing InstallScope to perUser).

Using WiX v4 or the dotnet-wix tool is recommended for .NET 6+ projects; adjust commands accordingly.
