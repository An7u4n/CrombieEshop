using Model.DTO;

namespace Service.Interfaces
{
    public interface IUsuarioService
    {
        void ActualizarUsuario(UsuarioDTO usuarioDTO);
        void CrearUsuario(UsuarioDTO usuarioDTO);
        void AgregarItemsWishList(int idUsuario, int idProducto);
        void EliminarItemsWishList(int idUsuario, int idProducto);
        ICollection<ProductoDTO> ListarItemsWishList(int idUsuario);
    }
}