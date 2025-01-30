namespace Service.Interfaces
{
    public interface IS3Service
    {
        Task<string> SubirArchivoAsync(Stream fileStream, string fileKey, string contentType);
        Task<string> SubirImagenAsync(Stream fileStream, string fileName, string contentType);
        public string GeneratePresignedURL(string objectKey);
    }
}