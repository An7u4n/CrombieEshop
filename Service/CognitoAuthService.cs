using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Data.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Model.DTO;
using Model.Entity;
using Service.Interfaces;

namespace Service
{
    public class CognitoAuthService : IAuthService
    {
        private readonly AmazonCognitoIdentityProviderClient _provider;
        private readonly CognitoUserPool _userPool;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly IUsuarioRepository _usuarioRepository;

        public CognitoAuthService(IConfiguration config, IUsuarioRepository userRepo)
        {
            _usuarioRepository = userRepo;
            var _awsAccessKey = config["AWS:AccessKey"];
            var _awsSecretKey = config["AWS:SecretKey"];
            var _awsRegion = config["AWS:Region"];
            var _userPoolId = config["Cognito:PoolID"];
            _clientId = config["Cognito:Client:ID"];
            _clientSecret = config["Cognito:Client:Secret"];


            var awsCredentials = new BasicAWSCredentials(_awsAccessKey, _awsSecretKey);
            _provider = new AmazonCognitoIdentityProviderClient(awsCredentials, Amazon.RegionEndpoint.GetBySystemName(_awsRegion));
            _userPool = new CognitoUserPool(_userPoolId, _clientId, _provider, _clientSecret);

        }

        public async Task<bool> ConfirmarRegistro(string code, string username)
        {
            var signUpRequest = new ConfirmSignUpRequest
            {
                ClientId = _clientId,
                ConfirmationCode = code,
                Username = username,
                SecretHash = CalculateSecretHash(_clientId, _clientSecret, username)
            };
            var response = await _provider.ConfirmSignUpAsync(signUpRequest);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
        async public Task<UsuarioDTO> LoginUsuario(AuthDTO user)
        {
            UsuarioDTO userDTO = null;
            var authParams = new Dictionary<string, string>
            {
                { "PASSWORD", user.Contrasena }
            };
            if (user.Email != null)
            {
                authParams.Add("USERNAME", user.Email);
                authParams.Add("SECRET_HASH", CalculateSecretHash(_clientId, _clientSecret, user.Email));
                userDTO = new UsuarioDTO(_usuarioRepository.FindByEmail(user.Email));
            }
            else if (user.NombreUsuario != null)
            {
                authParams.Add("USERNAME", user.NombreUsuario);
                authParams.Add("SECRET_HASH", CalculateSecretHash(_clientId, _clientSecret, user.NombreUsuario));
                userDTO = new UsuarioDTO(_usuarioRepository.FindByNombreUsuario(user.NombreUsuario));
            }
            else
            {
                throw new Exception("Error al iniciar sesion");
            }
            var authRequest = new InitiateAuthRequest

            {
                ClientId = _clientId,
                AuthParameters = authParams,
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
            };

            var response = await _provider.InitiateAuthAsync(authRequest);
            if (response == null || response.AuthenticationResult == null || userDTO == null)
            {
                throw new Exception("Error al iniciar sesion");
            }
            return userDTO;
        }

        async public Task<UsuarioDTO> RegistrarUsuario(UsuarioDTO user)
        {
            var userAttrMail = new AttributeType
            {
                Name = "email",
                Value = user.Email,
            };
            var userAttrName = new AttributeType
            {
                Name = "preferred_username",
                Value = user.NombreDeUsuario,
            };

            var userAttrsList = new List<AttributeType>();

            userAttrsList.Add(userAttrMail);
            userAttrsList.Add(userAttrName);

            var signUpRequest = new SignUpRequest
            {
                UserAttributes = userAttrsList,
                Username = user.NombreDeUsuario,
                ClientId = _clientId,
                Password = user.Contrasena,
                SecretHash = CalculateSecretHash(_clientId, _clientSecret, user.NombreDeUsuario)
            };

            var response = await _provider.SignUpAsync(signUpRequest);
            if (response.Session == null)
            {
                throw new Exception("Error al crear usuario");
            }
            var createdUser = _usuarioRepository.CrearUsuario(new Usuario(user));
            return new UsuarioDTO(createdUser);
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
