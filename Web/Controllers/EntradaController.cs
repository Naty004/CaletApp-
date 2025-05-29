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
    public class EntradaController : Controller
    {
        private readonly IEntradaService _entradaService;

        public EntradaController(IEntradaService entradaService)
        {
            _entradaService = entradaService;
        }

        private string ObtenerUsuarioId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        }

        // GET: /Entrada
        public async Task<IActionResult> Index()
        {
            var usuarioId = ObtenerUsuarioId();
            var ingresos = await _entradaService.ObtenerIngresosPorUsuarioAsync(usuarioId);
            return View(ingresos);
        }

        // GET: /Entrada/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Entrada/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string descripcion, decimal monto, DateTime fecha)
        {
            if (string.IsNullOrWhiteSpace(descripcion) || monto <= 0 || fecha == default)
            {
                ModelState.AddModelError("", "Datos inválidos para crear el ingreso.");
                return View();
            }

            var usuarioId = ObtenerUsuarioId();

            var ingreso = new Ingreso(descripcion, monto, fecha, usuarioId);
            await _entradaService.AgregarIngresoAsync(ingreso);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Entrada/TotalMensual?fecha=yyyy-MM-dd
        public async Task<IActionResult> TotalMensual(DateTime fecha)
        {
            var usuarioId = ObtenerUsuarioId();
            var total = await _entradaService.ObtenerTotalIngresosMensualesAsync(usuarioId, fecha);
            return Json(new { TotalIngresos = total, Mes = fecha.ToString("yyyy-MM") });
        }
    }
}
