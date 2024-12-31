using Data.Repository.Interfaces;
using Model.DTO;
using Model.Entity;
using Service.Interfaces;

namespace Service
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public ICollection<ProductoDTO> ObtenerProductos()
        {
            var productos = _productoRepository.ObtenerProductos();
            var productosDTO = new List<ProductoDTO>();

            if (productos == null) throw new Exception("No se han encontrado productos");

            foreach (var producto in productos)
            {
                var productoDTO = new ProductoDTO
                {
                    Id = producto.Id,
                    NombreProducto = producto.NombreProducto,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio
                };

                productosDTO.Add(productoDTO);
            }

            return productosDTO;
        }

        public ProductoDTO ObtenerProducto(int idProducto)
        {
            var producto = _productoRepository.ObtenerProducto(idProducto);

            if (producto == null)
            {
                throw new Exception("Producto no encontrado");
            }

            var productoDTO = new ProductoDTO
            {
                Id = producto.Id,
                NombreProducto = producto.NombreProducto,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio
            };

            return productoDTO;
        }

        public void GuardarProducto(ProductoDTO productoDTO)
        {
            var producto = new Producto(productoDTO);

            _productoRepository.GuardarProducto(producto);
        }

        public void ActualizarProducto(ProductoDTO productoDTO)
        {
            var producto = _productoRepository.ObtenerProducto(productoDTO.Id);

            if (producto == null)
            {
                throw new Exception("Producto no encontrado");
            }

            producto.NombreProducto = productoDTO.NombreProducto;
            producto.Descripcion = productoDTO.Descripcion;
            producto.Precio = productoDTO.Precio;

            _productoRepository.ActualizarProducto(producto);
        }

        public void EliminarProducto(int idProducto)
        {
            var producto = _productoRepository.ObtenerProducto(idProducto);

            if (producto == null)
            {
                throw new Exception("Producto no encontrado");
            }

            _productoRepository.EliminarProducto(producto.Id);
        }
    }
}
