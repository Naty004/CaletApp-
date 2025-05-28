using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.IServices
{
    public interface IEntradaService
    {
        /// <summary>
        /// Agrega una nueva entrada de dinero para un usuario.
        /// </summary>
        Task<Ingreso> AgregarIngresoAsync(Ingreso ingreso);

        /// <summary>
        /// Obtiene todos los ingresos de un usuario.
        /// </summary>
        Task<IEnumerable<Ingreso>> ObtenerIngresosPorUsuarioAsync(string usuarioId);

        /// <summary>
        /// Calcula el total de ingresos de un usuario en un mes específico.
        /// </summary>
        Task<decimal> ObtenerTotalIngresosMensualesAsync(string usuarioId, DateTime fechaReferencia);
    }
}
