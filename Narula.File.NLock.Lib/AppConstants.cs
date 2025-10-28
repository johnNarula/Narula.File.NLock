namespace Narula.File.NLock;
public static class AppConstants
{
	public const string MagicHeader = "NLOCKKEY"; // 8 bytes
	public const string Extension = ".nlock";    // File extension for encrypted files
	public const string MFAIssuer = "NLock";      // Issuer name for MFA
	public const int SaltSize = 16;          // 16 bytes for salt
	public const int PasswordHashSize = 32;  // 32 bytes for SHA-256 hash
	public const int IvSize = 16;            // 16 bytes for AES IV
	public const int DeriveKeyIterations = 100_000; // PBKDF2 iterations
	public const int RandomKeyLength = 20;     // 20 bytes for MFA secret key
	public const int TotpTimeStepSeconds = 30; //30 seconds time step for TOTP
	public const int TotpCodeDigits = 6;         // 6 digits for TOTP code
	public const int PixelsPerModule = 20; // QR code scale factor
	public const string QrTitlePrefix = MFAIssuer + ": "; // Prefix for QR code title
}
