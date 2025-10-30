namespace Narula.File.NLock;
public static class AppConstants
{
	// Obfuscated magic header - reconstructed at runtime
	private static readonly byte[] _magicHeaderBytes = { 0x4E, 0x4C, 0x6F, 0x63, 0x6B, 0x56, 0x32, 0x1A, 0x2B, 0x21, 0x5E, 0x6F, 0x70, 0x5C, 0x21, 0x21 };
	public static string MagicHeader => Encoding.ASCII.GetString(_magicHeaderBytes);
	public const int MagicHeaderSize = 16;
	public const string Extension = ".nlock";    // File extension for encrypted files
	public const string MFAIssuer = "NLock";      // Issuer name for MFA
	public const int SaltSize = 16;          // 16 bytes for salt
	public const int PasswordHashSize = 32;  // 32 bytes for SHA-256 hash
	public const int IvSize = 16;            // 16 bytes for AES IV
	public const int DeriveKeyIterations = 600_000; // PBKDF2 iterations - OWASP recommended minimum for 2024
	public const int RandomKeyLength = 20;     // 20 bytes for MFA secret key
	public const int TotpTimeStepSeconds = 30; //30 seconds time step for TOTP
	public const int TotpCodeDigits = 6;         // 6 digits for TOTP code
	public const int PixelsPerModule = 20; // QR code scale factor
	public const string QrTitlePrefix = MFAIssuer + ": "; // Prefix for QR code title
	
	// File size limits for security
	public const long MaxFileSizeBytes = 2L * 1024 * 1024 * 1024; // 2GB maximum file size
	public const long MaxMemoryBufferSize = 512L * 1024 * 1024;   // 512MB maximum memory buffer
	
	// Rate limiting constants
	public const int MaxFailedAttempts = 5;           // Maximum failed attempts before lockout
	public const int LockoutDurationMinutes = 15;     // Lockout duration in minutes
	public const int MaxAttemptsPerMinute = 10;       // Maximum attempts per minute
	
	// Persistent rate limiting
	public const string RateLimitDirectory = "RateLimit"; // Directory for rate limit files
	public const int RateLimitFileExpiryDays = 7;      // Rate limit files expire after 7 days
	
	// HMAC integrity verification
	public const int HmacSize = 32;                    // 32 bytes for HMAC-SHA256
	public const string HmacAlgorithm = "HMACSHA256";  // HMAC algorithm
}
