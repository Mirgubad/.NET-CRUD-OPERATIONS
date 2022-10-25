namespace front_to_back.Helpers
{
    public interface IFileService
    {
        Task<string> UploadAsync(IFormFile file, string webRootPath);
        void Delete(string webrootpah, string fileName);
        bool CheckFile(IFormFile file);
        bool MaxSize(IFormFile file, int maxSize);

    }
}
