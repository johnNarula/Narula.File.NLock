using OtpNet;

namespace Narula.File.NLock.Services;

public static class TOTPService
{
    public static string GenerateTotpSecret()//Base32
    {
        // Fix: Use OtpNet's KeyGeneration class, which is in the OtpNet namespace.
        var totpSecret = OtpNet.KeyGeneration.GenerateRandomKey(AppConstants.RandomKeyLength);
        return Base32Encoding.ToString(totpSecret);
    }

    public static string GenerateTotpQrCodeUri(string fileName, string totpSecret)
    {
        string targetFilename = $"{fileName}.{AppConstants.Extension}";
        string issuer = $"{AppConstants.MFAIssuer}: {targetFilename}";
        string label = targetFilename;

			return $"otpauth://totp/{label}?secret={totpSecret}&issuer={issuer}";
    }

	public static string GenerateTotpQrCodeUri(string totpSecret, string label, string issuerSuffix)
	{
        string issuer = $"{AppConstants.MFAIssuer}";

		if (!string.IsNullOrWhiteSpace(issuerSuffix))
            issuer += $": {issuerSuffix}";
         
		return $"otpauth://totp/{label}?secret={totpSecret}&issuer={issuer}";
	}

	public static bool ValidateAuthCode(string totpSecret, string authCode)
	{
		if (string.IsNullOrWhiteSpace(totpSecret) || string.IsNullOrWhiteSpace(authCode))
			return false;

		authCode = authCode.Trim().Replace(" ", "").PadLeft(6, '0');

		var secretBytes = OtpNet.Base32Encoding.ToBytes(totpSecret.ToUpperInvariant());
		var totp = new OtpNet.Totp(secretBytes, step: 30);

		// allow +/- 1 window for timing tolerance
		return totp.VerifyTotp(authCode, out _, new OtpNet.VerificationWindow(previous: 1, future: 1));
	}

}
