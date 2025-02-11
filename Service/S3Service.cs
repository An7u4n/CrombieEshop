﻿using Amazon.Runtime;
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

        public async Task<string> SubirImagenAsync(Stream fileStream, string fileName, string contentType)
        {
            if (!IMAGE_VALID_MIME_TYPES.Contains(contentType))
            {
                throw new InvalidOperationException("El archivo no es una imagen válida.");
            }
            return await SubirArchivoAsync(fileStream, fileName, contentType);
        }
        public async Task<(Stream, string)> ObtenerArchivoAsync(string fileKey)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileKey
                };

                var response = await _client.GetObjectAsync(request);
                return (response.ResponseStream, response.Headers["Content-Type"]);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error fetching file: {ex.Message}");
                throw;
            }
        }
        public async Task<bool> EliminarCarpetaAsync(string folderKey)
        {
            try
            {
                // Ensure folderKey ends with '/' (S3 treats prefixes like folders)
                if (!folderKey.EndsWith("/"))
                {
                    folderKey += "/";
                }

                // Step 1: List all objects in the folder
                var listRequest = new ListObjectsV2Request
                {
                    BucketName = _bucketName,
                    Prefix = folderKey
                };

                var listResponse = await _client.ListObjectsV2Async(listRequest);

                if (listResponse.S3Objects.Count == 0)
                {
                    return false;
                }

                var deleteRequest = new DeleteObjectsRequest
                {
                    BucketName = _bucketName,
                    Objects = listResponse.S3Objects
                        .Select(obj => new KeyVersion { Key = obj.Key })
                        .ToList()
                };

                var deleteResponse = await _client.DeleteObjectsAsync(deleteRequest);

                return deleteResponse.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error deleting folder: {ex.Message}");
                throw;
            }
        }

    }
}
