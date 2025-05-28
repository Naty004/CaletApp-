using Domain;
using Infrastructure.Identity;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Repositories
{
    public class Repository : BaseRepository, IRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public Repository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
            : base(context)
        {
            _userManager = userManager;
        }

        // Registrar usuario
        public async Task<IdentityResult> AddUsuario(ApplicationUser usuario, string password)
        {
            var result = await _userManager.CreateAsync(usuario, password);
            if (result.Succeeded)
            {
                Commit();
            }
            return result;
        }

        // Obtener todos los usuarios
        public async Task<List<ApplicationUser>> GetAllUsuarios()
        {
            return await _userManager.Users.ToListAsync();
        }

        // Obtener usuario por Id (string, porque Identity usa string para Id)
        public async Task<ApplicationUser> GetUsuarioById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        // Actualizar usuario
        public async Task<IdentityResult> UpdateUsuario(ApplicationUser usuario)
        {
            var result = await _userManager.UpdateAsync(usuario);
            if (result.Succeeded)
            {
                Commit();
            }
            return result;
        }

        // Eliminar usuario
        public async Task<IdentityResult> DeleteUsuario(ApplicationUser usuario)
        {
            var result = await _userManager.DeleteAsync(usuario);
            if (result.Succeeded)
            {
                Commit();
            }
            return result;
        }

        // Validar si correo o nombre de usuario ya existen
        public async Task<bool> UsuarioExiste(string correo, string nombreUsuario, string id = null)
        {
            var userByEmail = await _userManager.FindByEmailAsync(correo);
            if (userByEmail != null && userByEmail.Id != id)
                return true;

            var userByName = await _userManager.FindByNameAsync(nombreUsuario);
            if (userByName != null && userByName.Id != id)
                return true;

            return false;
        }

        // Validar login - aquí solo validamos email y password
        public async Task<ApplicationUser> ValUsuario(string correo, string contrasena)
        {
            var usuario = await _userManager.FindByEmailAsync(correo);
            if (usuario != null)
            {
                var isValid = await _userManager.CheckPasswordAsync(usuario, contrasena);
                if (isValid)
                    return usuario;
            }
            return null;
        }
    }
}
