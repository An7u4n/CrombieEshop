using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model.DTO;
using Service.Interfaces;

namespace Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _conf;
        public TokenService(IConfiguration conf)
        {
            _conf = conf;
        }
        public string CreateJWTAuthToken(UsuarioDTO user)
        {
            string secretKey = _conf["JWT:SECRET"] ?? "";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            if (secretKey == "")
            {
                throw new Exception("Unable to create token");
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Name, user.NombreDeUsuario),
                    new Claim("Roles", user.Role.ToString())
                    ]
                ),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = credentials,
                Issuer = _conf["JWT:ISSUER"],
                Audience = _conf["JWT:AUDIENCE"]
            };
            var handler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return token;
        }
    }
}
