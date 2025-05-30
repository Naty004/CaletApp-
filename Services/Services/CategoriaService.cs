using Application.Repositories;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<Categoria> AgregarCategoriaAsync(string nombre, string? descripcion, double porcentajeMaximo, string usuarioId)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio.");

            if (porcentajeMaximo < 0 || porcentajeMaximo > 100)
                throw new ArgumentOutOfRangeException("El porcentaje debe estar entre 0 y 100.");

            var categoriasVisibles = await _categoriaRepository.ObtenerCategoriasPorUsuarioAsync(usuarioId, soloVisibles: true);

            var porcentajeTotalExistente = categoriasVisibles.Sum(c => c.PorcentajeMaximoMensual);

            if (porcentajeTotalExistente + porcentajeMaximo > 100)
                return null;

            var nuevaCategoria = new Categoria(nombre, descripcion, porcentajeMaximo, usuarioId);

            return await _categoriaRepository.AgregarCategoriaAsync(nuevaCategoria);
        }

        public async Task<IEnumerable<Categoria>> ObtenerCategoriasVisiblesPorUsuarioAsync(string usuarioId)
        {
            return await _categoriaRepository.ObtenerCategoriasPorUsuarioAsync(usuarioId, soloVisibles: true);
        }

        public async Task EliminarCategoriaLogicamenteAsync(Guid categoriaId, string usuarioId)
        {
            var categoria = await _categoriaRepository.ObtenerCategoriaPorIdAsync(categoriaId);
            if (categoria == null || categoria.UsuarioId != usuarioId || !categoria.Visible)
                throw new UnauthorizedAccessException("Acceso denegado o categoría no válida.");

            await _categoriaRepository.EliminarLogicamenteCategoriaAsync(categoriaId);
        }

        public async Task AsignarPorcentajeAsync(Guid categoriaId, double nuevoPorcentaje)
        {
            if (nuevoPorcentaje < 0 || nuevoPorcentaje > 100)
                return;

            var categoria = await _categoriaRepository.ObtenerCategoriaPorIdAsync(categoriaId);
            if (categoria == null || !categoria.Visible)
                return;

            var categoriasVisibles = await _categoriaRepository.ObtenerCategoriasPorUsuarioAsync(categoria.UsuarioId, soloVisibles: true);

            var sumaSinEsta = categoriasVisibles.Where(c => c.Id != categoriaId).Sum(c => c.PorcentajeMaximoMensual);

            if (sumaSinEsta + nuevoPorcentaje > 100)
                return;

            await _categoriaRepository.AsignarPorcentajeACategoriaAsync(categoriaId, nuevoPorcentaje);
        }

        public async Task<Categoria?> ObtenerCategoriaPorIdAsync(Guid categoriaId)
        {
            return await _categoriaRepository.ObtenerCategoriaPorIdAsync(categoriaId);
        }
    }
}
