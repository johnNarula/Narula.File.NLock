using System.Security;

namespace Narula.File.NLock.Models;
public  class NLockInfo
{
	public string SourceFile { get; set; } = string.Empty;
	public string DestinationFile { get; set; } = string.Empty;
	public SecureString Password { get; set; } = new SecureString();
	public string TotpSecretCode { get; set; } = string.Empty;
	public string TotpAuthCode { get; set; } = string.Empty;

	/// <summary>
	/// Gets the password as a regular string for cryptographic operations
	/// Note: This creates a temporary string in memory
	/// </summary>
	public string GetPasswordString()
	{
		return Utilities.SecureUtils.SecureStringToString(Password);
	}

	/// <summary>
	/// Sets the password from a regular string
	/// </summary>
	public void SetPassword(string password)
	{
		Password.Dispose();
		Password = Utilities.SecureUtils.StringToSecureString(password);
	}

	/// <summary>
	/// Securely disposes of the password
	/// </summary>
	public void Dispose()
	{
		Password?.Dispose();
	}
}
