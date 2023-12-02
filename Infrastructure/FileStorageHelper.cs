using Microsoft.AspNetCore.Http;

namespace Infrastructure
{
    public class FileStorageHelper
    {
        private static string staticFilesDirectoryName = "wwwroot";
        public static string CreateDirectoryAsync(string id, string mediaDirectoryName)
        {
            string mediaPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                staticFilesDirectoryName,
                id,
                mediaDirectoryName
                );
            if (!Directory.Exists(mediaPath))
            {
                Directory.CreateDirectory(mediaPath);
            }
            return mediaPath;
        }
        public static bool UploadMediaAsync
            (string id, string mediaDirectoryName, string fileName, IFormFile media)
        {
            string filePath = Path.Combine(CreateDirectoryAsync(id, mediaDirectoryName), fileName);
            try
            {
                FileStream fileStream = new(filePath, FileMode.Create)
                {
                    Position = 0
                };
                media.CopyTo(fileStream);
                fileStream.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool DeleteMediaAsync(string id, string mediaDirectoryName, string fileName)
        {
            string FilePath =
            Path.Combine(
                Directory.GetCurrentDirectory(),
                staticFilesDirectoryName,
                id,
                mediaDirectoryName,
                fileName);
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
