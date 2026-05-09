using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iTunesArtworkEditor
{
    public partial class MainForm : Form
    {
        private List<string> allValidFiles = new List<string>();
        private List<string> currentITC2Files = new List<string>();
        private string? selectedFilePath = null;
        private byte[]? _pendingImageData = null;
        private static readonly System.Net.Http.HttpClient _httpClient = new();
        private readonly Dictionary<string, string> _artistNameCache = new();
        private CancellationTokenSource? _lookupCts;
        private bool _suppressSelectionChange;
        private List<ITC2FileInfo> _displayedFiles = new();
        private AppSettings _settings = new();

        public MainForm()
        {
            InitializeComponent();
            // Segoe MDL2 Assets U+E72C = Refresh icon
            button_Refresh.Text = "";
            LoadAppIcon();
        }

        private void LoadAppIcon()
        {
            using var stream = typeof(MainForm).Assembly
                .GetManifestResourceStream("iTunesArtworkEditor.icon.ico");
            if (stream != null)
                Icon = new System.Drawing.Icon(stream);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _settings = SettingsManager.Load();

            foreach (var kvp in _settings.ArtistNameCache)
                _artistNameCache[kvp.Key] = kvp.Value;

            string? dirToOpen = null;

            if (!string.IsNullOrEmpty(_settings.LastDirectory)
                && Directory.Exists(_settings.LastDirectory))
            {
                dirToOpen = _settings.LastDirectory;
            }
            else
            {
                string defaultPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Apple Computer", "iTunes", "Artwork Cache");
                if (Directory.Exists(defaultPath))
                    dirToOpen = defaultPath;
            }

            if (dirToOpen != null)
            {
                label_DirectoryPath.Text = dirToOpen;
                ScanDirectory(dirToOpen);
            }
            else
            {
                UpdateStatus("Ready");
            }
        }

        private void OpenDirectory(string path)
        {
            label_DirectoryPath.Text = path;
            _settings.LastDirectory = path;
            ScanDirectory(path);
        }

        private void Button_BrowseDirectory_Click(object sender, EventArgs e)
        {
            folderBrowserDialog_Main.SelectedPath = label_DirectoryPath.Text;

            if (folderBrowserDialog_Main.ShowDialog() == DialogResult.OK)
                OpenDirectory(folderBrowserDialog_Main.SelectedPath);
        }

        private void Button_Refresh_Click(object sender, EventArgs e)
        {
            ScanDirectory(label_DirectoryPath.Text);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                ScanDirectory(label_DirectoryPath.Text);
            else if (e.KeyCode == Keys.Delete
                     && selectedFilePath != null
                     && !(ActiveControl is TextBox))
                MenuItem_DeleteFromLibrary_Click(sender, e);
        }

        private void ScanDirectory(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
            {
                if (!string.IsNullOrWhiteSpace(directoryPath))
                    MessageBox.Show("Please select a valid directory.", "Invalid Directory",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UpdateStatus("Ready");
                return;
            }

            try
            {
                UpdateStatus("Scanning directory for .itc2 files...");
                Application.DoEvents();

                currentITC2Files = ITC2FileHandler.ScanDirectoryForITC2Files(directoryPath);

                var validFiles = currentITC2Files
                    .Where(f => ITC2FileHandler.IsValidITC2File(f))
                    .OrderBy(f => Path.GetFileName(f))
                    .ToList();

                allValidFiles = validFiles;
                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error scanning directory: {ex.Message}", "Scan Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Error scanning directory");
            }
        }

        private void ApplyFilter()
        {
            var infos = allValidFiles
                .Select(f => ITC2FileHandler.GetFileInfo(f))
                .ToList();

            var filesWithImages = infos
                .Where(i => i.ImageType != "Unknown")
                .Select(i => i.FilePath)
                .ToList();

            exportAllToolStripMenuItem.Enabled = filesWithImages.Count > 0;
            useLatestAllToolStripMenuItem.Enabled = infos.Any(i => i.ArtworkKind == "Artist" && i.ImageType != "Unknown");

            var displayed = checkBox_HideEmpty.Checked
                ? infos.Where(i => i.ImageType != "Unknown")
                : infos.AsEnumerable();

            string kindFilter = comboBox_Filter.SelectedItem?.ToString() ?? "All Types";
            if (kindFilter != "All Types")
                displayed = displayed.Where(i => i.ArtworkKind == kindFilter);

            PopulateFileList(displayed.ToList());

            UpdateStatus("Ready");
        }

        private void CheckBox_HideEmpty_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ComboBox_Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void PopulateFileList(List<ITC2FileInfo> files)
        {
            _displayedFiles = files;
            _lookupCts?.Cancel();
            _lookupCts = new CancellationTokenSource();

            var toFetch = files
                .Where(f => f.ArtworkKind == "Artist")
                .Select(f => Path.GetFileNameWithoutExtension(f.FilePath))
                .Where(id => !_artistNameCache.ContainsKey(id))
                .Distinct()
                .ToList();

            if (toFetch.Count == 0)
            {
                SortAndRepopulate(files);
                return;
            }

            // Show unsorted initial list (with any already-cached names) while fetch runs
            _suppressSelectionChange = true;
            listBox_ITC2Files.BeginUpdate();
            listBox_ITC2Files.Items.Clear();
            currentITC2Files = files.Select(f => f.FilePath).ToList();
            foreach (var file in files)
            {
                string id = Path.GetFileNameWithoutExtension(file.FilePath);
                string displayName = Path.GetFileName(file.FilePath);
                if (file.ArtworkKind == "Artist"
                    && _artistNameCache.TryGetValue(id, out string? cached)
                    && !string.IsNullOrEmpty(cached))
                    displayName = cached;
                listBox_ITC2Files.Items.Add(displayName);
            }
            listBox_ITC2Files.EndUpdate();
            label_Files.Text = $"Files ({files.Count})";
            _suppressSelectionChange = false;

            if (files.Count > 0)
                listBox_ITC2Files.SelectedIndex = 0;
            else
                ClearPreview();

            _ = FetchAndSortAsync(files, toFetch, _lookupCts.Token);
        }

        private async Task FetchAndSortAsync(List<ITC2FileInfo> allFiles, List<string> idsToFetch, CancellationToken token)
        {
            listBox_ITC2Files.Enabled = false;
            int total = idsToFetch.Count;
            int done = 0;
            bool cancelled = false;
            UpdateStatus($"Fetching artist names (0/{total})...");

            foreach (string id in idsToFetch)
            {
                if (token.IsCancellationRequested) { cancelled = true; break; }

                try
                {
                    string json = await _httpClient.GetStringAsync(
                        $"https://itunes.apple.com/lookup?id={id}", token);
                    string? name = null;
                    using var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("results", out var results))
                        foreach (var result in results.EnumerateArray())
                            if (result.TryGetProperty("artistName", out var nameProp))
                            { name = nameProp.GetString(); break; }
                    _artistNameCache[id] = name ?? "";
                }
                catch (OperationCanceledException) { cancelled = true; break; }
                catch { _artistNameCache[id] = ""; }

                done++;
                UpdateStatus($"Fetching artist names ({done}/{total})...");
            }

            if (!cancelled)
            {
                SortAndRepopulate(allFiles, selectFirst: true);
                listBox_ITC2Files.Enabled = true;
                UpdateStatus("Ready");
            }
            // If cancelled, the new PopulateFileList call owns re-enabling the listbox
        }

        private void SortAndRepopulate(List<ITC2FileInfo> files, bool selectFirst = false)
        {
            var sorted = files.Select(f =>
            {
                string id = Path.GetFileNameWithoutExtension(f.FilePath);
                string displayName = Path.GetFileName(f.FilePath);
                if (f.ArtworkKind == "Artist"
                    && _artistNameCache.TryGetValue(id, out string? cached)
                    && !string.IsNullOrEmpty(cached))
                    displayName = cached;
                return (info: f, displayName);
            })
            .OrderBy(x => ArtistSortCategory(x.displayName))
            .ThenBy(x => ArtistSortKey(x.displayName), StringComparer.OrdinalIgnoreCase)
            .ToList();

            string search = textBox_Search.Text.Trim();
            if (!string.IsNullOrEmpty(search))
                sorted = sorted.Where(x => x.displayName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            _suppressSelectionChange = true;
            listBox_ITC2Files.BeginUpdate();
            listBox_ITC2Files.Items.Clear();
            currentITC2Files = sorted.Select(x => x.info.FilePath).ToList();
            foreach (var item in sorted)
                listBox_ITC2Files.Items.Add(item.displayName);
            listBox_ITC2Files.EndUpdate();
            label_Files.Text = $"Files ({sorted.Count})";
            _suppressSelectionChange = false;

            int idx = (!selectFirst && selectedFilePath != null) ? currentITC2Files.IndexOf(selectedFilePath) : -1;
            if (idx < 0 && currentITC2Files.Count > 0) idx = 0;

            if (idx >= 0)
                listBox_ITC2Files.SelectedIndex = idx;
            else
                ClearPreview();
        }

        private static string ArtistSortKey(string displayName)
        {
            if (displayName.StartsWith("The ", StringComparison.OrdinalIgnoreCase) && displayName.Length > 4)
                return displayName.Substring(4);
            return displayName;
        }

        private static int ArtistSortCategory(string displayName)
        {
            if (string.IsNullOrEmpty(displayName)) return 3;
            if (displayName.EndsWith(".itc2", StringComparison.OrdinalIgnoreCase)) return 3; // Unresolved filename
            if (char.IsLetter(displayName[0])) return 1; // A-Z
            if (char.IsDigit(displayName[0])) return 2; // 0-9
            return 0; // Symbols (¥$, etc.) sort before A
        }

        private void ListBox_ITC2Files_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressSelectionChange) return;

            if (_pendingImageData != null)
            {
                var answer = MessageBox.Show("Do you want to save changes?", "Unsaved Changes",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer == DialogResult.Yes && selectedFilePath != null)
                {
                    try { ITC2FileHandler.ReplaceImageBytes(selectedFilePath, _pendingImageData); }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving: {ex.Message}", "Save Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                _pendingImageData = null;
                saveToolStripMenuItem.Enabled = false;
            }

            if (listBox_ITC2Files.SelectedIndex < 0 || listBox_ITC2Files.SelectedIndex >= currentITC2Files.Count)
            {
                ClearPreview();
                return;
            }

            selectedFilePath = currentITC2Files[listBox_ITC2Files.SelectedIndex];
            DisplayFilePreview(selectedFilePath);
        }

        private void DisplayFilePreview(string filePath)
        {
            try
            {
                // Get file info
                var fileInfo = ITC2FileHandler.GetFileInfo(filePath);

                if (!fileInfo.IsValid)
                {
                    label_FileInfo.Text = "Invalid .itc2 file";
                    pictureBox_Preview.Image = null;
                    button_ReplaceImage.Enabled = false;
                    button_UseLatest.Enabled = false;
                    return;
                }

                // Extract and display image
                Bitmap image = ITC2FileHandler.ExtractImage(filePath);

                label_FileInfo.Text = $"File: {fileInfo.FileName}\n" +
                                      $"Size: {FormatFileSize(fileInfo.FileSize)}\n" +
                                      $"Image: {fileInfo.ImageType} ({FormatFileSize(fileInfo.ImageSize)})\n" +
                                      $"Resolution: {image.Width} × {image.Height}";

                if (pictureBox_Preview.Image != null)
                {
                    pictureBox_Preview.Image.Dispose();
                }
                pictureBox_Preview.Image = image;

                button_ReplaceImage.Enabled = true;
                button_ExportImage.Enabled = true;
                button_UseLatest.Enabled = fileInfo.ArtworkKind == "Artist";
                UpdateStatus("Ready");
            }
            catch (Exception ex)
            {
                label_FileInfo.Text = $"Error loading file:\n{ex.Message}";
                pictureBox_Preview.Image = null;
                button_ReplaceImage.Enabled = false;
                UpdateStatus($"Error: {ex.Message}");
            }
        }

        private void ClearPreview()
        {
            _pendingImageData = null;
            saveToolStripMenuItem.Enabled = false;
            label_FileInfo.Text = "Select a file to view details...";
            if (pictureBox_Preview.Image != null)
            {
                pictureBox_Preview.Image.Dispose();
                pictureBox_Preview.Image = null;
            }
            button_ReplaceImage.Enabled = false;
            button_ExportImage.Enabled = false;
            button_UseLatest.Enabled = false;
            selectedFilePath = null;
        }

        private void Button_ReplaceImage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectedFilePath) || !File.Exists(selectedFilePath))
            {
                MessageBox.Show("No file selected.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (openFileDialog_Image.ShowDialog() != DialogResult.OK)
                return;

            string newImagePath = openFileDialog_Image.FileName;

            // Validate image
            try
            {
                byte[] imageData = File.ReadAllBytes(newImagePath);

                if (ITC2FileHandler.DetectImageType(imageData) == "Unknown")
                {
                    MessageBox.Show("Selected file is not a valid JPEG or PNG image.", "Invalid Image",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int imgWidth, imgHeight;
                using (var ms = new MemoryStream(imageData))
                using (var img = System.Drawing.Image.FromStream(ms))
                { imgWidth = img.Width; imgHeight = img.Height; }

                if (imgWidth < 176 || imgHeight < 176)
                {
                    if (MessageBox.Show(
                            $"This artwork is {imgWidth}×{imgHeight}, smaller than the 176×176 size Apple uses.\n\nIt may appear blurry. Use it anyway?",
                            "Low Resolution", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                        return;
                }

                if (imgWidth != imgHeight)
                {
                    if (MessageBox.Show(
                            $"This artwork is {imgWidth}×{imgHeight} and is not square (1:1 aspect ratio).\n\niTunes uses square artwork. Use it anyway?",
                            "Non-Square Aspect Ratio", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                        return;
                }

                StageImage(imageData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error replacing image: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus($"Error: {ex.Message}");
            }
        }

        private async void Button_UseLatest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectedFilePath)) return;

            string artistId = Path.GetFileNameWithoutExtension(selectedFilePath);
            button_UseLatest.Enabled = false;
            UpdateStatus("Fetching latest artist image...");

            try
            {
                byte[]? imageData = await FetchLatestArtistImageAsync(artistId);

                if (imageData == null || imageData.Length == 0)
                {
                    MessageBox.Show("No image found for this artist on Apple Music.", "Not Found",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateStatus("Ready");
                    return;
                }

                StageImage(imageData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching image: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Ready");
            }
            finally
            {
                if (selectedFilePath != null)
                    button_UseLatest.Enabled = true;
            }
        }

        private async Task<byte[]?> FetchLatestArtistImageAsync(string artistId)
        {
            // The iTunes Search API doesn't expose artist profile photos.
            // Apple Music server-renders an og:image meta tag on the artist page
            // containing the actual artist profile image used in the store.
            using var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://music.apple.com/us/artist/{artistId}");
            request.Headers.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string html = await response.Content.ReadAsStringAsync();

            string? artworkUrl = null;
            int ogPos = html.IndexOf("og:image", StringComparison.OrdinalIgnoreCase);
            if (ogPos >= 0)
            {
                int searchLen = Math.Min(300, html.Length - ogPos);
                int cIdx = html.IndexOf("content=\"", ogPos, searchLen, StringComparison.OrdinalIgnoreCase);
                if (cIdx >= 0)
                {
                    int start = cIdx + 9;
                    int end = html.IndexOf('"', start);
                    if (end > start)
                    {
                        string candidate = System.Net.WebUtility.HtmlDecode(html[start..end]);
                        if (candidate.StartsWith("https://") && candidate.Contains("mzstatic"))
                            artworkUrl = candidate;
                    }
                }
            }

            if (string.IsNullOrEmpty(artworkUrl)) return null;

            // Scale to highest quality by replacing the size token in the URL.
            int dot = artworkUrl.LastIndexOf('.');
            int slash = artworkUrl.LastIndexOf('/', dot);
            if (dot > 0 && slash > 0)
                artworkUrl = artworkUrl[..(slash + 1)] + "3000x3000bb" + artworkUrl[dot..];

            return await _httpClient.GetByteArrayAsync(artworkUrl);
        }

        private void Button_ExportImage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectedFilePath) || !File.Exists(selectedFilePath))
            {
                MessageBox.Show("No file selected.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var fileInfo = ITC2FileHandler.GetFileInfo(selectedFilePath);
                string ext = fileInfo.ImageType == "PNG" ? "png" : "jpg";
                string baseName = Path.GetFileNameWithoutExtension(selectedFilePath);

                using var dlg = new SaveFileDialog
                {
                    Title = "Export Artwork",
                    FileName = $"{baseName}.{ext}",
                    Filter = fileInfo.ImageType == "PNG"
                        ? "PNG Image (*.png)|*.png|All files (*.*)|*.*"
                        : "JPEG Image (*.jpg)|*.jpg;*.jpeg|All files (*.*)|*.*",
                    DefaultExt = ext
                };

                if (dlg.ShowDialog() != DialogResult.OK) return;

                byte[] imageBytes = ITC2FileHandler.GetImageBytes(selectedFilePath);
                File.WriteAllBytes(dlg.FileName, imageBytes);

                UpdateStatus("Ready");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting artwork: {ex.Message}", "Export Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus($"Error: {ex.Message}");
            }
        }

        private void ExportAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var exportFiles = allValidFiles
                .Where(f => ITC2FileHandler.GetFileInfo(f).ImageType != "Unknown")
                .ToList();

            if (exportFiles.Count == 0) return;

            using var dlg = new FolderBrowserDialog
            {
                Description = "Select a folder to export all artwork into",
                UseDescriptionForTitle = true
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            string outputDir = dlg.SelectedPath;
            int exported = 0;
            int skipped = 0;
            var errors = new List<string>();

            UpdateStatus("Exporting artwork...");
            Application.DoEvents();

            foreach (string filePath in exportFiles)
            {
                try
                {
                    var fileInfo = ITC2FileHandler.GetFileInfo(filePath);
                    string ext = fileInfo.ImageType == "PNG" ? "png" : "jpg";
                    string baseName = Path.GetFileNameWithoutExtension(filePath);
                    string destPath = Path.Combine(outputDir, $"{baseName}.{ext}");

                    int counter = 1;
                    while (File.Exists(destPath))
                    {
                        destPath = Path.Combine(outputDir, $"{baseName}_{counter}.{ext}");
                        counter++;
                    }

                    byte[] imageBytes = ITC2FileHandler.GetImageBytes(filePath);
                    File.WriteAllBytes(destPath, imageBytes);
                    exported++;
                }
                catch (Exception ex)
                {
                    errors.Add($"{Path.GetFileName(filePath)}: {ex.Message}");
                    skipped++;
                }
            }

            UpdateStatus("Ready");

            string summary = $"Exported {exported} image{(exported != 1 ? "s" : "")} to:\n{outputDir}";
            if (errors.Count > 0)
                summary += $"\n\n{skipped} file{(skipped != 1 ? "s" : "")} failed:\n" + string.Join("\n", errors);

            MessageBox.Show(summary, "Export Complete",
                MessageBoxButtons.OK, errors.Count > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SavePendingChanges();
        }

        private void OpenFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog_Main.SelectedPath = label_DirectoryPath.Text;

            if (folderBrowserDialog_Main.ShowDialog() == DialogResult.OK)
                OpenDirectory(folderBrowserDialog_Main.SelectedPath);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _settings.ArtistNameCache = new Dictionary<string, string>(_artistNameCache);
            SettingsManager.Save(_settings);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SavePendingChanges()
        {
            if (_pendingImageData == null || selectedFilePath == null) return;

            try
            {
                ITC2FileHandler.ReplaceImageBytes(selectedFilePath, _pendingImageData);
                _pendingImageData = null;
                saveToolStripMenuItem.Enabled = false;
                DisplayFilePreview(selectedFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving: {ex.Message}", "Save Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ListBox_ITC2Files_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int idx = listBox_ITC2Files.IndexFromPoint(e.Location);
                if (idx >= 0)
                    listBox_ITC2Files.SelectedIndex = idx;
            }
        }

        private void ContextMenu_FileList_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool hasFile = selectedFilePath != null && File.Exists(selectedFilePath);
            menuItem_ShowInExplorer.Enabled = hasFile;
            menuItem_DeleteFromLibrary.Enabled = hasFile;
        }

        private void MenuItem_ShowInExplorer_Click(object sender, EventArgs e)
        {
            if (selectedFilePath == null || !File.Exists(selectedFilePath)) return;
            Process.Start("explorer.exe", $"/select,\"{selectedFilePath}\"");
        }

        private void MenuItem_DeleteFromLibrary_Click(object sender, EventArgs e)
        {
            if (selectedFilePath == null || !File.Exists(selectedFilePath)) return;

            string fileName = Path.GetFileName(selectedFilePath);
            if (MessageBox.Show(
                    $"Delete \"{fileName}\" from your iTunes library?\n\nThis cannot be undone.",
                    "Delete From Library",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                File.Delete(selectedFilePath);
                allValidFiles.Remove(selectedFilePath);
                _pendingImageData = null;
                saveToolStripMenuItem.Enabled = false;
                selectedFilePath = null;
                ApplyFilter();
                UpdateStatus("File deleted");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting file: {ex.Message}", "Delete Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StageImage(byte[] imageData)
        {
            string imageType = ITC2FileHandler.DetectImageType(imageData);
            int imgWidth, imgHeight;
            using (var ms = new MemoryStream(imageData))
            using (var img = System.Drawing.Image.FromStream(ms))
            { imgWidth = img.Width; imgHeight = img.Height; }

            _pendingImageData = imageData;
            using (var ms = new MemoryStream(imageData))
            {
                var bmp = new Bitmap(ms);
                pictureBox_Preview.Image?.Dispose();
                pictureBox_Preview.Image = bmp;
            }
            var fi = ITC2FileHandler.GetFileInfo(selectedFilePath!);
            label_FileInfo.Text = $"File: {fi.FileName}\n" +
                                  $"Size: {FormatFileSize(fi.FileSize)}\n" +
                                  $"Image: {imageType} ({FormatFileSize(imageData.Length)})\n" +
                                  $"Resolution: {imgWidth} × {imgHeight}";
            saveToolStripMenuItem.Enabled = true;
            UpdateStatus("Unsaved changes");
        }

        private void TextBox_Search_TextChanged(object sender, EventArgs e)
        {
            if (_displayedFiles.Count > 0)
                SortAndRepopulate(_displayedFiles);
        }

        private void PictureBox_Preview_DragEnter(object sender, DragEventArgs e)
        {
            if (selectedFilePath == null || !e.Data!.GetDataPresent(DataFormats.FileDrop))
            { e.Effect = DragDropEffects.None; return; }

            var files = (string[])e.Data.GetData(DataFormats.FileDrop)!;
            e.Effect = files.Any(f => new[] { ".jpg", ".jpeg", ".png" }
                .Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase))
                ? DragDropEffects.Copy
                : DragDropEffects.None;
        }

        private void PictureBox_Preview_DragDrop(object sender, DragEventArgs e)
        {
            if (selectedFilePath == null) return;
            var files = (string[])e.Data!.GetData(DataFormats.FileDrop)!;
            string? imagePath = files.FirstOrDefault(f => new[] { ".jpg", ".jpeg", ".png" }
                .Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase));
            if (imagePath == null) return;

            try
            {
                byte[] imageData = File.ReadAllBytes(imagePath);
                if (ITC2FileHandler.DetectImageType(imageData) == "Unknown")
                {
                    MessageBox.Show("This file is not a valid JPEG or PNG image.", "Invalid Image",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int w, h;
                using (var ms = new MemoryStream(imageData))
                using (var img = System.Drawing.Image.FromStream(ms))
                { w = img.Width; h = img.Height; }

                if (w < 176 || h < 176)
                    if (MessageBox.Show($"This artwork is {w}×{h}, smaller than the 176×176 size Apple uses.\n\nIt may appear blurry. Use it anyway?",
                            "Low Resolution", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                if (w != h)
                    if (MessageBox.Show($"This artwork is {w}×{h} and is not square (1:1 aspect ratio).\n\niTunes uses square artwork. Use it anyway?",
                            "Non-Square Aspect Ratio", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                StageImage(imageData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading artwork: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            copyImageToolStripMenuItem.Enabled = pictureBox_Preview.Image != null;
            pasteImageToolStripMenuItem.Enabled = selectedFilePath != null
                && (Clipboard.ContainsImage() || Clipboard.ContainsFileDropList());
        }

        private void CopyImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox_Preview.Image == null) return;
            Clipboard.SetImage(pictureBox_Preview.Image);
            UpdateStatus("Artwork copied to clipboard");
        }

        private void PasteImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedFilePath == null) return;
            PasteFromClipboard();
        }

        private void PasteFromClipboard()
        {
            if (selectedFilePath == null) return;
            try
            {
                byte[]? imageData = null;

                if (Clipboard.ContainsFileDropList())
                {
                    string[] imageExts = { ".jpg", ".jpeg", ".png" };
                    foreach (string f in Clipboard.GetFileDropList())
                    {
                        if (imageExts.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase))
                        { imageData = File.ReadAllBytes(f); break; }
                    }
                }
                else if (Clipboard.ContainsImage())
                {
                    using var clipImg = Clipboard.GetImage();
                    if (clipImg != null)
                    {
                        using var ms = new MemoryStream();
                        clipImg.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imageData = ms.ToArray();
                    }
                }

                if (imageData == null || imageData.Length == 0) return;

                int w, h;
                using (var ms = new MemoryStream(imageData))
                using (var img = System.Drawing.Image.FromStream(ms))
                { w = img.Width; h = img.Height; }

                if (w < 176 || h < 176)
                    if (MessageBox.Show($"This artwork is {w}×{h}, smaller than the 176×176 size Apple uses.\n\nIt may appear blurry. Use it anyway?",
                            "Low Resolution", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                if (w != h)
                    if (MessageBox.Show($"This artwork is {w}×{h} and is not square (1:1 aspect ratio).\n\niTunes uses square artwork. Use it anyway?",
                            "Non-Square Aspect Ratio", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                StageImage(imageData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error pasting image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UseLatestAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var artists = allValidFiles
                .Where(f => ITC2FileHandler.GetFileInfo(f).ArtworkKind == "Artist" && File.Exists(f))
                .Select(f =>
                {
                    string id = Path.GetFileNameWithoutExtension(f);
                    string name = _artistNameCache.TryGetValue(id, out string? n) && !string.IsNullOrEmpty(n) ? n : id;
                    return (filePath: f, artistId: id, displayName: name);
                })
                .ToList();

            if (artists.Count == 0) return;

            if (MessageBox.Show(
                    $"Fetch and overwrite artwork for {artists.Count} artist{(artists.Count != 1 ? "s" : "")}?\n\nArtwork will be saved immediately.",
                    "Use Latest Artwork for All Artists",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            using var progressForm = new UseLatestProgressForm(artists, FetchLatestArtistImageAsync);
            progressForm.ShowDialog(this);

            int updated = progressForm.UpdatedCount;
            var failures = progressForm.Failures;

            if (selectedFilePath != null)
            {
                var fi = ITC2FileHandler.GetFileInfo(selectedFilePath);
                button_UseLatest.Enabled = fi.ArtworkKind == "Artist";
                DisplayFilePreview(selectedFilePath);
            }

            string summary = $"Updated {updated} artist{(updated != 1 ? "s" : "")}.";
            if (failures.Count > 0)
                summary += $"\n\n{failures.Count} failed:\n" + string.Join("\n", failures.Take(10));
            if (failures.Count > 10)
                summary += $"\n...and {failures.Count - 10} more";

            UpdateStatus("Ready");
            MessageBox.Show(summary, "Use Latest Artwork for All Artists",
                MessageBoxButtons.OK, failures.Count > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }

        private void UpdateStatus(string message)
        {
            label_Status.Text = message;
        }

        private string FormatFileSize(long bytes)
        {
            if (bytes <= 0) return "0 B";

            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }
}
