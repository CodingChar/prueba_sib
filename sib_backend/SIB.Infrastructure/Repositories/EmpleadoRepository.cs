using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SB.Management.Application.Interfaces;
using SB.Management.Domain.Entities;
using SB.Management.Infrastructure.Persistence;

namespace SB.Management.Infrastructure.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly SbGestionPagosDbContext _context;

        public EmpleadoRepository(SbGestionPagosDbContext context)
        {
            _context = context;
        }

        public async Task<Empleado?> ObtenerPorIdAsync(int id)
        {
            return await _context.Empleados.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Empleado>> ObtenerConFiltrosAsync(string? nombre, string? departamento, string? estado)
        {
            var query = _context.Empleados.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                query = query.Where(e =>
                    (e.PrimerNombre != null && e.PrimerNombre.Contains(nombre)) ||
                    e.ApellidoPaterno.Contains(nombre));
            }

            if (!string.IsNullOrWhiteSpace(departamento))
            {
                query = query.Where(e => e.Departamento == departamento);
            }

            if (!string.IsNullOrWhiteSpace(estado))
            {
                query = query.Where(e => e.Estado == estado);
            }

            return await query.ToListAsync();
        }

        public async Task AgregarAsync(Empleado empleado)
        {
            await _context.Empleados.AddAsync(empleado);
        }

        public Task ActualizarAsync(Empleado empleado)
        {
            _context.Empleados.Update(empleado);
            return Task.CompletedTask;
        }

        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}