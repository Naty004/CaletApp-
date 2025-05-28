using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IIngresoRepository
    {
        Task<Ingreso> AdicionarIngresoAsync(Ingreso ingreso);
        Task<IEnumerable<Ingreso>> ObtenerIngresosPorUsuarioAsync(string usuarioId);
        Task<decimal> ObtenerIngresoTotalMensualAsync(string usuarioId, int mes, int anio);
        Task<decimal> ObtenerIngresoTotalAsync(string usuarioId);
        Task<Ingreso?> ObtenerIngresoPorIdAsync(Guid ingresoId);
        Task<bool> EliminarIngresoAsync(Guid ingresoId);
    }
}
