using Microsoft.AspNetCore.Hosting;

namespace front_to_back.Helpers
{
    public class FileService : IFileService
    {
        public async Task<string> UploadAsync(IFormFile file, string webRootPath)
        {
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            string path = Path.Combine(webRootPath, "assets/img", fileName);
            using (FileStream stream = new(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
        public void Delete(string webrootpah, string fileName)
        {
            string path = Path.Combine(webrootpah, "assets/img", fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public bool CheckFile(IFormFile file)
        {
            if (file.ContentType.Contains("image/"))
            {
                return true;
            }
            return false;
        }
        public bool MaxSize(IFormFile file, int maxSize)
        {
            if (file.Length / 1024 > maxSize)
            {
                return false;
            }
            return true;
        }
    }
}
