using Application.Services;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Infrastructure.Identity;



namespace WebApp.Controllers
{
    public class GastoController : Controller
    {
        private readonly IGastoService _gastoService;
       private readonly UserManager<ApplicationUser> _userManager;

public GastoController(IGastoService gastoService, UserManager<ApplicationUser> userManager)
{
    _gastoService = gastoService;
    _userManager = userManager;
}


        // GET: /Gasto
        public async Task<IActionResult> Index()
        {
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null) return Unauthorized();

            var gastos = await _gastoService.ObtenerGastosPorUsuarioAsync(usuarioId);
            return View(gastos);
        }

        // GET: /Gasto/Create
        public async Task<IActionResult> Create()
        {
            var categorias = await _gastoService.ObtenerGastosPorUsuarioAsync(await ObtenerUsuarioIdAsync());

            var vm = new GastoCreateViewModel
            {
                Fecha = DateTime.Today,
                Categorias = categorias
                    .Select(g => g.Categoria)
                    .Distinct()
                    .Select(c => new GastoCreateViewModel.CategoriaItem
                    {
                        Id = c.Id,
                        Nombre = c.Nombre
                    })
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

            return View(gasto);
        }

        // POST: /Gasto/Eliminar/{id}
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(Guid id)
        {
            try
            {
                await _gastoService.EliminarGastoYReajustarAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: /Gasto/Total
        public async Task<IActionResult> Total()
        {
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null) return Unauthorized();

            var total = await _gastoService.ObtenerGastoTotalPorUsuarioAsync(usuarioId);
            ViewBag.Tipo = "Total";
            return View("TotalView", total);
        }

        // GET: /Gasto/Mensual?mes=5&anio=2025
        public async Task<IActionResult> Mensual(int mes, int anio)
        {
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null) return Unauthorized();

            var total = await _gastoService.ObtenerGastoTotalMensualAsync(usuarioId, mes, anio);
            ViewBag.Tipo = $"Mensual ({mes}/{anio})";
            return View("TotalView", total);
        }

        // GET: /Gasto/Semanal?fecha=2025-05-27
        public async Task<IActionResult> Semanal(DateTime fecha)
        {
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null) return Unauthorized();

            var total = await _gastoService.ObtenerGastoTotalSemanalAsync(usuarioId, fecha);
            ViewBag.Tipo = $"Semanal ({fecha:dd/MM/yyyy})";
            return View("TotalView", total);
        }

        // GET: /Gasto/Diario?fecha=2025-05-27
        public async Task<IActionResult> Diario(DateTime fecha)
        {
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null) return Unauthorized();

            var total = await _gastoService.ObtenerGastoTotalDiarioAsync(usuarioId, fecha);
            ViewBag.Tipo = $"Diario ({fecha:dd/MM/yyyy})";
            return View("TotalView", total);
        }

        private async Task<string?> ObtenerUsuarioIdAsync()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                return user?.Id;
            }
            return null;
        }

        private async Task<IEnumerable<GastoCreateViewModel.CategoriaItem>> ObtenerCategoriasParaViewModel(string usuarioId)
        {
            var gastos = await _gastoService.ObtenerGastosPorUsuarioAsync(usuarioId);

            return gastos
                .Select(g => g.Categoria)
                .Distinct()
                .Select(c => new GastoCreateViewModel.CategoriaItem
                {
                    Id = c.Id,
                    Nombre = c.Nombre
                });
        }
    }
}
