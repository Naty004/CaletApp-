using Services.Dtos;

namespace Services.IServices
{
    public interface IService
    {
        /**Task AddMilk(int v, DateTime now);
        Task<List<MilkModel>> GetAllMilks();**/

        // Rgeistro de usuarios
        Task AddUsuario(RegistroModel registromodel);
        Task<List<RegistroModel>> GetAllUsuarios();

        //Login
        Task<LoginModel> Login(string correo, string contrasena);


        // Administración de usuarios
        Task<RegistroModel> GetUsuarioById(Guid id);
        Task UpdateUsuario(RegistroModel model);
        Task DeleteUsuario(Guid id);
        Task<bool> UsuarioExiste(string correo, string nombreUsuario, Guid? id = null);

        // Categorias
        Task<List<CategoriaModels>> GetAllCategorias();
        Task<CategoriaModels> GetCategoriaById(int id);
        Task AddCategoria(CategoriaModels categoria);
        Task UpdateCategoria(CategoriaModels categoria);
        Task DeleteCategoria(int id);

    }
}

