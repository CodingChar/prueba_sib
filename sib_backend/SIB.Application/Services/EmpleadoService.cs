using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SB.Management.Application.DTOs;
using SB.Management.Application.Interfaces;
using SB.Management.Domain.Entities;

namespace SB.Management.Application.Services
{
    public class EmpleadoService
    {
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly IPagoRepository _pagoRepository;
        private readonly ILogger<EmpleadoService> _logger;

        public EmpleadoService(
            IEmpleadoRepository empleadoRepository,
            IPagoRepository pagoRepository,
            ILogger<EmpleadoService> logger)
        {
            _empleadoRepository = empleadoRepository;
            _pagoRepository = pagoRepository;
            _logger = logger;
        }

        public async Task<int> CrearAsalariadoAsync(CrearEmpleadoAsalariadoDto dto)
        {
            var empleado = new EmpleadoAsalariado
            {
                PrimerNombre = dto.PrimerNombre,
                ApellidoPaterno = dto.ApellidoPaterno,
                NumeroSeguroSocial = dto.NumeroSeguroSocial,
                Departamento = dto.Departamento,
                SalarioSemanal = dto.SalarioSemanal
            };

            await _empleadoRepository.AgregarAsync(empleado);
            await _empleadoRepository.GuardarCambiosAsync();
            _logger.LogInformation("Empleado asalariado {NumeroSeguroSocial} creado con Id {Id}", dto.NumeroSeguroSocial, empleado.Id);
            return empleado.Id;
        }

        public async Task<int> CrearPorHoraAsync(CrearEmpleadoPorHoraDto dto)
        {
            var empleado = new EmpleadoPorHora
            {
                ApellidoPaterno = dto.ApellidoPaterno,
                NumeroSeguroSocial = dto.NumeroSeguroSocial,
                Departamento = dto.Departamento,
                SueldoPorHora = dto.SueldoPorHora,
                HorasTrabajadas = dto.HorasTrabajadas
            };

            await _empleadoRepository.AgregarAsync(empleado);
            await _empleadoRepository.GuardarCambiosAsync();
            _logger.LogInformation("Empleado por hora {NumeroSeguroSocial} creado con Id {Id}", dto.NumeroSeguroSocial, empleado.Id);
            return empleado.Id;
        }

        public async Task<int> CrearPorComisionAsync(CrearEmpleadoPorComisionDto dto)
        {
            var empleado = new EmpleadoPorComision
            {
                PrimerNombre = dto.PrimerNombre,
                ApellidoPaterno = dto.ApellidoPaterno,
                NumeroSeguroSocial = dto.NumeroSeguroSocial,
                Departamento = dto.Departamento,
                VentasBrutas = dto.VentasBrutas,
                TarifaComision = dto.TarifaComision
            };

            await _empleadoRepository.AgregarAsync(empleado);
            await _empleadoRepository.GuardarCambiosAsync();
            _logger.LogInformation("Empleado por comisión {NumeroSeguroSocial} creado con Id {Id}", dto.NumeroSeguroSocial, empleado.Id);
            return empleado.Id;
        }

        public async Task<int> CrearAsalariadoComisionAsync(CrearEmpleadoAsalariadoComisionDto dto)
        {
            var empleado = new EmpleadoAsalariadoComision
            {
                PrimerNombre = dto.PrimerNombre,
                ApellidoPaterno = dto.ApellidoPaterno,
                NumeroSeguroSocial = dto.NumeroSeguroSocial,
                Departamento = dto.Departamento,
                VentasBrutas = dto.VentasBrutas,
                TarifaComision = dto.TarifaComision,
                SalarioBase = dto.SalarioBase
            };

            await _empleadoRepository.AgregarAsync(empleado);
            await _empleadoRepository.GuardarCambiosAsync();
            _logger.LogInformation("Empleado asalariado por comisión {NumeroSeguroSocial} creado con Id {Id}", dto.NumeroSeguroSocial, empleado.Id);
            return empleado.Id;
        }

        public async Task<List<EmpleadoResponseDto>> BuscarAsync(string? nombre, string? departamento, string? estado)
        {
            var empleados = await _empleadoRepository.ObtenerConFiltrosAsync(nombre, departamento, estado);
            return empleados.Select(e => MapearADto(e, null)).ToList();
        }

        public async Task<ReporteSemanalDto> GenerarReportePorPeriodoAsync(DateOnly fechaInicio, DateOnly fechaFin)
        {
            var empleados = await _empleadoRepository.ObtenerConFiltrosAsync(null, null, "Activo");
            var dtos = new List<EmpleadoResponseDto>();
            decimal totalNomina = 0;

            foreach (var empleado in empleados)
            {
                var monto = empleado.CalcularPago();
                totalNomina += monto;

                await _pagoRepository.AgregarAsync(new Pago
                {
                    EmpleadoId = empleado.Id,
                    FechaPago = fechaFin,
                    MontoCalculado = monto,
                    DetalleCalculo = "Calculado como " + empleado.GetType().Name
                });

                dtos.Add(MapearADto(empleado, monto));
            }

            await _pagoRepository.GuardarCambiosAsync();
            _logger.LogInformation("Reporte generado del {FechaInicio} al {FechaFin}: {Cantidad} empleados, total {Total}", fechaInicio, fechaFin, dtos.Count, totalNomina);

            return new ReporteSemanalDto(fechaInicio, fechaFin, dtos, totalNomina);
        }

        private static EmpleadoResponseDto MapearADto(Empleado empleado, decimal? montoPrecalculado)
        {
            string tipo;
            switch (empleado)
            {
                case EmpleadoAsalariado:
                    tipo = "Asalariado";
                    break;
                case EmpleadoPorHora:
                    tipo = "PorHora";
                    break;
                case EmpleadoPorComision:
                    tipo = "PorComision";
                    break;
                case EmpleadoAsalariadoComision:
                    tipo = "AsalariadoComision";
                    break;
                default:
                    tipo = "Desconocido";
                    break;
            }

            return new EmpleadoResponseDto(
                empleado.Id,
                tipo,
                empleado.PrimerNombre,
                empleado.ApellidoPaterno,
                empleado.NumeroSeguroSocial,
                empleado.Departamento,
                empleado.Estado,
                montoPrecalculado ?? empleado.CalcularPago());
        }
    }
}