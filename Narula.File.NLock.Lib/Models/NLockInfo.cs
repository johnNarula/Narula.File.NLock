namespace Narula.File.NLock.Models;
public  class NLockInfo
{
	public string SourceFile { get; set; } = string.Empty;
	public string DestinationFile { get; set; } = string.Empty;
	public  string Password { get; set; } = string.Empty;
	public string TotpSecretCode { get; set; } = string.Empty;
	public string TotpAuthCode { get; set; } = string.Empty;

}
