using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SB.Management.Application.DTOs;
using SB.Management.Application.Services;

namespace SB.Management.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EntidadesGubernamentalesController : ControllerBase
    {
        private readonly EntidadGubernamentalService _service;
        private readonly ILogger<EntidadesGubernamentalesController> _logger;

        public EntidadesGubernamentalesController(EntidadGubernamentalService service, ILogger<EntidadesGubernamentalesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<EntidadGubernamentalResponseDto>>> ObtenerTodas()
        {
            var entidades = await _service.ObtenerTodasAsync();
            return Ok(entidades);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EntidadGubernamentalResponseDto>> ObtenerPorId(int id)
        {
            var entidad = await _service.ObtenerPorIdAsync(id);
            if (entidad is null)
            {
                return NotFound();
            }
            return Ok(entidad);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Crear(CrearEntidadGubernamentalDto dto)
        {
            var id = await _service.CrearAsync(dto);
            _logger.LogInformation("Entidad gubernamental creada con Id {Id}", id);
            return Ok(new { id });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Actualizar(int id, CrearEntidadGubernamentalDto dto)
        {
            await _service.ActualizarAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _service.EliminarAsync(id);
            _logger.LogInformation("Entidad gubernamental {Id} eliminada", id);
            return NoContent();
        }
    }
}