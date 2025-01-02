using System.ComponentModel.DataAnnotations;

namespace Model.Entity
{
    public class WishListItem
    {
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public int ProductoId { get; set; }
        public virtual Producto Producto { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
