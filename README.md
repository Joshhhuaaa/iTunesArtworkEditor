# <img src="iTunesArtworkEditor\icon.ico" width="32"> iTunes Artwork Editor
iTunes Artwork Editor is a utility for managing artwork stored in Apple's proprietary `.itc2` artwork format.

## Usage
- Run the executable, `iTunesArtworkEditor.exe`.
- Click `Browse…` and select your iTunes **Album Artwork** folder. It will begin automatically retrieving artist names using the iTunes Search API.
- Select an artist to view their artwork. You can extract, replace, or download the latest artwork from Apple. Press `File` > `Save` or `Ctrl+S` to apply changes.
- If iTunes is running, you must restart iTunes to see the applied artwork changes.

> [!IMPORTANT]
> It is strongly recommended to export all artwork before modifying any `.itc2` files. Original artwork may be lost permanently if Apple's servers have updated it and you have an older version. To export, go to `File` > `Export All Artwork…`, and choose a folder to save the artwork.
>
> <img width="260" height="178" alt="Export All Artwork" src="https://github.com/user-attachments/assets/f55d3ed2-f75f-42cc-9697-65453d435fb1"/>

> [!NOTE]
> Artworks from albums in `.itc2` format may also appear. You can extract or replace their embedded artwork, but the album name currently doesn't get retrieved using the iTunes Search API.

### Portable Mode
To enable portable mode, create a `portable.txt` file in the same directory as the executable. In portable mode, the `settings.json` file is stored alongside the executable. By default, `settings.json` is stored at `%LocalAppData%\iTunesArtworkEditor\settings.json`.

The settings file stores the last used iTunes Album Artwork directory and caches Artist IDs previously retrieved from the iTunes Search API to reduce lookup time on future launches.
