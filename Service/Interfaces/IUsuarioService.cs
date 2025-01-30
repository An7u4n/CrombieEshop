using Model.DTO;

namespace Service.Interfaces
{
    public interface IUsuarioService
    {
        void ActualizarUsuario(UsuarioDTO usuarioDTO);
        void EliminarUsuario(int idUsuario);
        UsuarioDTO CrearUsuario(UsuarioDTO usuarioDTO);
        UsuarioDTO ObtenerUsuario(int idUsuario);
        ICollection<UsuarioDTO> ObtenerUsuarios();
        void AgregarItemsWishList(int idUsuario, int idProducto);
        void EliminarItemsWishList(int idUsuario, int idProducto);
        ICollection<ProductoDTO> ListarItemsWishList(int idUsuario);
        Task<string> SubirImagenPerfilAsync(Stream fileStream, string fileName, int idUsuario, string contentType);
    }
}