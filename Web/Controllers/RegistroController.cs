using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using System.Threading.Tasks;

namespace MvcTemplate.Controllers
{
    public class RegistroController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegistroController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult AddUsuario()
        {
            return View(); // Devuelve la vista AddUsuario.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> AddUsuario(RegistroModel registro)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = registro.CorreoElectronico,
                    Email = registro.CorreoElectronico,
                    Nombres = registro.Nombres,       // <-- Asignar nombre
                    Apellidos = registro.Apellidos    // <-- Asignar apellido
                };

                var result = await _userManager.CreateAsync(user, registro.Contrasena);
                if (result.Succeeded)
                {
                    return RedirectToAction("GetLogin", "Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(registro);
        }
    }
}
