using Services.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IService
    {
        // Registro de usuarios
        Task AddUsuario(RegistroModel registromodel, string password);

        Task<List<RegistroModel>> GetAllUsuarios();

        // Login
        Task<LoginModel> Login(string correo, string contrasena);

        // Administración de usuarios
        Task<RegistroModel> GetUsuarioById(string id);
        Task UpdateUsuario(RegistroModel model);
        Task DeleteUsuario(string id);
        Task<bool> UsuarioExiste(string correo, string nombreUsuario, string? id = null);

        // Categorías
        Task<List<CategoriaModels>> GetAllCategorias();
        Task<CategoriaModels> GetCategoriaById(int id);
        Task AddCategoria(CategoriaModels categoria);
        Task UpdateCategoria(CategoriaModels categoria);
        Task DeleteCategoria(int id);
    }
}
