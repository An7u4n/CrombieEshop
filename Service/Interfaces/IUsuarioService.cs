using Model.DTO;

namespace Service.Interfaces
{
    public interface IUsuarioService
    {
        void ActualizarUsuario(UsuarioDTO usuarioDTO);
        void CrearUsuario(UsuarioDTO usuarioDTO);
    }
}