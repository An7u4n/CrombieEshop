using Data.Repository.Interfaces;
using Model.Entity;

namespace Data.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;
        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public Usuario ObtenerUsuario(int idUsuario)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Id == idUsuario);
        }

        public Usuario CrearUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return usuario;
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
        }

        public void EliminarUsuario(int idUsuario)
        {
            var usuario = ObtenerUsuario(idUsuario);
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }

        ICollection<Usuario> IUsuarioRepository.ObtenerUsuarios()
        {
            var usuarios = _context.Usuarios.ToList();
            return usuarios;
        }

        public Usuario EncontrarPorEmail(string email)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            return user;
        }

        public Usuario EncontrarPorNombreUsuario(string nombreUsuario)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.NombreDeUsuario == nombreUsuario);
            return user;
        }
    }
}
