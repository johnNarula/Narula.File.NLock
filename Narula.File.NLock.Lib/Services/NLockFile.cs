using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Narula.File.NLock.Services;
public static class NLockFile
{
	public static NLockProcessResult TryLock(this NLockInfo nlockInfo)
	{
		NLockProcessResult result = new();
		byte[]? fileBytes = null;
		byte[] totpBytes = Array.Empty<byte>();
		byte[] masterKey = Array.Empty<byte>();
		byte[] passwordHash = Array.Empty<byte>();
		byte[] salt = Array.Empty<byte>();

		try
		{
			//1️ Prepare encryption parameters
			salt = CryptoService.GenerateSalt();
			var passwordString = nlockInfo.GetPasswordString();
			try
			{
				passwordHash = CryptoService.HashPassword(passwordString, salt);
				masterKey = CryptoService.DeriveKey(passwordString, salt);
			}
			finally
			{
				// Securely clear the temporary password string
				SecureUtils.SecureClearString(ref passwordString);
			}

			//2️ Read file contents with size validation
			fileBytes = FileUtility.ReadFileBytesSafely(nlockInfo.SourceFile);
			if (fileBytes == null)
			{
				result.ResultCode = NLockProcessResultCode.FileTooLarge;
				return result;
			}

			//3️ Encrypt file data (AES with explicit IV)
			var (encryptedFile, iv1) = CryptoService.EncryptWithIv(fileBytes, masterKey);

			//4 Prepare and encrypt TOTP secret
			totpBytes = Encoding.ASCII.GetBytes(nlockInfo.TotpSecretCode.ToUpperInvariant());
			var (encryptedTotp, iv2) = CryptoService.EncryptWithIv(totpBytes, masterKey);

			//5️ Generate HMAC for integrity verification
			// Combine all data for HMAC: salt + passwordHash + iv1 + encryptedFile + iv2 + encryptedTotp
			var hmacData = new byte[salt.Length + passwordHash.Length + iv1.Length + encryptedFile.Length + iv2.Length + encryptedTotp.Length];
			var offset = 0;
			Buffer.BlockCopy(salt, 0, hmacData, offset, salt.Length);
			offset += salt.Length;
			Buffer.BlockCopy(passwordHash, 0, hmacData, offset, passwordHash.Length);
			offset += passwordHash.Length;
			Buffer.BlockCopy(iv1, 0, hmacData, offset, iv1.Length);
			offset += iv1.Length;
			Buffer.BlockCopy(encryptedFile, 0, hmacData, offset, encryptedFile.Length);
			offset += encryptedFile.Length;
			Buffer.BlockCopy(iv2, 0, hmacData, offset, iv2.Length);
			offset += iv2.Length;
			Buffer.BlockCopy(encryptedTotp, 0, hmacData, offset, encryptedTotp.Length);
			
			var hmac = CryptoService.ComputeHmac(hmacData, masterKey);

			//6️ Write all data to destination file
			//Ensure Destination directory exists
			FileUtility.EnsureDirectoryOfFileExists(nlockInfo.DestinationFile);
			FileUtility.WriteEncryptedFile(nlockInfo.DestinationFile,
												salt,
												passwordHash,
												encryptedFile,
												encryptedTotp,
												iv1,
												iv2,
												hmac);

			return result;
		}
		catch (Exception ex)
		{
			result.Exception = ex;
			result.ResultCode = NLockProcessResultCode.UnexpectedError;
			return result;
		}
		finally
		{
			//Secure memory cleanup
			if (fileBytes != null) SecureUtils.ClearBytes(fileBytes);
			SecureUtils.ClearBytes(masterKey);
			SecureUtils.ClearBytes(totpBytes);
			SecureUtils.ClearBytes(passwordHash);
			SecureUtils.ClearBytes(salt);
		}
	}

	public static NLockProcessResult TryUnlock(this NLockInfo nlockInfo)
	{
		NLockProcessResult result = new();
		byte[] fileBytes = Array.Empty<byte>();
		byte[] totpBytes = Array.Empty<byte>();
		byte[] masterKey = Array.Empty<byte>();
		byte[] passwordHash = Array.Empty<byte>();
		byte[] salt = Array.Empty<byte>();
		byte[] iv1 = Array.Empty<byte>();
		byte[] iv2 = Array.Empty<byte>();
		byte[] storedHash = Array.Empty<byte>();
		byte[] encryptedFile = Array.Empty<byte>();
		byte[] encryptedTotp = Array.Empty<byte>();
		byte[] totpSeedBytes = Array.Empty<byte>();
		byte[] hmac = Array.Empty<byte>();
		string totpSecret = string.Empty;

		try
		{
			// Check rate limiting before processing
			var identifier = Path.GetFileName(nlockInfo.SourceFile);
			if (SecureUtils.IsRateLimited(identifier))
			{
				result.ResultCode = NLockProcessResultCode.AccountLocked;
				return result;
			}

			if (SecureUtils.IsTooManyAttempts(identifier))
			{
				result.ResultCode = NLockProcessResultCode.RateLimited;
				return result;
			}

			// Validate file size before processing
			if (!FileUtility.ValidateFileSize(nlockInfo.SourceFile, out long fileSize))
			{
				result.ResultCode = NLockProcessResultCode.FileTooLarge;
				return result;
			}

			if (!FileUtility.ReadEncryptedFile(nlockInfo.SourceFile,
												out salt,
												out storedHash,
												out encryptedFile,
												out encryptedTotp,
												out iv1,
												out iv2,
												out hmac))
			{
				result.ResultCode = NLockProcessResultCode.InvalidFile;
				return result;
			}

			var passwordString = nlockInfo.GetPasswordString();
			try
			{
				var inputHash = CryptoService.HashPassword(passwordString, salt);
				if (!CompareHashes(storedHash, inputHash))
				{
					SecureUtils.RecordFailedAttempt(identifier);
					result.ResultCode = NLockProcessResultCode.IncorrectPassword;
					return result;
				}

				var key = CryptoService.DeriveKey(passwordString, salt);

				// Verify HMAC integrity before proceeding (only for new format files)
				if (hmac.Length > 0)
				{
					var hmacData = new byte[salt.Length + storedHash.Length + iv1.Length + encryptedFile.Length + iv2.Length + encryptedTotp.Length];
					var offset = 0;
					Buffer.BlockCopy(salt, 0, hmacData, offset, salt.Length);
					offset += salt.Length;
					Buffer.BlockCopy(storedHash, 0, hmacData, offset, storedHash.Length);
					offset += storedHash.Length;
					Buffer.BlockCopy(iv1, 0, hmacData, offset, iv1.Length);
					offset += iv1.Length;
					Buffer.BlockCopy(encryptedFile, 0, hmacData, offset, encryptedFile.Length);
					offset += encryptedFile.Length;
					Buffer.BlockCopy(iv2, 0, hmacData, offset, iv2.Length);
					offset += iv2.Length;
					Buffer.BlockCopy(encryptedTotp, 0, hmacData, offset, encryptedTotp.Length);
					
					if (!CryptoService.VerifyHmac(hmacData, key, hmac))
					{
						result.ResultCode = NLockProcessResultCode.IntegrityCheckFailed;
						return result;
					}
				}
				// For old format files (hmac.Length == 0), skip HMAC verification for backward compatibility

				// ✅ use the fixed API
				totpSeedBytes = CryptoService.DecryptWithIv(encryptedTotp, key, iv2);
				totpSecret = Encoding.ASCII.GetString(totpSeedBytes)
					.ToUpperInvariant()
					.Trim('\0', ' ', '\r', '\n');
				totpSecret = GetTopSecret(totpSecret);

				if (string.IsNullOrWhiteSpace(totpSecret))
				{
					result.ResultCode = NLockProcessResultCode.InvalidTotpCode;
					return result;
				}

				if (!TOTPService.ValidateAuthCode(totpSecret, nlockInfo.TotpAuthCode))
				{
					SecureUtils.RecordFailedAttempt(identifier);
					result.ResultCode = NLockProcessResultCode.IncorrectTotpCode;
					return result;
				}
				
				// Clear rate limiting on successful authentication
				SecureUtils.ClearRateLimit(identifier);
				
				//System.IO.File.WriteAllBytes(outputFilePath, result.DataBytes);
				result.DataBytes = CryptoService.DecryptWithIv(encryptedFile, key, iv1);
				var saved = result.DataBytes.SaveFile(nlockInfo.DestinationFile);
				result.ResultCode = (saved) ? NLockProcessResultCode.Success : NLockProcessResultCode.UnableToWriteFile;
				
				return result;
			}
			finally
			{
				// Securely clear the temporary password string
				SecureUtils.SecureClearString(ref passwordString);
			}
		}
		catch (Exception ex)
		{
			result.Exception = ex;
			result.ResultCode = NLockProcessResultCode.UnexpectedError;
			return result;
		}
		finally
		{
			//Secure memory cleanup
			SecureUtils.ClearBytes(fileBytes);
			SecureUtils.ClearBytes(masterKey);
			SecureUtils.ClearBytes(totpBytes);
			SecureUtils.ClearBytes(passwordHash);
			SecureUtils.ClearBytes(salt);
			SecureUtils.ClearBytes(iv1);
			SecureUtils.ClearBytes(iv2);
			SecureUtils.ClearBytes(storedHash);
			SecureUtils.ClearBytes(encryptedFile);
			SecureUtils.ClearBytes(encryptedTotp);
			SecureUtils.ClearBytes(totpSeedBytes);
			SecureUtils.ClearBytes(hmac);
			SecureUtils.ClearString(ref totpSecret);
		}
	}

	private static string GetTopSecret(string totpSecret)
	{

		//TODO: To validate which is better of the two code block
		return new string(Array.FindAll(
			totpSecret.ToCharArray(),
			c => (c >= 'A' && c <= 'Z') || (c >= '2' && c <= '7')));
		

		//return new string(Array.FindAll(
		//	totpSecret.ToCharArray(),
		//	c => c is >= 'A' and <= 'Z' or >= '2' and <= '7'));
	}

	private static bool CompareHashes(byte[] a, byte[] b)
	{
		// Use constant-time comparison to prevent timing attacks
		return CryptographicOperations.FixedTimeEquals(a, b);
	}
}
