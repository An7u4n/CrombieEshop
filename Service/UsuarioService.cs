using Data.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Model.DTO;
using Model.Entity;
using Service.Interfaces;


namespace Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IWishListItemsRepository _wishListItemsRepository;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public UsuarioService(IUsuarioRepository usuarioRepository, IWishListItemsRepository wishListItemsRepository)
        {
            _usuarioRepository = usuarioRepository;
            _wishListItemsRepository = wishListItemsRepository;
        }

        public UsuarioDTO CrearUsuario(UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO.NombreDeUsuario == null || usuarioDTO.NombreDeUsuario == string.Empty)
                throw new ArgumentNullException("El nombre de usuario es requerido.");
            if (usuarioDTO.Nombre == null || usuarioDTO.Nombre == string.Empty)
                throw new ArgumentNullException("El nombre es requerido.");
            if (usuarioDTO.Contrasena == null || usuarioDTO.Contrasena == string.Empty)
                throw new ArgumentNullException("La contraseña es requerida.");
            if (usuarioDTO.Email == null || usuarioDTO.Email == string.Empty)
                throw new ArgumentNullException("El email es requerido.");

            var usuario = new Usuario(usuarioDTO);

            var usuarioNuevo = _usuarioRepository.CrearUsuario(usuario);
            return new UsuarioDTO(usuarioNuevo);
        }

        public void ActualizarUsuario(UsuarioDTO usuarioDTO)
        {
            var usuario = _usuarioRepository.ObtenerUsuario(usuarioDTO.Id);

            if (usuario == null)
                throw new Exception("El usuario no existe.");

            if (usuarioDTO.NombreDeUsuario == null || usuarioDTO.NombreDeUsuario == string.Empty)
                throw new ArgumentNullException("El nombre de usuario es requerido.");
            if (usuarioDTO.Nombre == null || usuarioDTO.Nombre == string.Empty)
                throw new ArgumentNullException("El nombre es requerido.");
            if (usuarioDTO.Contrasena == null || usuarioDTO.Contrasena == string.Empty)
                throw new ArgumentNullException("La contraseña es requerida.");
            if (usuarioDTO.Email == null || usuarioDTO.Email == string.Empty)
                throw new ArgumentNullException("El email es requerido.");



            usuario.NombreDeUsuario = usuarioDTO.NombreDeUsuario;
            usuario.Nombre = usuarioDTO.Nombre;
            usuario.Email = usuarioDTO.Email;
            usuario.Contrasena = _passwordHasher.HashPassword(usuario, usuarioDTO.Contrasena);

            _usuarioRepository.ActualizarUsuario(usuario);
        }

        void IUsuarioService.AgregarItemsWishList(int idUsuario, int idProducto)
        {
            _wishListItemsRepository.AgregarProductoWishList(idUsuario, idProducto);

        }

        void IUsuarioService.EliminarItemsWishList(int idUsuario, int idProducto)
        {
            _wishListItemsRepository.EliminarProductoWishList(idUsuario, idProducto);
        }

        ICollection<ProductoDTO> IUsuarioService.ListarItemsWishList(int idUsuario)
        {
            var productos = _wishListItemsRepository.ObtenerProductosWishList(idUsuario);
            var productosDTO = productos.Select(p => new ProductoDTO(p)).ToList();
            return productosDTO;
        }

        public void EliminarUsuario(int idUsuario)
        {
            _usuarioRepository.EliminarUsuario(idUsuario);
        }

        public UsuarioDTO ObtenerUsuario(int idUsuario)
        {
            var usuario = _usuarioRepository.ObtenerUsuario(idUsuario);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");
            return new UsuarioDTO(usuario);
        }

        public ICollection<UsuarioDTO> ObtenerUsuarios()
        {
            var usuarios = _usuarioRepository.ObtenerUsuarios();
            if (usuarios == null)
                throw new Exception("No se han encontrado usuarios.");
            var usuariosDTO = usuarios.Select(u => new UsuarioDTO(u)).ToList();
            return usuariosDTO;
        }
    }
}
