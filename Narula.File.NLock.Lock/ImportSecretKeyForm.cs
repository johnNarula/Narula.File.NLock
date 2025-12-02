using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Narula.File.NLock.Lib.UI;

using Resx = Narula.File.NLock.Lib.UI.Properties;

namespace Narula.File.NLock;
public partial class ImportSecretKeyForm : Form
{
	public ImportSecretKeyForm()
	{
		InitializeComponent();
		//this.Icon = NLockResx.NLock_Lock_nobg_Icon;
	}
	bool _validated = false;

	public string SecretKey => authGeneratedAuthCodeTextBox.Text.Trim();
	private void authCodeTextBox_TextChanged(object sender, EventArgs e)
	{
		ValidateReadiness();
		if (validateQrCodeButton.Enabled)
			validateQrCodeButton.PerformClick();
	}

	private void ValidateReadiness()
	{
		if (authCodeTextBox.Text.Trim().Length != 6)
		{
			validateQrCodeButton.Enabled = false;
			thumbsPicture.Image = null;
			validateQrCodeButton.Enabled = false;
			okButton.Enabled = false;
		}
		else
		{
			validateQrCodeButton.Enabled = true;

		}
	}

	private void authGeneratedAuthCodeTextBox_TextChanged(object sender, EventArgs e)
	{
		if (authGeneratedAuthCodeTextBox.Text.Trim().Length != AppConstants.SecretKeyLength)
		{
			authCodeTextBox.Enabled = false;
			if (authCodeTextBox.Text != "")
				authCodeTextBox.Text = "";
		}
		else
		{
			authCodeTextBox.Enabled = true;
			ValidateReadiness();
		}
	}

	private void validateQrCodeButton_Click(object sender, EventArgs e)
	{

		_validated = TOTPService.ValidateAuthCode(authGeneratedAuthCodeTextBox.Text.Trim(),
														authCodeTextBox.Text.Trim());
		if (_validated)
		{
			thumbsPicture.Image = Resx.Resources.thumbsUpIcon;
			AppConstants.FailedAttempts = 0;
		}
		else
		{
			authCodeTextBox.Clear();
			thumbsPicture.Image = Resx.Resources.thumbsDownIcon;

			if (++AppConstants.FailedAttempts >= AppConstants.MAX_FAIL_ATTEMPTS)
			{
				MessageBox.Show("Too many failed attempts. Exiting for security.");
				Application.Exit();
			}
		}

		okButton.Enabled = _validated;
	}

	private void okButton_Click(object sender, EventArgs e)
	{
		this.DialogResult = DialogResult.OK;
		this.Close();
	}

	private void cancelButton_Click(object sender, EventArgs e)
	{
		this.Close();
	}
}
