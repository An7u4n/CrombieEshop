using Model.DTO;

namespace Service.Interfaces
{
    public interface IProductoService
    {
        void ActualizarProducto(ProductoDTO productoDTO);
        void EliminarProducto(int idProducto);
        void GuardarProducto(ProductoDTO productoDTO);
        ProductoDTO ObtenerProducto(int idProducto);
        ICollection<ProductoDTO> ObtenerProductos();
    }
}