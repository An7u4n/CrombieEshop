using Model.DTO;

namespace Service.Interfaces
{
    public interface IUsuarioService
    {
        void ActualizarUsuario(UsuarioDTO usuarioDTO);
        UsuarioDTO CrearUsuario(UsuarioDTO usuarioDTO);
        void AgregarItemsWishList(int idUsuario, int idProducto);
        void EliminarItemsWishList(int idUsuario, int idProducto);
        ICollection<ProductoDTO> ListarItemsWishList(int idUsuario);
    }
}