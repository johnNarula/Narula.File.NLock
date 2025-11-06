namespace Narula.File.NLock.Lib.UI;

partial class TermsAndConditionAcceptForm
{
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		okButton = new Button();
		cancelButton = new Button();
		contentControl = new AcceptanceOfTCControl();
		SuspendLayout();
		// 
		// okButton
		// 
		okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		okButton.DialogResult = DialogResult.OK;
		okButton.Enabled = false;
		okButton.Location = new Point(297, 426);
		okButton.Name = "okButton";
		okButton.Size = new Size(75, 23);
		okButton.TabIndex = 0;
		okButton.Text = "&OK";
		okButton.UseVisualStyleBackColor = true;
		okButton.Click += okButton_Click;
		// 
		// cancelButton
		// 
		cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		cancelButton.DialogResult = DialogResult.Cancel;
		cancelButton.Location = new Point(216, 426);
		cancelButton.Name = "cancelButton";
		cancelButton.Size = new Size(75, 23);
		cancelButton.TabIndex = 2;
		cancelButton.Text = "&Cancel";
		cancelButton.UseVisualStyleBackColor = true;
		cancelButton.Click += cancelButton_Click;
		// 
		// contentControl
		// 
		contentControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		contentControl.Location = new Point(12, 12);
		contentControl.Name = "contentControl";
		contentControl.Size = new Size(360, 408);
		contentControl.TabIndex = 0;
		contentControl.TermAccepted += contentControl_TermAccepted;
		// 
		// TermsAndConditionAcceptForm
		// 
		AcceptButton = okButton;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		BackgroundImageLayout = ImageLayout.Center;
		CancelButton = cancelButton;
		ClientSize = new Size(384, 461);
		Controls.Add(contentControl);
		Controls.Add(cancelButton);
		Controls.Add(okButton);
		FormBorderStyle = FormBorderStyle.FixedSingle;
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "TermsAndConditionAcceptForm";
		ShowIcon = false;
		ShowInTaskbar = false;
		StartPosition = FormStartPosition.CenterParent;
		Text = "NLock";
		ResumeLayout(false);
	}

	#endregion

	private Button okButton;
	private Button cancelButton;
	private AcceptanceOfTCControl contentControl;
}