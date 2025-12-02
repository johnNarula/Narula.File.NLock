namespace Narula.File.NLock;
public class AuthCodeInfo
{
    public bool Validated { get; set; } = false;
    public string AuthCode { get; set; } = "";
    public string TotpSecret { get; set; } = "";
    public Image? QrImage { get; set; } = null;
    public string QrUri { get; set; } = "";

    public AuthCodeInfo()
    {

    }
}
