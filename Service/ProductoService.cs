using Data.Repository.Interfaces;
using Model.DTO;
using Model.Entity;
using Service.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace Service
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IS3Service _s3Service;
        public ProductoService(IProductoRepository productoRepository, ICategoriaRepository categoriaRepository, IS3Service s3Service)
        {
            _productoRepository = productoRepository;
            _categoriaRepository = categoriaRepository;
            _s3Service = s3Service;
        }

        public ICollection<ProductoDTO> ObtenerProductos()
        {
            var productos = _productoRepository.ObtenerProductos();
            var productosDTO = new List<ProductoDTO>();

            if (productos == null) throw new Exception("No se han encontrado productos");

            productosDTO = productos.Select(p => new ProductoDTO(p)).ToList();

            return productosDTO;
        }

        public ProductoDTO ObtenerProducto(int idProducto)
        {
            var producto = _productoRepository.ObtenerProducto(idProducto);

            if (producto == null)
            {
                throw new Exception("Producto no encontrado");
            }

            var productoDTO = new ProductoDTO(producto);

            return productoDTO;
        }

        public ProductoDTO CrearProducto(ProductoDTO productoDTO)
        {
            var producto = new Producto(productoDTO);

            var productoCreado = _productoRepository.CrearProducto(producto);

            return new ProductoDTO(productoCreado);
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

        public ProductoBusquedaDTO BuscarProductos(ProductoParametrosBusquedaDTO parametros)
        {
            return _productoRepository.BuscarProductos(parametros);
        }

        public ProductoDTO AddCategoria(int idProducto, int idCategoria)
        {
            var producto = _productoRepository.ObtenerProducto(idProducto);
            var categoria = _categoriaRepository.ObtenerCategoria(idCategoria);
            if (producto == null)
            {
                throw new Exception("Producto no encontrado");
            }
            if (categoria == null)
            {
                throw new Exception("Categoria no encontrada");
            }
            producto.Categorias.Add(categoria);
            _productoRepository.ActualizarProducto(producto);
            return new ProductoDTO(producto);
        }

        public async Task<string> SubirImagenAsync(Stream fileStream, string fileName, int idProducto, string contentType)
        {
            try
            {
                var mimeValidos = new[] { "image/jpeg", "image/png", "image/gif" };

                if (!mimeValidos.Contains(contentType))
                {
                    throw new InvalidOperationException("El archivo no es una imagen válida.");
                }

                var producto = _productoRepository.ObtenerProducto(idProducto);
                if(producto == null) throw new Exception("Producto no encontrado");

                var fileKey = Get3Key(fileName, idProducto);
                var imagenUrl = await _s3Service.SubirImagenAsync(fileStream, fileKey, contentType);

                producto.ImagenesProducto.Add(new ProductoImagen(producto, imagenUrl));
                _productoRepository.ActualizarProducto(producto);
                return imagenUrl;
            }
            catch(Exception ex)
            {
                throw new Exception("Error al subir imagen: " + ex.Message);
            }
        }

        private static string Get3Key(string fileName, int idProducto)
        {
            return $"productos/{idProducto}/{fileName}";
        }
    }
}
