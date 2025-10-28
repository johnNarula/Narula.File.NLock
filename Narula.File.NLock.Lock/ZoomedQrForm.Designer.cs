namespace Narula.File.NLock.Lock;

partial class ZoomedQrForm
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
		authGeneratedAuthCodeTextBox = new TextBox();
		thumbsPicture = new PictureBox();
		qrCodePicture = new PictureBox();
		authCodeTextBox = new TextBox();
		validateQrCodeButton = new Button();
		((System.ComponentModel.ISupportInitialize)thumbsPicture).BeginInit();
		((System.ComponentModel.ISupportInitialize)qrCodePicture).BeginInit();
		SuspendLayout();
		// 
		// authGeneratedAuthCodeTextBox
		// 
		authGeneratedAuthCodeTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		authGeneratedAuthCodeTextBox.BackColor = SystemColors.Control;
		authGeneratedAuthCodeTextBox.BorderStyle = BorderStyle.None;
		authGeneratedAuthCodeTextBox.Font = new Font("OCR A Extended", 12F);
		authGeneratedAuthCodeTextBox.Location = new Point(3, 349);
		authGeneratedAuthCodeTextBox.MaxLength = 50;
		authGeneratedAuthCodeTextBox.Name = "authGeneratedAuthCodeTextBox";
		authGeneratedAuthCodeTextBox.ReadOnly = true;
		authGeneratedAuthCodeTextBox.Size = new Size(338, 17);
		authGeneratedAuthCodeTextBox.TabIndex = 0;
		authGeneratedAuthCodeTextBox.Text = "-SXQDVK4TW4TXK7KD4LU7U3KVQIG2BG5Z-";
		authGeneratedAuthCodeTextBox.TextAlign = HorizontalAlignment.Center;
		authGeneratedAuthCodeTextBox.WordWrap = false;
		// 
		// thumbsPicture
		// 
		thumbsPicture.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		thumbsPicture.BackColor = Color.Transparent;
		thumbsPicture.BackgroundImageLayout = ImageLayout.None;
		thumbsPicture.Location = new Point(58, 372);
		thumbsPicture.Name = "thumbsPicture";
		thumbsPicture.Size = new Size(78, 57);
		thumbsPicture.SizeMode = PictureBoxSizeMode.Zoom;
		thumbsPicture.TabIndex = 23;
		thumbsPicture.TabStop = false;
		// 
		// qrCodePicture
		// 
		qrCodePicture.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		qrCodePicture.BackgroundImageLayout = ImageLayout.None;
		qrCodePicture.BorderStyle = BorderStyle.FixedSingle;
		qrCodePicture.Location = new Point(3, 3);
		qrCodePicture.MinimumSize = new Size(338, 338);
		qrCodePicture.Name = "qrCodePicture";
		qrCodePicture.Size = new Size(338, 338);
		qrCodePicture.SizeMode = PictureBoxSizeMode.Zoom;
		qrCodePicture.TabIndex = 22;
		qrCodePicture.TabStop = false;
		// 
		// authCodeTextBox
		// 
		authCodeTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		authCodeTextBox.BorderStyle = BorderStyle.FixedSingle;
		authCodeTextBox.Font = new Font("OCR A Extended", 12F);
		authCodeTextBox.ForeColor = Color.Red;
		authCodeTextBox.Location = new Point(186, 405);
		authCodeTextBox.MaxLength = 6;
		authCodeTextBox.Name = "authCodeTextBox";
		authCodeTextBox.Size = new Size(70, 24);
		authCodeTextBox.TabIndex = 1;
		authCodeTextBox.WordWrap = false;
		authCodeTextBox.TextChanged += authCodeTextBox_TextChanged;
		// 
		// validateQrCodeButton
		// 
		validateQrCodeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		validateQrCodeButton.BackColor = SystemColors.Control;
		validateQrCodeButton.FlatStyle = FlatStyle.Flat;
		validateQrCodeButton.Location = new Point(262, 405);
		validateQrCodeButton.Name = "validateQrCodeButton";
		validateQrCodeButton.Size = new Size(70, 24);
		validateQrCodeButton.TabIndex = 2;
		validateQrCodeButton.Text = "&Validate";
		validateQrCodeButton.UseVisualStyleBackColor = false;
		validateQrCodeButton.Click += validateQrCodeButton_Click;
		// 
		// ZoomedQrForm
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(344, 441);
		Controls.Add(validateQrCodeButton);
		Controls.Add(authGeneratedAuthCodeTextBox);
		Controls.Add(thumbsPicture);
		Controls.Add(qrCodePicture);
		Controls.Add(authCodeTextBox);
		FormBorderStyle = FormBorderStyle.SizableToolWindow;
		MinimizeBox = false;
		MinimumSize = new Size(360, 480);
		Name = "ZoomedQrForm";
		ShowIcon = false;
		ShowInTaskbar = false;
		SizeGripStyle = SizeGripStyle.Show;
		StartPosition = FormStartPosition.CenterParent;
		Text = "Authentication Code";
		TopMost = true;
		Load += ZoomedQrForm_Load;
		((System.ComponentModel.ISupportInitialize)thumbsPicture).EndInit();
		((System.ComponentModel.ISupportInitialize)qrCodePicture).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private TextBox authGeneratedAuthCodeTextBox;
	private PictureBox thumbsPicture;
	private PictureBox qrCodePicture;
	private TextBox authCodeTextBox;
	private Button validateQrCodeButton;
}