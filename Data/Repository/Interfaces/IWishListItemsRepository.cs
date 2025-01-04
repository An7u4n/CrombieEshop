using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces
{
    public interface IWishListItemsRepository
    {
        void AgregarProductoWishList(int idUsuario, int idProducto);
        void EliminarProductoWishList(int idUsuario, int idProducto);
        ICollection<Producto> ObtenerProductosWishList(int idUsuario);
    }
}
