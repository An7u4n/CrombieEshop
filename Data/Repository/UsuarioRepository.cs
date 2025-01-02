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
    }
}
