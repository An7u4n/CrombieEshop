using Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.DTO;
using Model.Entity;

namespace Data.Repository
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDbContext _context;
        private const int DEFAULT_PAGE_SIZE = 5;
        public ProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        public ICollection<Producto> ObtenerProductos()
        {
            return _context.Productos.Include(p => p.Categorias).Include(p => p.ImagenesProducto).ToList();
        }

        public Producto ObtenerProducto(int idProducto)
        {
            return _context.Productos.Include(p => p.Categorias).Include(p => p.ImagenesProducto).FirstOrDefault(p => p.Id == idProducto);
        }

        public Producto CrearProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            _context.SaveChanges();
            return producto;
        }

        public void ActualizarProducto(Producto producto)
        {
            _context.Productos.Update(producto);
            _context.SaveChanges();
        }

        public void EliminarProducto(int idProducto)
        {
            var producto = ObtenerProducto(idProducto);
            _context.Productos.Remove(producto);
            _context.SaveChanges();
        }

        public ProductoBusquedaDTO BuscarProductos(ProductoParametrosBusquedaDTO parametros)
        {
            int page = parametros.Page ?? 1;
            page = Math.Max(page, 1);
            int pageSize = parametros.PageSize ?? DEFAULT_PAGE_SIZE;
            var query = _context.Productos.Include(p => p.ImagenesProducto).AsQueryable();
            if (!string.IsNullOrEmpty(parametros.NombreProducto))
            {
                query = query.Where(p => p.NombreProducto.Contains(parametros.NombreProducto));
            }
            if (parametros.PrecioMinimo.HasValue)
            {
                query = query.Where(p => p.Precio >= parametros.PrecioMinimo);
            }
            if (parametros.PrecioMaximo.HasValue)
            {
                query = query.Where(p => p.Precio <= parametros.PrecioMaximo);
            }
            int totalPages = (int)Math.Ceiling(query.Count() / (double)pageSize);
            page = Math.Min(page, totalPages);
            var productos = query.Skip((page - 1) * pageSize).Take(pageSize).ToList().Select(p => new ProductoDTO(p)).ToList();
            return new ProductoBusquedaDTO
            {
                Productos = productos,
                CurrentPage = page,
                TotalPages = totalPages
            };
        }
    }
}
