namespace Narula.File.NLock.Models;

public class NLockProcessResult
{
	public NLockProcessResultCode ResultCode { get; set; } = NLockProcessResultCode.Success;
	public byte[] DataBytes { get; set; } = [];
	public Exception? Exception { get; set; } = null;
}
