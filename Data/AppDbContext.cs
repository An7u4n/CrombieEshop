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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(usuario =>
            {
                usuario.ToTable("Usuarios");
                usuario.HasKey(u => u.Id);
                usuario.Property(u => u.NombreDeUsuario).HasMaxLength(80).IsRequired();
                usuario.Property(u => u.Nombre).HasMaxLength(80).IsRequired();
                usuario.Property(u => u.Contrasena).HasMaxLength(256).IsRequired();
                usuario.Property(u => u.Email).HasMaxLength(320).IsRequired();
                usuario.Property(u => u.Role).IsRequired().HasDefaultValue(Role.User);
                //var hasher = new PasswordHasher<Usuario>();
                //var adminUser = new Usuario
                //{
                //    Id = 1,
                //    NombreDeUsuario = "admin",
                //    Nombre = "Administrador",
                //    Email = "admin@admin.com",
                //    Role = Role.Admin,
                //    Contrasena = hasher.HashPassword(null, "admin") // Note: passing null as first parameter
                //};

                //usuario.HasData(adminUser);
            });

            modelBuilder.Entity<Producto>(producto =>
            {
                producto.ToTable("Productos");
                producto.HasKey(p => p.Id);
                producto.Property(p => p.NombreProducto).HasMaxLength(80).IsRequired();
                producto.Property(p => p.Descripcion).HasMaxLength(512).IsRequired();
                producto.Property(p => p.Precio).IsRequired();
            });
            modelBuilder.Entity<WishListItem>(wishListItems => {
                wishListItems.ToTable("WishListItems");
                wishListItems.HasKey(wli => new { wli.UsuarioId, wli.ProductoId });
                wishListItems.HasOne(wli => wli.Usuario).WithMany(u => u.WishListItems).HasForeignKey(wli => wli.UsuarioId);
                wishListItems.HasOne(wli => wli.Producto).WithOne().HasForeignKey<WishListItem>(wli => wli.ProductoId);
                wishListItems.Property(wli => wli.CreatedAt).HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
