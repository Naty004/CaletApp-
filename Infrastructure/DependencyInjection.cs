using Application.Repositories;
using Infrastructure.Repositories.IRepositories;
using Infrastructure.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var conection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(conection, ServerVersion.AutoDetect(conection)));

            // Repositorios usados en la aplicación
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IGastoRepository, GastoRepository>();
            services.AddScoped<IIngresoRepository, IngresoRepository>();

            return services;
        }
    }
}
