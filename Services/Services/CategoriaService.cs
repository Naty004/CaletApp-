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


                  // Validar nombre único
            if (categoriasVisibles.Any(c => c.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
              throw new InvalidOperationException("Ya existe una categoría con ese nombre.");

            if (porcentajeTotalExistente + porcentajeMaximo > 100)
                 throw new InvalidOperationException("El porcentaje total de las categorías no puede superar el 100%.");


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
                throw new UnauthorizedAccessException("Acceso denegado o categor�a no v�lida.");

            await _categoriaRepository.EliminarLogicamenteCategoriaAsync(categoriaId);
        }

     public async Task AsignarPorcentajeAsync(Guid categoriaId, double nuevoPorcentaje, string usuarioId)
{
    if (nuevoPorcentaje < 0 || nuevoPorcentaje > 100)
        throw new ArgumentOutOfRangeException("El porcentaje debe estar entre 0 y 100.");

    var categoria = await _categoriaRepository.ObtenerCategoriaPorIdAsync(categoriaId);
    if (categoria == null || !categoria.Visible || categoria.UsuarioId != usuarioId)
        throw new InvalidOperationException("Categoría no válida o acceso denegado.");

    var categoriasVisibles = await _categoriaRepository.ObtenerCategoriasPorUsuarioAsync(usuarioId, soloVisibles: true);
    var sumaSinEsta = categoriasVisibles
                        .Where(c => c.Id != categoriaId)
                        .Sum(c => c.PorcentajeMaximoMensual);

    if (sumaSinEsta + nuevoPorcentaje > 100)
        throw new InvalidOperationException("La suma total de los porcentajes no puede superar el 100%.");

    await _categoriaRepository.AsignarPorcentajeACategoriaAsync(categoriaId, nuevoPorcentaje);
}

        public async Task<Categoria?> ObtenerCategoriaPorIdAsync(Guid categoriaId)
        {
            return await _categoriaRepository.ObtenerCategoriaPorIdAsync(categoriaId);
        }
    }
}
