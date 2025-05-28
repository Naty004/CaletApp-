using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.IServices
{
    public interface IGastoService
    {
        /// <summary>
        /// Agrega un nuevo gasto a una categoría.
        /// </summary>
        Task<Gasto> AgregarGastoAsync(Gasto gasto);

        /// <summary>
        /// Obtiene todos los gastos de un usuario.
        /// </summary>
        Task<IEnumerable<Gasto>> ObtenerGastosPorUsuarioAsync(string usuarioId);

        /// <summary>
        /// Obtiene los gastos totales de un usuario.
        /// </summary>
        Task<decimal> ObtenerGastoTotalAsync(string usuarioId);

        /// <summary>
        /// Obtiene los gastos del usuario filtrados por rango de fechas.
        /// </summary>
        Task<IEnumerable<Gasto>> ObtenerGastosPorRangoFechaAsync(string usuarioId, DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Elimina un gasto por su Id.
        /// </summary>
        Task<bool> EliminarGastoAsync(Guid gastoId);

        /// <summary>
        /// Obtiene un gasto específico por Id.
        /// </summary>
        Task<Gasto?> ObtenerGastoPorIdAsync(Guid gastoId);
    }
}
