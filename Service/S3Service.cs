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
        private const int PRESIGNED_URL_DURATION = 5;
        public S3Service(IConfiguration configuration)
        {
            var awsAccessKeyId = configuration["AWS:AccessKeyId"];
            var awsSecretAccessKey = configuration["AWS:SecretAccessKey"];
            _region = configuration["AWS:Region"];
            _bucketName = configuration["AWS:BucketName"];
            var awsCredentials = new BasicAWSCredentials(awsAccessKeyId, awsSecretAccessKey);
            _client = new AmazonS3Client(awsCredentials, Amazon.RegionEndpoint.GetBySystemName(_region));
        }

        public async Task<string> SubirImagenAsync(Stream fileStream, string fileKey, string contentType)
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
                    return GeneratePresignedURL(fileKey);
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
        public string GeneratePresignedURL(string objectKey)
        {
            string urlString = string.Empty;
            try
            {
                var request = new GetPreSignedUrlRequest()
                {
                    BucketName = _bucketName,
                    Key = objectKey,
                    Expires = DateTime.UtcNow.AddMinutes(PRESIGNED_URL_DURATION),
                };
                urlString = _client.GetPreSignedURL(request);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error:'{ex.Message}'");
            }

            return urlString;
        }

        private string GetS3Url(string fileKey)
        {
            return $"https://{_bucketName}.s3.{_region}.amazonaws.com/{fileKey}";
        }
    }
}
