

using Application.Services;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.IServices;
using Services.Services;

namespace Application
{
    public static class DependencyInjection
    {
        /// <summary>
        /// add services injection here
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            //adicione aqui los servicios que va a usar en la aplicacion
            services.AddTransient<IService, Service>();
            services.AddTransient<IIngresoService, IngresoService>();
            services.AddTransient<ICategoriaService, CategoriaService>();
            services.AddTransient<IGastoService, GastoService>();

            return services;
        }

    }
}