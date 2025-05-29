using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IGastoService
    {
        // Visualizar todos los gastos por usuario
        Task<IEnumerable<Gasto>> ObtenerGastosPorUsuarioAsync(string usuarioId);

        // Visualizar gastos totales (monto acumulado)
        Task<decimal> ObtenerGastoTotalPorUsuarioAsync(string usuarioId);

        // Visualizar gastos mensuales
        Task<decimal> ObtenerGastoTotalMensualAsync(string usuarioId, int mes, int anio);

        // Visualizar gastos semanales
        Task<decimal> ObtenerGastoTotalSemanalAsync(string usuarioId, DateTime semanaReferencia);

        // Visualizar gastos diarios
        Task<decimal> ObtenerGastoTotalDiarioAsync(string usuarioId, DateTime dia);

        // Agregar un gasto
        Task<Gasto> AdicionarGastoAsync(string descripcion, decimal monto, DateTime fecha, Guid categoriaId, string usuarioId);

        // Eliminar gasto con devolución y reajuste
        Task<IEnumerable<(Categoria Categoria, decimal GastoActual, decimal GastoMaximo)>> EliminarGastoYReajustarAsync(Guid gastoId);
    }
}
