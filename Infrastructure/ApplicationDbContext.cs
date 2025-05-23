using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) // Llama al constructor de la clase base
        { }
        //adicionar aqui las entidades que se van a usar en la aplicacion
        //public DbSet<Milk> Milks { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Gastos> Gastos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Configuración de la relación entre Gastos y Categorias
            builder.Entity<Gastos>()
                .HasOne(g => g.Categoria)
                .WithMany(c => c.Gastos)
                .HasForeignKey(g => g.CategoriaId)
                .OnDelete(DeleteBehavior.Cascade); // Configura la eliminación en cascada si es necesario;
                
        }
    }
}