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

                //usuario.Property(u => u.NombreDeUsuario)
                //    .IsUnique();
            });

            modelBuilder.Entity<Producto>().ToTable("Productos");

            modelBuilder.Entity<WishList>(wishList =>
            {
                wishList
                    .HasOne(w => w.Usuario)
                    .WithMany(u => u.WishLists)
                    .HasForeignKey(w => w.UsuarioId);

                wishList
                    .HasMany(w => w.Productos)
                    .WithMany(p => p.WishList)
                    .UsingEntity(j => j.ToTable("WishListProducto"));
            });
        }
    }
}
