using System.Runtime.InteropServices;

namespace Narula.File.NLock.Utilities;

public static class SecureUtils
{
    public static void ClearBytes(byte[] data)
    {
        if (data == null) return;
        for (int i = 0; i < data.Length; i++)
            data[i] = 0;
    }

	public static void ClearString(ref string str)
    {
        if (str == null) return;
        GCHandle handle = GCHandle.Alloc(str, GCHandleType.Pinned);
        try
        {
            Marshal.Copy(new char[str.Length], 0, handle.AddrOfPinnedObject(), str.Length);
        }
        finally
        {
            handle.Free();
            str = null;
        }
    }
}
