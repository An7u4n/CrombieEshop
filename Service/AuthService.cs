using Data.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Model.DTO;
using Model.Entity;
using Service.Interfaces;
using System.Net.WebSockets;

namespace Service
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _userRepository;
        private readonly ICognitoAuthService _cognitoAuthService;
        private readonly PasswordHasher<Usuario> _passwordHasher = new PasswordHasher<Usuario>();
        public AuthService(IUsuarioRepository usuarioRepository,ICognitoAuthService cognitoAuthService)
        {
            _userRepository = usuarioRepository;
            _cognitoAuthService = cognitoAuthService;
        }
        // Should fetch user via email/username and password
        public UsuarioDTO LoginUsuario(AuthDTO userData)
        {

            Usuario user = null;
            if (userData.Email != null)
            {
                user = _userRepository.FindByEmail(userData.Email);
            }
            else if (userData.NombreUsuario != null)
            {
                user = _userRepository.FindByNombreUsuario(userData.NombreUsuario);
            }
            PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(user, user.Contrasena, userData.Contrasena);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid password");
            }

            return new UsuarioDTO(user);
        }

        public async Task<UsuarioDTO> RegistrarUsuario(UsuarioDTO userData)
        {
            try
            {
                Usuario user = new Usuario(userData);
                await _cognitoAuthService.RegistrarAsync(userData.Email, userData.Contrasena);
                _userRepository.CrearUsuario(user);
                var newUser = new UsuarioDTO(user);
                newUser.Contrasena = null;
                return newUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
