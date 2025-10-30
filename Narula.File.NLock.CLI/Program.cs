using System.Security;
using Narula.File.NLock;
using Narula.File.NLock.Models;
using Narula.File.NLock.Services;
using Narula.File.NLock.Utilities;
using System.Diagnostics;
using System.Text.Json;

namespace Narula.File.NLock.CLI;

class Program
{
    private static readonly string DeveloperEmail = "developer@narula.com";
    private static readonly string Version = "1.0.0";

    static async Task<int> Main(string[] args)
    {
        if (args.Length == 0)
        {
            ShowHelp();
            return 0;
        }

        var command = args[0].ToLowerInvariant();

        // Global GUI switch: if -g/--gui is present as only arg or trailing, launch GUI
        if (IsGuiRequested(args))
        {
            // Determine which GUI to launch based on command keyword
            if (command == "lock")
            {
                var (files, dirs, output, force) = ParseLockArgs(args);
                LaunchLockGuiWithJson(files, dirs, output, force);
                return 0;
            }
            if (command == "unlock")
            {
                var (files, dirs, output) = ParseUnlockArgs(args);
                LaunchUnlockGuiWithJson(files, dirs, output);
                return 0;
            }

            // Default to Lock UI when only -g is provided without a verb
            LaunchGui("NLock.Lock.exe", Array.Empty<string>());
            return 0;
        }

        try
        {
            return command switch
            {
                "lock" => await HandleLockCommand(args),
                "unlock" => await HandleUnlockCommand(args),
                "new" => await HandleNewCommand(args),
                "validate" => await HandleValidateCommand(args),
                "help" => ShowHelp(),
                "contact" => ShowContact(),
                "--help" or "-h" => ShowHelp(),
                "--version" or "-v" => ShowVersion(),
                _ => ShowUnknownCommand(command)
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.SanitizeExceptionMessage()}");
            return 2;
        }
    }

    private static async Task<int> HandleLockCommand(string[] args)
    {
        var files = new List<string>();
        var directories = new List<string>();
        string? password = null;
        string? totpSecret = null;
        string? outputFolder = null;
        bool force = false;

        // Parse arguments
        for (int i = 1; i < args.Length; i++)
        {
            switch (args[i].ToLowerInvariant())
            {
                case "-g":
                case "--gui":
                    // If GUI flag is found in lock command, launch Lock GUI and exit
                    LaunchLockGuiWithJson(files, directories, outputFolder, force);
                    return 0;
                case "-f":
                case "--file":
                    // Consume one or more file paths until the next option (starts with '-') or end of args
                    while (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                    {
                        files.Add(args[++i]);
                    }
                    break;
                case "-d":
                case "--directory":
                    // Consume one or more directory paths until the next option (starts with '-') or end of args
                    while (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                    {
                        directories.Add(args[++i]);
                    }
                    break;
                case "-p":
                case "--password":
                    if (i + 1 < args.Length)
                    {
                        password = args[++i];
                    }
                    break;
                case "-t":
                case "--totpcode":
                    if (i + 1 < args.Length)
                    {
                        totpSecret = args[++i];
                    }
                    break;
                case "-o":
                case "--outputFolder":
                    if (i + 1 < args.Length)
                    {
                        outputFolder = args[++i];
                        System.IO.Directory.CreateDirectory(outputFolder);
                        if (!System.IO.Directory.Exists(outputFolder))
                        {
                            Console.WriteLine($"Error: Directory '{outputFolder}' does not exist.");
                            return 1;
                        }
                    }
                    break;
                case "--force":
                    force = true;
                    break;
            }
        }

        // Validate inputs
        if (files.Count == 0 && directories.Count == 0)
        {
            Console.WriteLine("Error: Either --file or --directory must be specified.");
            return 1;
        }

        if (string.IsNullOrEmpty(password))
        {
            password = ReadPassword("Enter password: ");
        }

        if (string.IsNullOrEmpty(totpSecret))
        {
            Console.WriteLine("Error: TOTP secret code is required. Use 'nlock new' to generate one.");
            return 1;
        }

        // Get files to process
        var filesToProcess = new List<string>();
        filesToProcess.AddRange(files);
        
        if (directories.Count > 0)
        {
            foreach (var dir in directories)
            {
                if (!System.IO.Directory.Exists(dir))
                {
                    Console.WriteLine($"Error: Directory '{dir}' does not exist.");
                    return 1;
                }
                filesToProcess.AddRange(System.IO.Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly));
            }
        }

        // Filter files based on force option
        if (!force)
        {
            filesToProcess = filesToProcess.Where(f => !f.EndsWith(".nlock")).ToList();
        }

        if (filesToProcess.Count == 0)
        {
            Console.WriteLine("No files to process.");
            return 0;
        }

        // Process each file
        int successCount = 0;
        int errorCount = 0;

        foreach (var file in filesToProcess)
        {
            try
            {
                if (!System.IO.File.Exists(file))
                {
                    Console.WriteLine($"Warning: File '{file}' does not exist. Skipping.");
		continue;
	}

                var nlockInfo = new NLockInfo
                {
                    SourceFile = file,
                    DestinationFile = GetDestinationFile(file, outputFolder),
                    Password = SecureUtils.StringToSecureString(password)
                };

                var result = nlockInfo.TryLock();
                
                if (result.ResultCode == NLockProcessResultCode.Success)
                {
                    Console.WriteLine($"✓ Successfully locked: {file}");
                    successCount++;
                }
                else
                {
                    Console.WriteLine($"✗ Failed to lock '{file}': {result.Exception?.SanitizeExceptionMessage() ?? "Unknown error"}");
                    errorCount++;
                }

                nlockInfo.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error processing '{file}': {ex.SanitizeExceptionMessage()}");
                errorCount++;
            }
        }

        Console.WriteLine($"\nLock operation completed: {successCount} successful, {errorCount} errors.");
        return errorCount > 0 ? 1 : 0;
    }

    private static async Task<int> HandleUnlockCommand(string[] args)
    {
        var files = new List<string>();
        var directories = new List<string>();
        string? password = null;
        string? authCode = null;
        string? outputFolder = null;

        // Parse arguments
        for (int i = 1; i < args.Length; i++)
        {
            switch (args[i].ToLowerInvariant())
            {
                case "-g":
                case "--gui":
                    // If GUI flag is found in unlock command, launch Unlock GUI and exit
                    LaunchUnlockGuiWithJson(files, directories, outputFolder);
                    return 0;
                case "-f":
                case "--file":
                    // Consume one or more file paths until the next option (starts with '-') or end of args
                    while (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                    {
                        files.Add(args[++i]);
                    }
                    break;
                case "-d":
                case "--directory":
                    // Consume one or more directory paths until the next option (starts with '-') or end of args
                    while (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                    {
                        directories.Add(args[++i]);
                    }
                    break;
                case "-p":
                case "--password":
                    if (i + 1 < args.Length)
                    {
                        password = args[++i];
                    }
                    break;
                case "-c":
                case "--authcode":
                    if (i + 1 < args.Length)
                    {
                        authCode = args[++i];
                    }
                    break;
                case "-o":
                case "--outputFolder":
                    if (i + 1 < args.Length)
                    {
                        outputFolder = args[++i];
                    }
                    break;
            }
        }

        // Validate inputs
        if (files.Count == 0 && directories.Count == 0)
        {
            Console.WriteLine("Error: Either --file or --directory must be specified.");
            return 1;
        }

        if (string.IsNullOrEmpty(password))
        {
            password = ReadPassword("Enter password: ");
        }

        if (string.IsNullOrEmpty(authCode))
        {
            Console.Write("Enter auth code: ");
            authCode = Console.ReadLine() ?? "";
        }

        // Get files to process (only .nlock files)
        var filesToProcess = new List<string>();
        filesToProcess.AddRange(files.Where(f => f.ToLower().EndsWith(".nlock")));
        
        if (directories.Count > 0)
        {
            foreach (var dir in directories)
            {
                if (!System.IO.Directory.Exists(dir))
                {
                    Console.WriteLine($"Error: Directory '{dir}' does not exist.");
                    return 1;
                }
                filesToProcess.AddRange(System.IO.Directory.GetFiles(dir, "*.nlock", SearchOption.TopDirectoryOnly));
            }
        }

        if (filesToProcess.Count == 0)
        {
            Console.WriteLine("No .nlock files to process.");
            return 0;
        }

        // Process each file
        int successCount = 0;
        int errorCount = 0;

        foreach (var file in filesToProcess)
        {
            try
            {
                if (!System.IO.File.Exists(file))
                {
                    Console.WriteLine($"Warning: File '{file}' does not exist. Skipping.");
		continue;
	}

                var nlockInfo = new NLockInfo
                {
                    SourceFile = file,
                    DestinationFile = GetDestinationFile(file.Replace(".nlock", ""), outputFolder),
                    Password = SecureUtils.StringToSecureString(password)
                };

                var result = nlockInfo.TryUnlock();
                
                if (result.ResultCode == NLockProcessResultCode.Success)
                {
                    Console.WriteLine($"✓ Successfully unlocked: {file}");
                    successCount++;
                }
                else
                {
                    Console.WriteLine($"✗ Failed to unlock '{file}': {result.Exception?.SanitizeExceptionMessage() ?? "Unknown error"}");
                    errorCount++;
                }

                nlockInfo.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error processing '{file}': {ex.SanitizeExceptionMessage()}");
                errorCount++;
            }
        }

        Console.WriteLine($"\nUnlock operation completed: {successCount} successful, {errorCount} errors.");
        return errorCount > 0 ? 1 : 0;
    }

    private static async Task<int> HandleNewCommand(string[] args)
    {
        string? title = null;
        string? subtitle = null;

        // Parse arguments
        for (int i = 1; i < args.Length; i++)
        {
            switch (args[i].ToLowerInvariant())
            {
                case "--title":
                    if (i + 1 < args.Length)
                    {
                        title = args[++i];
                    }
                    break;
                case "-st":
                case "--subtitle":
                    if (i + 1 < args.Length)
                    {
                        subtitle = args[++i];
                    }
                    break;
            }
        }

        try
        {
            var totpSecret = TOTPService.GenerateTotpSecretBase32();
            var qrUri = TOTPService.CreateTotpUri(totpSecret, AppConstants.MFAIssuer, title ?? "NLock Secret");

            Console.WriteLine("New TOTP secret generated successfully!");
            Console.WriteLine();
            Console.WriteLine($"Secret: {totpSecret}");
            Console.WriteLine($"QR URI: {qrUri}");
            Console.WriteLine();
            Console.WriteLine("Please scan the QR code with your authenticator app:");
            Console.WriteLine("(QR code would be displayed here in a GUI application)");
            Console.WriteLine();
            Console.WriteLine("Use this secret with the 'lock' command:");
            Console.WriteLine($"nlock lock -p \"yourpassword\" -t \"{totpSecret}\" -f \"yourfile.txt\"");
            
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.SanitizeExceptionMessage()}");
            return 2;
        }
    }

    private static async Task<int> HandleValidateCommand(string[] args)
    {
        string? totpSecret = null;
        string? authCode = null;

        // Parse arguments
        for (int i = 1; i < args.Length; i++)
        {
            switch (args[i].ToLowerInvariant())
            {
                case "-t":
                case "--totpcode":
                    if (i + 1 < args.Length)
                    {
                        totpSecret = args[++i];
                    }
                    break;
                case "-c":
                case "--authcode":
                    if (i + 1 < args.Length)
                    {
                        authCode = args[++i];
                    }
                    break;
            }
        }

        if (string.IsNullOrEmpty(totpSecret))
        {
            Console.WriteLine("Error: TOTP secret code is required.");
            return 1;
        }

        if (string.IsNullOrEmpty(authCode))
        {
            Console.Write("Enter auth code to validate: ");
            authCode = Console.ReadLine() ?? "";
        }

        try
        {
            var isValid = TOTPService.ValidateAuthCode(totpSecret, authCode);

            if (isValid)
            {
                Console.WriteLine("Valid");
                return 0;
            }
            else
            {
                Console.WriteLine("Invalid");
                return 1;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.SanitizeExceptionMessage()}");
            return 2;
        }
    }

    private static int ShowHelp()
    {
        Console.WriteLine("NLock - Secure file encryption with TOTP authentication");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  nlock [command] [options]");
        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("  lock        Lock specified file(s), encrypting them with a password and TOTP secret code");
        Console.WriteLine("  unlock      Unlock specified file(s) using the correct password and Auth code");
        Console.WriteLine("  new         Generate a new TOTP secret code for use with NLock");
        Console.WriteLine("  validate    Validate an Auth code against a given TOTP secret");
        Console.WriteLine("  help        Display help information about NLock commands");
        Console.WriteLine("  contact     Display the developer's contact information");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  -h, --help              Show help information");
        Console.WriteLine("  -v, --version           Show version information");
        Console.WriteLine("  -f, --file              Specify the file(s) to lock or unlock");
        Console.WriteLine("  -d, --directory         Specify the directory containing files to lock or unlock");
        Console.WriteLine("  -p, --password          Specify the password for locking or unlocking");
        Console.WriteLine("  -c, --authcode          Specify the Auth code for unlocking");
        Console.WriteLine("  -t, --totpcode          Specify the TOTP secret code for locking or validating");
        Console.WriteLine("  --title                 Optional. Specify a title for the TOTP secret code");
        Console.WriteLine("  -st, --subtitle         Optional. Specify a subtitle for the TOTP secret code");
        Console.WriteLine("  --force                 Optional. Force to lock .nlock files even though they are already locked");
        Console.WriteLine("  -o, --outputFolder      Optional. Specify the output directory to put processed files");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  # Generate new TOTP secret");
        Console.WriteLine("  nlock new");
        Console.WriteLine();
        Console.WriteLine("  # Lock a file");
        Console.WriteLine("  nlock lock -p \"mypassword\" -t \"JBSWY3DPEHPK3PXP\" -f \"file.txt\"");
	Console.WriteLine();
        Console.WriteLine("  # Unlock a file");
        Console.WriteLine("  nlock unlock -p \"mypassword\" -c \"123456\" -f \"file.txt.nlock\"");
	Console.WriteLine();
        Console.WriteLine("  # Validate auth code");
        Console.WriteLine("  nlock validate -t \"JBSWY3DPEHPK3PXP\" -c \"123456\"");
        
        return 0;
    }

    private static int ShowContact()
    {
        Console.WriteLine("NLock Developer Contact Information");
        Console.WriteLine();
        Console.WriteLine($"Email: {DeveloperEmail}");
		Console.WriteLine();
        Console.WriteLine("For support, bug reports, or feature requests, please contact the developer.");
        
        return 0;
    }

    private static int ShowVersion()
    {
        Console.WriteLine($"NLock version {Version}");
        return 0;
    }

    private static bool IsGuiRequested(string[] args)
    {
        if (args == null || args.Length == 0) return false;
        // If only -g/--gui provided
        if (args.Length == 1 && (args[0] == "-g" || args[0] == "--gui")) return true;
        // If last token is -g/--gui
        return args[^1] == "-g" || args[^1] == "--gui";
    }

    private static (List<string> files, List<string> directories, string? output, bool force) ParseLockArgs(string[] args)
    {
        var files = new List<string>();
        var dirs = new List<string>();
        string? output = null; bool force = false;
        for (int i = 1; i < args.Length; i++)
        {
            switch (args[i].ToLowerInvariant())
            {
                case "-f": case "--file":
                    while (i + 1 < args.Length && !args[i + 1].StartsWith("-")) files.Add(args[++i]);
                    break;
                case "-d": case "--directory":
                    while (i + 1 < args.Length && !args[i + 1].StartsWith("-")) dirs.Add(args[++i]);
                    break;
                case "-o": case "--outputfolder":
                    if (i + 1 < args.Length) output = args[++i];
                    break;
                case "--force":
                    force = true; break;
            }
        }
        return (files, dirs, output, force);
    }

    private static (List<string> files, List<string> directories, string? output) ParseUnlockArgs(string[] args)
    {
        var files = new List<string>();
        var dirs = new List<string>();
        string? output = null;
        for (int i = 1; i < args.Length; i++)
        {
            switch (args[i].ToLowerInvariant())
            {
                case "-f": case "--file":
                    while (i + 1 < args.Length && !args[i + 1].StartsWith("-")) files.Add(args[++i]);
                    break;
                case "-d": case "--directory":
                    while (i + 1 < args.Length && !args[i + 1].StartsWith("-")) dirs.Add(args[++i]);
                    break;
                case "-o": case "--outputfolder":
                    if (i + 1 < args.Length) output = args[++i];
                    break;
            }
        }
        return (files, dirs, output);
    }

    private static void LaunchLockGuiWithJson(IEnumerable<string> files, IEnumerable<string> directories, string? outputFolder, bool force)
    {
        var request = new LockGuiLaunchRequest
        {
            Files = files.Select(Path.GetFullPath).ToArray(),
            Directories = directories.Select(Path.GetFullPath).ToArray(),
            OutputFolder = string.IsNullOrWhiteSpace(outputFolder) ? null : Path.GetFullPath(outputFolder!),
            Force = force,
            CreatedAtUtc = DateTime.UtcNow,
            CliVersion = Version
        };
        var json = JsonSerializer.Serialize(request, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
        var b64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));
        LaunchGui("NLock.Lock.exe", new[] { "-json64", b64 });
    }

    private static void LaunchUnlockGuiWithJson(IEnumerable<string> files, IEnumerable<string> directories, string? outputFolder)
    {
        var request = new UnlockGuiLaunchRequest
        {
            Files = files.Select(Path.GetFullPath).ToArray(),
            Directories = directories.Select(Path.GetFullPath).ToArray(),
            OutputFolder = string.IsNullOrWhiteSpace(outputFolder) ? null : Path.GetFullPath(outputFolder!),
            CreatedAtUtc = DateTime.UtcNow,
            CliVersion = Version
        };
        var json = JsonSerializer.Serialize(request, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
        var b64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));
        LaunchGui("NLock.Unlock.exe", new[] { "-json64", b64 });
    }

    private static void LaunchGui(string exeName, string[] forwardedArgs)
    {
        try
        {
            // Try current base directory
            var baseDir = AppContext.BaseDirectory;
            var candidate = System.IO.Path.Combine(baseDir, exeName);
            if (!System.IO.File.Exists(candidate))
            {
                // Try parent directory (useful when running from project root/bin/Debug)
                var parent = System.IO.Directory.GetParent(baseDir)?.Parent?.Parent?.FullName;
                if (!string.IsNullOrEmpty(parent))
                {
                    var alt = System.IO.Path.Combine(parent, exeName);
                    if (System.IO.File.Exists(alt)) candidate = alt;
                }
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = candidate,
                Arguments = string.Join(' ', forwardedArgs.Select(QuoteIfNeeded)),
                UseShellExecute = true
            };
            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to launch GUI: {ex.SanitizeExceptionMessage()}");
        }
    }

    private static string QuoteIfNeeded(string s)
    {
        if (string.IsNullOrEmpty(s)) return "\"\"";
        return s.Contains(' ') || s.Contains('\t') || s.Contains('"') ? $"\"{s.Replace("\"", "\\\"")}\"" : s;
    }

    private static int ShowUnknownCommand(string command)
    {
        Console.WriteLine($"Unknown command: {command}");
        Console.WriteLine("Use 'nlock help' for available commands.");
        return 1;
    }

    private static string ReadPassword(string prompt)
    {
        Console.Write(prompt);
        var password = "";
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
                break;
            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[0..^1];
                Console.Write("\b \b");
            }
            else if (key.KeyChar != '\0')
            {
                password += key.KeyChar;
                Console.Write("*");
            }
	}
	Console.WriteLine();
        return password;
    }

    private static string GetDestinationFile(string sourceFile, string? outputFolder)
    {
        if (string.IsNullOrEmpty(outputFolder))
        {
            return sourceFile + AppConstants.Extension;
        }

        var fileName = Path.GetFileName(sourceFile);
        var destinationFile = Path.Combine(outputFolder, fileName + AppConstants.Extension);
        
        // Ensure output directory exists
        System.IO.Directory.CreateDirectory(outputFolder);
        
        return destinationFile;
    }
}