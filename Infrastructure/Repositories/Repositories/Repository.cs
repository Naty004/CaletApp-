using Domain;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Repositories
{
    public class Repository : BaseRepository, IRepository
    {
        public Repository(ApplicationDbContext _context) : base(_context)
        {
        }

        // Registro de usuarios
        public async Task AddUsuario(Usuarios usuarios)
        {
            try
            {
                Begin();
                context.Usuarios.Add(usuarios);
                await context.SaveChangesAsync();
                Commit();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar usuario", ex);
            }
        }

        public Task<List<Usuarios>> GetAllUsuarios()
        {
            return context.Usuarios.ToListAsync();
        }

        // Obtener usuario por ID
        public async Task<Usuarios> GetUsuarioById(Guid id)
        {
            return await context.Usuarios.FindAsync(id);
        }

        // Actualizar usuario
        public async Task UpdateUsuario(Usuarios usuario)
        {
            try
            {
                Begin();
                context.Usuarios.Update(usuario);
                await context.SaveChangesAsync();
                Commit();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar usuario", ex);
            }
        }

        // Eliminar usuario
        public async Task DeleteUsuario(Guid id)
        {
            try
            {
                Begin();
                var usuario = await context.Usuarios.FindAsync(id);
                if (usuario != null)
                {
                    context.Usuarios.Remove(usuario);
                    await context.SaveChangesAsync();
                    Commit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar usuario", ex);
            }
        }

        // Validar si correo o nombre de usuario ya existen
        public async Task<bool> UsuarioExiste(string correo, string nombreUsuario, Guid? id = null)
        {
            return await context.Usuarios.AnyAsync(u =>
                (u.CorreoElectronico == correo || u.NombreUsuario == nombreUsuario) &&
                (!id.HasValue || u.Id != id.Value));
        }

        // Login
        public async Task<Usuarios> ValUsuario(string correo, string contrasena)
        {
            return await context.Usuarios.FirstOrDefaultAsync(u =>
u.CorreoElectronico == correo && u.Contrasena == contrasena);
        }
    }
}
