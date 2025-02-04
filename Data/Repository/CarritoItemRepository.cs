using Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class CarritoItemRepository : ICarritoItemRepository
    {
        private readonly AppDbContext _context;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IProductoRepository _productoRepository;

        public CarritoItemRepository(AppDbContext context, IUsuarioRepository usuarioRepository, IProductoRepository productoRepository)
        {
            _context = context;
            _usuarioRepository = usuarioRepository;
            _productoRepository = productoRepository;
        }
        public void EliminarProductoCarrito(int idUsuario, int idProducto)
        {
            var item = _context.CarritoItems.FirstOrDefault(w => w.Usuario.Id == idUsuario && w.Producto.Id == idProducto);
            _context.CarritoItems.Remove(item);
            _context.SaveChanges();
        }

        public ICollection<CarritoItem> ObtenerCarrito(int idUsuario)
        {
            var items = _context.CarritoItems.Where(w => w.Usuario.Id == idUsuario).ToList();
            return items;
        }

        public void SetProductoCarrito(int idUsuario, int idProducto, int cantidad)
        {
            var existingCarritoItem = _context.CarritoItems
                .FirstOrDefault(w => w.Usuario.Id == idUsuario && w.Producto.Id == idProducto);

            if (existingCarritoItem != null)
            {
                existingCarritoItem.Cantidad = cantidad;
            }
            else
            {
                var usuario = _usuarioRepository.ObtenerUsuario(idUsuario);
                var producto = _productoRepository.ObtenerProducto(idProducto);

                if (usuario == null || producto == null) return;

                var newItem = new CarritoItem
                {
                    Usuario = usuario,
                    Producto = producto,
                    Cantidad = cantidad
                };
                _context.CarritoItems.Add(newItem);
            }

            _context.SaveChanges();
        }

        public void SetProductosCarrito(int idUsuario, ICollection<(int idProducto, int cantidad)> items)
        {
            if (items == null || !items.Any()) return;

            var idsProductos = items.Select(i => i.idProducto).ToList();

            var existingItems = _context.CarritoItems
                .Where(w => w.Usuario.Id == idUsuario && idsProductos.Contains(w.Producto.Id))
                .ToList();

            var itemDict = items.ToDictionary(i => i.idProducto);

            // Update existing items
            foreach (var existingItem in existingItems)
            {
                if (itemDict.TryGetValue(existingItem.Producto.Id, out var newItem))
                {
                    existingItem.Cantidad = newItem.cantidad;
                }
            }

            // Add new items
            var existingProductIds = existingItems.Select(e => e.Producto.Id).ToHashSet();
            var newItems = items.Where(i => !existingProductIds.Contains(i.idProducto)).ToList();

            var usuario = _usuarioRepository.ObtenerUsuario(idUsuario);
            if (usuario == null) throw new Exception("Usuario no encontrado");

            var productos = _productoRepository.ObtenerProductos(newItems.Select(i => i.idProducto).ToList())
                .ToDictionary(p => p.Id);

            var newItemEntities = newItems
                .Where(i => productos.ContainsKey(i.idProducto)) // Ensure product exists
                .Select(i => new CarritoItem
                {
                    Usuario = usuario,
                    Producto = productos[i.idProducto],
                    Cantidad = i.cantidad
                })
                .ToList();

            _context.CarritoItems.AddRange(newItemEntities);
            _context.SaveChanges();
        }


    }
}
