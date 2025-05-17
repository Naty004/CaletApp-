using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.IServices;

namespace MvcTemplate.Controllers
{
    public class RegistroController:Controller
    {
        private IService Service { get; set; }
        public RegistroController(IService service)
        {
            Service = service;
        }

        public IActionResult Registro()
        {
            return View(); // Devuelve la vista de registro
        }

        [HttpGet]
        public IActionResult AddUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUsuario(RegistroModel registro)
        {
            if (ModelState.IsValid)
            {
                await Service.AddUsuario(registro);
                return RedirectToAction("GetLogin","Login");
            }

            return View(registro);
        }

        public async Task<IActionResult> ListUsuarios()
        {
            var usuarios = await Service.GetAllUsuarios();
            return View(usuarios);
        }


    }
}
