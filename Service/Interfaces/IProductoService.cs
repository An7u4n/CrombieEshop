using Model.DTO;

namespace Service.Interfaces
{
    public interface IProductoService
    {
        void ActualizarProducto(ProductoDTO productoDTO);
        void EliminarProducto(int idProducto);
        ProductoDTO CrearProducto(ProductoDTO productoDTO);
        ProductoBusquedaDTO BuscarProductos(ProductoParametrosBusquedaDTO parametros);
        ProductoDTO ObtenerProducto(int idProducto);
        ICollection<ProductoDTO> ObtenerProductos();
    }
}