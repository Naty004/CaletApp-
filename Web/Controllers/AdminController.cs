using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.IServices;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MvcTemplate.Controllers
{

    public class AdminController : Controller
    {
        private readonly IService _service;

      public AdminController(IService service)
        {
            _service = service;
        }
            // Vista para registrar un nuevo usuario
            [HttpGet]
        public IActionResult AddUsuario()
        {
            return View();
        }

        // Crear usuario
        [HttpPost]
        public async Task<IActionResult> AddUsuario(RegistroModel registro)
        {
            if (!ModelState.IsValid)
                return View(registro);

            // Validar unicidad de correo y usuario
            if (await _service.UsuarioExiste(registro.CorreoElectronico, registro.NombreUsuario))
            {
                ModelState.AddModelError("", "El correo o nombre de usuario ya está en uso.");
                return View(registro);
            }

            await _service.AddUsuario(registro);
            return RedirectToAction("ListUsuarios");
        }

        // Listar usuarios
        public async Task<IActionResult> ListUsuarios()
        {
            var usuarios = await _service.GetAllUsuarios();
            return View(usuarios);
        }

        // Vista para editar usuario
        [HttpGet]
        public async Task<IActionResult> EditUsuario(Guid id)
        {
            var usuario = await _service.GetUsuarioById(id);
            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // Modificar usuario
        [HttpPost]
        public async Task<IActionResult> EditUsuario(RegistroModel usuario)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            var existe = await _service.UsuarioExiste(usuario.CorreoElectronico, usuario.NombreUsuario, usuario.Id);
            if (existe)
            {
                ModelState.AddModelError("", "El correo o nombre de usuario ya está en uso por otro usuario.");
                return View(usuario);
            }

            await _service.UpdateUsuario(usuario);
            return RedirectToAction("ListUsuarios");
        }

        // Eliminar usuario
        [HttpPost]
        public async Task<IActionResult> DeleteUsuario(Guid id)
        {
            await _service.DeleteUsuario(id);
            return RedirectToAction("ListUsuarios");
        }
    }
}
