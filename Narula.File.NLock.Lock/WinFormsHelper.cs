namespace Narula.File.NLock;
internal static class WinFormsHelper
{
	public static Bitmap GetQrImage(this QRCodeData qrData, int pixelPerModule = 20)
	{
		using var qrCode = new QRCode(qrData);
		return qrCode.GetGraphic(pixelPerModule);
	}
}
