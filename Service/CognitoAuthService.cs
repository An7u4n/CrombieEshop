using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Service.Interfaces;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Service
{
    public class CognitoAuthService : ICognitoAuthService
    {
        private readonly AmazonCognitoIdentityProviderClient _provider;
        private readonly CognitoUserPool _userPool;
        private readonly string _clientId;
        private readonly string _clientSecret;

        public CognitoAuthService(IConfiguration configuration)
        {
            var awsAccessKeyId = configuration["AWS:AccessKeyId"];
            var awsSecretAccessKey = configuration["AWS:SecretAccessKey"];
            var userPoolId = configuration["AWS:UserPoolId"];
            var region = configuration["AWS:Region"];

            _clientId = configuration["AWS:ClientId"] ?? "";
            _clientSecret = configuration["AWS:ClientSecret"] ?? "";

            var awsCredentials = new BasicAWSCredentials(awsAccessKeyId, awsSecretAccessKey);
            _provider = new AmazonCognitoIdentityProviderClient(awsCredentials, Amazon.RegionEndpoint.GetBySystemName(region));
            _userPool = new CognitoUserPool(userPoolId, _clientId, _provider, _clientSecret);
        }

        public async Task RegistrarAsync(string email, string password)
        {
            try
            {
                var request = new SignUpRequest
                {
                    ClientId = _clientId,
                    Username = email,
                    Password = password,
                    SecretHash = CalculateSecretHash(_clientId, _clientSecret, email)
                };

                var response = await _provider.SignUpAsync(request);

                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("Fallo de registro, error desconocido.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error durante registro: {ex.Message}");
            }
        }

        public static string CalculateSecretHash(string clientId, string clientSecret, string username)
        {
            var message = username + clientId;
            var key = Encoding.UTF8.GetBytes(clientSecret);

            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
