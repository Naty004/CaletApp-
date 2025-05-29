using Application.Services.IServices;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        private string ObtenerUsuarioId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        }

        // GET: /Categoria
        public async Task<IActionResult> Index()
        {
            var usuarioId = ObtenerUsuarioId();
            var categorias = await _categoriaService.ObtenerCategoriasConTotalesAsync(usuarioId);
            return View(categorias);
        }

        // GET: /Categoria/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Categoria/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string nombre, string? descripcion, double porcentaje)
        {
            if (string.IsNullOrWhiteSpace(nombre) || porcentaje < 0 || porcentaje > 100)
            {
                ModelState.AddModelError("", "Datos inválidos. El porcentaje debe estar entre 0 y 100.");
                return View();
            }

            var categoria = new Categoria(nombre, descripcion, porcentaje, ObtenerUsuarioId());
            await _categoriaService.AgregarCategoriaAsync(categoria);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Categoria/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var categoria = await _categoriaService.ObtenerCategoriaPorIdAsync(id);
            if (categoria == null || categoria.UsuarioId != ObtenerUsuarioId())
            {
                return NotFound();
            }

            ViewBag.PorcentajeActual = categoria.PorcentajeMaximoMensual;
            ViewBag.CategoriaId = categoria.Id;
            ViewBag.Nombre = categoria.Nombre;
            return View();
        }

        // POST: /Categoria/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, double nuevoPorcentaje)
        {
            if (nuevoPorcentaje < 0 || nuevoPorcentaje > 100)
            {
                ModelState.AddModelError("", "El porcentaje debe estar entre 0 y 100.");
                return View();
            }

            var categoria = await _categoriaService.ObtenerCategoriaPorIdAsync(id);
            if (categoria == null || categoria.UsuarioId != ObtenerUsuarioId())
            {
                return NotFound();
            }

            await _categoriaService.AsignarPorcentajeACategoriaAsync(id, nuevoPorcentaje);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Categoria/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var categoria = await _categoriaService.ObtenerCategoriaPorIdAsync(id);
            if (categoria == null || categoria.UsuarioId != ObtenerUsuarioId())
            {
                return NotFound();
            }

            await _categoriaService.EliminarLogicamenteCategoriaAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
