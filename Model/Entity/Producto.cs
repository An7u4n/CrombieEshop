using Model.DTO;
using System.ComponentModel.DataAnnotations;

namespace Model.Entity
{
    public class Producto
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }

        public Producto() { }

        public Producto(ProductoDTO producto)
        {
            NombreProducto = producto.NombreProducto;
            Descripcion = producto.Descripcion;
            Precio = producto.Precio;
        }
    }
}
