using Domain;

namespace Infrastructure.Repositories.IRepositories
{
    public interface IRepository
    {
        /**Task AddMilk(Milk milk);
        Task<List<Milk>> GetAllMilks();**/

        //Registro de usuarios
        Task AddUsuario(Usuarios usuarios);
        Task<List<Usuarios>> GetAllUsuarios();

        //Login
        Task<Usuarios> ValUsuario(string correo, string contrasena);

        //Admin

        Task<Usuarios> GetUsuarioById(Guid id);
        Task UpdateUsuario(Usuarios usuario);
        Task DeleteUsuario(Guid id);
        Task<bool> UsuarioExiste(string correo, string nombreUsuario, Guid? id = null);


    }
}
