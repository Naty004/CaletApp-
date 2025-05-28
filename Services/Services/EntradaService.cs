using Application.Repositories;
using Application.Services.IServices;
using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Services
{
    public class EntradaService : IEntradaService
    {
        private readonly IIngresoRepository _ingresoRepository;

        public EntradaService(IIngresoRepository ingresoRepository)
        {
            _ingresoRepository = ingresoRepository;
        }

        public async Task<Ingreso> AgregarIngresoAsync(Ingreso ingreso)
        {
            if (ingreso == null)
                throw new ArgumentNullException(nameof(ingreso));

            ingreso.Fecha = ingreso.Fecha.ToUniversalTime();
            return await _ingresoRepository.AdicionarIngresoAsync(ingreso);
        }

        public async Task<IEnumerable<Ingreso>> ObtenerIngresosPorUsuarioAsync(string usuarioId)
        {
            if (string.IsNullOrEmpty(usuarioId))
                throw new ArgumentException("El usuarioId no puede estar vacío.");

            return await _ingresoRepository.ObtenerIngresosPorUsuarioAsync(usuarioId);
        }

        public async Task<decimal> ObtenerTotalIngresosMensualesAsync(string usuarioId, DateTime fechaReferencia)
        {
            if (string.IsNullOrEmpty(usuarioId))
                throw new ArgumentException("El usuarioId no puede estar vacío.");

            return await _ingresoRepository.ObtenerIngresoTotalMensualAsync(usuarioId, fechaReferencia.Month, fechaReferencia.Year);
        }
    }
}
