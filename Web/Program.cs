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

        // Validar cadena de conexi�n
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("La cadena de conexi�n 'DefaultConnection' no est� configurada.");
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
            options.SignIn.RequireConfirmedAccount = false; // Puedes ajustar seg�n ambiente
        })
        .AddEntityFrameworkStores<ApplicationDbContext>();

        // Configuraci�n de la cookie de autenticaci�n
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Login/GetLogin";
            options.AccessDeniedPath = "/Login/GetLogin";
        });

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configuraci�n del pipeline HTTP
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

        app.UseAuthentication();  // Habilita autenticaci�n
        app.UseAuthorization();   // Habilita autorizaci�n

        // MapStaticAssets y WithStaticAssets son m�todos personalizados,
        // aseg�rate que est�n implementados o comenta estas l�neas
        // app.MapStaticAssets();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Login}/{action=GetLogin}/{id?}");
        // .WithStaticAssets();  // Descomenta solo si tienes esta extensi�n

        app.MapRazorPages();
        // .WithStaticAssets();    // Descomenta solo si tienes esta extensi�n

        app.Run();
    }
}
