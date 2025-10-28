namespace Narula.File.NLock;
public class AuthCodeInfo
{
    public bool Validated { get; set; }
    public string AuthCode { get; set; }
    public string TotpSecret { get; set; }
    public Image QrImage { get; set; }
    public string QrUri { get; set; }
}
