using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Service.Interfaces;
using Service.Utilities;
using System.Net;

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
                    SecretHash = SecurityHelper.CalculateSecretHash(_clientId, _clientSecret, email)
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

        public async Task ConfirmarRegistroAsync(string email, string code)
        {
            try
            {
                var signUpRequest = new ConfirmSignUpRequest
                {
                    ClientId = this._clientId,
                    ConfirmationCode = code,
                    Username = email,
                    SecretHash = SecurityHelper.CalculateSecretHash(_clientId, _clientSecret, email)
                };

                var response = await _provider.ConfirmSignUpAsync(signUpRequest);
                if (response.HttpStatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(email + " no pudo ser confirmado");
                }
                Console.WriteLine($"{email} fue confirmado");
            }
            catch(Exception ex) 
            {
                throw new Exception($"Error durante confirmación: {ex.Message}");
            }
        }
    }
}
