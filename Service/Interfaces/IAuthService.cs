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
        UsuarioDTO RegistrarUsuario(UsuarioDTO user);
        UsuarioDTO LoginUsuario(AuthDTO user);
    }
}
