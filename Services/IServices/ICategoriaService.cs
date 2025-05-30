using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ICategoriaService
    {
        Task<Categoria> AgregarCategoriaAsync(string nombre, string? descripcion, double porcentajeMaximo, string usuarioId);

        Task<IEnumerable<Categoria>> ObtenerCategoriasVisiblesPorUsuarioAsync(string usuarioId);

        Task EliminarCategoriaLogicamenteAsync(Guid categoriaId, string usuarioId);

        /// <summary>
        /// Reasigna el porcentaje máximo para una categoría y ajusta o valida que la suma total no exceda 100%
        /// </summary>
        /// <exception cref="PorcentajeExcedidoException"></exception>
        Task AsignarPorcentajeAsync(Guid categoriaId, double nuevoPorcentaje);

        Task<Categoria?> ObtenerCategoriaPorIdAsync(Guid categoriaId);
    }
}
