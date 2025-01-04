using System.ComponentModel.DataAnnotations;

namespace Model.Entity
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreDeUsuario { get; set; }
        public string Nombre { get; set; }
        public string Contrasena { get; set; }
        [EmailAddress] public string Email { get; set; }
        public virtual List<WishListItem> WishListItems { get; set; } = [];
    }
}
