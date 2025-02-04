using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Model.Entity;
using Model.Enums;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<WishListItem> WishListsItems { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<ProductoImagen> ProductoImagenes { get; set; }

        public DbSet<CarritoItem> CarritoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarritoItem>(carritoItem =>
            {
                carritoItem.ToTable("CarritoItems");
                carritoItem.HasKey(ci => new { ci.UsuarioId, ci.ProductoId });
                carritoItem.HasOne(ci => ci.Usuario).WithMany(u => u.CarritoItems).HasForeignKey(ci => ci.UsuarioId);
                carritoItem.HasOne(ci => ci.Producto).WithOne().HasForeignKey<CarritoItem>(ci => ci.ProductoId);
                carritoItem.Property(ci => ci.Cantidad).IsRequired();
                carritoItem.Property(ci => ci.CreatedAt).HasDefaultValueSql("GETDATE()");
            });
            modelBuilder.Entity<Usuario>(usuario =>
            {
                usuario.ToTable("Usuarios");
                usuario.HasKey(u => u.Id);
                usuario.Property(u => u.NombreDeUsuario).HasMaxLength(80).IsRequired();
                usuario.HasIndex(u => u.NombreDeUsuario).IsUnique();
                usuario.Property(u => u.Nombre).HasMaxLength(80).IsRequired();
                usuario.Property(u => u.Email).HasMaxLength(320).IsRequired();
                usuario.HasIndex(u => u.Email).IsUnique();
                usuario.OwnsOne(u => u.Imagen, img =>
                {
                    img.Property(i => i.FotoPerfilKey).HasMaxLength(64);
                }
                );
            });

            modelBuilder.Entity<Producto>(producto =>
            {
                producto.ToTable("Productos");
                producto.HasKey(p => p.Id);
                producto.Property(p => p.NombreProducto).HasMaxLength(80).IsRequired();
                producto.Property(p => p.Descripcion).HasMaxLength(512).IsRequired();
                producto.Property(p => p.Precio).IsRequired();
                producto.HasMany(p => p.Categorias).WithMany(c => c.Productos);
                producto.HasMany(p => p.ImagenesProducto).WithOne(pi => pi.Producto).HasForeignKey(pi => pi.ProductoId);
            });

            modelBuilder.Entity<Categoria>(c =>
            {
                c.ToTable("Categorias");
                c.HasKey(c => c.Id);
                c.Property(c => c.Nombre).HasMaxLength(80).IsRequired();
                c.HasIndex(c => c.Nombre).IsUnique();
            });

            modelBuilder.Entity<WishListItem>(wishListItems =>
            {
                wishListItems.ToTable("WishListItems");
                wishListItems.HasKey(wli => new { wli.UsuarioId, wli.ProductoId });
                wishListItems.HasOne(wli => wli.Usuario).WithMany(u => u.WishListItems).HasForeignKey(wli => wli.UsuarioId);
                wishListItems.HasOne(wli => wli.Producto).WithOne().HasForeignKey<WishListItem>(wli => wli.ProductoId);
                wishListItems.Property(wli => wli.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<ProductoImagen>(productoImagen =>
            {
                productoImagen.ToTable("ProductoImagenes");
                productoImagen.HasKey(pi => pi.Id);
                productoImagen.Property(pi => pi.UrlImagen).HasMaxLength(512).IsRequired();
                productoImagen.HasOne(pi => pi.Producto).WithMany(p => p.ImagenesProducto).HasForeignKey(pi => pi.ProductoId);
            });
        }
    }
}
