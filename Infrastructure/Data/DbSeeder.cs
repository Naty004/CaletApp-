using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class DbSeeder
    {
        private const string AdminEmail = "admin@tuapp.com";
        private const string AdminPassword = "Admin123!"; // Cámbialo luego en producción
        private const string AdminRole = "Admin";

        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Crear el rol Admin si no existe
            if (!await roleManager.RoleExistsAsync(AdminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(AdminRole));
            }

            // 2. Crear el usuario admin si no existe
            var adminUser = await userManager.FindByEmailAsync(AdminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = AdminEmail,
                    Email = AdminEmail,
                    Apellidos = "Admin",
                    Nombres = "Administrador",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, AdminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("No se pudo crear el usuario administrador: " +
                        string.Join(", ", result.Errors));
                }
            }

            // 3. Asignar rol Admin al usuario si aún no lo tiene
            if (!await userManager.IsInRoleAsync(adminUser, AdminRole))
            {
                await userManager.AddToRoleAsync(adminUser, AdminRole);
            }
        }
    }
}
