namespace Service.Interfaces
{
    public interface IS3Service
    {
        Task<string> UploadFileAsync(Stream fileStream, string key, string contentType);
        Task<bool> DeleteFileAsync(string key);
    }
}