using System.Security.Cryptography;
namespace Narula.File.NLock.Services;

public static class CryptoService
{
	internal static byte[] GenerateSalt(int size = AppConstants.SaltSize)
	{
		var salt = new byte[size];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(salt);

		return salt;
	}

	internal static byte[] DeriveKey(string password, byte[] salt, int iterations = AppConstants.DeriveKeyIterations)
	{
		using var rfc = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
		return rfc.GetBytes(32); // AES-256
	}

	internal static byte[] HashPassword(string password, byte[] salt)
	{
		using var sha = SHA256.Create();
		var pass = Encoding.UTF8.GetBytes(password);
		var combined = new byte[pass.Length + salt.Length];
		Buffer.BlockCopy(pass, 0, combined, 0, pass.Length);
		Buffer.BlockCopy(salt, 0, combined, pass.Length, salt.Length);

		return sha.ComputeHash(combined);
	}

	// 🔐 New: explicit IV-based encryption
	internal static (byte[] Cipher, byte[] Iv) EncryptWithIv(byte[] data, byte[] key)
	{
		using var aes = Aes.Create();
		aes.Key = key;
		aes.GenerateIV();

		using var ms = new MemoryStream();
		using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
		{
			cs.Write(data, 0, data.Length);
		}

		return (ms.ToArray(), aes.IV);
	}

	// 🔐 New: explicit IV-based decryption
	internal static byte[] DecryptWithIv(byte[] cipher, byte[] key, byte[] iv)
	{
		using var aes = Aes.Create();
		aes.Key = key;
		aes.IV = iv;
		aes.Padding = PaddingMode.PKCS7;

		using var msIn = new MemoryStream(cipher);
		using var cs = new CryptoStream(msIn, aes.CreateDecryptor(), CryptoStreamMode.Read);
		using var msOut = new MemoryStream();
		cs.CopyTo(msOut);

		return msOut.ToArray();
	}

	// HMAC integrity verification
	internal static byte[] ComputeHmac(byte[] data, byte[] key)
	{
		using var hmac = new HMACSHA256(key);
		return hmac.ComputeHash(data);
	}

	internal static bool VerifyHmac(byte[] data, byte[] key, byte[] expectedHmac)
	{
		var computedHmac = ComputeHmac(data, key);
		return CryptographicOperations.FixedTimeEquals(computedHmac, expectedHmac);
	}
}