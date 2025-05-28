using Application.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class IngresoRepository : IIngresoRepository
    {
        private readonly ApplicationDbContext _context;

        public IngresoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Ingreso> AdicionarIngresoAsync(Ingreso ingreso)
        {
            _context.Ingresos.Add(ingreso);
            await _context.SaveChangesAsync();
            return ingreso;
        }

        public async Task<IEnumerable<Ingreso>> ObtenerIngresosPorUsuarioAsync(string usuarioId)
        {
            return await _context.Ingresos
                .Where(i => i.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<decimal> ObtenerIngresoTotalMensualAsync(string usuarioId, int mes, int anio)
        {
            return await _context.Ingresos
                .Where(i => i.UsuarioId == usuarioId && i.Fecha.Month == mes && i.Fecha.Year == anio)
                .SumAsync(i => i.Monto);
        }

        public async Task<decimal> ObtenerIngresoTotalAsync(string usuarioId)
        {
            return await _context.Ingresos
                .Where(i => i.UsuarioId == usuarioId)
                .SumAsync(i => i.Monto);
        }

        public async Task<Ingreso?> ObtenerIngresoPorIdAsync(Guid ingresoId)
        {
            return await _context.Ingresos
                .FirstOrDefaultAsync(i => i.Id == ingresoId);
        }

        public async Task<bool> EliminarIngresoAsync(Guid ingresoId)
        {
            var ingreso = await _context.Ingresos.FindAsync(ingresoId);
            if (ingreso == null) return false;

            _context.Ingresos.Remove(ingreso);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
