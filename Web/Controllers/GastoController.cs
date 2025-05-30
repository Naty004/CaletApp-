using Application.Services;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Infrastructure.Identity;
using System.Collections.Generic;

namespace WebApp.Controllers
{
    public class GastoController : Controller
    {
        private readonly IGastoService _gastoService;
        private readonly IIngresoService _ingresoService;
        private readonly ICategoriaService _categoriaService;
        private readonly UserManager<ApplicationUser> _userManager;

        public GastoController(
            IGastoService gastoService,
            IIngresoService ingresoService,
            ICategoriaService categoriaService,
            UserManager<ApplicationUser> userManager)
        {
            _gastoService = gastoService;
            _ingresoService = ingresoService;
            _categoriaService = categoriaService;
            _userManager = userManager;
        }

        // GET: /Gasto
        public async Task<IActionResult> Index()
        {
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null) return Unauthorized();

            var hoy = DateTime.Today;
            var resumen = await _ingresoService.RecalcularTopesGastoAsync(usuarioId, hoy.Month, hoy.Year);

            var modelo = resumen.Select(r => new ResumenGastoViewModel
            {
                CategoriaId = r.Categoria.Id,
                NombreCategoria = r.Categoria.Nombre,
                GastoActual = r.GastoActual,
                GastoMaximo = r.GastoMaximo,
               
            }).ToList();

            return View(modelo);
        }

        // GET: /Gasto/Listar
        public async Task<IActionResult> Listar()
        {
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null) return Unauthorized();

            var gastos = await _gastoService.ObtenerGastosPorUsuarioAsync(usuarioId);

            var modelo = gastos.Select(g => new GastoViewModel
            {
                GastoId = g.Id,
                NombreCategoria = g.Categoria?.Nombre ?? "Sin Categoría",
                Descripcion = g.Descripcion,
                Monto = g.Monto,
                Fecha = g.Fecha
            }).ToList();

            return View(modelo);
        }

        // GET: /Gasto/Create
        public async Task<IActionResult> Create()
        {
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null) return Unauthorized();

            var categorias = await _categoriaService.ObtenerCategoriasVisiblesPorUsuarioAsync(usuarioId);

            var vm = new GastoCreateViewModel
            {
                Fecha = DateTime.Today,
                Categorias = categorias.Select(c => new GastoCreateViewModel.CategoriaItem
                {
                    Id = c.Id,
                    Nombre = c.Nombre
                }).ToList()
            };

            return View(vm);
        }

        // POST: /Gasto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GastoCreateViewModel model)
        {
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null) return Unauthorized();

            if (!ModelState.IsValid)
            {
                model.Categorias = await ObtenerCategoriasParaViewModel(usuarioId);
                return View(model);
            }

            try
            {
                await _gastoService.AdicionarGastoAsync(
                    model.Descripcion,
                    model.Monto,
                    model.Fecha,
                    model.CategoriaId,
                    usuarioId
                );

                // Recalcular topes tras adicionar gasto
                await _ingresoService.RecalcularTopesGastoAsync(usuarioId, model.Fecha.Month, model.Fecha.Year);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.Categorias = await ObtenerCategoriasParaViewModel(usuarioId);
                return View(model);
            }
        }

        // GET: /Gasto/Eliminar/{id}
        public async Task<IActionResult> Eliminar(Guid id)
        {
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null) return Unauthorized();

            var gastos = await _gastoService.ObtenerGastosPorUsuarioAsync(usuarioId);
            var gasto = gastos.FirstOrDefault(g => g.Id == id);

            if (gasto == null) return NotFound();

            var vm = new GastoViewModel
            {
                GastoId = gasto.Id,
                NombreCategoria = gasto.Categoria?.Nombre ?? "Sin Categoría",
                Descripcion = gasto.Descripcion,
                Monto = gasto.Monto,
                Fecha = gasto.Fecha
            };

            return View(vm);
        }

        // POST: /Gasto/Eliminar/{id}
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(Guid id)
        {
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null) return Unauthorized();

            try
            {
                var gastos = await _gastoService.ObtenerGastosPorUsuarioAsync(usuarioId);
                var gasto = gastos.FirstOrDefault(g => g.Id == id);
                if (gasto == null) return NotFound();

                await _gastoService.EliminarGastoYReajustarAsync(id);

                await _ingresoService.RecalcularTopesGastoAsync(usuarioId, gasto.Fecha.Month, gasto.Fecha.Year);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(Listar));
            }
        }

        // Métodos auxiliares

        private async Task<string?> ObtenerUsuarioIdAsync()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                return user?.Id;
            }
            return null;
        }

        private async Task<List<GastoCreateViewModel.CategoriaItem>> ObtenerCategoriasParaViewModel(string usuarioId)
        {
            var categorias = await _categoriaService.ObtenerCategoriasVisiblesPorUsuarioAsync(usuarioId);

            return categorias.Select(c => new GastoCreateViewModel.CategoriaItem
            {
                Id = c.Id,
                Nombre = c.Nombre
            }).ToList();
        }

        private string ObtenerColorSemaforo(decimal gastoActual, decimal gastoMaximo)
        {
            if (gastoMaximo == 0) return "gray";

            var porcentaje = (gastoActual / gastoMaximo) * 100;

            if (porcentaje < 70) return "green";
            if (porcentaje < 90) return "yellow";
            return "red";
        }
    }
}
