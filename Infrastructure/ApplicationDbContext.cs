using Domain;
using Infrastructure.Identity;  // ← Correcto
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relación Gasto -> Categoria
            builder.Entity<Gasto>()
                .HasOne(g => g.Categoria)
                .WithMany(c => c.Gastos)
                .HasForeignKey(g => g.CategoriaId)
                .OnDelete(DeleteBehavior.Cascade);

            // No configuramos Gasto -> Usuario directamente, solo por UsuarioId
            builder.Entity<Gasto>()
                .Property(g => g.UsuarioId)
                .IsRequired();

            // Solo configuración por UsuarioId
            builder.Entity<Ingreso>()
                .Property(i => i.UsuarioId)
                .IsRequired();

            builder.Entity<Categoria>()
                .Property(c => c.UsuarioId)
                .IsRequired();
        }
    }
}
