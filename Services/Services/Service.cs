﻿using AutoMapper;
using Infrastructure.Identity; // <- Aquí está ApplicationUser
using Infrastructure.Repositories.IRepositories;
using Services.Dtos;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{
    public class Service : IService
    {
        private IRepository Repository { get; set; }
        private IMapper Mapper { get; set; }

        public Service(IRepository repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        // Registrar usuario
        public async Task AddUsuario(RegistroModel registroModel, string password)
        {
            var entity = Mapper.Map<ApplicationUser>(registroModel);
            await Repository.AddUsuario(entity, password);
        }

        // Obtener todos los usuarios
        public async Task<List<RegistroModel>> GetAllUsuarios()
        {
            var usuarios = await Repository.GetAllUsuarios();
            return Mapper.Map<List<RegistroModel>>(usuarios);
        }

        // Obtener usuario por ID (cambiar Guid a string)
        public async Task<RegistroModel> GetUsuarioById(string id)
        {
            var usuario = await Repository.GetUsuarioById(id);
            return Mapper.Map<RegistroModel>(usuario);
        }

        // Actualizar usuario
        public async Task UpdateUsuario(RegistroModel model)
        {
            var entity = Mapper.Map<ApplicationUser>(model);
            await Repository.UpdateUsuario(entity);
        }

        // Eliminar usuario (cambiar Guid a string)
        public async Task DeleteUsuario(string id)
        {
            var usuario = await Repository.GetUsuarioById(id);
            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            var result = await Repository.DeleteUsuario(usuario);
            if (!result.Succeeded)
            {
                throw new Exception("Error al eliminar usuario");
            }
        }

        // Validar si el usuario o correo ya existen
        // Cambiar Guid? id a string id (opcional, puede ser null)
        public async Task<bool> UsuarioExiste(string correo, string nombreUsuario, string? id = null)
        {
            return await Repository.UsuarioExiste(correo, nombreUsuario, id);
        }

        // Login
        public async Task<LoginModel> Login(string correo, string contrasena)
        {
            var usu = await Repository.ValUsuario(correo, contrasena);
            if (usu == null) return null;
            return Mapper.Map<LoginModel>(usu);
        }

        // Métodos no implementados...
        public Task<List<CategoriaModels>> GetAllCategorias()
        {
            throw new NotImplementedException();
        }

        public Task<CategoriaModels> GetCategoriaById(int id)
        {
            throw new NotImplementedException();
        }

        public Task AddCategoria(CategoriaModels categoria)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategoria(CategoriaModels categoria)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategoria(int id)
        {
            throw new NotImplementedException();
        }
    }
}
