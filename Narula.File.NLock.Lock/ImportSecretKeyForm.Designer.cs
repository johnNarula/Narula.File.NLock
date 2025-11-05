namespace Narula.File.NLock;

partial class ImportSecretKeyForm
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
		var resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportSecretKeyForm));
		okButton = new Button();
		cancelButton = new Button();
		validateQrCodeButton = new Button();
		authGeneratedAuthCodeTextBox = new TextBox();
		thumbsPicture = new PictureBox();
		authCodeTextBox = new TextBox();
		label1 = new Label();
		label6 = new Label();
		((System.ComponentModel.ISupportInitialize)thumbsPicture).BeginInit();
		SuspendLayout();
		// 
		// okButton
		// 
		okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		okButton.Enabled = false;
		okButton.Location = new Point(257, 178);
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
		cancelButton.Location = new Point(176, 178);
		cancelButton.Name = "cancelButton";
		cancelButton.Size = new Size(75, 23);
		cancelButton.TabIndex = 1;
		cancelButton.Text = "&Cancel";
		cancelButton.UseVisualStyleBackColor = true;
		cancelButton.Click += cancelButton_Click;
		// 
		// validateQrCodeButton
		// 
		validateQrCodeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		validateQrCodeButton.BackColor = SystemColors.Control;
		validateQrCodeButton.Enabled = false;
		validateQrCodeButton.FlatStyle = FlatStyle.Flat;
		validateQrCodeButton.Location = new Point(269, 97);
		validateQrCodeButton.Name = "validateQrCodeButton";
		validateQrCodeButton.Size = new Size(70, 24);
		validateQrCodeButton.TabIndex = 26;
		validateQrCodeButton.Text = "&Validate";
		validateQrCodeButton.UseVisualStyleBackColor = false;
		validateQrCodeButton.Click += validateQrCodeButton_Click;
		// 
		// authGeneratedAuthCodeTextBox
		// 
		authGeneratedAuthCodeTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		authGeneratedAuthCodeTextBox.BackColor = SystemColors.Window;
		authGeneratedAuthCodeTextBox.BorderStyle = BorderStyle.FixedSingle;
		authGeneratedAuthCodeTextBox.Font = new Font("OCR A Extended", 12F);
		authGeneratedAuthCodeTextBox.ForeColor = Color.Blue;
		authGeneratedAuthCodeTextBox.Location = new Point(5, 47);
		authGeneratedAuthCodeTextBox.MaxLength = 32;
		authGeneratedAuthCodeTextBox.Name = "authGeneratedAuthCodeTextBox";
		authGeneratedAuthCodeTextBox.Size = new Size(334, 24);
		authGeneratedAuthCodeTextBox.TabIndex = 24;
		authGeneratedAuthCodeTextBox.TextAlign = HorizontalAlignment.Center;
		authGeneratedAuthCodeTextBox.WordWrap = false;
		authGeneratedAuthCodeTextBox.TextChanged += authGeneratedAuthCodeTextBox_TextChanged;
		// 
		// thumbsPicture
		// 
		thumbsPicture.BackColor = Color.Transparent;
		thumbsPicture.BackgroundImageLayout = ImageLayout.None;
		thumbsPicture.Location = new Point(6, 84);
		thumbsPicture.Name = "thumbsPicture";
		thumbsPicture.Size = new Size(51, 37);
		thumbsPicture.SizeMode = PictureBoxSizeMode.Zoom;
		thumbsPicture.TabIndex = 27;
		thumbsPicture.TabStop = false;
		// 
		// authCodeTextBox
		// 
		authCodeTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		authCodeTextBox.BackColor = Color.FromArgb(255, 255, 192);
		authCodeTextBox.BorderStyle = BorderStyle.FixedSingle;
		authCodeTextBox.Enabled = false;
		authCodeTextBox.Font = new Font("OCR A Extended", 12F);
		authCodeTextBox.ForeColor = Color.Green;
		authCodeTextBox.Location = new Point(63, 97);
		authCodeTextBox.MaxLength = 6;
		authCodeTextBox.Name = "authCodeTextBox";
		authCodeTextBox.Size = new Size(201, 24);
		authCodeTextBox.TabIndex = 25;
		authCodeTextBox.TextAlign = HorizontalAlignment.Center;
		authCodeTextBox.WordWrap = false;
		authCodeTextBox.TextChanged += authCodeTextBox_TextChanged;
		// 
		// label1
		// 
		label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		label1.Font = new Font("OCR A Extended", 12F);
		label1.Location = new Point(5, 21);
		label1.Name = "label1";
		label1.Size = new Size(334, 23);
		label1.TabIndex = 28;
		label1.Text = "Secret Key";
		label1.TextAlign = ContentAlignment.MiddleCenter;
		// 
		// label6
		// 
		label6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		label6.Location = new Point(63, 76);
		label6.Name = "label6";
		label6.Size = new Size(201, 21);
		label6.TabIndex = 29;
		label6.Text = "Auth Code";
		label6.TextAlign = ContentAlignment.BottomCenter;
		// 
		// ImportSecretKeyForm
		// 
		AcceptButton = okButton;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		CancelButton = cancelButton;
		ClientSize = new Size(344, 213);
		Controls.Add(label1);
		Controls.Add(validateQrCodeButton);
		Controls.Add(authGeneratedAuthCodeTextBox);
		Controls.Add(thumbsPicture);
		Controls.Add(authCodeTextBox);
		Controls.Add(cancelButton);
		Controls.Add(okButton);
		Controls.Add(label6);
		FormBorderStyle = FormBorderStyle.FixedSingle;
		Icon = (Icon)resources.GetObject("$this.Icon");
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "ImportSecretKeyForm";
		ShowIcon = false;
		ShowInTaskbar = false;
		StartPosition = FormStartPosition.CenterParent;
		Text = "Import Private Secret Key";
		TopMost = true;
		((System.ComponentModel.ISupportInitialize)thumbsPicture).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private Button okButton;
	private Button cancelButton;
	private Button validateQrCodeButton;
	private TextBox authGeneratedAuthCodeTextBox;
	private PictureBox thumbsPicture;
	private TextBox authCodeTextBox;
	private Label label1;
	private Label label6;
}