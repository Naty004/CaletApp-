using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.IServices;

namespace MvcTemplate.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly IService _service;

        public CategoriasController(IService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _service.GetAllCategorias(); // Usamos el servicio para obtener categorías
            return View(categorias);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CategoriaModels categoriaDto)
        {
            if (ModelState.IsValid)
            {
                await _service.AddCategoria(categoriaDto); // Aquí usamos el DTO
                return RedirectToAction("Index");
            }
            return View(categoriaDto);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var categoria = await _service.GetCategoriaById(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(CategoriaModels categoriaDto)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateCategoria(categoriaDto);
                return RedirectToAction("Index");
            }
            return View(categoriaDto);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var categoria = await _service.GetCategoriaById(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            await _service.DeleteCategoria(id);
            return RedirectToAction("Index");
        }
    }
}