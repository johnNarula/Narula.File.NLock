namespace Narula.File.NLock;

partial class LockForm
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
		var resources = new System.ComponentModel.ComponentResourceManager(typeof(LockForm));
		cancelButton = new Button();
		lockFileButton = new Button();
		progressBar = new ProgressBar();
		browseSourceButton = new Button();
		browseOutputFolderButton = new Button();
		outputFolderTextBox = new TextBox();
		label2 = new Label();
		passwordTextBox = new TextBox();
		label4 = new Label();
		label5 = new Label();
		outputFolderDialog = new FolderBrowserDialog();
		sourceFilesDialog = new OpenFileDialog();
		sourceFilesGroup = new GroupBox();
		sourceFilesGrid = new DataGridView();
		progressFilenameLabel = new Label();
		authCodeTextBox = new TextBox();
		qrCodePicture = new PictureBox();
		validateQrCodeButton = new Button();
		thumbsPicture = new PictureBox();
		reloadAuthCodeButton = new PictureBox();
		authGeneratedAuthCodeTextBox = new TextBox();
		messageLabel = new Label();
		sourcefileCol = new DataGridViewTextBoxColumn();
		sourcefilePathCol = new DataGridViewTextBoxColumn();
		lockFilenameCol = new DataGridViewTextBoxColumn();
		selectedFilesCol = new DataGridViewCheckBoxColumn();
		messageCol = new DataGridViewTextBoxColumn();
		sourceFilesGroup.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)sourceFilesGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)qrCodePicture).BeginInit();
		((System.ComponentModel.ISupportInitialize)thumbsPicture).BeginInit();
		((System.ComponentModel.ISupportInitialize)reloadAuthCodeButton).BeginInit();
		SuspendLayout();
		// 
		// cancelButton
		// 
		cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		cancelButton.Location = new Point(520, 388);
		cancelButton.Name = "cancelButton";
		cancelButton.Size = new Size(120, 23);
		cancelButton.TabIndex = 10;
		cancelButton.Text = "&Cancel";
		cancelButton.UseVisualStyleBackColor = true;
		cancelButton.Click += cancelButton_Click;
		// 
		// lockFileButton
		// 
		lockFileButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		lockFileButton.Enabled = false;
		lockFileButton.Location = new Point(646, 388);
		lockFileButton.Name = "lockFileButton";
		lockFileButton.Size = new Size(120, 23);
		lockFileButton.TabIndex = 11;
		lockFileButton.Text = "&Lock File(s)";
		lockFileButton.UseVisualStyleBackColor = true;
		lockFileButton.Click += lockFileButton_Click;
		// 
		// progressBar
		// 
		progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		progressBar.Location = new Point(12, 342);
		progressBar.Name = "progressBar";
		progressBar.Size = new Size(760, 23);
		progressBar.Style = ProgressBarStyle.Continuous;
		progressBar.TabIndex = 9;
		// 
		// browseSourceButton
		// 
		browseSourceButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		browseSourceButton.Location = new Point(632, 115);
		browseSourceButton.Name = "browseSourceButton";
		browseSourceButton.Size = new Size(120, 23);
		browseSourceButton.TabIndex = 1;
		browseSourceButton.Text = "&Browse Files ...";
		browseSourceButton.UseVisualStyleBackColor = true;
		browseSourceButton.Click += browseSourceButton_Click;
		// 
		// browseOutputFolderButton
		// 
		browseOutputFolderButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		browseOutputFolderButton.Location = new Point(646, 164);
		browseOutputFolderButton.Name = "browseOutputFolderButton";
		browseOutputFolderButton.Size = new Size(120, 23);
		browseOutputFolderButton.TabIndex = 3;
		browseOutputFolderButton.Text = "&Output Folder...";
		browseOutputFolderButton.UseVisualStyleBackColor = true;
		browseOutputFolderButton.Click += browseOutputFolderButton_Click;
		// 
		// outputFolderTextBox
		// 
		outputFolderTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		outputFolderTextBox.Location = new Point(147, 164);
		outputFolderTextBox.Name = "outputFolderTextBox";
		outputFolderTextBox.Size = new Size(493, 23);
		outputFolderTextBox.TabIndex = 2;
		outputFolderTextBox.WordWrap = false;
		outputFolderTextBox.TextChanged += ValidateReadinessEvent;
		// 
		// label2
		// 
		label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		label2.AutoSize = true;
		label2.Location = new Point(20, 167);
		label2.Name = "label2";
		label2.Size = new Size(81, 15);
		label2.TabIndex = 1;
		label2.Text = "Output Folder";
		label2.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// passwordTextBox
		// 
		passwordTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		passwordTextBox.BackColor = Color.FromArgb(255, 192, 192);
		passwordTextBox.Location = new Point(147, 193);
		passwordTextBox.MaxLength = 50;
		passwordTextBox.Name = "passwordTextBox";
		passwordTextBox.Size = new Size(493, 23);
		passwordTextBox.TabIndex = 4;
		passwordTextBox.UseSystemPasswordChar = true;
		passwordTextBox.WordWrap = false;
		passwordTextBox.TextChanged += ValidateReadinessEvent;
		passwordTextBox.Leave += ValidateReadinessEvent;
		// 
		// label4
		// 
		label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		label4.AutoSize = true;
		label4.Location = new Point(20, 196);
		label4.Name = "label4";
		label4.Size = new Size(57, 15);
		label4.TabIndex = 6;
		label4.Text = "Password";
		label4.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// label5
		// 
		label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		label5.AutoSize = true;
		label5.Location = new Point(20, 222);
		label5.Name = "label5";
		label5.Size = new Size(64, 15);
		label5.TabIndex = 5;
		label5.Text = "Auth Code";
		label5.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// sourceFilesDialog
		// 
		sourceFilesDialog.Filter = "All Files (*.*)|*.*";
		sourceFilesDialog.Multiselect = true;
		sourceFilesDialog.OkRequiresInteraction = true;
		// 
		// sourceFilesGroup
		// 
		sourceFilesGroup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		sourceFilesGroup.Controls.Add(sourceFilesGrid);
		sourceFilesGroup.Controls.Add(browseSourceButton);
		sourceFilesGroup.Location = new Point(14, 14);
		sourceFilesGroup.Name = "sourceFilesGroup";
		sourceFilesGroup.Size = new Size(758, 144);
		sourceFilesGroup.TabIndex = 0;
		sourceFilesGroup.TabStop = false;
		sourceFilesGroup.Text = "Files to Lock";
		// 
		// sourceFilesGrid
		// 
		sourceFilesGrid.AllowUserToAddRows = false;
		sourceFilesGrid.AllowUserToDeleteRows = false;
		sourceFilesGrid.AllowUserToOrderColumns = true;
		sourceFilesGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		sourceFilesGrid.BackgroundColor = SystemColors.Control;
		sourceFilesGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
		sourceFilesGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		sourceFilesGrid.Columns.AddRange(new DataGridViewColumn[] { sourcefileCol, sourcefilePathCol, lockFilenameCol, selectedFilesCol, messageCol });
		sourceFilesGrid.Location = new Point(6, 22);
		sourceFilesGrid.Name = "sourceFilesGrid";
		sourceFilesGrid.Size = new Size(746, 87);
		sourceFilesGrid.TabIndex = 0;
		sourceFilesGrid.CellEndEdit += sourceFilesGrid_CellEndEdit;
		sourceFilesGrid.CellValueChanged += sourceFilesGrid_CellValueChanged;
		// 
		// progressFilenameLabel
		// 
		progressFilenameLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		progressFilenameLabel.BackColor = Color.Transparent;
		progressFilenameLabel.Location = new Point(12, 368);
		progressFilenameLabel.Name = "progressFilenameLabel";
		progressFilenameLabel.Size = new Size(760, 17);
		progressFilenameLabel.TabIndex = 13;
		progressFilenameLabel.TextAlign = ContentAlignment.MiddleCenter;
		// 
		// authCodeTextBox
		// 
		authCodeTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		authCodeTextBox.BorderStyle = BorderStyle.FixedSingle;
		authCodeTextBox.Font = new Font("OCR A Extended", 12F);
		authCodeTextBox.ForeColor = Color.Red;
		authCodeTextBox.Location = new Point(267, 313);
		authCodeTextBox.MaxLength = 6;
		authCodeTextBox.Name = "authCodeTextBox";
		authCodeTextBox.Size = new Size(70, 24);
		authCodeTextBox.TabIndex = 7;
		authCodeTextBox.WordWrap = false;
		authCodeTextBox.TextChanged += ValidateReadinessEvent;
		authCodeTextBox.KeyDown += authCodeTextBox_KeyDown;
		// 
		// qrCodePicture
		// 
		qrCodePicture.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		qrCodePicture.BackgroundImageLayout = ImageLayout.None;
		qrCodePicture.BorderStyle = BorderStyle.FixedSingle;
		qrCodePicture.Location = new Point(147, 222);
		qrCodePicture.Name = "qrCodePicture";
		qrCodePicture.Size = new Size(114, 114);
		qrCodePicture.SizeMode = PictureBoxSizeMode.Zoom;
		qrCodePicture.TabIndex = 14;
		qrCodePicture.TabStop = false;
		qrCodePicture.DoubleClick += qrCodePicture_DoubleClick;
		// 
		// validateQrCodeButton
		// 
		validateQrCodeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		validateQrCodeButton.BackColor = SystemColors.Control;
		validateQrCodeButton.FlatStyle = FlatStyle.Flat;
		validateQrCodeButton.Location = new Point(343, 313);
		validateQrCodeButton.Name = "validateQrCodeButton";
		validateQrCodeButton.Size = new Size(70, 24);
		validateQrCodeButton.TabIndex = 8;
		validateQrCodeButton.Text = "&Validate";
		validateQrCodeButton.UseVisualStyleBackColor = false;
		validateQrCodeButton.Click += validateQrCodeButton_Click;
		// 
		// thumbsPicture
		// 
		thumbsPicture.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		thumbsPicture.BackColor = Color.Transparent;
		thumbsPicture.BackgroundImageLayout = ImageLayout.None;
		thumbsPicture.Location = new Point(267, 250);
		thumbsPicture.Name = "thumbsPicture";
		thumbsPicture.Size = new Size(70, 57);
		thumbsPicture.SizeMode = PictureBoxSizeMode.Zoom;
		thumbsPicture.TabIndex = 17;
		thumbsPicture.TabStop = false;
		// 
		// reloadAuthCodeButton
		// 
		reloadAuthCodeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		reloadAuthCodeButton.BackColor = Color.Transparent;
		reloadAuthCodeButton.BackgroundImageLayout = ImageLayout.None;
		reloadAuthCodeButton.Image = (Image)resources.GetObject("reloadAuthCodeButton.Image");
		reloadAuthCodeButton.Location = new Point(116, 306);
		reloadAuthCodeButton.Name = "reloadAuthCodeButton";
		reloadAuthCodeButton.Size = new Size(25, 30);
		reloadAuthCodeButton.SizeMode = PictureBoxSizeMode.Zoom;
		reloadAuthCodeButton.TabIndex = 18;
		reloadAuthCodeButton.TabStop = false;
		reloadAuthCodeButton.Click += reloadAuthCodeButton_Click;
		// 
		// authGeneratedAuthCodeTextBox
		// 
		authGeneratedAuthCodeTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		authGeneratedAuthCodeTextBox.BackColor = SystemColors.Control;
		authGeneratedAuthCodeTextBox.BorderStyle = BorderStyle.None;
		authGeneratedAuthCodeTextBox.Font = new Font("OCR A Extended", 12F);
		authGeneratedAuthCodeTextBox.Location = new Point(267, 222);
		authGeneratedAuthCodeTextBox.Name = "authGeneratedAuthCodeTextBox";
		authGeneratedAuthCodeTextBox.ReadOnly = true;
		authGeneratedAuthCodeTextBox.Size = new Size(493, 17);
		authGeneratedAuthCodeTextBox.TabIndex = 6;
		authGeneratedAuthCodeTextBox.Text = "Auth Code";
		authGeneratedAuthCodeTextBox.WordWrap = false;
		// 
		// messageLabel
		// 
		messageLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		messageLabel.BackColor = Color.Transparent;
		messageLabel.ForeColor = Color.Red;
		messageLabel.Location = new Point(12, 389);
		messageLabel.Name = "messageLabel";
		messageLabel.Size = new Size(502, 20);
		messageLabel.TabIndex = 19;
		messageLabel.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// sourcefileCol
		// 
		sourcefileCol.HeaderText = "File Name";
		sourcefileCol.MaxInputLength = 255;
		sourcefileCol.MinimumWidth = 180;
		sourcefileCol.Name = "sourcefileCol";
		sourcefileCol.ReadOnly = true;
		sourcefileCol.Width = 180;
		// 
		// sourcefilePathCol
		// 
		sourcefilePathCol.HeaderText = "Path";
		sourcefilePathCol.MinimumWidth = 50;
		sourcefilePathCol.Name = "sourcefilePathCol";
		sourcefilePathCol.ReadOnly = true;
		sourcefilePathCol.Width = 150;
		// 
		// lockFilenameCol
		// 
		lockFilenameCol.HeaderText = "Lock File Name";
		lockFilenameCol.MinimumWidth = 200;
		lockFilenameCol.Name = "lockFilenameCol";
		lockFilenameCol.Width = 200;
		// 
		// selectedFilesCol
		// 
		selectedFilesCol.FalseValue = "false";
		selectedFilesCol.HeaderText = "✅";
		selectedFilesCol.MinimumWidth = 30;
		selectedFilesCol.Name = "selectedFilesCol";
		selectedFilesCol.Resizable = DataGridViewTriState.False;
		selectedFilesCol.SortMode = DataGridViewColumnSortMode.Automatic;
		selectedFilesCol.TrueValue = "true";
		selectedFilesCol.Width = 30;
		// 
		// messageCol
		// 
		messageCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
		messageCol.HeaderText = "Message";
		messageCol.Name = "messageCol";
		messageCol.ReadOnly = true;
		// 
		// LockForm
		// 
		AcceptButton = lockFileButton;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		CancelButton = cancelButton;
		ClientSize = new Size(784, 421);
		Controls.Add(messageLabel);
		Controls.Add(authGeneratedAuthCodeTextBox);
		Controls.Add(reloadAuthCodeButton);
		Controls.Add(thumbsPicture);
		Controls.Add(validateQrCodeButton);
		Controls.Add(qrCodePicture);
		Controls.Add(authCodeTextBox);
		Controls.Add(progressFilenameLabel);
		Controls.Add(sourceFilesGroup);
		Controls.Add(label5);
		Controls.Add(passwordTextBox);
		Controls.Add(label4);
		Controls.Add(browseOutputFolderButton);
		Controls.Add(outputFolderTextBox);
		Controls.Add(label2);
		Controls.Add(progressBar);
		Controls.Add(lockFileButton);
		Controls.Add(cancelButton);
		Icon = (Icon)resources.GetObject("$this.Icon");
		MinimizeBox = false;
		Name = "LockForm";
		SizeGripStyle = SizeGripStyle.Show;
		StartPosition = FormStartPosition.CenterScreen;
		Text = "Lock File";
		Load += LockForm_Load;
		sourceFilesGroup.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)sourceFilesGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)qrCodePicture).EndInit();
		((System.ComponentModel.ISupportInitialize)thumbsPicture).EndInit();
		((System.ComponentModel.ISupportInitialize)reloadAuthCodeButton).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private Button cancelButton;
	private Button lockFileButton;
	private ProgressBar progressBar;
	private Button browseSourceButton;
	private Button browseOutputFolderButton;
	private TextBox outputFolderTextBox;
	private Label label2;
	private TextBox passwordTextBox;
	private Label label4;
	private Label label5;
	private FolderBrowserDialog outputFolderDialog;
	private OpenFileDialog sourceFilesDialog;
	private GroupBox sourceFilesGroup;
	private DataGridView sourceFilesGrid;
	private Label progressFilenameLabel;
	private TextBox authCodeTextBox;
	private PictureBox qrCodePicture;
	private Button validateQrCodeButton;
	private Label authGeneratedAuthCodeLabel;
	private PictureBox thumbsPicture;
	private PictureBox reloadAuthCodeButton;
	private TextBox authGeneratedAuthCodeTextBox;
	private Label messageLabel;
	private DataGridViewTextBoxColumn sourcefileCol;
	private DataGridViewTextBoxColumn sourcefilePathCol;
	private DataGridViewTextBoxColumn lockFilenameCol;
	private DataGridViewCheckBoxColumn selectedFilesCol;
	private DataGridViewTextBoxColumn messageCol;
}