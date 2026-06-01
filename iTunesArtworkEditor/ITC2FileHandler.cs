using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace iTunesArtworkEditor
{
    /// <summary>
    /// Handles reading, extracting, and replacing images in .itc2 files.
    /// iTunes uses .itc2 files to store artist artwork in a proprietary binary format.
    /// </summary>
    public class ITC2FileHandler
    {
        // Fixed offsets in .itc2 format
        private const uint HEADER_SIZE = 0x11C;        // 284 bytes
        private const uint IMAGE_DATA_OFFSET = 0x01E0; // 480 bytes - where image data starts
        private const uint DATA_MARKER_OFFSET = 0x01DC; // "data" marker location
        private const uint KIND_MARKER_OFFSET = 0x148;  // 4-byte kind tag: "stor"=artist, "CLPU"=album
        private const string MAGIC_BYTES = "itch";
        private const string DATA_MARKER = "data";

        /// <summary>
        /// Validates that a file is a valid .itc2 file.
        /// </summary>
        public static bool IsValidITC2File(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return false;

                byte[] data = File.ReadAllBytes(filePath);
                return ValidateITC2Data(data);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validates the binary structure of .itc2 data.
        /// </summary>
        private static bool ValidateITC2Data(byte[] data)
        {
            if (data.Length < IMAGE_DATA_OFFSET)
                return false;

            // Check magic bytes
            string magic = System.Text.Encoding.ASCII.GetString(data, 4, 4);
            if (magic != MAGIC_BYTES)
                return false;

            // Check data marker
            string marker = System.Text.Encoding.ASCII.GetString(data, (int)DATA_MARKER_OFFSET, 4);
            if (marker != DATA_MARKER)
                return false;

            return true;
        }

        /// <summary>
        /// Extracts the image from an .itc2 file and returns it as a Bitmap.
        /// Supports both JPEG and PNG formats.
        /// </summary>
        public static Bitmap ExtractImage(string itc2FilePath)
        {
            if (!File.Exists(itc2FilePath))
                throw new FileNotFoundException($"File not found: {itc2FilePath}");

            byte[] data = File.ReadAllBytes(itc2FilePath);

            if (!ValidateITC2Data(data))
                throw new InvalidOperationException("File is not a valid .itc2 file");

            // Image data starts at offset 0x01E0 and goes to the end of file
            int imageDataLength = data.Length - (int)IMAGE_DATA_OFFSET;
            if (imageDataLength <= 0)
                throw new InvalidOperationException("No image data found in .itc2 file");

            byte[] imageData = new byte[imageDataLength];
            Array.Copy(data, IMAGE_DATA_OFFSET, imageData, 0, imageDataLength);

            // Convert to Bitmap
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                return new Bitmap(ms);
            }
        }

        /// <summary>
        /// Returns the raw image bytes from an .itc2 file without decoding.
        /// </summary>
        public static byte[] GetImageBytes(string itc2FilePath)
        {
            byte[] data = File.ReadAllBytes(itc2FilePath);
            if (!ValidateITC2Data(data))
                throw new InvalidOperationException("File is not a valid .itc2 file");
            int length = data.Length - (int)IMAGE_DATA_OFFSET;
            if (length <= 0)
                throw new InvalidOperationException("No image data found in .itc2 file");
            byte[] imageData = new byte[length];
            Array.Copy(data, IMAGE_DATA_OFFSET, imageData, 0, length);
            return imageData;
        }

        /// <summary>
        /// Replaces the image in an .itc2 file with a new image.
        /// Supports JPEG and PNG formats.
        /// </summary>
        public static void ReplaceImage(string itc2FilePath, string newImagePath)
        {
            if (!File.Exists(itc2FilePath))
                throw new FileNotFoundException($".itc2 file not found: {itc2FilePath}");

            if (!File.Exists(newImagePath))
                throw new FileNotFoundException($"Image file not found: {newImagePath}");

            // Read original .itc2 file
            byte[] itc2Data = File.ReadAllBytes(itc2FilePath);

            if (!ValidateITC2Data(itc2Data))
                throw new InvalidOperationException("File is not a valid .itc2 file");

            // Read new image
            byte[] newImageData = File.ReadAllBytes(newImagePath);

            // Validate new image is JPEG or PNG
            if (!IsValidImageFile(newImageData))
                throw new InvalidOperationException("Image file is not a valid JPEG or PNG");

            WriteItc2(itc2FilePath, itc2Data, newImageData);
        }

        /// <summary>
        /// Replaces the image in an .itc2 file using image bytes already in memory.
        /// </summary>
        public static void ReplaceImageBytes(string itc2FilePath, byte[] newImageData)
        {
            if (!File.Exists(itc2FilePath))
                throw new FileNotFoundException($".itc2 file not found: {itc2FilePath}");

            byte[] itc2Data = File.ReadAllBytes(itc2FilePath);

            if (!ValidateITC2Data(itc2Data))
                throw new InvalidOperationException("File is not a valid .itc2 file");

            if (!IsValidImageFile(newImageData))
                throw new InvalidOperationException("Image data is not a valid JPEG or PNG");

            WriteItc2(itc2FilePath, itc2Data, newImageData);
        }

        private static void WriteItc2(string itc2FilePath, byte[] originalData, byte[] newImageData)
        {
            byte[] newItc2Data = new byte[(int)IMAGE_DATA_OFFSET + newImageData.Length];
            Array.Copy(originalData, 0, newItc2Data, 0, (int)IMAGE_DATA_OFFSET);
            Array.Copy(newImageData, 0, newItc2Data, (int)IMAGE_DATA_OFFSET, newImageData.Length);

            // The "itch" atom (bytes 0-3) is always 0x11C = 284 - never changes.
            // The "item" atom starts at HEADER_SIZE (0x11C). Its size field covers
            // from HEADER_SIZE to end of file: item_size = total_file_size - HEADER_SIZE.
            WriteUInt32BE(newItc2Data, (int)HEADER_SIZE, (uint)(newItc2Data.Length - HEADER_SIZE));

            // Update stored image dimensions (width at 0x154, height at 0x158).
            try
            {
                using var ms = new MemoryStream(newImageData);
                using var img = Image.FromStream(ms);
                WriteUInt32BE(newItc2Data, 0x154, (uint)img.Width);
                WriteUInt32BE(newItc2Data, 0x158, (uint)img.Height);
            }
            catch { /* leave original dimensions if decode fails */ }

            File.WriteAllBytes(itc2FilePath, newItc2Data);
        }

        private static void WriteUInt32BE(byte[] data, int offset, uint value)
        {
            data[offset]     = (byte)(value >> 24);
            data[offset + 1] = (byte)(value >> 16);
            data[offset + 2] = (byte)(value >> 8);
            data[offset + 3] = (byte)value;
        }

        /// <summary>
        /// Validates that data is a valid JPEG or PNG image.
        /// </summary>
        private static bool IsValidImageFile(byte[] data)
        {
            if (data.Length < 4)
                return false;

            // Check for JPEG (FF D8 FF)
            if (data[0] == 0xFF && data[1] == 0xD8 && data[2] == 0xFF)
                return true;

            // Check for PNG (89 50 4E 47 = "‰PNG")
            if (data[0] == 0x89 && data[1] == 0x50 && data[2] == 0x4E && data[3] == 0x47)
                return true;

            return false;
        }

        /// <summary>
        /// Detects the image type (JPEG or PNG) from binary data.
        /// </summary>
        public static string DetectImageType(byte[] data)
        {
            if (data.Length < 4)
                return "Unknown";

            if (data[0] == 0xFF && data[1] == 0xD8 && data[2] == 0xFF)
                return "JPEG";

            if (data[0] == 0x89 && data[1] == 0x50 && data[2] == 0x4E && data[3] == 0x47)
                return "PNG";

            return "Unknown";
        }

        /// <summary>
        /// Scans a directory recursively for all .itc2 files.
        /// </summary>
        public static List<string> ScanDirectoryForITC2Files(string directoryPath)
        {
            List<string> itc2Files = new List<string>();

            try
            {
                if (!Directory.Exists(directoryPath))
                    return itc2Files;

                DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
                FileInfo[] files = dirInfo.GetFiles("*.itc2", SearchOption.AllDirectories);

                itc2Files = files.Select(f => f.FullName).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error scanning directory: {ex.Message}");
            }

            return itc2Files;
        }

        /// <summary>
        /// Gets file information for an .itc2 file.
        /// </summary>
        public static ITC2FileInfo GetFileInfo(string itc2FilePath)
        {
            var info = new ITC2FileInfo
            {
                FilePath = itc2FilePath,
                FileName = Path.GetFileName(itc2FilePath),
                FileSize = 0,
                IsValid = false,
                ImageType = "Unknown"
            };

            try
            {
                if (!File.Exists(itc2FilePath))
                    return info;

                FileInfo fileInfo = new FileInfo(itc2FilePath);
                info.FileSize = fileInfo.Length;

                byte[] data = File.ReadAllBytes(itc2FilePath);
                info.IsValid = ValidateITC2Data(data);

                if (info.IsValid && data.Length > IMAGE_DATA_OFFSET)
                {
                    int imageDataLength = data.Length - (int)IMAGE_DATA_OFFSET;
                    byte[] imageData = new byte[imageDataLength];
                    Array.Copy(data, IMAGE_DATA_OFFSET, imageData, 0, imageDataLength);
                    info.ImageType = DetectImageType(imageData);
                    info.ImageSize = imageDataLength;

                    string kindTag = System.Text.Encoding.ASCII.GetString(data, (int)KIND_MARKER_OFFSET, 4);
                    info.ArtworkKind = kindTag switch
                    {
                        "stor" => "Artist",
                        "CLPU" => "Album",
                        _      => "Unknown"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting file info: {ex.Message}");
            }

            return info;
        }
    }

    /// <summary>
    /// Information about an .itc2 file.
    /// </summary>
    public class ITC2FileInfo
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public long ImageSize { get; set; }
        public bool IsValid { get; set; }
        public string ImageType { get; set; } = string.Empty;
        public string ArtworkKind { get; set; } = "Unknown";
    }
}
