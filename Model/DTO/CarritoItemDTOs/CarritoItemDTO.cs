using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO.CarritoItemDTOs
{
    public class CarritoItemDTO
    {
        public int UsuarioId { get; set; }
        public ProductoDTO Producto { get; set; }
        public int Cantidad { get; set; }

        public CarritoItemDTO() { }
        public CarritoItemDTO(CarritoItem item)
        {
            UsuarioId = item.UsuarioId;
            Producto = new ProductoDTO(item.Producto);
            Cantidad = item.Cantidad;
        }
    }
}
