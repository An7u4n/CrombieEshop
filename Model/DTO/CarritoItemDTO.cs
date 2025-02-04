using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class CarritoItemDTO
    {
        public int UsuarioId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }

        public CarritoItemDTO() { }
        public CarritoItemDTO(CarritoItem item)
        {
            UsuarioId = item.UsuarioId;
            ProductoId = item.ProductoId;
            Cantidad = item.Cantidad;
        }
    }
}
