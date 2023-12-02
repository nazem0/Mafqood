using Microsoft.AspNetCore.Http;

namespace Infrastructure
{
    public class FileStorageHelper
    {
        private static readonly string staticFilesDirectoryName = "wwwroot";
        private static readonly string currentDirectory = Directory.GetCurrentDirectory();
        public static string CreateDirectory(string mediaDirectoryName, string id)
        {
            string mediaPath = Path.Combine(currentDirectory, staticFilesDirectoryName, mediaDirectoryName, id);
            if (!Directory.Exists(mediaPath))
            {
                Directory.CreateDirectory(mediaPath);
            }
            return mediaPath;
        }
        public static async Task<bool> UploadAsync(string directoryName, string id, IFormFile media, string? fileName = null)
        {
            try
            {
                string filePath = Path.Combine(CreateDirectory(directoryName, id), (fileName ?? media.FileName));
                FileStream fileStream = File.Create(filePath);
                await media.CopyToAsync(fileStream);
                fileStream.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool Delete(string mediaDirectoryName, string id, string fileName)
        {
            string FilePath =
            Path.Combine(currentDirectory, staticFilesDirectoryName, mediaDirectoryName, id, fileName);
            if (!File.Exists(FilePath))
                return false;
            try
            {
                File.Delete(FilePath);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
