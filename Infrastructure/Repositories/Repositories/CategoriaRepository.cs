using Application.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Categoria> AgregarCategoriaAsync(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<IEnumerable<Categoria>> ObtenerCategoriasPorUsuarioAsync(string usuarioId, bool soloVisibles = true)
        {
            return await _context.Categorias
                .Where(c => c.UsuarioId == usuarioId && (!soloVisibles || c.Visible))
                .ToListAsync();
        }

        public async Task<Categoria?> ObtenerCategoriaPorIdAsync(Guid categoriaId)
        {
            return await _context.Categorias
                .Include(c => c.Gastos)
                .FirstOrDefaultAsync(c => c.Id == categoriaId);
        }

        public async Task EliminarLogicamenteCategoriaAsync(Guid categoriaId)
        {
            var categoria = await _context.Categorias.FindAsync(categoriaId);
            if (categoria != null)
            {
                categoria.Visible = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AsignarPorcentajeACategoriaAsync(Guid categoriaId, double nuevoPorcentaje)
        {
            var categoria = await _context.Categorias.FindAsync(categoriaId);
            if (categoria != null)
            {
                categoria.PorcentajeMaximoMensual = nuevoPorcentaje;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<decimal> ObtenerGastoTotalPorCategoriaAsync(Guid categoriaId)
        {
            return await _context.Gastos
                .Where(g => g.CategoriaId == categoriaId)
                .SumAsync(g => g.Monto);
        }

        public async Task<IEnumerable<(Categoria Categoria, decimal GastoActual, decimal GastoMaximo)>> ObtenerCategoriasConTotalesAsync(string usuarioId, DateTime? fechaReferencia = null)
        {
            var fecha = fechaReferencia ?? DateTime.Now;
            var categorias = await _context.Categorias
                .Where(c => c.UsuarioId == usuarioId && c.Visible)
                .Include(c => c.Gastos)
                .ToListAsync();

            var ingresosTotalesDelMes = await _context.Ingresos
                .Where(i => i.UsuarioId == usuarioId &&
                            i.Fecha.Month == fecha.Month &&
                            i.Fecha.Year == fecha.Year)
                .SumAsync(i => i.Monto);

            return categorias.Select(c =>
            {
                var gastosDelMes = c.Gastos
                    .Where(g => g.Fecha.Month == fecha.Month && g.Fecha.Year == fecha.Year)
                    .Sum(g => g.Monto);

                var topeMaximo = (decimal)(c.PorcentajeMaximoMensual / 100.0) * ingresosTotalesDelMes;

                return (c, gastosDelMes, topeMaximo);
            });
        }
    }
}

