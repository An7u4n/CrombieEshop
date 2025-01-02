using Microsoft.EntityFrameworkCore;
using Model.Entity;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<WishList> WishLists { get; set; }

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
                usuario.HasOne(u => u.WishList).WithOne(w => w.Usuario).HasForeignKey<WishList>(w => w.UsuarioId);
            });

            modelBuilder.Entity<Producto>(producto =>
            {
                producto.ToTable("Productos");
                producto.HasKey(p => p.Id);
                producto.Property(p => p.NombreProducto).HasMaxLength(80).IsRequired();
                producto.Property(p => p.Descripcion).HasMaxLength(512).IsRequired();
                producto.Property(p => p.Precio).IsRequired();
            });

            modelBuilder.Entity<WishList>(wishList =>
            {
                wishList.ToTable("WishLists");
                wishList.HasKey(w => w.Id);
                wishList
                    .HasMany(w => w.Productos)
                    .WithMany()
                    .UsingEntity(j => j.ToTable("WishListProducto"));
            });
        }
    }
}
