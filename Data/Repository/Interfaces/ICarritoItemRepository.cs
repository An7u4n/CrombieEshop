using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces
{
    public interface ICarritoItemRepository
    {
        void SetProductoCarrito(int idUsuario,int idProducto,int cantidad);
        void SetProductosCarrito(int idUsuario, ICollection<(int idProducto,int cantidad)> items );
        void EliminarProductoCarrito(int idUsuario, int idProducto);
        ICollection<CarritoItem> ObtenerCarrito(int idUsuario);
    }
}
