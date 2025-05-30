using Application;
using AutoMapper;
using Infrastructure.Identity;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Common;
using Infrastructure.Data;


namespace MvcTemplate;

public class Program
{
    public static async Task Main(string[] args)
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
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
      {
        options.SignIn.RequireConfirmedAccount = false;
      })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

       

        // Configuraci�n de la cookie de autenticaci�n
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Login/GetLogin";
            options.AccessDeniedPath = "/Login/GetLogin";
        });

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();


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

        app.UseAuthentication();  // Habilita autenticacion
        app.UseAuthorization();   // Habilita autorizacion

        // MapStaticAssets y WithStaticAssets son metodos personalizados,
        // aseg�rate que est�n implementados o comenta estas lineas
         app.MapStaticAssets();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Login}/{action=GetLogin}/{id?}");
        // .WithStaticAssets();  // Descomenta solo si tienes esta extension

        app.MapRazorPages();
        // .WithStaticAssets();    // Descomenta solo si tienes esta extension

        using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbSeeder.SeedAsync(services);
}
             
        app.Run();
    }
}
