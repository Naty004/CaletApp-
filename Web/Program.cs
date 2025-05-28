using Application;
using AutoMapper;
using Infrastructure.Identity;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Services.Common;

namespace MvcTemplate;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // AutoMapper
        var mappingConfiguration = new MapperConfiguration(m => m.AddProfile(new MProfile()));
        IMapper mapper = mappingConfiguration.CreateMapper();
        builder.Services.AddSingleton(mapper);

        // Servicios personalizados
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddServices(builder.Configuration);

        // Identity usando ApplicationUser
        builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            options.SignIn.RequireConfirmedAccount = false)
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

        app.MapStaticAssets();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Login}/{action=GetLogin}/{id?}")
            .WithStaticAssets();

        app.MapRazorPages().WithStaticAssets();

        app.Run();
    }
}
