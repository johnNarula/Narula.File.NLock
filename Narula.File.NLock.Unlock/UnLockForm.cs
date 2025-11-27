namespace Narula.File.NLock;

using Narula.File.NLock.Lib.UI;
using Narula.File.NLock.Models;
public partial class UnLockForm : Form
{
	public UnLockForm()
	{
		InitializeComponent();
		logo.Image = NLockResx.NUnlock_PNG;
	}
	public UnLockForm(params string[] sourceFiles) : this()
	{
		this.SourceFiles = sourceFiles;
	}
	public UnLockForm(UnlockGuiLaunchRequest? launchRequest) : this()
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
						files.AddRange(Directory.GetFiles(dir, AppConstants.Extension, SearchOption.TopDirectoryOnly));
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

	[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
	public string[] SourceFiles { get; set; } = Array.Empty<string>();
	private UnlockGuiLaunchRequest? _launchRequest;

	private void LoadSourceFilesGrid(bool clearRows = true)
	{
		if (clearRows)
		{
			sourceFilesGrid.Rows.Clear();
		}
		CleanSourceFiles();

		foreach (var file in SourceFiles)
		{
			var fi = new FileInfo(file);
			var isNLockFile = fi.Extension.Equals(AppConstants.Extension, StringComparison.OrdinalIgnoreCase);
			var unlockFilename = GetTargetFileName(fi);
			int rowIndex = sourceFilesGrid.Rows.Add();
			sourceFilesGrid.Rows[rowIndex].Cells["sourcefileCol"].Value = fi.Name;
			sourceFilesGrid.Rows[rowIndex].Cells["sourcefilePathCol"].Value = fi.DirectoryName;
			sourceFilesGrid.Rows[rowIndex].Cells["messageCol"].Value = string.Empty;

			if (isNLockFile)
			{
				sourceFilesGrid.Rows[rowIndex].Cells["selectedFilesCol"].Value = true;
				sourceFilesGrid.Rows[rowIndex].Cells["unlockFilenameCol"].Value = Path.GetFileNameWithoutExtension(fi.Name);
			}
			else //should never happen because of CleanSourceFiles(), but just in case
			{
				sourceFilesGrid.Rows[rowIndex].Cells["unlockFilenameCol"].Value = fi.Name;
				sourceFilesGrid.Rows[rowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
				sourceFilesGrid.Rows[rowIndex].Cells["selectedFilesCol"].Value = false;
				sourceFilesGrid.Rows[rowIndex].Cells["selectedFilesCol"].ReadOnly = true;
			}
		}

		UpdateOutputFolder();
	}
	private string GetTargetFileName(FileInfo fi)
	{
		var initialFilename = Path.GetFileNameWithoutExtension(fi.Name);
		return FileUtility.GetUniqueTargetFilenameInDirectory(fi.DirectoryName, initialFilename);
	}
	private void CleanSourceFiles()
	{
		//remove any duplicate files from SourceFiles
		SourceFiles = SourceFiles.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
		//remove any files whose extension is not .nlock or files that do not exist
		SourceFiles = SourceFiles.Where(f =>
		{
			var fi = new FileInfo(f);
			if (fi.Extension.Equals(AppConstants.Extension, StringComparison.OrdinalIgnoreCase)
				&& fi.Exists)
			{
				return true;
			}
			return false;
		}).ToArray();

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
		//get unlockFilenameCol value where first row where selectedFilesCol is checked
		var firstRow = sourceFilesGrid.Rows.Cast<DataGridViewRow>()
			.FirstOrDefault(row => Convert.ToBoolean(row.Cells["selectedFilesCol"].Value) == true);
		if (firstRow == null)
			return null;

		var outputFileName = firstRow.Cells["unlockFilenameCol"].Value?.ToString().Trim() ?? string.Empty;
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
	private int GetNumberOfSelectedFiles()
	{
		try
		{
			var checkedOutputs = sourceFilesGrid.Rows.Cast<DataGridViewRow>()
						.Count(row => Convert.ToBoolean(row.Cells["selectedFilesCol"].Value) == true);

			return checkedOutputs;
		}
		catch
		{
			return 0;
		}
	}
	private void ValidateOutputFiles()
	{
		//change background color of unlockFilenameCol cells to light red if the file already exists in the output folder
		foreach (DataGridViewRow row in sourceFilesGrid.Rows)
		{
			var outputFileName = row.Cells["unlockFilenameCol"].Value?.ToString() ?? string.Empty;
			var outputFolder = outputFolderTextBox.Text;
			if (string.IsNullOrWhiteSpace(outputFileName) || string.IsNullOrWhiteSpace(outputFolder))
			{
				row.Cells["unlockFilenameCol"].Style.BackColor = System.Drawing.Color.White;
				row.Cells["selectedFilesCol"].Value = false;
				row.Cells["messageCol"].Value = string.Empty;
				continue;
			}

			//should never happen, but...
			if (row.Cells["selectedFilesCol"].ReadOnly == true)
			{
				row.Cells["unlockFilenameCol"].Style.BackColor = System.Drawing.Color.LightGray;
				row.Cells["messageCol"].Value = string.Empty;
				continue;
			}

			var outputFilePath = Path.Combine(outputFolder, outputFileName);
			if (System.IO.File.Exists(outputFilePath))
			{
				row.Cells["unlockFilenameCol"].Style.BackColor = System.Drawing.Color.LightCoral;
				row.Cells["selectedFilesCol"].Value = false;
				row.Cells["messageCol"].Value = "Output file already exists.";
				continue;
			}

			row.Cells["unlockFilenameCol"].Style.BackColor = System.Drawing.Color.White;
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
				&& authCodeTextBox.Text.Trim().Length == 6);
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
		unlockFileButton.Enabled = false;
		cancelButton.Enabled = false;
	}
	private void UnLockAllControlsOnTheForm()
	{
		sourceFilesGrid.Enabled = true;
		browseSourceButton.Enabled = true;
		browseOutputFolderButton.Enabled = true;
		outputFolderTextBox.Enabled = true;
		passwordTextBox.Enabled = true;
		authCodeTextBox.Enabled = true;
		unlockFileButton.Enabled = true;
		cancelButton.Enabled = true;
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
			unlockFileButton.Enabled = ValidateReadiness();
		}
	}
	private void sourceFilesGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
	{
		//ignore if cell is not in unlockFilenameCol
		if (e.ColumnIndex != sourceFilesGrid.Columns["unlockFilenameCol"].Index)
			return;
		//uncheck the selectedFilesCol if value does not ends with ".nlock"
		var cellValue = sourceFilesGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString().Trim() ?? string.Empty;
		if (cellValue.EndsWith(AppConstants.Extension, StringComparison.OrdinalIgnoreCase))
		{
			//remove the extension
			cellValue = cellValue.Substring(0, cellValue.Length - AppConstants.Extension.Length);
			sourceFilesGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = cellValue;
		}

		sourceFilesGrid.Rows[e.RowIndex].Cells["selectedFilesCol"].Value = cellValue.Length > 0;
		ValidateOutputFiles();
	}
	private void cancelButton_Click(object sender, EventArgs e)
	{
		this.Close();
	}
	private void unlockFileButton_ClickAsync(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(passwordTextBox.Text.Trim()) || string.IsNullOrEmpty(authCodeTextBox.Text.Trim()))
		{
			MessageBox.Show("Please enter your password and current Auth Code.");
			return;
		}

		if (!Directory.Exists(outputFolderTextBox.Text.Trim()))
		{
			MessageBox.Show("Please select a valid output folder.");
			return;
		}
		LockAllControlsOnTheForm();

		RunUnlock();

		progressFilenameLabel.Text = "";
		UnLockAllControlsOnTheForm();

	}
	private void RunUnlock()
	{
		var rowsToUnlock = sourceFilesGrid.Rows.Cast<DataGridViewRow>()
			.Where(row => Convert.ToBoolean(row.Cells["selectedFilesCol"].Value) == true)
			.ToList();

		progressBar.Maximum = rowsToUnlock.Count;
		progressBar.Value = 0;
		var outputFolder = outputFolderTextBox.Text.Trim();
		FileUtility.EnsureDirectoryExists(outputFolder);

		bool failedDueToPasswordOrAuthCode = false;

		foreach (var row in rowsToUnlock)
		{
			var sourceFileName = row.Cells["sourcefileCol"].Value?.ToString() ?? string.Empty;
			var sourceFilePath = row.Cells["sourcefilePathCol"].Value?.ToString() ?? string.Empty;
			var unlockFileName = row.Cells["unlockFilenameCol"].Value?.ToString() ?? string.Empty;
			var fullSourceFilePath = Path.Combine(sourceFilePath, sourceFileName);

			var fullOutputFilePath = Path.Combine(outputFolder, unlockFileName);
			try
			{
				var password = passwordTextBox.Text.Trim();
				var totpCode = authCodeTextBox.Text.Trim();

				progressBar.Increment(1);
				progressFilenameLabel.Text = $"Unlocking {sourceFileName} ...";
				progressFilenameLabel.Update();

				//sleep for a short duration to allow UI to update
				System.Threading.Thread.Sleep(100);

				NLockInfo nlockInfo = new()
				{
					SourceFile = fullSourceFilePath,
					DestinationFile = fullOutputFilePath,
					TotpAuthCode = totpCode
				};
				nlockInfo.SetPassword(password);
				var result = NLockFile.TryUnlock(nlockInfo);
				if (result.ResultCode == NLockProcessResultCode.Success)
				{
					row.Cells["messageCol"].Value = "Success";
					row.Cells["messageCol"].Style.BackColor = System.Drawing.Color.LightGreen;
				}
				else
				{
					if (result.ResultCode is NLockProcessResultCode.IncorrectPassword or NLockProcessResultCode.InvalidTotpCode)
					{
						failedDueToPasswordOrAuthCode = true;
					}
					row.Cells["messageCol"].Value = SecureUtils.SanitizeExceptionMessage(result.Exception, result.ResultCode.ToString());
					row.Cells["messageCol"].Style.BackColor = System.Drawing.Color.LightCoral;
				}
				row.Cells["messageCol"].Style.BackColor = System.Drawing.Color.White;
			}
			catch
			{
			}
			if (failedDueToPasswordOrAuthCode)
				CheckAttempts(AppConstants.FailedAttempts++);
		}
	}

	private void CheckAttempts(int attempts)
	{
		if (attempts >= AppConstants.MAX_FAIL_ATTEMPTS)
		{
			MessageBox.Show($"Maximum of {AppConstants.MAX_FAIL_ATTEMPTS} failed attempts due to password or auth code reached. The application will now close.");
			Application.Exit();
		}

	}

	private void sourceFilesGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
	{
		unlockFileButton.Enabled = ValidateReadiness();
	}
	private void UnlockForm_Load(object sender, EventArgs e)
	{
		LoadSourceFilesGrid(true);
		sourceFilesGrid.Update();
		ValidateOutputFiles();
		unlockFileButton.Enabled = ValidateReadiness();
	}
	private void ValidateReadinessEvent(object sender, EventArgs e)
	{
		unlockFileButton.Enabled = ValidateReadiness();
	}

	private void authCodeTextBox_KeyDown(object sender, KeyEventArgs e)
	{
		//ignore non-numeric value
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

	private void logo_Click(object sender, EventArgs e)
	{
		using(var aboutBox = new AboutBox())
		{
			aboutBox.ShowDialog(this);
		}
	}
}
