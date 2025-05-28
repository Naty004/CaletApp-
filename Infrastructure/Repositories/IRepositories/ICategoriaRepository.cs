using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface ICategoriaRepository
    {
        Task<Categoria> AgregarCategoriaAsync(Categoria categoria);
        Task<IEnumerable<Categoria>> ObtenerCategoriasPorUsuarioAsync(string usuarioId, bool soloVisibles = true);
        Task<Categoria?> ObtenerCategoriaPorIdAsync(Guid categoriaId);
        Task EliminarLogicamenteCategoriaAsync(Guid categoriaId);
        Task AsignarPorcentajeACategoriaAsync(Guid categoriaId, double nuevoPorcentaje);
        Task<decimal> ObtenerGastoTotalPorCategoriaAsync(Guid categoriaId);
        Task<IEnumerable<(Categoria Categoria, decimal GastoActual, decimal GastoMaximo)>> ObtenerCategoriasConTotalesAsync(string usuarioId, DateTime? fechaReferencia = null);
    }
}
