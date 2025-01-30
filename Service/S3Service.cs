using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Service.Interfaces;

namespace Service
{
    public class S3Service : IS3Service
    {
        private readonly AmazonS3Client _client;
        private readonly string _region;
        private readonly string _bucketName;
        private static readonly int PRESIGNED_URL_DURATION = 5;
        private static readonly string[] IMAGE_VALID_MIME_TYPES = { "image/jpeg", "image/png", "image/gif" };
        public S3Service(IConfiguration configuration)
        {
            var awsAccessKeyId = configuration["AWS:AccessKeyId"];
            var awsSecretAccessKey = configuration["AWS:SecretAccessKey"];
            _region = configuration["AWS:Region"];
            _bucketName = configuration["AWS:BucketName"];
            var awsCredentials = new BasicAWSCredentials(awsAccessKeyId, awsSecretAccessKey);
            _client = new AmazonS3Client(awsCredentials, Amazon.RegionEndpoint.GetBySystemName(_region));
        }

        public async Task<string> SubirArchivoAsync(Stream fileStream, string fileKey, string contentType)
        {
            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileKey,
                    InputStream = fileStream,
                    ContentType = contentType
                };

                var response = await _client.PutObjectAsync(request);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    return GetS3Url(fileKey);
                }
                else
                {
                    throw new Exception("Error al subir imagen");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al subir imagen: {ex.Message}");
            }
        }

        private string GetS3Url(string fileKey)
        {
            return $"https://{_bucketName}.s3.{_region}.amazonaws.com/{fileKey}";
        }

        public async Task<string> SubirImagenAsync(Stream fileStream, string fileName, string contentType)
        {
            if (!IMAGE_VALID_MIME_TYPES.Contains(contentType))
            {
                throw new InvalidOperationException("El archivo no es una imagen válida.");
            }
            return await SubirArchivoAsync(fileStream, fileName, contentType);
        }
    }
}
