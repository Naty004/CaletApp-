using Application.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GastoRepository : IGastoRepository
    {
        private readonly ApplicationDbContext _context;

        public GastoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Gasto> AdicionarGastoAsync(Gasto gasto)
        {
            _context.Gastos.Add(gasto);
            await _context.SaveChangesAsync();
            return gasto;
        }

        public async Task<IEnumerable<Gasto>> ObtenerGastosPorUsuarioAsync(string usuarioId)
        {
            return await _context.Gastos
                .Where(g => g.UsuarioId == usuarioId)
                .Include(g => g.Categoria)
                .ToListAsync();
        }

        public async Task<IEnumerable<Gasto>> ObtenerGastosPorCategoriaAsync(Guid categoriaId)
        {
            return await _context.Gastos
                .Where(g => g.CategoriaId == categoriaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Gasto>> ObtenerGastosPorRangoFechaAsync(string usuarioId, DateTime desde, DateTime hasta)
        {
            return await _context.Gastos
                .Where(g => g.UsuarioId == usuarioId && g.Fecha >= desde && g.Fecha <= hasta)
                .ToListAsync();
        }

        public async Task<decimal> ObtenerGastoTotalPorUsuarioAsync(string usuarioId)
        {
            return await _context.Gastos
                .Where(g => g.UsuarioId == usuarioId)
                .SumAsync(g => g.Monto);
        }

        public async Task<decimal> ObtenerGastoTotalMensualAsync(string usuarioId, int mes, int anio)
        {
            return await _context.Gastos
                .Where(g => g.UsuarioId == usuarioId && g.Fecha.Month == mes && g.Fecha.Year == anio)
                .SumAsync(g => g.Monto);
        }

        public async Task<decimal> ObtenerGastoTotalSemanalAsync(string usuarioId, DateTime semanaReferencia)
        {
            var inicioSemana = semanaReferencia.Date.AddDays(-(int)semanaReferencia.DayOfWeek);
            var finSemana = inicioSemana.AddDays(7);

            return await _context.Gastos
                .Where(g => g.UsuarioId == usuarioId && g.Fecha >= inicioSemana && g.Fecha < finSemana)
                .SumAsync(g => g.Monto);
        }

        public async Task<decimal> ObtenerGastoTotalDiarioAsync(string usuarioId, DateTime dia)
        {
            var fechaInicio = dia.Date;
            var fechaFin = fechaInicio.AddDays(1);

            return await _context.Gastos
                .Where(g => g.UsuarioId == usuarioId && g.Fecha >= fechaInicio && g.Fecha < fechaFin)
                .SumAsync(g => g.Monto);
        }

        public async Task<bool> EliminarGastoAsync(Guid gastoId)
        {
            var gasto = await _context.Gastos.FindAsync(gastoId);
            if (gasto == null) return false;

            _context.Gastos.Remove(gasto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Gasto?> ObtenerGastoPorIdAsync(Guid gastoId)
        {
            return await _context.Gastos
                .Include(g => g.Categoria)
                .FirstOrDefaultAsync(g => g.Id == gastoId);
        }
    }
}
