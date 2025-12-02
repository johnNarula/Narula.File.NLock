using Resx = Narula.File.NLock.Lib.UI.Properties;
namespace Narula.File.NLock.Lock;

public partial class ZoomedQrForm : Form
{
	public AuthCodeInfo _authCodeInfo = new();
	public ZoomedQrForm()
	{
		InitializeComponent();

	}
	public ZoomedQrForm(AuthCodeInfo authCodeInfo) : this()
	{
		_authCodeInfo = authCodeInfo;
	}

	private void ZoomedQrForm_Load(object sender, EventArgs e)
	{
		qrCodePicture.Image = _authCodeInfo.QrImage;
		authCodeTextBox.Text = _authCodeInfo.AuthCode;
		authGeneratedAuthCodeTextBox.Text = _authCodeInfo.TotpSecret;
		if (_authCodeInfo.Validated)
		{
			thumbsPicture.Image = Resx.Resources.thumbsUpIcon;
		}
		else
		{
			thumbsPicture.Image = null;
		}
	}

	private void authCodeTextBox_TextChanged(object sender, EventArgs e)
	{
		_authCodeInfo.Validated = false;
		_authCodeInfo.AuthCode = authCodeTextBox.Text;
		if (sender == authCodeTextBox)
		{
			if (validateQrCodeButton.Enabled && _authCodeInfo.AuthCode.Length == 6)
			{
				validateQrCodeButton.PerformClick();
			}
		}
	}

	private void validateQrCodeButton_Click(object sender, EventArgs e)
	{
		_authCodeInfo.Validated = false;
		if (string.IsNullOrWhiteSpace(_authCodeInfo.AuthCode))
		{
			MessageBox.Show("Please enter the 6-digit Auth code.");
			return;
		}

		_authCodeInfo.Validated = TOTPService.ValidateAuthCode(_authCodeInfo.TotpSecret, _authCodeInfo.AuthCode);
		if (_authCodeInfo.Validated)
		{
			thumbsPicture.Image = Resx.Resources.thumbsUpIcon;
			AppConstants.FailedAttempts = 0;
		}
		else
		{
			thumbsPicture.Image = Resx.Resources.thumbsDownIcon;

			authCodeTextBox.Clear();
			_authCodeInfo.AuthCode = string.Empty;

			if (++AppConstants.FailedAttempts >= AppConstants.MAX_FAIL_ATTEMPTS)
			{
				MessageBox.Show("Too many failed attempts. Exiting for security.");
				Application.Exit();
			}
		}
	}

	private void authGeneratedAuthCodeTextBox_TextChanged(object sender, EventArgs e)
	{
		importantIcon.Visible = authGeneratedAuthCodeTextBox.Text.Trim().Length > 0;
	}
}
