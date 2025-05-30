using Application.Services;
using Domain;
using Infrastructure.Identity;  
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoriaController(ICategoriaService categoriaService, UserManager<ApplicationUser> userManager)
        {
            _categoriaService = categoriaService;
            _userManager = userManager;
        }

        // GET: /Categoria
        public async Task<IActionResult> Index()
        {
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null)
                return RedirectToAction("GetLogin", "Login");

            var categorias = await _categoriaService.ObtenerCategoriasVisiblesPorUsuarioAsync(usuarioId);
            return View("CategoriasIndex", categorias);

        }

        // GET: /Categoria/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var categoria = await _categoriaService.ObtenerCategoriaPorIdAsync(id);
            if (categoria == null || !categoria.Visible)
                return NotFound();

            return View(categoria);
        }

        // GET: /Categoria/Create
        public IActionResult Create()
        {
            return View();
        }

         // POST: /Categoria/Create
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(string nombre, string? descripcion, double porcentajeMaximoMensual)
{
    if (string.IsNullOrWhiteSpace(nombre))
        ModelState.AddModelError("Nombre", "El nombre es obligatorio.");

    if (porcentajeMaximoMensual < 0 || porcentajeMaximoMensual > 100)
        ModelState.AddModelError("PorcentajeMaximoMensual", "El porcentaje debe estar entre 0 y 100.");

    if (!ModelState.IsValid)
        return View();

    var usuarioId = await ObtenerUsuarioIdAsync();
    if (usuarioId == null)
        return RedirectToAction("GetLogin", "Login");

    try
    {
        var resultado = await _categoriaService.AgregarCategoriaAsync(nombre, descripcion, porcentajeMaximoMensual, usuarioId);
        return RedirectToAction(nameof(Index));
    }
    catch (InvalidOperationException ex)
    {
        // Error de negocio: nombre duplicado o porcentaje total excedido
        ModelState.AddModelError("", ex.Message);
    }
    catch (Exception)
    {
        // Error genérico inesperado
        ModelState.AddModelError("", "Ocurrió un error inesperado. Intenta de nuevo.");
    }

    // Si algo falla, vuelve a mostrar la vista con los errores
    return View();
}


        // GET: /Categoria/EditPorcentaje/{id}
        public async Task<IActionResult> EditPorcentaje(Guid id)
        {
            var categoria = await _categoriaService.ObtenerCategoriaPorIdAsync(id);
            if (categoria == null || !categoria.Visible)
                return NotFound();

            return View(categoria);
        }

        // POST: /Categoria/EditPorcentaje/{id}
     [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> EditPorcentaje(Guid id, double nuevoPorcentaje)
{
    if (nuevoPorcentaje < 0 || nuevoPorcentaje > 100)
    {
        ModelState.AddModelError("nuevoPorcentaje", "El porcentaje debe estar entre 0 y 100.");
        var cat = await _categoriaService.ObtenerCategoriaPorIdAsync(id);
        return View(cat);
    }

    var usuarioId = await ObtenerUsuarioIdAsync();
    if (usuarioId == null)
        return RedirectToAction("GetLogin", "Login");

    try
    {
        await _categoriaService.AsignarPorcentajeAsync(id, nuevoPorcentaje, usuarioId);
        return RedirectToAction(nameof(Index));
    }
    catch (InvalidOperationException ex)
    {
        // Mensaje de negocio: suma total > 100%
        ModelState.AddModelError("", ex.Message);
    }
    catch (Exception)
    {
        // Error inesperado
        ModelState.AddModelError("", "Ocurrió un error inesperado al actualizar el porcentaje.");
    }

    var categoriaExistente = await _categoriaService.ObtenerCategoriaPorIdAsync(id);
    return View(categoriaExistente);
}

        // GET: /Categoria/Eliminar/{id}
        public async Task<IActionResult> Eliminar(Guid id)
        {
            var categoria = await _categoriaService.ObtenerCategoriaPorIdAsync(id);
            if (categoria == null || !categoria.Visible)
                return NotFound();

            return View(categoria);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(Guid id)
        {
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null)
                return RedirectToAction("GetLogin", "Login");

            await _categoriaService.EliminarCategoriaLogicamenteAsync(id, usuarioId);
            return RedirectToAction(nameof(Index));
        }


        // Ahora método asíncrono para obtener el Id real del usuario
        private async Task<string?> ObtenerUsuarioIdAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Id;
        }
    }
}
