using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Narula.File.NLock.Services;
public static class NLockFile
{
	public static NLockProcessResult TryLock(this NLockInfo nlockInfo)
	{
		NLockProcessResult result = new();
		byte[] fileBytes = Array.Empty<byte>();
		byte[] totpBytes = Array.Empty<byte>();
		byte[] masterKey = Array.Empty<byte>();
		byte[] passwordHash = Array.Empty<byte>();
		byte[] salt = Array.Empty<byte>();

		try
		{
			//1️ Prepare encryption parameters
			salt = CryptoService.GenerateSalt();
			passwordHash = CryptoService.HashPassword(nlockInfo.Password, salt);
			masterKey = CryptoService.DeriveKey(nlockInfo.Password, salt);

			//2️ Read file contents
			fileBytes = System.IO.File.ReadAllBytes(nlockInfo.SourceFile);

			//3️ Encrypt file data (AES with explicit IV)
			var (encryptedFile, iv1) = CryptoService.EncryptWithIv(fileBytes, masterKey);

			//4 Prepare and encrypt TOTP secret
			totpBytes = Encoding.ASCII.GetBytes(nlockInfo.TotpSecretCode.ToUpperInvariant());
			var (encryptedTotp, iv2) = CryptoService.EncryptWithIv(totpBytes, masterKey);

			//5️ Write all data to destination file
			//Ensure Destination directory exists
			FileUtility.EnsureDirectoryOfFileExists(nlockInfo.DestinationFile);
			FileUtility.WriteEncryptedFile(nlockInfo.DestinationFile,
												salt,
												passwordHash,
												encryptedFile,
												encryptedTotp,
												iv1,
												iv2);

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
			SecureUtils.ClearBytes(fileBytes);
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
		string totpSecret = string.Empty;

		try
		{
			if (!FileUtility.ReadEncryptedFile(nlockInfo.SourceFile,
												out salt,
												out storedHash,
												out encryptedFile,
												out encryptedTotp,
												out iv1,
												out iv2))
			{
				result.ResultCode = NLockProcessResultCode.InvalidFile;
				return result;
			}

			var inputHash = CryptoService.HashPassword(nlockInfo.Password, salt);
			if (!CompareHashes(storedHash, inputHash))
			{
				result.ResultCode = NLockProcessResultCode.IncorrectPassword;
				return result;
			}

			var key = CryptoService.DeriveKey(nlockInfo.Password, salt);

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
				result.ResultCode = NLockProcessResultCode.IncorrectTotpCode;
				return result;
			}
			
			//System.IO.File.WriteAllBytes(outputFilePath, result.DataBytes);
			result.DataBytes = CryptoService.DecryptWithIv(encryptedFile, key, iv1);
			var saved = result.DataBytes.SaveFile(nlockInfo.DestinationFile);
			result.ResultCode = (saved) ? NLockProcessResultCode.Success : NLockProcessResultCode.UnableToWriteFile;
			
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
		if (a.Length != b.Length) return false;
		bool same = true;
		for (int i = 0; i < a.Length; i++) same &= (a[i] == b[i]);

		return same;
	}
}
