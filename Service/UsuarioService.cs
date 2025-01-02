using Data.Repository.Interfaces;
using Model.DTO;
using Model.Entity;
using Service.Interfaces;
using Service.Utility;

namespace Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public void CrearUsuario(UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO.NombreDeUsuario == null || usuarioDTO.NombreDeUsuario == string.Empty)
                throw new ArgumentNullException("El nombre de usuario es requerido.");
            if (usuarioDTO.Nombre == null || usuarioDTO.Nombre == string.Empty)
                throw new ArgumentNullException("El nombre es requerido.");
            if (usuarioDTO.Contrasena == null || usuarioDTO.Contrasena == string.Empty)
                throw new ArgumentNullException("La contraseña es requerida.");
            if (usuarioDTO.Email == null || usuarioDTO.Email == string.Empty)
                throw new ArgumentNullException("El email es requerido.");

            var usuario = new Usuario
            {
                NombreDeUsuario = usuarioDTO.NombreDeUsuario,
                Nombre = usuarioDTO.Nombre,
                Contrasena = PasswordHasher.HashPassword(usuarioDTO.Contrasena),
                Email = usuarioDTO.Email
            };

            _usuarioRepository.GuardarUsuario(usuario);
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
            usuario.Contrasena = PasswordHasher.HashPassword(usuarioDTO.Contrasena);
            usuario.Email = usuarioDTO.Email;

            _usuarioRepository.ActualizarUsuario(usuario);
        }
    }
}
