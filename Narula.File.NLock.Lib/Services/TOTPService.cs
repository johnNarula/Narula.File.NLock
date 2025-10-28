using System.Reflection;
using System.Reflection.Emit;
using System.Web;

using OtpNet;

using QRCoder;

namespace Narula.File.NLock.Services;

public static class TOTPService
{
    public static string GenerateTotpSecretBase32()
    {
		var totpSecret = OtpNet.KeyGeneration.GenerateRandomKey(AppConstants.RandomKeyLength);
        return Base32Encoding.ToString(totpSecret);
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

	public static string CreateQrCodePngBase64Uri(string totpUri, int pixelsPerModule = AppConstants.PixelsPerModule)
	{
		string base64Image = CreateQrCodePngBase64(totpUri, pixelsPerModule);

		// Return the data URI format for easy embedding in HTML <img> tags
		return $"data:image/png;base64,{base64Image}";
	}
	public static string CreateQrCodePngBase64(string totpUri, int pixelsPerModule = AppConstants.PixelsPerModule)
	{
		byte[] qrCodeBytes = CreatePngQrCodeBytes(totpUri, pixelsPerModule);
		string base64Image = Convert.ToBase64String(qrCodeBytes);
		return base64Image;
	}

	public static QRCodeData CreateQrCodeData(string totpUri)
	{
		using var qrGenerator = new QRCodeGenerator();
		QRCodeData qrCodeData = qrGenerator.CreateQrCode(totpUri, QRCodeGenerator.ECCLevel.Q);
		return qrCodeData;
	}

	public static byte[] CreatePngQrCodeBytes(string totpUri, int pixelsPerModule = AppConstants.PixelsPerModule)
	{
		using var qrCodeData = CreateQrCodeData(totpUri);
		using var qrCode = new PngByteQRCode(qrCodeData);
		byte[] qrCodeBytes = qrCode.GetGraphic(pixelsPerModule);

		return qrCodeBytes;
	}

	/// <summary>	
	/// Constructs a valid TOTP URI string.
	/// </summary>
	/// <param name="secretKey">The user's secret key, MUST be Base32 encoded (e.g., JBSWY3DPEHPK3PXP).</param>
	/// <param name="issuer">The name of your application/service (e.g., MyCompany).</param>
	/// <param name="label">The user's identity (e.g., user@example.com).</param>
	/// <param name="timeStepSeconds">The TOTP period in seconds (default is 30).</param>
	/// <param name="digits">The number of digits in the TOTP code (default is 6).</param>
	/// <returns>A constructed, encoded string Uri</returns>
	public static string CreateTotpUri(string base32SecretKey, string issuer, string label,
										int timeStepSeconds = AppConstants.TotpTimeStepSeconds,
										int digits = AppConstants.TotpCodeDigits,
										TotpAlgorithm algorithm = TotpAlgorithm.SHA1)
	{
		//label = HttpUtility.UrlEncode(label);
		//issuer = HttpUtility.UrlEncode(issuer);
		//* Not using UrlEncode because it replaces space with + sign.

		//FORMAT: otpauth://totp/<Label>?secret=<Base32>&issuer=<Issuer>&algorithm=<SHA1|SHA256|SHA512>&digits=<6|7|8>&period=<seconds>

		string uri = $"otpauth://totp/{label}?secret={base32SecretKey}&issuer={issuer}&algorithm={algorithm.ToString()}&digits={digits}&period={timeStepSeconds}";
		return uri;
	}

	public enum TotpAlgorithm
	{
		SHA1,
		SHA256,
		SHA215
	}
}
