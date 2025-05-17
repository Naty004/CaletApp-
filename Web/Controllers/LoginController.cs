using Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;

namespace MvcTemplate.Controllers
{
    public class LoginController:Controller
    {
        private IService Service { get; set; }
        public LoginController(IService service)
        {
            Service = service;
        }

        [HttpGet]
        public IActionResult GetLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetLogin(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var usu = await Service.Login(model.CorreoElectronico, model.Contrasena);
                if (usu != null)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Credenciales incorrectas.");
            }

            return View(model);
        }

    }
}
