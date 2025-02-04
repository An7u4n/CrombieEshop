namespace Service.Interfaces
{
    public interface IS3Service
    {
        Task<string> SubirArchivoAsync(Stream fileStream, string fileKey, string contentType);
        Task<string> SubirImagenAsync(Stream fileStream, string fileName, string contentType);
        Task<(Stream, string)> ObtenerArchivoAsync(string fileKey);
        Task<bool> EliminarCarpetaAsync(string folderKey);
        public string GeneratePresignedURL(string objectKey);
    }
}