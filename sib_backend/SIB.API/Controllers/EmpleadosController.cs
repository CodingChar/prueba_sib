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
    }
}