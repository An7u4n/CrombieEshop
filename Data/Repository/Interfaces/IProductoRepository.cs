using Model.Entity;

namespace Data.Repository.Interfaces
{
    public interface IProductoRepository
    {
        void ActualizarProducto(Producto producto);
        void EliminarProducto(int idProducto);
        void GuardarProducto(Producto producto);
        Producto ObtenerProducto(int idProducto);
        ICollection<Producto> ObtenerProductos();
    }
}