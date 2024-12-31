using Model.Entity;

namespace Model.DTO
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string NombreDeUsuario { get; set; }
        public string Nombre { get; set; }
        public string Contrasena { get; set; }
        public string Email { get; set; }

        public UsuarioDTO() { }

        public UsuarioDTO(Usuario usuario)
        {
            Id = usuario.Id;
            NombreDeUsuario = usuario.NombreDeUsuario;
            Nombre = usuario.Nombre;
            Contrasena = usuario.Contrasena;
            Email = usuario.Email;
        }
    }
}
