using Infrastructure.Repositories.IRepositories;
using Infrastructure.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.Repositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var conection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(conection, ServerVersion.AutoDetect(conection)));


            //adicione aqui los repositorios que va a usar en la aplicacion
            services.AddScoped<IRepository, Repository>();
            return services;
        }
    }
}
