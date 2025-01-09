using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.DTO;
using Model.Entity;
using Service.Interfaces;

namespace Service
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _userRepository;
        private readonly PasswordHasher<Usuario> _passwordHasher = new PasswordHasher<Usuario>();
        public AuthService(IUsuarioRepository usuarioRepository)
        {
            _userRepository = usuarioRepository;
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

        public UsuarioDTO RegistrarUsuario(UsuarioDTO userData)
        {
            Usuario user = new Usuario(userData);
            _userRepository.CrearUsuario(user);
            return new UsuarioDTO(user);
        }
    }
}
