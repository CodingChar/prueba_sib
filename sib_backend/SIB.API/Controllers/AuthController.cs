using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SB.Management.Infrastructure.Security;

namespace SB.Management.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public record LoginRequest(string Username, string Password);
        public record RegistroRequest(string Username, string Password, int RolId);

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var token = await _authService.AutenticarAsync(request.Username, request.Password);
            if (token is null)
            {
                _logger.LogWarning("Intento de login fallido para {Username}", request.Username);
                return Unauthorized(new { mensaje = "Credenciales inválidas" });
            }

            _logger.LogInformation("Login exitoso para {Username}", request.Username);
            return Ok(new { token });
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro(RegistroRequest request)
        {
            var id = await _authService.RegistrarAsync(request.Username, request.Password, request.RolId);
            _logger.LogInformation("Usuario {Username} registrado con Id {Id}", request.Username, id);
            return Ok(new { id });
        }
    }
}