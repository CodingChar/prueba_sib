using System.Threading.Tasks;
using SB.Management.Domain.Entities;

namespace SB.Management.Application.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObtenerPorUsernameAsync(string username);
        Task AgregarAsync(Usuario usuario);
        Task GuardarCambiosAsync();
    }
}