using Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Infrastructure.Identity;


namespace WebApp.Controllers
{
    public class IngresoController : Controller
    {
        private readonly IIngresoService _ingresoService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IngresoController(IIngresoService ingresoService, UserManager<ApplicationUser> userManager)
        {
            _ingresoService = ingresoService;
            _userManager = userManager;
        }

        // GET: /Ingreso
        public async Task<IActionResult> Index()
        {
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null) return Challenge(); // o RedirectToAction("Login", "Account");

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
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null) return Challenge();

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
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null) return Challenge();

            var total = await _ingresoService.ObtenerIngresoTotalAsync(usuarioId);
            ViewBag.Tipo = "Total";
            return View("TotalView", total);
        }

        // GET: /Ingreso/Mensual?mes=5&anio=2025
        public async Task<IActionResult> Mensual(int mes, int anio)
        {
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null) return Challenge();

            var total = await _ingresoService.ObtenerIngresoTotalMensualAsync(usuarioId, mes, anio);
            ViewBag.Tipo = $"Mensual ({mes}/{anio})";
            return View("TotalView", total);
        }

        private async Task<string?> ObtenerUsuarioIdAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Id;
        }
    }
}
