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
public partial class AcceptanceOfTCControl : UserControl
{
	public event EventHandler? TermAccepted; 
	public bool _viewedFully = false;

	public AcceptanceOfTCControl()
	{
		InitializeComponent();
		SetupControl();
		SetTCContent();
	}

	private void SetupControl()
	{
		tcContent.Dock = DockStyle.Fill;
		tcContent.ReadOnly = true;
		tcContent.DetectUrls = false;
		tcContent.WordWrap = true;
		tcContent.ScrollBars = RichTextBoxScrollBars.Vertical;
		tcContent.VScroll += TcContent_VScroll;
		chkAccept.CheckedChanged += ChkAccept_CheckedChanged;
	}

	private static readonly object _lock = new();
	private void TcContent_VScroll(object? sender, EventArgs e)
	{
		if (_viewedFully) return;
		lock (_lock)
		{
			if (tcContent.IsScrolledToBottomProbe())
				MarkContentViewedFully();
		}
	}
	private async void MarkContentViewedFully()
	{
		if (_viewedFully) return;
		_viewedFully = true;

		await Task.Delay(3000);
		
		chkAccept.Enabled = true;
	}
	private void ChkAccept_CheckedChanged(object? sender, EventArgs e)
	{
		if (chkAccept.Checked)
		{
			chkAccept.Enabled = false;
			TermAccepted?.Invoke(this, EventArgs.Empty);
		}
	}

	private void SetTCContent()
	{
		tcContent.Clear();

		var headingFont = new Font("Segoe UI", 10, FontStyle.Bold);
		var bodyFont = new Font("Segoe UI", 9, FontStyle.Regular);

		void Append(string text, Font font, bool addNewLine = true)
		{
			tcContent.SelectionStart = tcContent.TextLength;
			tcContent.SelectionFont = font;
			tcContent.AppendText(text + (addNewLine ? "\n\n" : ""));
		}

		Append("ACCEPTANCE OF TERMS AND CONDITIONS OF USE AND RISK DISCLOSURE", headingFont);
		Append("By clicking “OK”, you acknowledge, understand, and agree to the following terms and conditions governing the use of this software (“the Application”).", bodyFont);

		Append("1. IRRETRIEVABLE ENCRYPTION:", headingFont, false);
		Append(" Once a file is encrypted (locked) using this Application, it can only be decrypted using the correct password and secret key. "
			 + "If you forget, misplace, or lose this information, your encrypted file(s) will be permanently inaccessible. "
			 + "The developer and distributor of this Application assume no responsibility or liability for any data loss resulting from forgotten credentials or lost keys.", bodyFont);

		Append("2. FILE INTEGRITY:", headingFont, false);
		Append(" While this Application does not intentionally delete or modify your original source file(s), it is your sole responsibility to verify that any encrypted file can be successfully decrypted before deleting or altering your original file(s). "
			 + "You are advised to maintain independent backups of all important data.", bodyFont);

		Append("3. NO GUARANTEE OF DATA INTEGRITY OR AVAILABILITY:", headingFont, false);
		Append(" The developer makes no guarantees that encrypted or decrypted file(s) will remain free from corruption, damage, or future incompatibility arising from software updates, system errors, hardware failures, or any other causes.", bodyFont);

		Append("4. ASSUMPTION OF RISK:", headingFont, false);
		Append(" You expressly assume all risks associated with the use of this Application, including but not limited to data loss, corruption, or inaccessibility. "
			 + "The developer, its affiliates, and contributors shall not be held liable for any damages, direct or indirect, incidental, consequential, or otherwise, arising from your use of or inability to use this Application.", bodyFont);

		Append("By proceeding, you affirm that you have read, understood, and accepted these terms in full. "
			 + "If you do not agree, click “Cancel” and discontinue use of this feature.", bodyFont, false);

		tcContent.SelectionStart = 0;
		tcContent.ScrollToCaret();
	}
}
