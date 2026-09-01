using System.Collections.Generic;
using System.Threading.Tasks;
using SB.Management.Domain.Entities;

namespace SB.Management.Application.Interfaces
{
    public interface IEmpleadoRepository
    {
        Task<Empleado?> ObtenerPorIdAsync(int id);
        Task<List<Empleado>> ObtenerConFiltrosAsync(string? nombre, string? departamento, string? estado);
        Task AgregarAsync(Empleado empleado);
        Task ActualizarAsync(Empleado empleado);
        Task GuardarCambiosAsync();
    }
}