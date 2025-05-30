using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: /Admin/ListUsuarios
        public IActionResult ListUsuarios()
        {
            var usuarios = _userManager.Users.ToList();
            return View(usuarios);
        }

        // POST: /Admin/DeleteUsuario/{id}
        [HttpPost]
        public async Task<IActionResult> DeleteUsuario(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario != null)
            {
                await _userManager.DeleteAsync(usuario);
            }

            return RedirectToAction("ListUsuarios");
        }

        // GET: /Admin/AddUsuario
        [HttpGet]
        public IActionResult AddUsuario()
        {
            return View();
        }

        // POST: /Admin/AddUsuario
        [HttpPost]
        public async Task<IActionResult> AddUsuario(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Email y contraseña son requeridos.");
                return View();
            }

            var nuevoUsuario = new ApplicationUser { UserName = email, Email = email };
            var resultado = await _userManager.CreateAsync(nuevoUsuario, password);

            if (resultado.Succeeded)
            {
                await _userManager.AddToRoleAsync(nuevoUsuario, "User"); // Rol por defecto
                return RedirectToAction("ListUsuarios");
            }

            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }
    }
}

