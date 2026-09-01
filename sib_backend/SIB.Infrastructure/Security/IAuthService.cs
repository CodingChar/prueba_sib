using System.Threading.Tasks;

namespace SB.Management.Infrastructure.Security
{
    public interface IAuthService
    {
        Task<string?> AutenticarAsync(string username, string password);
        Task<int> RegistrarAsync(string username, string password, int rolId);
    }
}