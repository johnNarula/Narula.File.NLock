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
		components = new System.ComponentModel.Container();
		var resources = new System.ComponentModel.ComponentResourceManager(typeof(ZoomedQrForm));
		authGeneratedAuthCodeTextBox = new TextBox();
		thumbsPicture = new PictureBox();
		qrCodePicture = new PictureBox();
		authCodeTextBox = new TextBox();
		validateQrCodeButton = new Button();
		toolTip = new ToolTip(components);
		importantIcon = new PictureBox();
		label6 = new Label();
		((System.ComponentModel.ISupportInitialize)thumbsPicture).BeginInit();
		((System.ComponentModel.ISupportInitialize)qrCodePicture).BeginInit();
		((System.ComponentModel.ISupportInitialize)importantIcon).BeginInit();
		SuspendLayout();
		// 
		// authGeneratedAuthCodeTextBox
		// 
		authGeneratedAuthCodeTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		authGeneratedAuthCodeTextBox.BackColor = SystemColors.Window;
		authGeneratedAuthCodeTextBox.BorderStyle = BorderStyle.FixedSingle;
		authGeneratedAuthCodeTextBox.Font = new Font("OCR A Extended", 12F);
		authGeneratedAuthCodeTextBox.ForeColor = Color.Blue;
		authGeneratedAuthCodeTextBox.Location = new Point(5, 349);
		authGeneratedAuthCodeTextBox.MaxLength = 32;
		authGeneratedAuthCodeTextBox.Name = "authGeneratedAuthCodeTextBox";
		authGeneratedAuthCodeTextBox.ReadOnly = true;
		authGeneratedAuthCodeTextBox.Size = new Size(334, 24);
		authGeneratedAuthCodeTextBox.TabIndex = 0;
		authGeneratedAuthCodeTextBox.Text = "SECRET CODE";
		authGeneratedAuthCodeTextBox.TextAlign = HorizontalAlignment.Center;
		authGeneratedAuthCodeTextBox.WordWrap = false;
		authGeneratedAuthCodeTextBox.TextChanged += authGeneratedAuthCodeTextBox_TextChanged;
		// 
		// thumbsPicture
		// 
		thumbsPicture.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		thumbsPicture.BackColor = Color.Transparent;
		thumbsPicture.BackgroundImageLayout = ImageLayout.None;
		thumbsPicture.Location = new Point(5, 392);
		thumbsPicture.Name = "thumbsPicture";
		thumbsPicture.Size = new Size(51, 37);
		thumbsPicture.SizeMode = PictureBoxSizeMode.Zoom;
		thumbsPicture.TabIndex = 23;
		thumbsPicture.TabStop = false;
		// 
		// qrCodePicture
		// 
		qrCodePicture.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		qrCodePicture.BackgroundImageLayout = ImageLayout.None;
		qrCodePicture.BorderStyle = BorderStyle.FixedSingle;
		qrCodePicture.Location = new Point(5, 5);
		qrCodePicture.Name = "qrCodePicture";
		qrCodePicture.Size = new Size(334, 334);
		qrCodePicture.SizeMode = PictureBoxSizeMode.Zoom;
		qrCodePicture.TabIndex = 22;
		qrCodePicture.TabStop = false;
		// 
		// authCodeTextBox
		// 
		authCodeTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		authCodeTextBox.BackColor = Color.FromArgb(255, 255, 192);
		authCodeTextBox.BorderStyle = BorderStyle.FixedSingle;
		authCodeTextBox.Font = new Font("OCR A Extended", 12F);
		authCodeTextBox.ForeColor = Color.Green;
		authCodeTextBox.Location = new Point(62, 405);
		authCodeTextBox.MaxLength = 6;
		authCodeTextBox.Name = "authCodeTextBox";
		authCodeTextBox.Size = new Size(201, 24);
		authCodeTextBox.TabIndex = 1;
		authCodeTextBox.TextAlign = HorizontalAlignment.Center;
		authCodeTextBox.WordWrap = false;
		authCodeTextBox.TextChanged += authCodeTextBox_TextChanged;
		// 
		// validateQrCodeButton
		// 
		validateQrCodeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		validateQrCodeButton.BackColor = SystemColors.Control;
		validateQrCodeButton.FlatStyle = FlatStyle.Flat;
		validateQrCodeButton.Location = new Point(269, 405);
		validateQrCodeButton.Name = "validateQrCodeButton";
		validateQrCodeButton.Size = new Size(70, 24);
		validateQrCodeButton.TabIndex = 2;
		validateQrCodeButton.Text = "&Validate";
		validateQrCodeButton.UseVisualStyleBackColor = false;
		validateQrCodeButton.Click += validateQrCodeButton_Click;
		// 
		// toolTip
		// 
		toolTip.ToolTipIcon = ToolTipIcon.Warning;
		toolTip.ToolTipTitle = "Important";
		// 
		// importantIcon
		// 
		importantIcon.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		importantIcon.BackColor = Color.Transparent;
		importantIcon.BackgroundImageLayout = ImageLayout.None;
		importantIcon.Image = Narula.File.NLock.Lock.Properties.Resources.important32;
		importantIcon.Location = new Point(323, 345);
		importantIcon.Name = "importantIcon";
		importantIcon.Size = new Size(16, 16);
		importantIcon.SizeMode = PictureBoxSizeMode.Zoom;
		importantIcon.TabIndex = 24;
		importantIcon.TabStop = false;
		toolTip.SetToolTip(importantIcon, resources.GetString("importantIcon.ToolTip"));
		// 
		// label6
		// 
		label6.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		label6.Location = new Point(5, 381);
		label6.Name = "label6";
		label6.Size = new Size(334, 21);
		label6.TabIndex = 25;
		label6.Text = "Auth Code";
		label6.TextAlign = ContentAlignment.BottomCenter;
		// 
		// ZoomedQrForm
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(344, 441);
		Controls.Add(importantIcon);
		Controls.Add(validateQrCodeButton);
		Controls.Add(authGeneratedAuthCodeTextBox);
		Controls.Add(thumbsPicture);
		Controls.Add(qrCodePicture);
		Controls.Add(authCodeTextBox);
		Controls.Add(label6);
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
		((System.ComponentModel.ISupportInitialize)importantIcon).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private TextBox authGeneratedAuthCodeTextBox;
	private PictureBox thumbsPicture;
	private PictureBox qrCodePicture;
	private TextBox authCodeTextBox;
	private Button validateQrCodeButton;
	private ToolTip toolTip;
	private PictureBox importantIcon;
	private Label label6;
}