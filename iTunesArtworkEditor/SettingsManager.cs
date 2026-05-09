using System;
using System.IO;
using System.Text.Json;

namespace iTunesArtworkEditor
{
    internal static class SettingsManager
    {
        private static readonly string SettingsDir = ResolveSettingsDir();
        private static readonly string SettingsPath = Path.Combine(SettingsDir, "settings.json");
        private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

        private static string ResolveSettingsDir()
        {
            string exeDir = Path.GetDirectoryName(Environment.ProcessPath) ?? AppContext.BaseDirectory;
            if (File.Exists(Path.Combine(exeDir, "portable.txt")))
                return exeDir;
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "iTunesArtworkEditor");
        }

        public static AppSettings Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    var settings = JsonSerializer.Deserialize<AppSettings>(json, _jsonOptions);
                    if (settings != null)
                    {
                        settings.ArtistNameCache ??= new();
                        return settings;
                    }
                }
            }
            catch { }
            return new AppSettings();
        }

        public static void Save(AppSettings settings)
        {
            try
            {
                Directory.CreateDirectory(SettingsDir);
                string json = JsonSerializer.Serialize(settings, _jsonOptions);
                File.WriteAllText(SettingsPath, json);
            }
            catch { }
        }
    }
}
