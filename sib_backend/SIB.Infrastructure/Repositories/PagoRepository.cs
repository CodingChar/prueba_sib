using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SB.Management.Application.Interfaces;
using SB.Management.Domain.Entities;
using SB.Management.Infrastructure.Persistence;

namespace SB.Management.Infrastructure.Repositories
{
    public class PagoRepository : IPagoRepository
    {
        private readonly SbGestionPagosDbContext _context;

        public PagoRepository(SbGestionPagosDbContext context)
        {
            _context = context;
        }

        public async Task AgregarAsync(Pago pago)
        {
            await _context.Pagos.AddAsync(pago);
        }

        public async Task<List<Pago>> ObtenerPorPeriodoAsync(DateOnly fechaInicio, DateOnly fechaFin)
        {
            return await _context.Pagos
                .Where(p => p.FechaPago >= fechaInicio && p.FechaPago <= fechaFin)
                .ToListAsync();
        }

        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}