using System.Collections.Generic;
using System.Threading.Tasks;
using SB.Management.Domain.Entities;

namespace SB.Management.Application.Interfaces
{
    public interface IEntidadGubernamentalRepository
    {
        Task<List<EntidadGubernamental>> ObtenerTodasAsync();
        Task<EntidadGubernamental?> ObtenerPorIdAsync(int id);
        Task AgregarAsync(EntidadGubernamental entidad);
        Task ActualizarAsync(EntidadGubernamental entidad);
        Task EliminarAsync(int id);
    }
}