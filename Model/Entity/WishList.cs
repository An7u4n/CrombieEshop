using System.ComponentModel.DataAnnotations;

namespace Model.Entity
{
    public class WishList
    {
        [Key][Required] public int Id { get; set; }
        [Required] public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }
    }
}
