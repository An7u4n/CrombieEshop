using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DTO;

namespace Service.Interfaces
{
    public interface ITokenService
    {
        public string CreateJWTAuthToken(UsuarioDTO user);
    }
}
