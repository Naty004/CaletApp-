using Infrastructure.Identity; // Importa donde está ApplicationUser
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using System.Threading.Tasks;

namespace MvcTemplate.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
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
                var user = await _userManager.FindByEmailAsync(model.CorreoElectronico);
                if (user != null)
                {
                    // Aquí el PasswordSignInAsync acepta ApplicationUser
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
