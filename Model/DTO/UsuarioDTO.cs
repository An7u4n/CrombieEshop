using System.Text.Json.Serialization;
using Model.Entity;
using Model.Enums;

namespace Model.DTO
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string NombreDeUsuario { get; set; }
        public string Nombre { get; set; }
        public string? Contrasena { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public Role Role { get; set; } = Role.User;

        public string FotoPerfilUrl { get; set; }
        public UsuarioDTO() { }

        public UsuarioDTO(Usuario usuario)
        {
            Id = usuario.Id;
            NombreDeUsuario = usuario.NombreDeUsuario;
            Nombre = usuario.Nombre;
            Email = usuario.Email;
            FotoPerfilUrl = usuario.Imagen?.FotoPerfilUrl ?? "";
        }
    }
}
