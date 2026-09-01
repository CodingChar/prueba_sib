using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SB.Management.Application.Interfaces;
using SB.Management.Domain.Entities;
using SB.Management.Infrastructure.Persistence;

namespace SB.Management.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly SbGestionPagosDbContext _context;

        public UsuarioRepository(SbGestionPagosDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObtenerPorUsernameAsync(string username)
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task AgregarAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
        }

        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}