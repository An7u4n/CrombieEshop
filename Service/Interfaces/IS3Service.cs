namespace Service.Interfaces
{
    public interface IS3Service
    {
        Task<string> SubirImagenAsync(Stream fileStream, string fileKey, string contentType);
    }
}