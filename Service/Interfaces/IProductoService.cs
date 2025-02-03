using Model.DTO;

namespace Service.Interfaces
{
    public interface IProductoService
    {
        void ActualizarProducto(ProductoDTO productoDTO);
        Task EliminarProducto(int idProducto);
        ProductoDTO CrearProducto(ProductoDTO productoDTO);
        ProductoBusquedaDTO BuscarProductos(ProductoParametrosBusquedaDTO parametros);

        ProductoDTO AddCategoria(int idProducto, int idCategoria);
        ProductoDTO ObtenerProducto(int idProducto);
        ICollection<ProductoDTO> ObtenerProductos();
        Task<string> SubirImagenAsync(Stream fileStream, string fileName, int idProducto, string contentType);
    }
}