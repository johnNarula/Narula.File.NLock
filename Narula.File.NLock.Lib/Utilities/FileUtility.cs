namespace Narula.File.NLock.Utilities;

public static class FileUtility
{
	internal static void WriteEncryptedFile(string outputPath,
										byte[] salt,
										byte[] passwordHash,
										byte[] encryptedFile,
										byte[] encryptedTotp,
										byte[] iv1,
										byte[] iv2)
	{
		using var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
		using var bw = new BinaryWriter(fs, Encoding.UTF8, false);

		bw.Write(Encoding.ASCII.GetBytes(AppConstants.MagicHeader));
		bw.Write(salt);                 // 16
		bw.Write(passwordHash);         // 32
		bw.Write(iv1);                  // 16
		bw.Write(encryptedFile.Length);
		bw.Write(encryptedFile);
		bw.Write(iv2);                  // 16
		bw.Write(encryptedTotp.Length);
		bw.Write(encryptedTotp);
	}

	internal static bool ReadEncryptedFile(string filePath,
										out byte[] salt,
										out byte[] passwordHash,
										out byte[] encryptedFile,
										out byte[] encryptedTotp,
										out byte[] iv1,
										out byte[] iv2)
	{
		salt = passwordHash = encryptedFile = encryptedTotp = iv1 = iv2 = Array.Empty<byte>();

		using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
		using var br = new BinaryReader(fs, Encoding.UTF8, false);

		var header = Encoding.ASCII.GetString(br.ReadBytes(8));
		if (header != AppConstants.MagicHeader)
			return false;

		salt = br.ReadBytes(16);
		passwordHash = br.ReadBytes(32);
		iv1 = br.ReadBytes(16);

		var fileLen = br.ReadInt32();
		encryptedFile = br.ReadBytes(fileLen);
		iv2 = br.ReadBytes(16);

		var totpLen = br.ReadInt32();
		encryptedTotp = br.ReadBytes(totpLen);

		return true;
	}

	public static bool SaveFile(this byte[] data, string destinationFile)
	{
		try
		{ 			
			EnsureDirectoryOfFileExists(destinationFile);
			System.IO.File.WriteAllBytes(destinationFile, data);
			return true;
		}
		catch
		{
			return false;
		}
	}
	public static bool EnsureDirectoryOfFileExists(string filePath)
	{
		var directory = Path.GetDirectoryName(filePath);
		if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}
		return true;
	}

	public static bool EnsureDirectoryExists(string directory)
	{
		if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}
		return true;
	}

	public static string GetUniqueTargetFilenameInDirectory(string directoryPath, string initialFilename)
	{
		if(System.IO.File.Exists(Path.Combine(directoryPath, initialFilename)) == false)
		{
			return initialFilename;
		}

		var baseFilename = Path.GetFileNameWithoutExtension(initialFilename);
		var extension = Path.GetExtension(initialFilename);

		var targetFileName = $"{baseFilename}{extension}";
		var counter = 1;
		while (true)
		{
			var targetFilePath = Path.Combine(directoryPath, targetFileName);
			if (!System.IO.File.Exists(targetFilePath))
			{
				return targetFileName;
			}
			targetFileName = $"{baseFilename}({counter}){extension}";
			counter++;
		}
	}
}
