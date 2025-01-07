using Data.Repository.Interfaces;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class WishListItemRepository : IWishListItemsRepository
    {
        private readonly AppDbContext _context;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IProductoRepository _productoRepository;

        public WishListItemRepository(AppDbContext context, IUsuarioRepository usuarioRepository, IProductoRepository productoRepository)
        {
            _context = context;
            _usuarioRepository = usuarioRepository;
            _productoRepository = productoRepository;
        }
        void IWishListItemsRepository.AgregarProductoWishList(int idUsuario, int idProducto)
        {
            var usuario = _usuarioRepository.ObtenerUsuario(idUsuario);
            var producto = _productoRepository.ObtenerProducto(idProducto);
            if (usuario == null || producto == null)
            {
                throw new ArgumentException("Usuario or Producto not found.");
            }
            var existingWishListItem = _context.WishListsItems.FirstOrDefault(w => w.Usuario.Id == idUsuario && w.Producto.Id == idProducto);
            if(existingWishListItem != null)
            {
                return;
            }
            var wishListItem = new WishListItem
            {
                Usuario = usuario,
                Producto = producto
            };
            _context.WishListsItems.Add(wishListItem);
            _context.SaveChanges();
        }

        void IWishListItemsRepository.EliminarProductoWishList(int idUsuario, int idProducto)
        {
            var wishListItem = _context.WishListsItems.FirstOrDefault(w => w.Usuario.Id == idUsuario && w.Producto.Id == idProducto);
            _context.WishListsItems.Remove(wishListItem);
            _context.SaveChanges();
        }

        ICollection<Producto> IWishListItemsRepository.ObtenerProductosWishList(int idUsuario)
        {
            var productos = _context.WishListsItems.Where(w => w.Usuario.Id == idUsuario).Select(w => w.Producto).ToList();
            return productos;
        }
    }
}
