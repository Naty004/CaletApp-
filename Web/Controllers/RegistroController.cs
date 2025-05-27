using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using System.Threading.Tasks;

namespace MvcTemplate.Controllers
{
    public class RegistroController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RegistroController(UserManager<IdentityUser> userManager)
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
                var user = new IdentityUser { UserName = registro.CorreoElectronico, Email = registro.CorreoElectronico };

                var result = await _userManager.CreateAsync(user, registro.Contrasena);
                if (result.Succeeded)
                {
                    // Usuario creado exitosamente, redirigimos a login
                    return RedirectToAction("GetLogin", "Login");
                }

                // Agrega los errores para mostrar en la vista
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(registro);
        }
    }
}
