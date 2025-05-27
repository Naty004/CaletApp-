using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using System.Threading.Tasks;

namespace MvcTemplate.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
                // Buscar usuario por correo (user name)
                var user = await _userManager.FindByEmailAsync(model.CorreoElectronico);
                if (user != null)
                {
                    // Intentar login con contraseña
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Contrasena, isPersistent: false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Credenciales incorrectas.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("GetLogin");
        }
    }
}
