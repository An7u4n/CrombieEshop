using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Azure.Core;
using Microsoft.Extensions.Configuration;
using Model.DTO;
using Service.Interfaces;
using Service.Utilities;
using System.Net;

namespace Service
{
    public class CognitoAuthService : IAuthService
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
            catch (Exception ex)
            {
                throw new Exception($"Error durante confirmación: {ex.Message}");
            }
        }

        public async Task<string> IniciarSesion(string email, string password)
        {
            try
            {
                var authParameters = new Dictionary<string, string>();
                authParameters.Add("USERNAME", email);
                authParameters.Add("PASSWORD", password);
                authParameters.Add("SECRET_HASH", SecurityHelper.CalculateSecretHash(_clientId, _clientSecret, email));

                var authRequest = new InitiateAuthRequest
                {
                    ClientId = _clientId,
                    AuthParameters = authParameters,
                    AuthFlow = AuthFlowType.USER_PASSWORD_AUTH
                };

                var response = await _provider.InitiateAuthAsync(authRequest);
                Console.WriteLine(response.AuthenticationResult);
                if (response.HttpStatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Error al iniciar sesión");
                }
                return response.AuthenticationResult.AccessToken;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al iniciar sesión: {ex.Message}");
            }
        }

        public async Task<bool> ActualizarImagenPerfilKey(string accessToken, string imagenKey)
        {
            try
            {
                var request = new UpdateUserAttributesRequest
                {
                    AccessToken = accessToken,
                    UserAttributes = new List<AttributeType>
                    {
                        new AttributeType
                        {
                            Name = "picture",
                            Value = imagenKey
                        }
                    }
                };
                var response = await _provider.UpdateUserAttributesAsync(request);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al subir imagen: {ex.Message}");
            }
        }

        public async Task<UserInfoDTO> ObtenerUsuarioDesdeAccessToken(string accessToken)
        {
            try
            {
                // Usar el método GetUser para obtener información del usuario desde el accessToken
                var getUserRequest = new GetUserRequest
                {
                    AccessToken = accessToken
                };

                var getUserResponse = await _provider.GetUserAsync(getUserRequest);

                // Extraer información relevante del usuario (opcional)
                var userInfo = new UserInfoDTO
                {
                    Username = getUserResponse.Username,
                    Attributes = getUserResponse.UserAttributes.ToDictionary(attr => attr.Name, attr => attr.Value)
                };

                return userInfo;
            }
            catch (NotAuthorizedException)
            {
                // Si el accessToken no es válido, lanzar una excepción
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar el accessToken: {ex.Message}");
            }
        }
        public async Task<bool> EliminarUsuarioAsync(string email)
        {
            try
            {
                var request = new AdminDeleteUserRequest
                {
                    UserPoolId = _userPool.PoolID,
                    Username = email
                };

                var response = await _provider.AdminDeleteUserAsync(request);
                return response.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (AmazonCognitoIdentityProviderException ex)
            {
                throw new Exception($"Error al eliminar usuario: {ex.Message}");
            }
        }

    }
}
