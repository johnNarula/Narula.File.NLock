using System.Collections.Generic;
using Narula.File.NLock.Models;

namespace Narula.File.NLock;
public partial class LockForm : Form
{
	public LockForm()
	{
		InitializeComponent();
	}
	public LockForm(params string[] sourceFiles) : this()
	{
		this.SourceFiles = sourceFiles;
	}
	public LockForm(LockGuiLaunchRequest? launchRequest) : this()
	{
		_launchRequest = launchRequest;
		var files = new List<string>();

		if (launchRequest != null)
		{
			if (launchRequest.Files != null) files.AddRange(launchRequest.Files);
			if (launchRequest.Directories != null)
			{
				foreach (var dir in launchRequest.Directories)
				{
					if (Directory.Exists(dir))
					{
						files.AddRange(Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly));
					}
				}
			}
			SourceFiles = files.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
			if (!string.IsNullOrWhiteSpace(launchRequest.OutputFolder))
			{
				try { outputFolderTextBox.Text = launchRequest.OutputFolder; } catch { }
			}
		}
	}

	private AuthCodeInfo _authCodeInfo = new AuthCodeInfo();
	[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
	public string[] SourceFiles { get; set; } = Array.Empty<string>();
	LockGuiLaunchRequest? _launchRequest;
	private const byte MAX_FAIL_ATTEMPTS = 5;
	private byte _failedAttempts = 0;
	private void LoadSourceFilesGrid(bool clearRows = true)
	{
		if (clearRows)
		{
			sourceFilesGrid.Rows.Clear();
		}

		foreach (var file in SourceFiles)
		{
			var fi = new FileInfo(file);
			var isNLockFile = fi.Extension.Equals(AppConstants.Extension, StringComparison.OrdinalIgnoreCase);
			var lockFilename = GetTargetFileName(fi);

			int rowIndex = sourceFilesGrid.Rows.Add();
			sourceFilesGrid.Rows[rowIndex].Cells["sourcefileCol"].Value = fi.Name;
			sourceFilesGrid.Rows[rowIndex].Cells["sourcefilePathCol"].Value = fi.DirectoryName;
			sourceFilesGrid.Rows[rowIndex].Cells["messageCol"].Value = string.Empty;
			sourceFilesGrid.Rows[rowIndex].Cells["lockFilenameCol"].Value = lockFilename;
			sourceFilesGrid.Rows[rowIndex].Cells["selectedFilesCol"].Value = !isNLockFile;
		}

		UpdateOutputFolder();
	}
	private string GetTargetFileName(FileInfo fi)
	{
		return FileUtility.GetUniqueTargetFilenameInDirectory(fi.DirectoryName, fi.Name + AppConstants.Extension);
	}
	private void GenerateAuthQrCode()
	{
		ClearAuthQrCode();
		var seclectedFileCount = GetNumberOfSelectedFiles();
		if (SourceFiles.Length == 0 || seclectedFileCount == 0)
		{
			messageLabel.Text = "Please select at least 1 file to generate TOTP Code.";
			return;
		}

		var destinationDir = outputFolderTextBox.Text.Trim();
		if (destinationDir.Length == 0 || !Directory.Exists(destinationDir))
		{
			messageLabel.Text = "Please select valid output directory to generate TOTP Code.";
			return;
		}

		messageLabel.Text = string.Empty;

		EnsureQrTitleStartsWithNLock();
		string issuer = qrTitleTextbox.Text; //do not trim here to preserve NLock: prefix spacing
		string label = qrSubtitleTextbox.Text.Trim();

		if (seclectedFileCount == 1)
		{
			if (issuer.Length == AppConstants.QrTitlePrefix.Length)
			{
				FileInfo fi = new(GetFirstOutputFilename(true) ?? string.Empty);
				qrTitleTextbox.Text = issuer = $"{AppConstants.QrTitlePrefix}[{fi.Name}]";
			}

			if (label.Length == 0)
				qrSubtitleTextbox.Text = label = issuer.Substring(AppConstants.QrTitlePrefix.Length);
		}
		else if (seclectedFileCount > 1)
		{
			if (issuer.Length == AppConstants.QrTitlePrefix.Length)
			{
				var dirName = Directory.GetParent(outputFolderTextBox.Text.Trim() + @"\_").Name;
				qrTitleTextbox.Text = issuer = $"{AppConstants.QrTitlePrefix}[{dirName}]";
			}

			if (label.Length == 0)
				qrSubtitleTextbox.Text = label = issuer.Substring(AppConstants.QrTitlePrefix.Length);
		}

		_authCodeInfo.TotpSecret = TOTPService.GenerateTotpSecretBase32();
		authGeneratedAuthCodeTextBox.Text = _authCodeInfo.TotpSecret;

		_authCodeInfo.QrUri = TOTPService.CreateTotpUri(_authCodeInfo.TotpSecret,
														  issuer,
														  label,
														  AppConstants.TotpTimeStepSeconds,
														  AppConstants.TotpCodeDigits);

		var qrCodePngBytes = TOTPService.CreatePngQrCodeBytes(_authCodeInfo.QrUri);
		qrCodePicture.Image = _authCodeInfo.QrImage = PngBytesToImage(qrCodePngBytes);

	}
	private static Image PngBytesToImage(byte[] pngBytes)
	{
		using var ms = new MemoryStream(pngBytes);
		return Image.FromStream(ms);
	}
	private void ClearAuthQrCode()
	{
		_authCodeInfo.Validated = false;
		thumbsPicture.Image = null;
		qrCodePicture.Image = null;
		authGeneratedAuthCodeTextBox.Clear();
	}
	private void UpdateOutputFolder()
	{
		if (string.IsNullOrWhiteSpace(outputFolderTextBox.Text) && SourceFiles.Length > 0)
		{
			var firstFile = GetFirstOutputFilename(true);
			if (firstFile is null)
			{
				return;
			}

			var fi = new FileInfo(firstFile);
			if (fi.DirectoryName != null)
			{
				outputFolderTextBox.Text = fi.DirectoryName;
			}
		}
	}
	private string? GetFirstOutputFilename(bool withPath = false)
	{
		//get lockFilenameCol value where first row where selectedFilesCol is checked
		var firstRow = sourceFilesGrid.Rows.Cast<DataGridViewRow>()
			.FirstOrDefault(row => Convert.ToBoolean(row.Cells["selectedFilesCol"].Value) == true);
		if (firstRow == null)
			return null;

		var outputFileName = firstRow.Cells["lockFilenameCol"].Value?.ToString().Trim() ?? string.Empty;
		if (withPath)
		{
			var file = SourceFiles.FirstOrDefault();
			if (!string.IsNullOrWhiteSpace(file))
			{
				var path = Path.GetDirectoryName(file) ?? string.Empty;
				outputFileName = Path.Combine(path, outputFileName);
			}
		}
		return outputFileName;
	}
	private void ValidateOutputFiles()
	{
		//change background color of lockFilenameCol cells to light red if the file already exists in the output folder
		foreach (DataGridViewRow row in sourceFilesGrid.Rows)
		{
			var outputFileName = row.Cells["lockFilenameCol"].Value?.ToString() ?? string.Empty;
			var outputFolder = outputFolderTextBox.Text;
			if (string.IsNullOrWhiteSpace(outputFileName) || string.IsNullOrWhiteSpace(outputFolder))
			{
				row.Cells["lockFilenameCol"].Style.BackColor = System.Drawing.Color.White;
				row.Cells["selectedFilesCol"].Value = false;
				row.Cells["messageCol"].Value = string.Empty;
				continue;
			}

			//should never happen, but...
			if (row.Cells["selectedFilesCol"].ReadOnly == true)
			{
				row.Cells["lockFilenameCol"].Style.BackColor = System.Drawing.Color.LightGray;
				row.Cells["messageCol"].Value = string.Empty;
				continue;
			}

			var outputFilePath = Path.Combine(outputFolder, outputFileName);
			if (System.IO.File.Exists(outputFilePath))
			{
				row.Cells["lockFilenameCol"].Style.BackColor = System.Drawing.Color.LightCoral;
				row.Cells["selectedFilesCol"].Value = false;
				row.Cells["messageCol"].Value = "Output file already exists. ";
				continue;
			}

			row.Cells["lockFilenameCol"].Style.BackColor = System.Drawing.Color.White;
		}

	}
	private int GetNumberOfSelectedFiles()
	{
		try
		{
			return GetSelectedGridViewRows().Count;
		}
		catch
		{
			return 0;
		}
	}
	private bool ValidateReadiness()
	{
		try
		{
			var anyChecked = sourceFilesGrid.RowCount > 0 && sourceFilesGrid.Rows.Cast<DataGridViewRow>()
						.Any(row => Convert.ToBoolean(row.Cells["selectedFilesCol"].Value) == true);

			return (anyChecked == true
				&& outputFolderTextBox.Text.Trim().Length > 0
				&& passwordTextBox.Text.Trim().Length > 0
				&& _authCodeInfo.AuthCode.Length == 6
				&& _authCodeInfo.Validated == true
				&& qrTitleTextbox.Text.StartsWith("NLock:", StringComparison.InvariantCulture)
				&& qrSubtitleTextbox.Text.Trim().Length > 0);

		}
		catch
		{
			return false;
		}
		finally
		{
			//reset progressBar
			progressBar.Value = 0;
		}
	}
	private void LockAllControlsOnTheForm()
	{
		sourceFilesGrid.Enabled = false;
		browseSourceButton.Enabled = false;
		browseOutputFolderButton.Enabled = false;
		outputFolderTextBox.Enabled = false;
		passwordTextBox.Enabled = false;
		authCodeTextBox.Enabled = false;
		lockFileButton.Enabled = false;
		cancelButton.Enabled = false;
		validateQrCodeButton.Enabled = false;
		qrTitleTextbox.Enabled = false;
		qrSubtitleTextbox.Enabled = false;
	}
	private void UnLockAllControlsOnTheForm()
	{
		sourceFilesGrid.Enabled = true;
		browseSourceButton.Enabled = true;
		browseOutputFolderButton.Enabled = true;
		outputFolderTextBox.Enabled = true;
		passwordTextBox.Enabled = true;
		authCodeTextBox.Enabled = true;
		lockFileButton.Enabled = true;
		cancelButton.Enabled = true;
		validateQrCodeButton.Enabled = true;
		qrTitleTextbox.Enabled = true;
		qrSubtitleTextbox.Enabled = true;
	}
	private void browseOutputFolderButton_Click(object sender, EventArgs e)
	{
		var dir = outputFolderTextBox.Text.Trim();
		if (dir.Length > 0 && Directory.Exists(dir))
		{
			outputFolderDialog.SelectedPath = dir;
		}
		if (outputFolderDialog.ShowDialog(this) == DialogResult.OK)
		{
			outputFolderTextBox.Text = outputFolderDialog.SelectedPath;
		}
	}
	private void browseSourceButton_Click(object sender, EventArgs e)
	{
		if (sourceFilesDialog.ShowDialog(this) == DialogResult.OK)
		{
			SourceFiles = sourceFilesDialog.FileNames;
			LoadSourceFilesGrid(true);
			GenerateAuthQrCode();
			lockFileButton.Enabled = ValidateReadiness();
		}
	}
	private void sourceFilesGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
	{
		//ignore if cell is not in lockFilenameCol
		if (e.ColumnIndex != sourceFilesGrid.Columns["lockFilenameCol"].Index)
			return;
		//uncheck the selectedFilesCol if value ends with ".nlock"
		var cellValue = sourceFilesGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString().Trim() ?? string.Empty;
		if (!cellValue.EndsWith(AppConstants.Extension, StringComparison.OrdinalIgnoreCase))
		{
			if (cellValue.Length > 0)
				cellValue = cellValue.Trim() + AppConstants.Extension;
			sourceFilesGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = cellValue;
		}

		sourceFilesGrid.Rows[e.RowIndex].Cells["selectedFilesCol"].Value = cellValue.Length > 0;
		ValidateOutputFiles();
	}
	private void cancelButton_Click(object sender, EventArgs e)
	{
		this.Close();
	}
	private void lockFileButton_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(passwordTextBox.Text.Trim()) || string.IsNullOrEmpty(_authCodeInfo.AuthCode))
		{
			MessageBox.Show("Please enter your password and current Auth Code.");
			return;
		}

		Directory.CreateDirectory(outputFolderTextBox.Text.Trim());
		if (!Directory.Exists(outputFolderTextBox.Text.Trim()))
		{
			MessageBox.Show("Please select a valid output folder.");
			return;
		}
		LockAllControlsOnTheForm();

		RunLock();

		progressFilenameLabel.Text = "";
		UnLockAllControlsOnTheForm();
		lockFileButton.Enabled = ValidateReadiness();

	}
	private void RunLock()
	{
		List<DataGridViewRow> rowsToLock = GetSelectedGridViewRows();

		progressBar.Maximum = rowsToLock.Count;
		progressBar.Value = 0;
		var outputFolder = outputFolderTextBox.Text.Trim();
		FileUtility.EnsureDirectoryExists(outputFolder);

		foreach (var row in rowsToLock)
		{
			var sourceFileName = row.Cells["sourcefileCol"].Value?.ToString() ?? string.Empty;
			var sourceFilePath = row.Cells["sourcefilePathCol"].Value?.ToString() ?? string.Empty;
			var unlockFileName = row.Cells["lockFilenameCol"].Value?.ToString() ?? string.Empty;
			var fullSourceFilePath = Path.Combine(sourceFilePath, sourceFileName);

			var fullOutputFilePath = Path.Combine(outputFolder, unlockFileName);
			try
			{
				var password = passwordTextBox.Text.Trim();

				progressBar.Increment(1);
				progressFilenameLabel.Text = $"Locking {sourceFileName} ...";
				progressFilenameLabel.Update();

				//sleep for a short duration to allow UI to update
				System.Threading.Thread.Sleep(100);

				NLockInfo nlockInfo = new()
				{
					SourceFile = fullSourceFilePath,
					DestinationFile = fullOutputFilePath,
					TotpSecretCode = _authCodeInfo.TotpSecret
				};
				nlockInfo.SetPassword(password);
				var result = NLockFile.TryLock(nlockInfo);
				if (result.ResultCode == NLockProcessResultCode.Success)
				{
					row.Cells["messageCol"].Value = "Success";
					row.Cells["messageCol"].Style.BackColor = System.Drawing.Color.LightGreen;
				}
				else
				{
					row.Cells["messageCol"].Value = SecureUtils.SanitizeExceptionMessage(result.Exception, result.ResultCode.ToString());
					row.Cells["messageCol"].Style.BackColor = System.Drawing.Color.LightCoral;
				}
				row.Cells["messageCol"].Style.BackColor = System.Drawing.Color.White;
			}
			catch
			{
			}
		}
	}
	private List<DataGridViewRow> GetSelectedGridViewRows()
	{
		List<DataGridViewRow> rows = new();
		foreach (DataGridViewRow row in sourceFilesGrid.Rows)
		{
			bool selected = false;
			var cellValue = row.Cells["selectedFilesCol"].Value;
			if (cellValue is bool b)
			{
				selected = b;
			}
			else if (cellValue is string s)
			{
				bool.TryParse(s, out selected);
			}
			else if (cellValue != null)
			{
				bool.TryParse(cellValue.ToString(), out selected);
			}

			if (selected)
			{
				rows.Add(row);
			}
		}

		return rows;
	}
	private void sourceFilesGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
	{
		lockFileButton.Enabled = ValidateReadiness();
	}
	private void LockForm_Load(object sender, EventArgs e)
	{
		LoadSourceFilesGrid(true);
		GenerateAuthQrCode();
		sourceFilesGrid.Update();
		lockFileButton.Enabled = ValidateReadiness();
	}
	private void ValidateReadinessEvent(object sender, EventArgs e)
	{
		//_authCodeInfo.Validated = false;
		_authCodeInfo.AuthCode = authCodeTextBox.Text.Trim();
		lockFileButton.Enabled = ValidateReadiness();

		if (sender == authCodeTextBox)
		{
			if (validateQrCodeButton.Enabled && _authCodeInfo.AuthCode.Length == 6)
			{
				validateQrCodeButton.PerformClick();
			}
		}
	}
	private void authCodeTextBox_KeyDown(object sender, KeyEventArgs e)
	{
		// If Enter pressed and the Validate button is enabled, trigger its click.
		if (e.KeyCode == Keys.Enter)
		{
			if (validateQrCodeButton.Enabled)
			{
				validateQrCodeButton.PerformClick();
			}
			// Prevent the system ding and further processing
			e.SuppressKeyPress = true;
			e.Handled = true;
			return;
		}

		// For all other keys, preserve existing numeric-only suppression logic.
		e.SuppressKeyPress = IsNumeric(e.KeyCode);
	}
	public static bool IsNumeric(Keys key)
	{
		return (key is not (>= Keys.D0
							and <= Keys.D9
							or >= Keys.NumPad0
							and <= Keys.NumPad9
							or Keys.Back
							or Keys.Tab
							or Keys.Left
							or Keys.Right));
	}
	private void validateQrCodeButton_Click(object sender, EventArgs e)
	{
		_authCodeInfo.Validated = false;
		if (string.IsNullOrWhiteSpace(_authCodeInfo.AuthCode))
		{
			MessageBox.Show("Please enter the 6-digit Auth code.");
			return;
		}

		_authCodeInfo.Validated = TOTPService.ValidateAuthCode(_authCodeInfo.TotpSecret, _authCodeInfo.AuthCode);
		if (_authCodeInfo.Validated)
		{
			thumbsPicture.Image = Resources.thumbsUpIcon;
			_failedAttempts = 0;
		}
		else
		{
			thumbsPicture.Image = Resources.thumbsDownIcon;

			authCodeTextBox.Clear();
			_authCodeInfo.AuthCode = string.Empty;

			if (++_failedAttempts >= MAX_FAIL_ATTEMPTS)
			{
				MessageBox.Show("Too many failed attempts. Exiting for security.");
				Application.Exit();
			}
		}
		lockFileButton.Enabled = ValidateReadiness();
	}
	private void reloadAuthCodeButton_Click(object sender, EventArgs e)
	{
		GenerateAuthQrCode();
		ValidateReadiness();
	}
	private void qrCodePicture_DoubleClick(object sender, EventArgs e)
	{
		ZoomedQrForm zoom = new(_authCodeInfo);
		zoom.ShowDialog(this);
		if (authCodeTextBox.Text == _authCodeInfo.AuthCode && _authCodeInfo.Validated)
		{
			thumbsPicture.Image = _authCodeInfo.Validated ? Resources.thumbsUpIcon : null;
		}
		else if (authCodeTextBox.Text != _authCodeInfo.AuthCode && _authCodeInfo.Validated)
		{
			authCodeTextBox.Text = _authCodeInfo.AuthCode;
			_authCodeInfo.Validated = true; //was reset in authCodeTextBox_TextChanged
			thumbsPicture.Image = _authCodeInfo.Validated ? Resources.thumbsUpIcon : null;
		}
		else
		{
			thumbsPicture.Image = null;
		}
		lockFileButton.Enabled = ValidateReadiness();
	}
	private void qrTitleTextbox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
	{
		EnsureQrTitleStartsWithNLock();
	}

	private void EnsureQrTitleStartsWithNLock()
	{
		var txt = qrTitleTextbox.Text.Trim();
		if (!txt.StartsWith(AppConstants.QrTitlePrefix, StringComparison.OrdinalIgnoreCase))
		{
			qrTitleTextbox.Text = AppConstants.QrTitlePrefix + txt;
		}
	}

	private void logo_Click(object sender, EventArgs e)
	{
		using (var aboutBox = new AboutBox())
		{
			aboutBox.ShowDialog(this);
		}
	}
}
