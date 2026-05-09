using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iTunesArtworkEditor
{
    public partial class UseLatestProgressForm : Form
    {
        private readonly List<(string filePath, string artistId, string displayName)> _artists;
        private readonly Func<string, Task<byte[]?>> _fetchImage;
        private readonly CancellationTokenSource _cts = new();
        private bool _isRunning;

        public int UpdatedCount { get; private set; }
        public List<string> Failures { get; private set; } = new();

        public UseLatestProgressForm(
            List<(string filePath, string artistId, string displayName)> artists,
            Func<string, Task<byte[]?>> fetchImage)
        {
            InitializeComponent();
            _artists = artists;
            _fetchImage = fetchImage;
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _isRunning = true;
            await RunAsync();
            _isRunning = false;
            Close();
        }

        private async Task RunAsync()
        {
            progressBar_Main.Maximum = _artists.Count;
            progressBar_Main.Value = 0;
            label_Count.Text = $"0 / {_artists.Count}";

            for (int i = 0; i < _artists.Count; i++)
            {
                if (_cts.Token.IsCancellationRequested) break;

                var (filePath, artistId, displayName) = _artists[i];
                label_Current.Text = displayName;

                try
                {
                    byte[]? imageData = await _fetchImage(artistId);
                    if (imageData != null && imageData.Length > 0)
                    {
                        ITC2FileHandler.ReplaceImageBytes(filePath, imageData);
                        UpdatedCount++;
                    }
                    else
                    {
                        Failures.Add($"{displayName}: no image found");
                    }
                }
                catch (Exception ex)
                {
                    Failures.Add($"{displayName}: {ex.Message}");
                }

                progressBar_Main.Value = i + 1;
                label_Count.Text = $"{i + 1} / {_artists.Count}";
            }
        }

        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            _cts.Cancel();
            button_Cancel.Enabled = false;
            label_Current.Text = "Cancelling...";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_isRunning)
            {
                _cts.Cancel();
                e.Cancel = true;
                button_Cancel.Enabled = false;
                label_Current.Text = "Cancelling...";
                return;
            }
            base.OnFormClosing(e);
        }
    }
}
