using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class IngresoController : Controller
    {
        private readonly IIngresoService _ingresoService;

        public IngresoController(IIngresoService ingresoService)
        {
            _ingresoService = ingresoService;
        }

        // GET: /Ingreso
        public async Task<IActionResult> Index()
        {
            var usuarioId = ObtenerUsuarioId();
            var ingresos = await _ingresoService.ObtenerIngresosPorUsuarioAsync(usuarioId);
            return View("Index", ingresos);
        }

        // GET: /Ingreso/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Ingreso/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(decimal monto, DateTime fecha)
        {
            var usuarioId = ObtenerUsuarioId();

            if (monto <= 0)
            {
                ModelState.AddModelError("Monto", "El monto debe ser mayor a cero.");
                return View();
            }

            try
            {
                await _ingresoService.AdicionarIngresoAsync(monto, fecha, usuarioId);
                TempData["Mensaje"] = "Ingreso registrado y topes recalculados exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: /Ingreso/Total
        public async Task<IActionResult> Total()
        {
            var usuarioId = ObtenerUsuarioId();
            var total = await _ingresoService.ObtenerIngresoTotalAsync(usuarioId);
            ViewBag.Tipo = "Total";
            return View("TotalView", total);
        }

        // GET: /Ingreso/Mensual?mes=5&anio=2025
        public async Task<IActionResult> Mensual(int mes, int anio)
        {
            var usuarioId = ObtenerUsuarioId();
            var total = await _ingresoService.ObtenerIngresoTotalMensualAsync(usuarioId, mes, anio);
            ViewBag.Tipo = $"Mensual ({mes}/{anio})";
            return View("TotalView", total);
        }

        private string ObtenerUsuarioId()
        {
            // Sustituir por autenticaci�n real
            return "usuario_demo";
        }
    }
}
