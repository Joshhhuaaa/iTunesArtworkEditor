namespace iTunesArtworkEditor
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.menuStrip_Main = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exportAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useLatestAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox_Search = new System.Windows.Forms.TextBox();
            this.panel_Top = new System.Windows.Forms.Panel();
            this.button_BrowseDirectory = new System.Windows.Forms.Button();
            this.label_Directory = new System.Windows.Forms.Label();
            this.panel_AddressBar = new System.Windows.Forms.Panel();
            this.label_DirectoryPath = new System.Windows.Forms.Label();
            this.button_Refresh = new System.Windows.Forms.Button();
            this.panel_Bottom = new System.Windows.Forms.Panel();
            this.button_UseLatest = new System.Windows.Forms.Button();
            this.button_ExportImage = new System.Windows.Forms.Button();
            this.button_ReplaceImage = new System.Windows.Forms.Button();
            this.label_Status = new System.Windows.Forms.Label();
            this.panel_Content = new System.Windows.Forms.Panel();
            this.splitContainer_Main = new System.Windows.Forms.SplitContainer();
            this.panel_Left = new System.Windows.Forms.Panel();
            this.label_Files = new System.Windows.Forms.Label();
            this.comboBox_Filter = new System.Windows.Forms.ComboBox();
            this.listBox_ITC2Files = new System.Windows.Forms.ListBox();
            this.checkBox_HideEmpty = new System.Windows.Forms.CheckBox();
            this.panel_Right = new System.Windows.Forms.Panel();
            this.label_Details = new System.Windows.Forms.Label();
            this.label_FileInfo = new System.Windows.Forms.Label();
            this.pictureBox_Preview = new System.Windows.Forms.PictureBox();
            this.folderBrowserDialog_Main = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog_Image = new System.Windows.Forms.OpenFileDialog();
            this.contextMenu_FileList = new System.Windows.Forms.ContextMenuStrip();
            this.menuItem_ShowInExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_DeleteFromLibrary = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu_FileList.SuspendLayout();
            this.menuStrip_Main.SuspendLayout();
            this.panel_AddressBar.SuspendLayout();
            this.panel_Top.SuspendLayout();
            this.panel_Bottom.SuspendLayout();
            this.panel_Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).BeginInit();
            this.splitContainer_Main.Panel1.SuspendLayout();
            this.splitContainer_Main.Panel2.SuspendLayout();
            this.splitContainer_Main.SuspendLayout();
            this.panel_Left.SuspendLayout();
            this.panel_Right.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Preview)).BeginInit();
            this.SuspendLayout();
            //
            // menuStrip_Main
            //
            this.menuStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.fileToolStripMenuItem, this.editToolStripMenuItem });
            this.menuStrip_Main.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_Main.Name = "menuStrip_Main";
            this.menuStrip_Main.Size = new System.Drawing.Size(900, 24);
            this.menuStrip_Main.TabIndex = 5;
            this.menuStrip_Main.Text = "menuStrip_Main";
            //
            // fileToolStripMenuItem
            //
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.openFolderToolStripMenuItem,
                this.saveToolStripMenuItem,
                this.toolStripSeparator2,
                this.exportAllToolStripMenuItem,
                this.useLatestAllToolStripMenuItem,
                this.toolStripSeparator3,
                this.exitToolStripMenuItem });
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            //
            // openFolderToolStripMenuItem
            //
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openFolderToolStripMenuItem.Text = "Open...";
            this.openFolderToolStripMenuItem.Click += new System.EventHandler(this.OpenFolderToolStripMenuItem_Click);
            //
            // saveToolStripMenuItem
            //
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            //
            // toolStripSeparator2
            //
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            //
            // exportAllToolStripMenuItem
            //
            this.exportAllToolStripMenuItem.Enabled = false;
            this.exportAllToolStripMenuItem.Name = "exportAllToolStripMenuItem";
            this.exportAllToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.exportAllToolStripMenuItem.Text = "Export All Artwork...";
            this.exportAllToolStripMenuItem.Click += new System.EventHandler(this.ExportAllToolStripMenuItem_Click);
            //
            // useLatestAllToolStripMenuItem
            //
            this.useLatestAllToolStripMenuItem.Enabled = false;
            this.useLatestAllToolStripMenuItem.Name = "useLatestAllToolStripMenuItem";
            this.useLatestAllToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.useLatestAllToolStripMenuItem.Text = "Use Latest Artwork for All Artists...";
            this.useLatestAllToolStripMenuItem.Click += new System.EventHandler(this.UseLatestAllToolStripMenuItem_Click);
            //
            // toolStripSeparator3
            //
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            //
            // exitToolStripMenuItem
            //
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeyDisplayString = "Alt+F4";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            //
            // editToolStripMenuItem
            //
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.copyImageToolStripMenuItem,
                this.pasteImageToolStripMenuItem });
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.DropDownOpening += new System.EventHandler(this.EditToolStripMenuItem_DropDownOpening);
            //
            // copyImageToolStripMenuItem
            //
            this.copyImageToolStripMenuItem.Enabled = true;
            this.copyImageToolStripMenuItem.Name = "copyImageToolStripMenuItem";
            this.copyImageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyImageToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.copyImageToolStripMenuItem.Text = "Copy Artwork";
            this.copyImageToolStripMenuItem.Click += new System.EventHandler(this.CopyImageToolStripMenuItem_Click);
            //
            // pasteImageToolStripMenuItem
            //
            this.pasteImageToolStripMenuItem.Enabled = true;
            this.pasteImageToolStripMenuItem.Name = "pasteImageToolStripMenuItem";
            this.pasteImageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteImageToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.pasteImageToolStripMenuItem.Text = "Paste Artwork";
            this.pasteImageToolStripMenuItem.Click += new System.EventHandler(this.PasteImageToolStripMenuItem_Click);
            //
            // panel_Top
            //
            this.panel_Top.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Top.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Top.Controls.Add(this.button_BrowseDirectory);
            this.panel_Top.Controls.Add(this.button_Refresh);
            this.panel_Top.Controls.Add(this.label_Directory);
            this.panel_Top.Controls.Add(this.panel_AddressBar);
            this.panel_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Top.Location = new System.Drawing.Point(0, 0);
            this.panel_Top.Name = "panel_Top";
            this.panel_Top.Padding = new System.Windows.Forms.Padding(10);
            this.panel_Top.Size = new System.Drawing.Size(900, 65);
            this.panel_Top.TabIndex = 0;
            // 
            // button_BrowseDirectory
            // 
            this.button_BrowseDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_BrowseDirectory.Location = new System.Drawing.Point(780, 30);
            this.button_BrowseDirectory.Name = "button_BrowseDirectory";
            this.button_BrowseDirectory.Size = new System.Drawing.Size(100, 25);
            this.button_BrowseDirectory.TabIndex = 2;
            this.button_BrowseDirectory.Text = "Browse...";
            this.button_BrowseDirectory.UseVisualStyleBackColor = true;
            this.button_BrowseDirectory.Click += new System.EventHandler(this.Button_BrowseDirectory_Click);
            // 
            // label_Directory
            // 
            this.label_Directory.AutoSize = true;
            this.label_Directory.Location = new System.Drawing.Point(10, 12);
            this.label_Directory.Name = "label_Directory";
            this.label_Directory.Size = new System.Drawing.Size(152, 15);
            this.label_Directory.TabIndex = 0;
            this.label_Directory.Text = "iTunes Artwork Directory:";
            //
            // panel_AddressBar
            //
            this.panel_AddressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_AddressBar.BackColor = System.Drawing.SystemColors.Window;
            this.panel_AddressBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_AddressBar.Controls.Add(this.label_DirectoryPath);
            this.panel_AddressBar.Location = new System.Drawing.Point(10, 30);
            this.panel_AddressBar.Name = "panel_AddressBar";
            this.panel_AddressBar.Size = new System.Drawing.Size(733, 25);
            this.panel_AddressBar.TabIndex = 1;
            //
            // label_DirectoryPath
            //
            this.label_DirectoryPath.AutoSize = false;
            this.label_DirectoryPath.BackColor = System.Drawing.SystemColors.Window;
            this.label_DirectoryPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_DirectoryPath.Location = new System.Drawing.Point(0, 0);
            this.label_DirectoryPath.Name = "label_DirectoryPath";
            this.label_DirectoryPath.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label_DirectoryPath.Size = new System.Drawing.Size(736, 21);
            this.label_DirectoryPath.TabIndex = 0;
            this.label_DirectoryPath.Text = "";
            this.label_DirectoryPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // button_Refresh
            //
            this.button_Refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Refresh.BackColor = System.Drawing.SystemColors.Control;
            this.button_Refresh.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.button_Refresh.FlatAppearance.BorderSize = 1;
            this.button_Refresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Refresh.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10F);
            this.button_Refresh.Location = new System.Drawing.Point(746, 30);
            this.button_Refresh.Name = "button_Refresh";
            this.button_Refresh.Size = new System.Drawing.Size(30, 25);
            this.button_Refresh.TabIndex = 3;
            this.button_Refresh.Text = "";
            this.button_Refresh.UseVisualStyleBackColor = false;
            this.button_Refresh.Click += new System.EventHandler(this.Button_Refresh_Click);
            // 
            // panel_Bottom
            // 
            this.panel_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Bottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Bottom.Controls.Add(this.button_ReplaceImage);
            this.panel_Bottom.Controls.Add(this.button_ExportImage);
            this.panel_Bottom.Controls.Add(this.button_UseLatest);
            this.panel_Bottom.Controls.Add(this.label_Status);
            this.panel_Bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_Bottom.Location = new System.Drawing.Point(0, 530);
            this.panel_Bottom.Name = "panel_Bottom";
            this.panel_Bottom.Padding = new System.Windows.Forms.Padding(10);
            this.panel_Bottom.Size = new System.Drawing.Size(900, 50);
            this.panel_Bottom.TabIndex = 1;
            //
            // button_UseLatest
            //
            this.button_UseLatest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_UseLatest.Enabled = false;
            this.button_UseLatest.Location = new System.Drawing.Point(705, 12);
            this.button_UseLatest.Name = "button_UseLatest";
            this.button_UseLatest.Size = new System.Drawing.Size(90, 25);
            this.button_UseLatest.TabIndex = 3;
            this.button_UseLatest.Text = "Get Latest";
            this.button_UseLatest.UseVisualStyleBackColor = true;
            this.button_UseLatest.Click += new System.EventHandler(this.Button_UseLatest_Click);
            //
            // button_ExportImage
            //
            this.button_ExportImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ExportImage.Enabled = false;
            this.button_ExportImage.Location = new System.Drawing.Point(620, 12);
            this.button_ExportImage.Name = "button_ExportImage";
            this.button_ExportImage.Size = new System.Drawing.Size(80, 25);
            this.button_ExportImage.TabIndex = 2;
            this.button_ExportImage.Text = "Export...";
            this.button_ExportImage.UseVisualStyleBackColor = true;
            this.button_ExportImage.Click += new System.EventHandler(this.Button_ExportImage_Click);
            //
            // button_ReplaceImage
            //
            this.button_ReplaceImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ReplaceImage.Enabled = false;
            this.button_ReplaceImage.Location = new System.Drawing.Point(800, 12);
            this.button_ReplaceImage.Name = "button_ReplaceImage";
            this.button_ReplaceImage.Size = new System.Drawing.Size(80, 25);
            this.button_ReplaceImage.TabIndex = 1;
            this.button_ReplaceImage.Text = "Replace...";
            this.button_ReplaceImage.UseVisualStyleBackColor = true;
            this.button_ReplaceImage.Click += new System.EventHandler(this.Button_ReplaceImage_Click);
            // 
            // label_Status
            // 
            this.label_Status.AutoSize = true;
            this.label_Status.Location = new System.Drawing.Point(10, 17);
            this.label_Status.Name = "label_Status";
            this.label_Status.Size = new System.Drawing.Size(39, 15);
            this.label_Status.TabIndex = 0;
            this.label_Status.Text = "Ready";
            // 
            // panel_Content
            // 
            this.panel_Content.Controls.Add(this.splitContainer_Main);
            this.panel_Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Content.Location = new System.Drawing.Point(0, 90);
            this.panel_Content.Name = "panel_Content";
            this.panel_Content.Size = new System.Drawing.Size(900, 440);
            this.panel_Content.TabIndex = 2;
            // 
            // splitContainer_Main
            // 
            this.splitContainer_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Main.Name = "splitContainer_Main";
            // 
            // splitContainer_Main.Panel1
            // 
            this.splitContainer_Main.Panel1.Controls.Add(this.panel_Left);
            // 
            // splitContainer_Main.Panel2
            // 
            this.splitContainer_Main.Panel2.Controls.Add(this.panel_Right);
            this.splitContainer_Main.Size = new System.Drawing.Size(900, 440);
            this.splitContainer_Main.SplitterDistance = 300;
            this.splitContainer_Main.TabIndex = 0;
            // 
            // panel_Left
            // 
            this.panel_Left.Controls.Add(this.checkBox_HideEmpty);
            this.panel_Left.Controls.Add(this.listBox_ITC2Files);
            this.panel_Left.Controls.Add(this.textBox_Search);
            this.panel_Left.Controls.Add(this.comboBox_Filter);
            this.panel_Left.Controls.Add(this.label_Files);
            this.panel_Left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Left.Location = new System.Drawing.Point(0, 0);
            this.panel_Left.Name = "panel_Left";
            this.panel_Left.Padding = new System.Windows.Forms.Padding(10);
            this.panel_Left.Size = new System.Drawing.Size(300, 440);
            this.panel_Left.TabIndex = 0;
            // 
            // label_Files
            // 
            this.label_Files.AutoSize = true;
            this.label_Files.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label_Files.Location = new System.Drawing.Point(10, 10);
            this.label_Files.Name = "label_Files";
            this.label_Files.Size = new System.Drawing.Size(49, 15);
            this.label_Files.TabIndex = 0;
            this.label_Files.Text = "Files (0)";
            //
            // comboBox_Filter
            //
            this.comboBox_Filter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_Filter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Filter.Items.AddRange(new object[] { "All Types", "Artist", "Album" });
            this.comboBox_Filter.Location = new System.Drawing.Point(10, 28);
            this.comboBox_Filter.Name = "comboBox_Filter";
            this.comboBox_Filter.Size = new System.Drawing.Size(280, 21);
            this.comboBox_Filter.TabIndex = 1;
            this.comboBox_Filter.SelectedIndex = 0;
            this.comboBox_Filter.SelectedIndexChanged += new System.EventHandler(this.ComboBox_Filter_SelectedIndexChanged);
            //
            // textBox_Search
            //
            this.textBox_Search.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Search.Location = new System.Drawing.Point(10, 53);
            this.textBox_Search.Name = "textBox_Search";
            this.textBox_Search.PlaceholderText = "Search";
            this.textBox_Search.Size = new System.Drawing.Size(280, 22);
            this.textBox_Search.TabIndex = 4;
            this.textBox_Search.TextChanged += new System.EventHandler(this.TextBox_Search_TextChanged);
            //
            // listBox_ITC2Files
            //
            this.listBox_ITC2Files.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_ITC2Files.Location = new System.Drawing.Point(10, 79);
            this.listBox_ITC2Files.Name = "listBox_ITC2Files";
            this.listBox_ITC2Files.Size = new System.Drawing.Size(280, 324);
            this.listBox_ITC2Files.ContextMenuStrip = this.contextMenu_FileList;
            this.listBox_ITC2Files.TabIndex = 2;
            this.listBox_ITC2Files.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBox_ITC2Files_MouseDown);
            this.listBox_ITC2Files.SelectedIndexChanged += new System.EventHandler(this.ListBox_ITC2Files_SelectedIndexChanged);
            //
            // contextMenu_FileList
            //
            this.contextMenu_FileList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.menuItem_ShowInExplorer,
                this.menuItem_DeleteFromLibrary });
            this.contextMenu_FileList.Name = "contextMenu_FileList";
            this.contextMenu_FileList.Size = new System.Drawing.Size(193, 48);
            this.contextMenu_FileList.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenu_FileList_Opening);
            //
            // menuItem_ShowInExplorer
            //
            this.menuItem_ShowInExplorer.Name = "menuItem_ShowInExplorer";
            this.menuItem_ShowInExplorer.Size = new System.Drawing.Size(192, 22);
            this.menuItem_ShowInExplorer.Text = "Show in Windows Explorer";
            this.menuItem_ShowInExplorer.Click += new System.EventHandler(this.MenuItem_ShowInExplorer_Click);
            //
            // menuItem_DeleteFromLibrary
            //
            this.menuItem_DeleteFromLibrary.Name = "menuItem_DeleteFromLibrary";
            this.menuItem_DeleteFromLibrary.Size = new System.Drawing.Size(192, 22);
            this.menuItem_DeleteFromLibrary.Text = "Delete From Library";
            this.menuItem_DeleteFromLibrary.Click += new System.EventHandler(this.MenuItem_DeleteFromLibrary_Click);
            //
            // checkBox_HideEmpty
            //
            this.checkBox_HideEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_HideEmpty.Checked = true;
            this.checkBox_HideEmpty.Location = new System.Drawing.Point(10, 408);
            this.checkBox_HideEmpty.Name = "checkBox_HideEmpty";
            this.checkBox_HideEmpty.Size = new System.Drawing.Size(280, 20);
            this.checkBox_HideEmpty.TabIndex = 3;
            this.checkBox_HideEmpty.Text = "Hide cache files without artwork";
            this.checkBox_HideEmpty.UseVisualStyleBackColor = true;
            this.checkBox_HideEmpty.CheckedChanged += new System.EventHandler(this.CheckBox_HideEmpty_CheckedChanged);
            //
            // panel_Right
            // 
            this.panel_Right.Controls.Add(this.pictureBox_Preview);
            this.panel_Right.Controls.Add(this.label_FileInfo);
            this.panel_Right.Controls.Add(this.label_Details);
            this.panel_Right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Right.Location = new System.Drawing.Point(0, 0);
            this.panel_Right.Name = "panel_Right";
            this.panel_Right.Padding = new System.Windows.Forms.Padding(10);
            this.panel_Right.Size = new System.Drawing.Size(596, 440);
            this.panel_Right.TabIndex = 0;
            // 
            // label_Details
            // 
            this.label_Details.AutoSize = true;
            this.label_Details.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label_Details.Location = new System.Drawing.Point(10, 10);
            this.label_Details.Name = "label_Details";
            this.label_Details.Size = new System.Drawing.Size(41, 15);
            this.label_Details.TabIndex = 0;
            this.label_Details.Text = "Details";
            // 
            // label_FileInfo
            // 
            this.label_FileInfo.AutoSize = true;
            this.label_FileInfo.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_FileInfo.Location = new System.Drawing.Point(10, 30);
            this.label_FileInfo.Name = "label_FileInfo";
            this.label_FileInfo.Size = new System.Drawing.Size(174, 15);
            this.label_FileInfo.TabIndex = 1;
            this.label_FileInfo.Text = "Select a file to view details...";
            // 
            // pictureBox_Preview
            // 
            this.pictureBox_Preview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_Preview.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pictureBox_Preview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_Preview.AllowDrop = true;
            this.pictureBox_Preview.Location = new System.Drawing.Point(10, 110);
            this.pictureBox_Preview.Name = "pictureBox_Preview";
            this.pictureBox_Preview.Size = new System.Drawing.Size(576, 320);
            this.pictureBox_Preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Preview.TabIndex = 2;
            this.pictureBox_Preview.TabStop = false;
            this.pictureBox_Preview.DragEnter += new System.Windows.Forms.DragEventHandler(this.PictureBox_Preview_DragEnter);
            this.pictureBox_Preview.DragDrop += new System.Windows.Forms.DragEventHandler(this.PictureBox_Preview_DragDrop);
            // 
            // folderBrowserDialog_Main
            // 
            // 
            // openFileDialog_Image
            // 
            this.openFileDialog_Image.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            this.openFileDialog_Image.Title = "Select replacement artwork";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 580);
            this.KeyPreview = true;
            this.Controls.Add(this.panel_Content);
            this.Controls.Add(this.panel_Bottom);
            this.Controls.Add(this.panel_Top);
            this.Controls.Add(this.menuStrip_Main);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MainMenuStrip = this.menuStrip_Main;
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "iTunes Artwork Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel_AddressBar.ResumeLayout(false);
            this.panel_Top.ResumeLayout(false);
            this.panel_Top.PerformLayout();
            this.panel_Bottom.ResumeLayout(false);
            this.panel_Bottom.PerformLayout();
            this.panel_Content.ResumeLayout(false);
            this.splitContainer_Main.Panel1.ResumeLayout(false);
            this.splitContainer_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).EndInit();
            this.splitContainer_Main.ResumeLayout(false);
            this.panel_Left.ResumeLayout(false);
            this.panel_Left.PerformLayout();
            this.panel_Right.ResumeLayout(false);
            this.panel_Right.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Preview)).EndInit();
            this.contextMenu_FileList.ResumeLayout(false);
            this.menuStrip_Main.ResumeLayout(false);
            this.menuStrip_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel panel_Top;
        private System.Windows.Forms.Label label_Directory;
        private System.Windows.Forms.Panel panel_AddressBar;
        private System.Windows.Forms.Label label_DirectoryPath;
        private System.Windows.Forms.Button button_Refresh;
        private System.Windows.Forms.Button button_BrowseDirectory;
        private System.Windows.Forms.Panel panel_Bottom;
        private System.Windows.Forms.Label label_Status;
        private System.Windows.Forms.Button button_UseLatest;
        private System.Windows.Forms.Button button_ExportImage;
        private System.Windows.Forms.Button button_ReplaceImage;
        private System.Windows.Forms.Panel panel_Content;
        private System.Windows.Forms.SplitContainer splitContainer_Main;
        private System.Windows.Forms.Panel panel_Left;
        private System.Windows.Forms.Label label_Files;
        private System.Windows.Forms.ComboBox comboBox_Filter;
        private System.Windows.Forms.ListBox listBox_ITC2Files;
        private System.Windows.Forms.CheckBox checkBox_HideEmpty;
        private System.Windows.Forms.Panel panel_Right;
        private System.Windows.Forms.Label label_Details;
        private System.Windows.Forms.Label label_FileInfo;
        private System.Windows.Forms.PictureBox pictureBox_Preview;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_Main;
        private System.Windows.Forms.OpenFileDialog openFileDialog_Image;
        private System.Windows.Forms.MenuStrip menuStrip_Main;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exportAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useLatestAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteImageToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox_Search;
        private System.Windows.Forms.ContextMenuStrip contextMenu_FileList;
        private System.Windows.Forms.ToolStripMenuItem menuItem_ShowInExplorer;
        private System.Windows.Forms.ToolStripMenuItem menuItem_DeleteFromLibrary;
    }
}
