using Model.DTO;

namespace Service.Interfaces
{
    public interface IProductoService
    {
        void ActualizarProducto(ProductoDTO productoDTO);
        void EliminarProducto(int idProducto);
        ProductoDTO CrearProducto(ProductoDTO productoDTO);
        ProductoBusquedaDTO BuscarProductos(ProductoParametrosBusquedaDTO parametros);

        ProductoDTO AddCategoria(int idProducto, int idCategoria);
        ProductoDTO ObtenerProducto(int idProducto);
        Task<ProductoDTO> SubirImagen(int idProducto, Stream stream, string fileName,string contentType);
        ICollection<ProductoDTO> ObtenerProductos();
    }
}