namespace Narula.File.NLock.Lib.UI;

partial class AboutBox
{
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
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
		var resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
		logoPictureBox = new PictureBox();
		labelProductName = new Label();
		labelVersion = new Label();
		labelCopyright = new Label();
		labelCompanyName = new Label();
		textBoxDescription = new TextBox();
		okButton = new Button();
		lblContact = new Label();
		tipQr = new PictureBox();
		tipJar = new PictureBox();
		label1 = new Label();
		((System.ComponentModel.ISupportInitialize)logoPictureBox).BeginInit();
		((System.ComponentModel.ISupportInitialize)tipQr).BeginInit();
		((System.ComponentModel.ISupportInitialize)tipJar).BeginInit();
		SuspendLayout();
		// 
		// logoPictureBox
		// 
		logoPictureBox.BackColor = Color.Transparent;
		logoPictureBox.Location = new Point(14, 13);
		logoPictureBox.Margin = new Padding(4, 3, 4, 3);
		logoPictureBox.Name = "logoPictureBox";
		logoPictureBox.Size = new Size(64, 64);
		logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
		logoPictureBox.TabIndex = 26;
		logoPictureBox.TabStop = false;
		// 
		// labelProductName
		// 
		labelProductName.Location = new Point(89, 13);
		labelProductName.Margin = new Padding(7, 0, 4, 0);
		labelProductName.MaximumSize = new Size(0, 20);
		labelProductName.Name = "labelProductName";
		labelProductName.Size = new Size(402, 20);
		labelProductName.TabIndex = 27;
		labelProductName.Text = "Product Name";
		labelProductName.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// labelVersion
		// 
		labelVersion.Location = new Point(89, 34);
		labelVersion.Margin = new Padding(7, 0, 4, 0);
		labelVersion.MaximumSize = new Size(0, 20);
		labelVersion.Name = "labelVersion";
		labelVersion.Size = new Size(402, 20);
		labelVersion.TabIndex = 25;
		labelVersion.Text = "Version";
		labelVersion.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// labelCopyright
		// 
		labelCopyright.Location = new Point(89, 76);
		labelCopyright.Margin = new Padding(7, 0, 4, 0);
		labelCopyright.MaximumSize = new Size(0, 20);
		labelCopyright.Name = "labelCopyright";
		labelCopyright.Size = new Size(402, 20);
		labelCopyright.TabIndex = 28;
		labelCopyright.Text = "Copyright";
		labelCopyright.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// labelCompanyName
		// 
		labelCompanyName.Location = new Point(89, 55);
		labelCompanyName.Margin = new Padding(7, 0, 4, 0);
		labelCompanyName.MaximumSize = new Size(0, 20);
		labelCompanyName.Name = "labelCompanyName";
		labelCompanyName.Size = new Size(402, 20);
		labelCompanyName.TabIndex = 29;
		labelCompanyName.Text = "Company Name";
		labelCompanyName.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// textBoxDescription
		// 
		textBoxDescription.Location = new Point(14, 120);
		textBoxDescription.Margin = new Padding(7, 3, 4, 3);
		textBoxDescription.Multiline = true;
		textBoxDescription.Name = "textBoxDescription";
		textBoxDescription.ReadOnly = true;
		textBoxDescription.Size = new Size(477, 96);
		textBoxDescription.TabIndex = 30;
		textBoxDescription.TabStop = false;
		textBoxDescription.Text = resources.GetString("textBoxDescription.Text");
		// 
		// okButton
		// 
		okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		okButton.DialogResult = DialogResult.Cancel;
		okButton.Location = new Point(390, 372);
		okButton.Margin = new Padding(4, 3, 4, 3);
		okButton.Name = "okButton";
		okButton.Size = new Size(108, 29);
		okButton.TabIndex = 31;
		okButton.Text = "&OK";
		// 
		// lblContact
		// 
		lblContact.Location = new Point(89, 97);
		lblContact.Margin = new Padding(7, 0, 4, 0);
		lblContact.MaximumSize = new Size(0, 20);
		lblContact.Name = "lblContact";
		lblContact.Size = new Size(402, 20);
		lblContact.TabIndex = 32;
		lblContact.Text = "Contact";
		lblContact.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// tipQr
		// 
		tipQr.BackColor = Color.Transparent;
		tipQr.Location = new Point(14, 222);
		tipQr.Margin = new Padding(4, 3, 4, 3);
		tipQr.Name = "tipQr";
		tipQr.Size = new Size(180, 180);
		tipQr.SizeMode = PictureBoxSizeMode.StretchImage;
		tipQr.TabIndex = 33;
		tipQr.TabStop = false;
		// 
		// tipJar
		// 
		tipJar.BackColor = Color.Transparent;
		tipJar.Location = new Point(202, 222);
		tipJar.Margin = new Padding(4, 3, 4, 3);
		tipJar.Name = "tipJar";
		tipJar.Size = new Size(180, 180);
		tipJar.SizeMode = PictureBoxSizeMode.StretchImage;
		tipJar.TabIndex = 34;
		tipJar.TabStop = false;
		// 
		// label1
		// 
		label1.AutoSize = true;
		label1.Font = new Font("Viner Hand ITC", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
		label1.Location = new Point(390, 222);
		label1.Name = "label1";
		label1.Size = new Size(108, 39);
		label1.TabIndex = 35;
		label1.Text = "Tip Me!";
		label1.TextAlign = ContentAlignment.MiddleCenter;
		// 
		// AboutBox
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(507, 414);
		Controls.Add(label1);
		Controls.Add(tipJar);
		Controls.Add(tipQr);
		Controls.Add(lblContact);
		Controls.Add(logoPictureBox);
		Controls.Add(labelProductName);
		Controls.Add(labelVersion);
		Controls.Add(labelCopyright);
		Controls.Add(labelCompanyName);
		Controls.Add(textBoxDescription);
		Controls.Add(okButton);
		FormBorderStyle = FormBorderStyle.FixedDialog;
		Margin = new Padding(4, 3, 4, 3);
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "AboutBox";
		Padding = new Padding(10);
		ShowIcon = false;
		ShowInTaskbar = false;
		StartPosition = FormStartPosition.CenterParent;
		((System.ComponentModel.ISupportInitialize)logoPictureBox).EndInit();
		((System.ComponentModel.ISupportInitialize)tipQr).EndInit();
		((System.ComponentModel.ISupportInitialize)tipJar).EndInit();
		ResumeLayout(false);
		PerformLayout();

	}

	#endregion

	private PictureBox logoPictureBox;
	private Label labelProductName;
	private Label labelVersion;
	private Label labelCopyright;
	private Label labelCompanyName;
	private TextBox textBoxDescription;
	private Button okButton;
	private Label lblContact;
	private PictureBox tipQr;
	private PictureBox tipJar;
	private Label label1;
}
