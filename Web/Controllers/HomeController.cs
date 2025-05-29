using Application.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcTemplate.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IGastoService _gastoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IEntradaService _entradaService;

        public HomeController(IGastoService gastoService, ICategoriaService categoriaService, IEntradaService entradaService)
        {
            _gastoService = gastoService;
            _categoriaService = categoriaService;
            _entradaService = entradaService;
        }

        private string ObtenerUsuarioId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = ObtenerUsuarioId();

            // Obtiene datos
            var gastos = await _gastoService.ObtenerGastosPorUsuarioAsync(usuarioId);
            var categorias = await _categoriaService.ObtenerCategoriasConTotalesAsync(usuarioId);
            var ingresos = await _entradaService.ObtenerIngresosPorUsuarioAsync(usuarioId);

            // Totales
            var totalIngresos = ingresos.Sum(i => i.Monto);
            var totalGastos = gastos.Sum(g => g.Monto);
            var saldoActual = totalIngresos - totalGastos;

            // Modelo dinámico para la vista
            var model = new
            {
                Gastos = gastos.OrderByDescending(g => g.Fecha).Take(5).ToList(), // últimos 5 gastos
                Categorias = categorias.Select(c => c.Categoria).ToList(),
                TotalIngresos = totalIngresos,
                TotalGastos = totalGastos,
                SaldoActual = saldoActual
            };

            return View(model);
        }

        // Otras acciones existentes...
    }
}
