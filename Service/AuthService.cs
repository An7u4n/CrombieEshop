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
        public AuthService(IUsuarioRepository usuarioRepository)
        {
            _userRepository = usuarioRepository;
        }

        public Task ConfirmarRegistroAsync(string userName, string code)
        {
            throw new NotImplementedException();
        }


        public Task<string> IniciarSesion(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task RegistrarAsync(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
