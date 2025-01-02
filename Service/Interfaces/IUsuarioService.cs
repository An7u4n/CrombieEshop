using Model.DTO;

namespace Service
{
    public interface IUsuarioService
    {
        void ActualizarUsuario(UsuarioDTO usuarioDTO);
        void CrearUsuario(UsuarioDTO usuarioDTO);
    }
}