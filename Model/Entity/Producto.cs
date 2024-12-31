using System.ComponentModel.DataAnnotations;

namespace Model.Entity
{
    public class Producto
    {
        [Key][Required] public int Id { get; set; }
        [Required][MaxLength(128)] public string NombreProducto { get; set; }
        [Required] public string Descripcion { get; set; }
        [Required] public double Precio { get; set; }
        public virtual ICollection<WishList> WishList { get; set; }
    }
}
