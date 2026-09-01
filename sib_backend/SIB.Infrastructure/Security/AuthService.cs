using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SB.Management.Application.Interfaces;
using SB.Management.Domain.Entities;

namespace SB.Management.Infrastructure.Security
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public async Task<string?> AutenticarAsync(string username, string password)
        {
            var usuario = await _usuarioRepository.ObtenerPorUsernameAsync(username);
            if (usuario is null || !BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash))
            {
                return null;
            }

            var nombreRol = usuario.Rol is not null ? usuario.Rol.Nombre : "Usuario";
            return GenerarToken(usuario.Username, nombreRol);
        }

        public async Task<int> RegistrarAsync(string username, string password, int rolId)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            var usuario = new Usuario
            {
                Username = username,
                PasswordHash = hash,
                RolId = rolId
            };

            await _usuarioRepository.AgregarAsync(usuario);
            await _usuarioRepository.GuardarCambiosAsync();
            return usuario.Id;
        }

        private string GenerarToken(string username, string rol)
        {
            const int HORAS_EXPIRACION_TOKEN = 8;

            var claveJwt = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Falta la configuración 'Jwt:Key' en appsettings.json");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, rol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveJwt));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(HORAS_EXPIRACION_TOKEN),
                signingCredentials: credenciales);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}