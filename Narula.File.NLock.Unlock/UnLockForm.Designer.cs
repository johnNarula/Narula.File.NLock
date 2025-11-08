namespace Narula.File.NLock;

partial class UnLockForm
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
		var resources = new System.ComponentModel.ComponentResourceManager(typeof(UnLockForm));
		cancelButton = new Button();
		unlockFileButton = new Button();
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
		sourcefileCol = new DataGridViewTextBoxColumn();
		sourcefilePathCol = new DataGridViewTextBoxColumn();
		unlockFilenameCol = new DataGridViewTextBoxColumn();
		selectedFilesCol = new DataGridViewCheckBoxColumn();
		messageCol = new DataGridViewTextBoxColumn();
		progressFilenameLabel = new Label();
		authCodeTextBox = new TextBox();
		messageLabel = new Label();
		logo = new PictureBox();
		sourceFilesGroup.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)sourceFilesGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)logo).BeginInit();
		SuspendLayout();
		// 
		// cancelButton
		// 
		cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		cancelButton.Location = new Point(520, 347);
		cancelButton.Name = "cancelButton";
		cancelButton.Size = new Size(120, 23);
		cancelButton.TabIndex = 9;
		cancelButton.Text = "&Cancel";
		cancelButton.UseVisualStyleBackColor = true;
		cancelButton.Click += cancelButton_Click;
		// 
		// unlockFileButton
		// 
		unlockFileButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		unlockFileButton.Enabled = false;
		unlockFileButton.Location = new Point(646, 347);
		unlockFileButton.Name = "unlockFileButton";
		unlockFileButton.Size = new Size(120, 23);
		unlockFileButton.TabIndex = 10;
		unlockFileButton.Text = "&Unlock File(s)";
		unlockFileButton.UseVisualStyleBackColor = true;
		unlockFileButton.Click += unlockFileButton_ClickAsync;
		// 
		// progressBar
		// 
		progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		progressBar.Location = new Point(14, 301);
		progressBar.Name = "progressBar";
		progressBar.Size = new Size(758, 23);
		progressBar.Style = ProgressBarStyle.Continuous;
		progressBar.TabIndex = 8;
		// 
		// browseSourceButton
		// 
		browseSourceButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		browseSourceButton.Location = new Point(626, 164);
		browseSourceButton.Name = "browseSourceButton";
		browseSourceButton.Size = new Size(120, 23);
		browseSourceButton.TabIndex = 1;
		browseSourceButton.Text = "&Browse NLock Files...";
		browseSourceButton.UseVisualStyleBackColor = true;
		browseSourceButton.Click += browseSourceButton_Click;
		// 
		// browseOutputFolderButton
		// 
		browseOutputFolderButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		browseOutputFolderButton.Location = new Point(640, 213);
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
		outputFolderTextBox.BorderStyle = BorderStyle.FixedSingle;
		outputFolderTextBox.Location = new Point(140, 213);
		outputFolderTextBox.Name = "outputFolderTextBox";
		outputFolderTextBox.Size = new Size(493, 23);
		outputFolderTextBox.TabIndex = 2;
		outputFolderTextBox.WordWrap = false;
		// 
		// label2
		// 
		label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		label2.AutoSize = true;
		label2.Location = new Point(13, 216);
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
		passwordTextBox.BorderStyle = BorderStyle.FixedSingle;
		passwordTextBox.Location = new Point(140, 242);
		passwordTextBox.MaxLength = 50;
		passwordTextBox.Name = "passwordTextBox";
		passwordTextBox.Size = new Size(493, 23);
		passwordTextBox.TabIndex = 5;
		passwordTextBox.UseSystemPasswordChar = true;
		passwordTextBox.WordWrap = false;
		passwordTextBox.Leave += ValidateReadinessEvent;
		// 
		// label4
		// 
		label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		label4.AutoSize = true;
		label4.Location = new Point(13, 245);
		label4.Name = "label4";
		label4.Size = new Size(57, 15);
		label4.TabIndex = 4;
		label4.Text = "Password";
		label4.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// label5
		// 
		label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		label5.AutoSize = true;
		label5.Location = new Point(13, 274);
		label5.Name = "label5";
		label5.Size = new Size(64, 15);
		label5.TabIndex = 6;
		label5.Text = "Auth Code";
		label5.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// sourceFilesDialog
		// 
		sourceFilesDialog.DefaultExt = "NLock";
		sourceFilesDialog.Filter = "NLock Files (*.Nlock)|*.NLock";
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
		sourceFilesGroup.Size = new Size(758, 193);
		sourceFilesGroup.TabIndex = 0;
		sourceFilesGroup.TabStop = false;
		sourceFilesGroup.Text = "NLock Files";
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
		sourceFilesGrid.Columns.AddRange(new DataGridViewColumn[] { sourcefileCol, sourcefilePathCol, unlockFilenameCol, selectedFilesCol, messageCol });
		sourceFilesGrid.Location = new Point(6, 22);
		sourceFilesGrid.Name = "sourceFilesGrid";
		sourceFilesGrid.Size = new Size(746, 136);
		sourceFilesGrid.TabIndex = 0;
		sourceFilesGrid.CellEndEdit += sourceFilesGrid_CellEndEdit;
		sourceFilesGrid.CellValueChanged += sourceFilesGrid_CellValueChanged;
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
		// unlockFilenameCol
		// 
		unlockFilenameCol.HeaderText = "Unlock File Name";
		unlockFilenameCol.MinimumWidth = 200;
		unlockFilenameCol.Name = "unlockFilenameCol";
		unlockFilenameCol.Width = 200;
		// 
		// selectedFilesCol
		// 
		selectedFilesCol.HeaderText = "✅";
		selectedFilesCol.MinimumWidth = 30;
		selectedFilesCol.Name = "selectedFilesCol";
		selectedFilesCol.Resizable = DataGridViewTriState.False;
		selectedFilesCol.SortMode = DataGridViewColumnSortMode.Automatic;
		selectedFilesCol.Width = 30;
		// 
		// messageCol
		// 
		messageCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
		messageCol.HeaderText = "Message";
		messageCol.Name = "messageCol";
		messageCol.ReadOnly = true;
		// 
		// progressFilenameLabel
		// 
		progressFilenameLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		progressFilenameLabel.BackColor = Color.Transparent;
		progressFilenameLabel.Location = new Point(14, 327);
		progressFilenameLabel.Name = "progressFilenameLabel";
		progressFilenameLabel.Size = new Size(758, 17);
		progressFilenameLabel.TabIndex = 9;
		progressFilenameLabel.TextAlign = ContentAlignment.MiddleCenter;
		// 
		// authCodeTextBox
		// 
		authCodeTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		authCodeTextBox.BorderStyle = BorderStyle.FixedSingle;
		authCodeTextBox.Font = new Font("OCR A Extended", 12F);
		authCodeTextBox.ForeColor = Color.Red;
		authCodeTextBox.Location = new Point(140, 271);
		authCodeTextBox.MaxLength = 6;
		authCodeTextBox.Name = "authCodeTextBox";
		authCodeTextBox.Size = new Size(70, 24);
		authCodeTextBox.TabIndex = 7;
		authCodeTextBox.WordWrap = false;
		authCodeTextBox.TextChanged += ValidateReadinessEvent;
		authCodeTextBox.KeyDown += authCodeTextBox_KeyDown;
		// 
		// messageLabel
		// 
		messageLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		messageLabel.BackColor = Color.Transparent;
		messageLabel.ForeColor = Color.Red;
		messageLabel.Location = new Point(51, 348);
		messageLabel.Name = "messageLabel";
		messageLabel.Size = new Size(450, 20);
		messageLabel.TabIndex = 17;
		messageLabel.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// logo
		// 
		logo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		logo.BackColor = Color.Transparent;
		logo.Location = new Point(13, 338);
		logo.Name = "logo";
		logo.Size = new Size(32, 32);
		logo.SizeMode = PictureBoxSizeMode.StretchImage;
		logo.TabIndex = 18;
		logo.TabStop = false;
		logo.Click += logo_Click;
		// 
		// UnLockForm
		// 
		AcceptButton = unlockFileButton;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		CancelButton = cancelButton;
		ClientSize = new Size(784, 380);
		Controls.Add(logo);
		Controls.Add(messageLabel);
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
		Controls.Add(unlockFileButton);
		Controls.Add(cancelButton);
		Icon = (Icon)resources.GetObject("$this.Icon");
		Name = "UnLockForm";
		StartPosition = FormStartPosition.CenterScreen;
		Text = "Unlock NLock File";
		Load += UnlockForm_Load;
		sourceFilesGroup.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)sourceFilesGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)logo).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private Button cancelButton;
	private Button unlockFileButton;
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
	private DataGridViewTextBoxColumn sourcefileCol;
	private DataGridViewTextBoxColumn sourcefilePathCol;
	private DataGridViewTextBoxColumn unlockFilenameCol;
	private DataGridViewCheckBoxColumn selectedFilesCol;
	private DataGridViewTextBoxColumn messageCol;
	private Label messageLabel;
	private PictureBox logo;
}