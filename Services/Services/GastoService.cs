using Application.Repositories;
using Application.Services.IServices;
using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Services
{
    public class GastoService : IGastoService
    {
        private readonly IGastoRepository _gastoRepository;

        public GastoService(IGastoRepository gastoRepository)
        {
            _gastoRepository = gastoRepository;
        }

        public async Task<Gasto> AgregarGastoAsync(Gasto gasto)
        {
            if (gasto == null)
                throw new ArgumentNullException(nameof(gasto));

            if (gasto.Monto <= 0)
                throw new ArgumentException("El monto del gasto debe ser mayor que cero.", nameof(gasto.Monto));

            if (string.IsNullOrEmpty(gasto.UsuarioId))
                throw new ArgumentException("El usuarioId no puede estar vacío.", nameof(gasto.UsuarioId));

            return await _gastoRepository.AdicionarGastoAsync(gasto);
        }

        public async Task<IEnumerable<Gasto>> ObtenerGastosPorUsuarioAsync(string usuarioId)
        {
            if (string.IsNullOrEmpty(usuarioId))
                throw new ArgumentException("El usuarioId no puede estar vacío.", nameof(usuarioId));

            return await _gastoRepository.ObtenerGastosPorUsuarioAsync(usuarioId);
        }

        public async Task<decimal> ObtenerGastoTotalAsync(string usuarioId)
        {
            if (string.IsNullOrEmpty(usuarioId))
                throw new ArgumentException("El usuarioId no puede estar vacío.", nameof(usuarioId));

            return await _gastoRepository.ObtenerGastoTotalPorUsuarioAsync(usuarioId);
        }

        public async Task<IEnumerable<Gasto>> ObtenerGastosPorRangoFechaAsync(string usuarioId, DateTime fechaInicio, DateTime fechaFin)
        {
            if (string.IsNullOrEmpty(usuarioId))
                throw new ArgumentException("El usuarioId no puede estar vacío.", nameof(usuarioId));

            if (fechaFin < fechaInicio)
                throw new ArgumentException("La fecha fin debe ser mayor o igual a la fecha inicio.");

            return await _gastoRepository.ObtenerGastosPorRangoFechaAsync(usuarioId, fechaInicio, fechaFin);
        }

        public async Task<bool> EliminarGastoAsync(Guid gastoId)
        {
            return await _gastoRepository.EliminarGastoAsync(gastoId);
        }

        public async Task<Gasto?> ObtenerGastoPorIdAsync(Guid gastoId)
        {
            return await _gastoRepository.ObtenerGastoPorIdAsync(gastoId);
        }
    }
}
