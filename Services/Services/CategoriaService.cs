using Application.Repositories;
using Application.Services.IServices;
using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<Categoria> AgregarCategoriaAsync(Categoria categoria)
        {
            if (categoria == null)
                throw new ArgumentNullException(nameof(categoria));

            // Validación adicional podría ir aquí (por ejemplo, porcentaje entre 0 y 100)
            if (categoria.PorcentajeMaximoMensual < 0 || categoria.PorcentajeMaximoMensual > 100)
                throw new ArgumentOutOfRangeException(nameof(categoria.PorcentajeMaximoMensual), "El porcentaje debe estar entre 0 y 100.");

            return await _categoriaRepository.AgregarCategoriaAsync(categoria);
        }

        public async Task<IEnumerable<(Categoria Categoria, decimal GastoActual, decimal GastoMaximo)>> ObtenerCategoriasConTotalesAsync(string usuarioId, DateTime? fechaReferencia = null)
        {
            if (string.IsNullOrEmpty(usuarioId))
                throw new ArgumentException("El usuarioId no puede estar vacío.", nameof(usuarioId));

            return await _categoriaRepository.ObtenerCategoriasConTotalesAsync(usuarioId, fechaReferencia);
        }

        public async Task<Categoria?> ObtenerCategoriaPorIdAsync(Guid categoriaId)
        {
            return await _categoriaRepository.ObtenerCategoriaPorIdAsync(categoriaId);
        }

        public async Task AsignarPorcentajeACategoriaAsync(Guid categoriaId, double nuevoPorcentaje)
        {
            if (nuevoPorcentaje < 0 || nuevoPorcentaje > 100)
                throw new ArgumentOutOfRangeException(nameof(nuevoPorcentaje), "El porcentaje debe estar entre 0 y 100.");

            await _categoriaRepository.AsignarPorcentajeACategoriaAsync(categoriaId, nuevoPorcentaje);
        }

        public async Task EliminarLogicamenteCategoriaAsync(Guid categoriaId)
        {
            await _categoriaRepository.EliminarLogicamenteCategoriaAsync(categoriaId);
        }

        public async Task<decimal> ObtenerGastoTotalPorCategoriaAsync(Guid categoriaId)
        {
            return await _categoriaRepository.ObtenerGastoTotalPorCategoriaAsync(categoriaId);
        }
    }
}
