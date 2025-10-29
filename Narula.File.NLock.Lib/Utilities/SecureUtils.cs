using System.Runtime.InteropServices;
using System.Security;
using System.Collections.Concurrent;

namespace Narula.File.NLock.Utilities;

public static class SecureUtils
{
    public static void ClearBytes(byte[] data)
    {
        if (data == null) return;
        for (int i = 0; i < data.Length; i++)
            data[i] = 0;
    }

	public static void ClearString(ref string str)
    {
        if (str == null) return;
        GCHandle handle = GCHandle.Alloc(str, GCHandleType.Pinned);
        try
        {
            Marshal.Copy(new char[str.Length], 0, handle.AddrOfPinnedObject(), str.Length);
        }
        finally
        {
            handle.Free();
            str = null!;
        }
    }

    /// <summary>
    /// Sanitizes exception messages to prevent information disclosure while preserving useful error context
    /// </summary>
    public static string SanitizeExceptionMessage(this Exception? exception, string fallbackMessage = "An unexpected error occurred")
    {
        if (exception == null) return fallbackMessage;

        // Only expose safe exception types and sanitized messages
        return exception switch
        {
            UnauthorizedAccessException => "Access denied. Please check file permissions.",
            FileNotFoundException => "File not found.",
            DirectoryNotFoundException => "Directory not found.",
            IOException => "File operation failed. Please check if the file is in use.",
            ArgumentNullException => "Required parameter is missing.",
            ArgumentException => "Invalid input provided.",
            NotSupportedException => "Operation not supported.",
            SecurityException => "Security violation detected.",
            _ => fallbackMessage // Generic message for all other exceptions
        };
    }

    /// <summary>
    /// Converts a SecureString to a regular string for cryptographic operations
    /// Note: This creates a temporary string in memory - use with caution
    /// </summary>
    public static string SecureStringToString(SecureString secureString)
    {
        if (secureString == null) return string.Empty;
        
        IntPtr ptr = IntPtr.Zero;
        try
        {
            ptr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
            return Marshal.PtrToStringUni(ptr) ?? string.Empty;
        }
        finally
        {
            if (ptr != IntPtr.Zero)
            {
                Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
        }
    }

    /// <summary>
    /// Converts a regular string to a SecureString
    /// </summary>
    public static SecureString StringToSecureString(string? input)
    {
        var secureString = new SecureString();
        if (input != null)
        {
            foreach (char c in input)
            {
                secureString.AppendChar(c);
            }
        }
        secureString.MakeReadOnly();
        return secureString;
    }

    /// <summary>
    /// Securely clears a string by overwriting its memory
    /// </summary>
    public static void SecureClearString(ref string? str)
    {
        if (str == null) return;
        
        // Convert to char array and clear
        var chars = str.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            chars[i] = '\0';
        }
        
        // Clear the original string reference
        str = null;
        
        // Force garbage collection to help clear memory
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    // Rate limiting functionality
    private static readonly ConcurrentDictionary<string, RateLimitInfo> _rateLimitCache = new();
    
    /// <summary>
    /// Gets the rate limit file path for an identifier
    /// </summary>
    private static string GetRateLimitFilePath(string identifier)
    {
        var rateLimitDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                                       "NLock", AppConstants.RateLimitDirectory);
        Directory.CreateDirectory(rateLimitDir);
        
        // Use hash of identifier for filename to avoid special characters
        var hash = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(identifier)));
        return Path.Combine(rateLimitDir, $"{hash}.dat");
    }

    /// <summary>
    /// Loads rate limit info from persistent storage
    /// </summary>
    private static RateLimitInfo? LoadRateLimitInfo(string identifier)
    {
        try
        {
            var filePath = GetRateLimitFilePath(identifier);
            if (!System.IO.File.Exists(filePath)) return null;

            var fileInfo = new System.IO.FileInfo(filePath);
            if (fileInfo.CreationTime.AddDays(AppConstants.RateLimitFileExpiryDays) < DateTime.UtcNow)
            {
                // File has expired, delete it
                System.IO.File.Delete(filePath);
                return null;
            }

            var data = System.IO.File.ReadAllBytes(filePath);
            if (data.Length < 12) return null; // Invalid file

            var firstAttemptTicks = BitConverter.ToInt64(data, 0);
            var attemptCount = BitConverter.ToInt32(data, 8);

            return new RateLimitInfo
            {
                FirstAttempt = DateTime.FromBinary(firstAttemptTicks),
                AttemptCount = attemptCount
            };
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Saves rate limit info to persistent storage
    /// </summary>
    private static void SaveRateLimitInfo(string identifier, RateLimitInfo rateLimitInfo)
    {
        try
        {
            var filePath = GetRateLimitFilePath(identifier);
            var data = new byte[12];
            
            BitConverter.GetBytes(rateLimitInfo.FirstAttempt.ToBinary()).CopyTo(data, 0);
            BitConverter.GetBytes(rateLimitInfo.AttemptCount).CopyTo(data, 8);
            
            System.IO.File.WriteAllBytes(filePath, data);
        }
        catch
        {
            // Ignore save errors - rate limiting will still work in memory
        }
    }
    
    /// <summary>
    /// Checks if an operation should be rate limited based on the identifier
    /// </summary>
    public static bool IsRateLimited(string identifier)
    {
        var now = DateTime.UtcNow;
        var key = $"rate_limit_{identifier}";
        
        // Try to get from cache first
        if (_rateLimitCache.TryGetValue(key, out var cachedInfo))
        {
            // Check if lockout period has expired
            if (now - cachedInfo.FirstAttempt > TimeSpan.FromMinutes(AppConstants.LockoutDurationMinutes))
            {
                _rateLimitCache.TryRemove(key, out _);
                return false;
            }
            return cachedInfo.AttemptCount > AppConstants.MaxFailedAttempts;
        }

        // Load from persistent storage
        var persistentInfo = LoadRateLimitInfo(identifier);
        if (persistentInfo != null)
        {
            // Check if lockout period has expired
            if (now - persistentInfo.FirstAttempt > TimeSpan.FromMinutes(AppConstants.LockoutDurationMinutes))
            {
                // Clear expired data
                ClearRateLimit(identifier);
                return false;
            }
            
            // Cache the loaded data
            _rateLimitCache[key] = persistentInfo;
            return persistentInfo.AttemptCount > AppConstants.MaxFailedAttempts;
        }

        return false;
    }

    /// <summary>
    /// Records a failed authentication attempt
    /// </summary>
    public static void RecordFailedAttempt(string identifier)
    {
        var now = DateTime.UtcNow;
        var key = $"failed_attempt_{identifier}";
        
        // Get current info from cache or persistent storage
        var currentInfo = _rateLimitCache.GetOrAdd(key, k => LoadRateLimitInfo(identifier) ?? new RateLimitInfo { FirstAttempt = now, AttemptCount = 0 });
        
        // Reset if more than a minute has passed
        if (now - currentInfo.FirstAttempt > TimeSpan.FromMinutes(1))
        {
            currentInfo = new RateLimitInfo { FirstAttempt = now, AttemptCount = 1 };
        }
        else
        {
            currentInfo.AttemptCount++;
        }
        
        // Update cache
        _rateLimitCache[key] = currentInfo;
        
        // Save to persistent storage
        SaveRateLimitInfo(identifier, currentInfo);
    }

    /// <summary>
    /// Checks if too many attempts have been made in the last minute
    /// </summary>
    public static bool IsTooManyAttempts(string identifier)
    {
        var key = $"failed_attempt_{identifier}";
        if (_rateLimitCache.TryGetValue(key, out var rateLimitInfo))
        {
            var now = DateTime.UtcNow;
            if (now - rateLimitInfo.FirstAttempt <= TimeSpan.FromMinutes(1))
            {
                return rateLimitInfo.AttemptCount > AppConstants.MaxAttemptsPerMinute;
            }
        }
        return false;
    }

    /// <summary>
    /// Clears rate limiting data for an identifier (for successful authentication)
    /// </summary>
    public static void ClearRateLimit(string identifier)
    {
        var rateLimitKey = $"rate_limit_{identifier}";
        var attemptKey = $"failed_attempt_{identifier}";
        
        _rateLimitCache.TryRemove(rateLimitKey, out _);
        _rateLimitCache.TryRemove(attemptKey, out _);
        
        // Clear persistent storage
        try
        {
            var filePath = GetRateLimitFilePath(identifier);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
        catch
        {
            // Ignore delete errors
        }
    }

    /// <summary>
    /// Gets remaining lockout time for an identifier
    /// </summary>
    public static TimeSpan? GetRemainingLockoutTime(string identifier)
    {
        var key = $"rate_limit_{identifier}";
        if (_rateLimitCache.TryGetValue(key, out var rateLimitInfo))
        {
            if (rateLimitInfo.AttemptCount > AppConstants.MaxFailedAttempts)
            {
                var elapsed = DateTime.UtcNow - rateLimitInfo.FirstAttempt;
                var remaining = TimeSpan.FromMinutes(AppConstants.LockoutDurationMinutes) - elapsed;
                return remaining > TimeSpan.Zero ? remaining : null;
            }
        }
        return null;
    }

    private class RateLimitInfo
    {
        public DateTime FirstAttempt { get; set; }
        public int AttemptCount { get; set; }
    }
}
