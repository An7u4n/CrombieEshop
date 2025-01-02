using Data.Repository.Interfaces;
using Model.Entity;

namespace Data.Repository
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDbContext _context;
        public ProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        public ICollection<Producto> ObtenerProductos()
        {
            return _context.Productos.ToList();
        }

        public Producto ObtenerProducto(int idProducto)
        {
            return _context.Productos.FirstOrDefault(p => p.Id == idProducto);
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
    }
}
