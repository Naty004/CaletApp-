using Application.Repositories;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class IngresoService : IIngresoService
    {
        private readonly IIngresoRepository _ingresoRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public IngresoService(IIngresoRepository ingresoRepository, ICategoriaRepository categoriaRepository)
        {
            _ingresoRepository = ingresoRepository;
            _categoriaRepository = categoriaRepository;
        }

        public async Task<Ingreso> AdicionarIngresoAsync(decimal monto, DateTime fecha, string usuarioId)
        {
            if (monto <= 0)
                throw new ArgumentException("El monto debe ser mayor a cero.");

            var ingreso = new Ingreso(monto, fecha, usuarioId);
            var ingresoGuardado = await _ingresoRepository.AdicionarIngresoAsync(ingreso);

            // Recalcular topes automáticamente al adicionar ingreso (on-demand)
            await RecalcularTopesGastoAsync(usuarioId, fecha.Month, fecha.Year);

            return ingresoGuardado;
        }

        public async Task<IEnumerable<Ingreso>> ObtenerIngresosPorUsuarioAsync(string usuarioId)
        {
            return await _ingresoRepository.ObtenerIngresosPorUsuarioAsync(usuarioId);
        }

        public async Task<decimal> ObtenerIngresoTotalMensualAsync(string usuarioId, int mes, int anio)
        {
            return await _ingresoRepository.ObtenerIngresoTotalMensualAsync(usuarioId, mes, anio);
        }

        public async Task<decimal> ObtenerIngresoTotalAsync(string usuarioId)
        {
            return await _ingresoRepository.ObtenerIngresoTotalAsync(usuarioId);
        }

        public async Task<Ingreso?> ObtenerIngresoPorIdAsync(Guid ingresoId)
        {
            return await _ingresoRepository.ObtenerIngresoPorIdAsync(ingresoId);
        }

        public async Task<bool> EliminarIngresoAsync(Guid ingresoId)
        {
            return await _ingresoRepository.EliminarIngresoAsync(ingresoId);
        }

        /// <summary>
        /// Devuelve las categorías visibles con el gasto actual y el tope máximo recalculado, basado en el ingreso mensual.
        /// </summary>
        public async Task<IEnumerable<(Categoria Categoria, decimal GastoActual, decimal GastoMaximo)>> RecalcularTopesGastoAsync(string usuarioId, int mes, int anio)
        {
            var ingresoTotal = await _ingresoRepository.ObtenerIngresoTotalMensualAsync(usuarioId, mes, anio);
            var categorias = await _categoriaRepository.ObtenerCategoriasPorUsuarioAsync(usuarioId, soloVisibles: true);

            var categoriasConTotales = new List<(Categoria, decimal, decimal)>();

            foreach (var categoria in categorias)
            {
                var gastoActual = await _categoriaRepository.ObtenerGastoTotalPorCategoriaAsync(categoria.Id);
                var tope = (decimal)categoria.PorcentajeMaximoMensual / 100m * ingresoTotal;

                categoriasConTotales.Add((categoria, gastoActual, tope));
            }

            return categoriasConTotales;
        }
    }
}
