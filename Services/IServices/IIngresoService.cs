using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IIngresoService
    {
        /// <summary>
        /// Adiciona un nuevo ingreso y devuelve el ingreso agregado.
        /// </summary>
        /// <param name="monto">Monto del ingreso</param>
        /// <param name="fecha">Fecha del ingreso</param>
        /// <param name="usuarioId">ID del usuario</param>
        Task<Ingreso> AdicionarIngresoAsync(decimal monto, DateTime fecha, string usuarioId);

        /// <summary>
        /// Devuelve todos los ingresos registrados por un usuario.
        /// </summary>
        Task<IEnumerable<Ingreso>> ObtenerIngresosPorUsuarioAsync(string usuarioId);

        /// <summary>
        /// Devuelve el ingreso total registrado por un usuario en el mes y año dados.
        /// </summary>
        Task<decimal> ObtenerIngresoTotalMensualAsync(string usuarioId, int mes, int anio);

        /// <summary>
        /// Devuelve el ingreso total histórico del usuario.
        /// </summary>
        Task<decimal> ObtenerIngresoTotalAsync(string usuarioId);

        /// <summary>
        /// Devuelve un ingreso específico por su ID.
        /// </summary>
        Task<Ingreso?> ObtenerIngresoPorIdAsync(Guid ingresoId);

        /// <summary>
        /// Elimina un ingreso existente.
        /// </summary>
        Task<bool> EliminarIngresoAsync(Guid ingresoId);

        Task<IEnumerable<(Categoria Categoria, decimal GastoActual, decimal GastoMaximo)>> RecalcularTopesGastoAsync(string usuarioId, int mes, int anio);

    }
}
