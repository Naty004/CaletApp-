using Infrastructure.Identity; // Para ApplicationUser
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.IRepositories
{
    public interface IRepository
    {
        // Registro de usuario con password
        Task<IdentityResult> AddUsuario(ApplicationUser usuario, string password);

        // Obtener todos los usuarios
        Task<List<ApplicationUser>> GetAllUsuarios();

        // Validar login
        Task<ApplicationUser> ValUsuario(string correo, string contrasena);

        // Obtener usuario por Id (string)
        Task<ApplicationUser> GetUsuarioById(string id);

        // Actualizar usuario
        Task<IdentityResult> UpdateUsuario(ApplicationUser usuario);

        // Eliminar usuario
        Task<IdentityResult> DeleteUsuario(ApplicationUser usuario);

        // Validar si correo o nombre de usuario ya existen
        Task<bool> UsuarioExiste(string correo, string nombreUsuario, string id = null);
    }
}
