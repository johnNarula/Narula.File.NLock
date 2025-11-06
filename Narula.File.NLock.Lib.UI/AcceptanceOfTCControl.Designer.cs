namespace Narula.File.NLock.Lib.UI;

partial class AcceptanceOfTCControl
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

	#region Component Designer generated code

	/// <summary> 
	/// Required method for Designer support - do not modify 
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		tcContent = new RichTextBox();
		chkAccept = new CheckBox();
		tblLayout = new TableLayoutPanel();
		tblLayout.SuspendLayout();
		SuspendLayout();
		// 
		// tcContent
		// 
		tcContent.BackColor = SystemColors.Info;
		tcContent.Dock = DockStyle.Fill;
		tcContent.Location = new Point(3, 3);
		tcContent.Name = "tcContent";
		tcContent.ReadOnly = true;
		tcContent.Size = new Size(334, 363);
		tcContent.TabIndex = 0;
		tcContent.Text = "";
		// 
		// chkAccept
		// 
		chkAccept.Dock = DockStyle.Fill;
		chkAccept.Enabled = false;
		chkAccept.Location = new Point(3, 372);
		chkAccept.Name = "chkAccept";
		chkAccept.Size = new Size(334, 19);
		chkAccept.TabIndex = 0;
		chkAccept.Text = "I have read and accept the Terms and Conditions";
		chkAccept.UseVisualStyleBackColor = true;
		// 
		// tblLayout
		// 
		tblLayout.ColumnCount = 1;
		tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
		tblLayout.Controls.Add(tcContent, 0, 0);
		tblLayout.Controls.Add(chkAccept, 0, 1);
		tblLayout.Dock = DockStyle.Fill;
		tblLayout.Location = new Point(0, 0);
		tblLayout.Name = "tblLayout";
		tblLayout.RowCount = 2;
		tblLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
		tblLayout.RowStyles.Add(new RowStyle());
		tblLayout.Size = new Size(340, 394);
		tblLayout.TabIndex = 2;
		// 
		// AcceptanceOfTCControl
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		Controls.Add(tblLayout);
		Name = "AcceptanceOfTCControl";
		Size = new Size(340, 394);
		tblLayout.ResumeLayout(false);
		ResumeLayout(false);
	}

	#endregion

	private RichTextBox tcContent;
	private CheckBox chkAccept;
	private TableLayoutPanel tblLayout;
}
