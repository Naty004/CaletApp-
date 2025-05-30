using Application.Repositories;
using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GastoService : IGastoService
    {
        private readonly IGastoRepository _gastoRepository;
        private readonly IIngresoService _ingresoService;

        public GastoService(IGastoRepository gastoRepository, IIngresoService ingresoService)
        {
            _gastoRepository = gastoRepository;
            _ingresoService = ingresoService;
        }

        public async Task<Gasto> AdicionarGastoAsync(string descripcion, decimal monto, DateTime fecha, Guid categoriaId, string usuarioId)
{
    // Validación básica de los parámetros
    if (string.IsNullOrWhiteSpace(descripcion))
        throw new ArgumentException("La descripción no puede estar vacía.");

    if (monto <= 0)
        throw new ArgumentException("El monto debe ser mayor a cero.");

    // 1. Obtener total de ingreso mensual del usuario
    var ingresoTotal = await _ingresoService.ObtenerIngresoTotalMensualAsync(usuarioId, fecha.Month, fecha.Year);

    // 2. Obtener total de gasto mensual actual del usuario
    var gastoActual = await _gastoRepository.ObtenerGastoTotalMensualAsync(usuarioId, fecha.Month, fecha.Year);


    // 3. Validar que haya saldo suficiente
    if ((gastoActual + monto) > ingresoTotal)
        throw new InvalidOperationException("No se puede registrar el gasto porque excede el ingreso mensual disponible.");

    // 4. Validar que el gasto no supere el tope de su categoría
    var categoriasConTopes = await _ingresoService.RecalcularTopesGastoAsync(usuarioId, fecha.Month, fecha.Year);
    var categoriaActual = categoriasConTopes.FirstOrDefault(x => x.Categoria.Id == categoriaId);

    if (categoriaActual == default)
        throw new InvalidOperationException("Categoría no encontrada o no visible.");

    if ((categoriaActual.GastoActual + monto) > categoriaActual.GastoMaximo)
        throw new InvalidOperationException("Este gasto excede el tope mensual permitido para la categoría.");

    // Crear el objeto Gasto
    var gasto = new Gasto(descripcion, monto, fecha, categoriaId, usuarioId);

    // Guardar el gasto en la base de datos
    return await _gastoRepository.AdicionarGastoAsync(gasto);
}


        public async Task<IEnumerable<Gasto>> ObtenerGastosPorUsuarioAsync(string usuarioId)
        {
            return await _gastoRepository.ObtenerGastosPorUsuarioAsync(usuarioId);
        }

        public async Task<decimal> ObtenerGastoTotalPorUsuarioAsync(string usuarioId)
        {
            return await _gastoRepository.ObtenerGastoTotalPorUsuarioAsync(usuarioId);
        }

        public async Task<decimal> ObtenerGastoTotalMensualAsync(string usuarioId, int mes, int anio)
        {
            return await _gastoRepository.ObtenerGastoTotalMensualAsync(usuarioId, mes, anio);
        }

        public async Task<decimal> ObtenerGastoTotalSemanalAsync(string usuarioId, DateTime semanaReferencia)
        {
            return await _gastoRepository.ObtenerGastoTotalSemanalAsync(usuarioId, semanaReferencia);
        }

        public async Task<decimal> ObtenerGastoTotalDiarioAsync(string usuarioId, DateTime dia)
        {
            return await _gastoRepository.ObtenerGastoTotalDiarioAsync(usuarioId, dia);
        }

        public async Task<IEnumerable<(Categoria Categoria, decimal GastoActual, decimal GastoMaximo)>> EliminarGastoYReajustarAsync(Guid gastoId)
        {
            var gasto = await _gastoRepository.ObtenerGastoPorIdAsync(gastoId);
            if (gasto == null)
                throw new Exception("Gasto no encontrado.");

            var usuarioId = gasto.UsuarioId;
            var mes = gasto.Fecha.Month;
            var anio = gasto.Fecha.Year;

            var eliminado = await _gastoRepository.EliminarGastoAsync(gastoId);
            if (!eliminado)
                throw new Exception("No se pudo eliminar el gasto.");

            // Recalcular topes para actualizar la disponibilidad real
            var categoriasConTotales = await _ingresoService.RecalcularTopesGastoAsync(usuarioId, mes, anio);

            return categoriasConTotales;
        }
    }
}
