using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IGastoRepository
    {
        Task<Gasto> AdicionarGastoAsync(Gasto gasto);
        Task<IEnumerable<Gasto>> ObtenerGastosPorUsuarioAsync(string usuarioId);
        Task<IEnumerable<Gasto>> ObtenerGastosPorCategoriaAsync(Guid categoriaId);
        Task<IEnumerable<Gasto>> ObtenerGastosPorRangoFechaAsync(string usuarioId, DateTime desde, DateTime hasta);
        Task<decimal> ObtenerGastoTotalPorUsuarioAsync(string usuarioId);
        Task<decimal> ObtenerGastoTotalMensualAsync(string usuarioId, int mes, int anio);
        Task<decimal> ObtenerGastoTotalSemanalAsync(string usuarioId, DateTime semanaReferencia);
        Task<decimal> ObtenerGastoTotalDiarioAsync(string usuarioId, DateTime dia);
        Task<bool> EliminarGastoAsync(Guid gastoId);
        Task<Gasto?> ObtenerGastoPorIdAsync(Guid gastoId);
    }
}
