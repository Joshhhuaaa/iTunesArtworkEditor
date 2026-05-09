# <img src="iTunesArtworkEditor\icon.ico" width="32"> iTunes Artwork Editor
iTunes Artwork Editor is a utility for managing artwork stored in Apple's proprietary `.itc2` artwork format.

## Usage
- Run the executable, `iTunesArtworkEditor.exe`.
- Click `Browse…` and select your iTunes **Album Artwork** folder. It will begin automatically retrieving artist names using the iTunes Search API.
- Select an artist to view their artwork. You can extract, replace, or download the latest artwork from Apple. Press `File` -> `Save` or `Ctrl+S` to apply changes.
  - Artworks from albums in `.itc2` format may also appear. You can extract or replace their embedded artwork, but the album name currently doesn't get retrieved using the iTunes Search API.

> [!IMPORTANT]
> It is strongly recommended to export all artwork before modifying any `.itc2` files. Original artwork may be lost permanently if Apple's servers have updated it and you have an older version. To export, go to `File` -> `Export All Artwork…`, and choose a folder to save the files.

> [!NOTE]
> If iTunes was running, you must restart iTunes to see the applied artwork changes.

> [!TIP]
> If you would like to enable portable mode where settings are stored alongside the executable, create a `portable.txt` in the same directory. By default, `settings.json` is stored in `%LocalAppData%\iTunesArtworkEditor\settings.json`.