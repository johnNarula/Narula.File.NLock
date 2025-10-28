namespace Narula.File.NLock.Models;
[Flags]
public enum NLockProcessResultCode
{
	Success,
	InvalidFile,
	IncorrectPassword,
	InvalidTotpCode,
	IncorrectTotpCode,
	UnexpectedError,
	UnableToReadFile,
	UnableToWriteFile
}
