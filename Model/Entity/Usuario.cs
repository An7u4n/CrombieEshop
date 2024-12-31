using System.ComponentModel.DataAnnotations;

namespace Model.Entity
{
    public class Usuario
    {
        [Key][Required] public int Id { get; set; }
        [Required][MaxLength(80)] public string NombreDeUsuario { get; set; }
        [Required][MaxLength(80)] public string Nombre { get; set; }
        [Required] public string Contrasena { get; set; }
        [Required][EmailAddress] public string Email { get; set; }
        public virtual WishList WishList { get; set; } = new WishList();
    }
}
