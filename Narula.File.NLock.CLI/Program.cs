// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Reflection.Emit;

using Narula.File.NLock.Models;
using Narula.File.NLock.Services;
using Narula.File.NLock.Utilities;

Console.WriteLine("Hello, World!");

const string SRC_FOLDER =		@"C:\Users\inter\OneDrive\Downloads\N256test\";
const string LOCKED_FOLDER =	@"C:\Users\inter\OneDrive\Downloads\N256test\locked";
const string UNLOCKED_FOLDER =	@"C:\Users\inter\OneDrive\Downloads\N256test\unlocked";

const string SRC_FILE =			@"C:\Users\inter\OneDrive\Downloads\N256test\pic1.png";
const string LOCKED_FILE =		@"C:\Users\inter\OneDrive\Downloads\N256test\locked\pic1.png.nlock";
const string UNLOCKED_FILE =	@"C:\Users\inter\OneDrive\Downloads\N256test\unlocked\pic1.png";
const string TOTP_CODE = "46FKY3BVDN2FC53NWG76NMGLBFPXJ4WV";

while (true)
{
	Console.Clear();
	Console.Write("Enter Auto code to validate: ");
	var authCode = Console.ReadLine();

	bool validated = TOTPService.ValidateAuthCode(TOTP_CODE, authCode ?? string.Empty);
	Console.WriteLine($"Auth code valid: {validated}");
	if (!validated)
	{
		Console.WriteLine("Unable to continue.  Hit any key to close...");
		Console.ReadKey();
		continue;
	}

	NLockInfo srcNFileInfo = new()
	{
		SourceFile = SRC_FILE,
		DestinationFile = LOCKED_FILE,
		TotpSecretCode = TOTP_CODE
	};
	srcNFileInfo.SetPassword("TestPassword123!");

	Console.WriteLine();
	Console.WriteLine();
	Console.Write("Locking file...");
	var result = NLockFile.TryLock(srcNFileInfo);
	Console.WriteLine(result.ResultCode.ToString());
	if (result.ResultCode != NLockProcessResultCode.Success)
	{
		Console.WriteLine("Exception: " + SecureUtils.SanitizeExceptionMessage(result.Exception));
		Console.WriteLine();
		Console.WriteLine("Unable to continue.  Hit any key to close...");
		Console.ReadKey();
		continue;
	}

	NLockInfo lockedNFileInfo = new()
	{
		SourceFile = LOCKED_FILE,
		DestinationFile = UNLOCKED_FILE,
		TotpAuthCode = authCode
	};
	lockedNFileInfo.SetPassword("TestPassword123!");

	Console.WriteLine();
	Console.WriteLine();
	Console.Write("Unlocking file...");
	result = NLockFile.TryUnlock(lockedNFileInfo);
	Console.WriteLine(result.ResultCode.ToString());
	if (result.ResultCode != NLockProcessResultCode.Success)
	{
		Console.WriteLine("Exception: " + SecureUtils.SanitizeExceptionMessage(result.Exception));
		Console.WriteLine();
		Console.WriteLine("Unable to continue.  Hit any key to close...");
		Console.ReadKey();
		continue;
	}
	Console.WriteLine();
	Console.WriteLine();
	Console.WriteLine("Ready for another round? Hit any key to continue...");
	Console.ReadKey();
}