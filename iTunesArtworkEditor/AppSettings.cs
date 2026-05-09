using System.Collections.Generic;

namespace iTunesArtworkEditor
{
    internal sealed class AppSettings
    {
        public string? LastDirectory { get; set; }
        public Dictionary<string, string> ArtistNameCache { get; set; } = new();
    }
}
