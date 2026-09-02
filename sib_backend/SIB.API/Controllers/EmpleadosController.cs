using System;
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
    public class EmpleadosController : ControllerBase
    {
        private readonly EmpleadoService _empleadoService;
        private readonly ILogger<EmpleadosController> _logger;

        public EmpleadosController(EmpleadoService empleadoService, ILogger<EmpleadosController> logger)
        {
            _empleadoService = empleadoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<EmpleadoResponseDto>>> Buscar(
            [FromQuery] string? nombre, [FromQuery] string? departamento, [FromQuery] string? estado)
        {
            _logger.LogInformation("Búsqueda de empleados: nombre={Nombre}, departamento={Departamento}, estado={Estado}", nombre, departamento, estado);
            var resultado = await _empleadoService.BuscarAsync(nombre, departamento, estado);
            return Ok(resultado);
        }

        [HttpPost("asalariado")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CrearAsalariado(CrearEmpleadoAsalariadoDto dto)
        {
            var id = await _empleadoService.CrearAsalariadoAsync(dto);
            return Ok(new { id });
        }

        [HttpPost("por-hora")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CrearPorHora(CrearEmpleadoPorHoraDto dto)
        {
            var id = await _empleadoService.CrearPorHoraAsync(dto);
            return Ok(new { id });
        }

        [HttpPost("por-comision")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CrearPorComision(CrearEmpleadoPorComisionDto dto)
        {
            var id = await _empleadoService.CrearPorComisionAsync(dto);
            return Ok(new { id });
        }

        [HttpPost("asalariado-comision")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CrearAsalariadoComision(CrearEmpleadoAsalariadoComisionDto dto)
        {
            var id = await _empleadoService.CrearAsalariadoComisionAsync(dto);
            return Ok(new { id });
        }

        [HttpGet("reporte")]
        public async Task<ActionResult<ReporteSemanalDto>> Reporte(
            [FromQuery] DateOnly fechaInicio, [FromQuery] DateOnly fechaFin)
        {
            var reporte = await _empleadoService.GenerarReportePorPeriodoAsync(fechaInicio, fechaFin);
            return Ok(reporte);
        }

        [HttpPut("asalariado/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActualizarAsalariado(int id, ActualizarEmpleadoAsalariadoDto dto)
        {
            var actualizado = await _empleadoService.ActualizarAsalariadoAsync(id, dto);
            if (!actualizado)
            {
                return NotFound(new { mensaje = "Empleado no encontrado o no es del tipo Asalariado." });
            }
            return NoContent();
        }

        [HttpPut("por-hora/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActualizarPorHora(int id, ActualizarEmpleadoPorHoraDto dto)
        {
            var actualizado = await _empleadoService.ActualizarPorHoraAsync(id, dto);
            if (!actualizado)
            {
                return NotFound(new { mensaje = "Empleado no encontrado o no es del tipo Por Hora." });
            }
            return NoContent();
        }

        [HttpPut("por-comision/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActualizarPorComision(int id, ActualizarEmpleadoPorComisionDto dto)
        {
            var actualizado = await _empleadoService.ActualizarPorComisionAsync(id, dto);
            if (!actualizado)
            {
                return NotFound(new { mensaje = "Empleado no encontrado o no es del tipo Por Comisión." });
            }
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmpleadoDetalleDto>> ObtenerDetalle(int id)
        {
            var detalle = await _empleadoService.ObtenerDetallePorIdAsync(id);
            if (detalle is null)
            {
                return NotFound();
            }
            return Ok(detalle);
        }

        [HttpPut("asalariado-comision/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActualizarAsalariadoComision(int id, ActualizarEmpleadoAsalariadoComisionDto dto)
        {
            var actualizado = await _empleadoService.ActualizarAsalariadoComisionAsync(id, dto);
            if (!actualizado)
            {
                return NotFound(new { mensaje = "Empleado no encontrado o no es del tipo Asalariado por Comisión." });
            }
            return NoContent();
        }
    }
}