using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DTO;

namespace Service.Interfaces
{
    public interface IAuthService
    {
        Task<UsuarioDTO> RegistrarUsuario(UsuarioDTO user);
        Task<UsuarioDTO> LoginUsuario(AuthDTO user);
        Task<Boolean> ConfirmarRegistro(string code, string username);
    }
}
