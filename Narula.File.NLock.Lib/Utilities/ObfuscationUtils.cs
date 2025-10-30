using System.Security.Cryptography;

namespace Narula.File.NLock.Utilities;

/// <summary>
/// Utility class for obfuscated operations to make reverse engineering harder
/// </summary>
public static class ObfuscationUtils
{
    // Obfuscated constants - split and reconstructed
    private static readonly int[] _pbkdf2Parts = { 100_000, 200_000, 300_000 };
    private static readonly int[] _saltParts = { 8, 8 };
    private static readonly int[] _hashParts = { 16, 16 };
    
    /// <summary>
    /// Gets PBKDF2 iterations in an obfuscated way
    /// </summary>
    public static int GetPbkdf2Iterations()
    {
        var result = 0;
        foreach (var part in _pbkdf2Parts)
        {
            result += part;
        }
        return result;
    }
    
    /// <summary>
    /// Gets salt size in an obfuscated way
    /// </summary>
    public static int GetSaltSize()
    {
        var result = 0;
        foreach (var part in _saltParts)
        {
            result += part;
        }
        return result;
    }
    
    /// <summary>
    /// Gets hash size in an obfuscated way
    /// </summary>
    public static int GetHashSize()
    {
        var result = 0;
        foreach (var part in _hashParts)
        {
            result += part;
        }
        return result;
    }
    
    /// <summary>
    /// Obfuscated string comparison to hide sensitive operations
    /// </summary>
    public static bool SecureCompare(byte[] a, byte[] b)
    {
        if (a == null || b == null) return false;
        if (a.Length != b.Length) return false;
        
        // Use XOR to make the comparison less obvious
        int result = 0;
        for (int i = 0; i < a.Length; i++)
        {
            result |= a[i] ^ b[i];
        }
        return result == 0;
    }
    
    /// <summary>
    /// Obfuscated key derivation - splits the operation
    /// </summary>
    public static byte[] DeriveKeyObfuscated(string password, byte[] salt)
    {
        // Split the operation to make it harder to trace
        var step1 = HashPasswordStep1(password, salt);
        var step2 = HashPasswordStep2(step1, salt);
        return step2;
    }
    
    private static byte[] HashPasswordStep1(string password, byte[] salt)
    {
        using var sha = SHA256.Create();
        var pass = Encoding.UTF8.GetBytes(password);
        var combined = new byte[pass.Length + salt.Length];
        Buffer.BlockCopy(pass, 0, combined, 0, pass.Length);
        Buffer.BlockCopy(salt, 0, combined, pass.Length, salt.Length);
        return sha.ComputeHash(combined);
    }
    
    private static byte[] HashPasswordStep2(byte[] step1, byte[] salt)
    {
        using var sha = SHA256.Create();
        var combined = new byte[step1.Length + salt.Length];
        Buffer.BlockCopy(step1, 0, combined, 0, step1.Length);
        Buffer.BlockCopy(salt, 0, combined, step1.Length, salt.Length);
        return sha.ComputeHash(combined);
    }
    
    /// <summary>
    /// Anti-debugging check
    /// </summary>
    public static bool IsDebuggerPresent()
    {
        try
        {
            return System.Diagnostics.Debugger.IsAttached;
        }
        catch
        {
            return true; // Assume debugger present if we can't check
        }
    }
    
    /// <summary>
    /// Exit if debugger detected
    /// </summary>
    public static void ExitIfDebugger()
    {
        if (IsDebuggerPresent())
        {
            Environment.Exit(1);
        }
    }
}
