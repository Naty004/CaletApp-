using Application.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGastoService _gastoService;
        private readonly IIngresoService _ingresoService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            IGastoService gastoService,
            IIngresoService ingresoService,
            UserManager<ApplicationUser> userManager)
        {
            _gastoService = gastoService;
            _ingresoService = ingresoService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = await ObtenerUsuarioIdAsync();
            if (usuarioId == null)
                return RedirectToAction("GetLogin", "Login");

            var hoy = DateTime.Now;
            var mes = hoy.Month;
            var anio = hoy.Year;

            var ingresoTotal = await _ingresoService.ObtenerIngresoTotalMensualAsync(usuarioId, mes, anio);
            var gastoTotal = await _gastoService.ObtenerGastoTotalMensualAsync(usuarioId, mes, anio);
            var categoriasConGasto = await _ingresoService.RecalcularTopesGastoAsync(usuarioId, mes, anio);

            var porcentajeTotalGastado = ingresoTotal == 0 ? 0 : Math.Round((double)(gastoTotal / ingresoTotal) * 100, 2);

            var model = new
            {
                IngresoTotal = ingresoTotal,
                GastoTotal = gastoTotal,
                PorcentajeGastado = porcentajeTotalGastado,
                Categorias = categoriasConGasto.Select(c => new
                {
                    Nombre = c.Categoria.Nombre,
                    GastoActual = c.GastoActual,
                    Tope = c.GastoMaximo,
                    Porcentaje = c.GastoMaximo == 0 ? 0 : Math.Round((double)(c.GastoActual / c.GastoMaximo) * 100, 2)
                }).ToList()
            };

            return View(model);
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
    }
}
