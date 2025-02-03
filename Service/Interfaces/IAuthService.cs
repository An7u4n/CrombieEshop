using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DTO;
using static Service.UsuarioService;

namespace Service.Interfaces
{
    public interface IAuthService
    {
        Task RegistrarAsync(string email, string password);
        Task ConfirmarRegistroAsync(string userName, string code);
        Task<string> IniciarSesion(string userName, string password);
        Task<bool> ActualizarImagenPerfilKey(string userName, string imagenKey);
        Task<UserInfoDTO> ObtenerUsuarioDesdeAccessToken(string accessToken);
    }
}
