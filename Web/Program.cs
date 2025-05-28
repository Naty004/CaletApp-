using Application;
using AutoMapper;
using Infrastructure.Identity;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Common;

namespace MvcTemplate;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Validar cadena de conexión
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");
        }

        // AutoMapper
        var mappingConfiguration = new MapperConfiguration(m => m.AddProfile(new MProfile()));
        IMapper mapper = mappingConfiguration.CreateMapper();
        builder.Services.AddSingleton(mapper);

        // Servicios personalizados e infraestructura (DbContext, repositorios)
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddServices(builder.Configuration);

        // Identity usando ApplicationUser y DbContext configurado
        builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false; // Puedes ajustar según ambiente
        })
        .AddEntityFrameworkStores<ApplicationDbContext>();

        // Configuración de la cookie de autenticación
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Login/GetLogin";
            options.AccessDeniedPath = "/Login/GetLogin";
        });

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configuración del pipeline HTTP
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();  // Habilita autenticación
        app.UseAuthorization();   // Habilita autorización

        // MapStaticAssets y WithStaticAssets son métodos personalizados,
        // asegúrate que están implementados o comenta estas líneas
        // app.MapStaticAssets();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Login}/{action=GetLogin}/{id?}");
        // .WithStaticAssets();  // Descomenta solo si tienes esta extensión

        app.MapRazorPages();
        // .WithStaticAssets();    // Descomenta solo si tienes esta extensión

        app.Run();
    }
}
