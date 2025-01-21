using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Data.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Service.Interfaces;

namespace Service
{
    public class S3Service : IS3Service
    {
        private readonly string _bucketName;
        private readonly string _awsRegion;
        private readonly IAmazonS3 _client;

        public S3Service(IConfiguration config)
        {
            var _awsAccessKey = config["AWS:AccessKey"];
            var _awsSecretKey = config["AWS:SecretKey"];
            _awsRegion = config["AWS:Region"];
            _bucketName = config["S3:BucketName"];
            var awsCredentials = new BasicAWSCredentials(_awsAccessKey, _awsSecretKey);
            _client = new AmazonS3Client(awsCredentials, Amazon.RegionEndpoint.GetBySystemName(_awsRegion));
        }
        async public Task<string> UploadFileAsync(Stream fileStream, string key, string contentType)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = fileStream,
                ContentType = contentType
            };

            var response = await _client.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                //Returns s3 url
                return GetS3Url(key);
            }
            else
            {
                return "";
            }
        }
        async public Task<bool> DeleteFileAsync(string key)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };
            var response = await _client.DeleteObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private string GetS3Url(string key)
        {
            return $"https://{_bucketName}.s3.{_awsRegion}.amazonaws.com/{key}";
        }

    }
}
