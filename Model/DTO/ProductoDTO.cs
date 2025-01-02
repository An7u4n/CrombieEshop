using Model.Entity;
using System.ComponentModel.DataAnnotations;

namespace Model.DTO
{
    public class ProductoDTO
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }

        public ProductoDTO() { }

        public ProductoDTO(Producto producto)
        {
            Id = producto.Id;
            NombreProducto = producto.NombreProducto;
            Descripcion = producto.Descripcion;
            Precio = producto.Precio;
        }
    }
}
