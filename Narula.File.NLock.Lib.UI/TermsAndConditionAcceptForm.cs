using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Narula.File.NLock.Lib.UI;
public partial class TermsAndConditionAcceptForm : Form
{
	public TermsAndConditionAcceptForm()
	{
		InitializeComponent();
	}

	private void contentControl_TermAccepted(object sender, EventArgs e)
	{
		okButton.Enabled = true;
	}

	private void cancelButton_Click(object sender, EventArgs e)
	{
		this.Close();
	}

	private void okButton_Click(object sender, EventArgs e)
	{
		this.DialogResult = DialogResult.OK;
		this.Close();
	}
}
