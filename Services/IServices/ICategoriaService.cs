using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.IServices
{
    public interface ICategoriaService
    {
        /// <summary>
        /// Agrega una nueva categoría para un usuario.
        /// </summary>
        Task<Categoria> AgregarCategoriaAsync(Categoria categoria);

        /// <summary>
        /// Obtiene todas las categorías visibles para un usuario, con sus gastos totales actuales y máximos.
        /// </summary>
        Task<IEnumerable<(Categoria Categoria, decimal GastoActual, decimal GastoMaximo)>> ObtenerCategoriasConTotalesAsync(string usuarioId, DateTime? fechaReferencia = null);

        /// <summary>
        /// Obtiene una categoría por su Id.
        /// </summary>
        Task<Categoria?> ObtenerCategoriaPorIdAsync(Guid categoriaId);

        /// <summary>
        /// Asigna un nuevo porcentaje de gasto máximo mensual a una categoría.
        /// </summary>
        Task AsignarPorcentajeACategoriaAsync(Guid categoriaId, double nuevoPorcentaje);

        /// <summary>
        /// Realiza la eliminación lógica (oculta la categoría sin eliminarla de la base de datos).
        /// </summary>
        Task EliminarLogicamenteCategoriaAsync(Guid categoriaId);

        /// <summary>
        /// Obtiene el gasto total acumulado de una categoría.
        /// </summary>
        Task<decimal> ObtenerGastoTotalPorCategoriaAsync(Guid categoriaId);
    }
}
