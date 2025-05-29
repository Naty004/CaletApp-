using Application.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    public class ReporteController : Controller
    {
        private readonly IGastoService _gastoService;
        private readonly IEntradaService _entradaService;

        public ReporteController(IGastoService gastoService, IEntradaService entradaService)
        {
            _gastoService = gastoService;
            _entradaService = entradaService;
        }

        private string ObtenerUsuarioId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        }

        // GET: /Reporte/Mensual?fecha=yyyy-MM-dd
        public async Task<IActionResult> Mensual(DateTime? fecha)
        {
            var usuarioId = ObtenerUsuarioId();
            var fechaReferencia = fecha ?? DateTime.Today;

            // Obtener total gastos en el mes
            var primerDiaMes = new DateTime(fechaReferencia.Year, fechaReferencia.Month, 1);
            var ultimoDiaMes = primerDiaMes.AddMonths(1).AddDays(-1);

            var gastos = await _gastoService.ObtenerGastosPorRangoFechaAsync(usuarioId, primerDiaMes, ultimoDiaMes);
            decimal totalGastos = 0m;
            foreach (var g in gastos) totalGastos += g.Monto;

            // Obtener total ingresos en el mes
            decimal totalIngresos = await _entradaService.ObtenerTotalIngresosMensualesAsync(usuarioId, fechaReferencia);

            // Calcular saldo
            decimal saldo = totalIngresos - totalGastos;

            // Puedes pasar estos datos a la vista con un ViewModel o ViewBag
            ViewBag.TotalGastos = totalGastos;
            ViewBag.TotalIngresos = totalIngresos;
            ViewBag.Saldo = saldo;
            ViewBag.FechaReferencia = fechaReferencia;

            return View(gastos); // o un ViewModel más completo si quieres
        }
    }
}
