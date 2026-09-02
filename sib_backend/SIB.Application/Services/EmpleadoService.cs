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

        public async Task<bool> ActualizarAsalariadoAsync(int id, ActualizarEmpleadoAsalariadoDto dto)
        {
            var empleado = await _empleadoRepository.ObtenerPorIdAsync(id);
            if (empleado is not EmpleadoAsalariado asalariado)
            {
                return false;
            }

            asalariado.SalarioSemanal = dto.SalarioSemanal;
            await _empleadoRepository.ActualizarAsync(asalariado);
            await _empleadoRepository.GuardarCambiosAsync();
            _logger.LogInformation("Empleado asalariado {Id} actualizado", id);
            return true;
        }

        public async Task<bool> ActualizarPorHoraAsync(int id, ActualizarEmpleadoPorHoraDto dto)
        {
            var empleado = await _empleadoRepository.ObtenerPorIdAsync(id);
            if (empleado is not EmpleadoPorHora porHora)
            {
                return false;
            }

            porHora.SueldoPorHora = dto.SueldoPorHora;
            porHora.HorasTrabajadas = dto.HorasTrabajadas;
            await _empleadoRepository.ActualizarAsync(porHora);
            await _empleadoRepository.GuardarCambiosAsync();
            _logger.LogInformation("Empleado por hora {Id} actualizado: {Horas} horas", id, dto.HorasTrabajadas);
            return true;
        }

        public async Task<bool> ActualizarPorComisionAsync(int id, ActualizarEmpleadoPorComisionDto dto)
        {
            var empleado = await _empleadoRepository.ObtenerPorIdAsync(id);
            if (empleado is not EmpleadoPorComision porComision)
            {
                return false;
            }

            porComision.VentasBrutas = dto.VentasBrutas;
            porComision.TarifaComision = dto.TarifaComision;
            await _empleadoRepository.ActualizarAsync(porComision);
            await _empleadoRepository.GuardarCambiosAsync();
            _logger.LogInformation("Empleado por comisión {Id} actualizado", id);
            return true;
        }
        public async Task<EmpleadoDetalleDto?> ObtenerDetallePorIdAsync(int id)
        {
            var empleado = await _empleadoRepository.ObtenerPorIdAsync(id);
            if (empleado is null)
            {
                return null;
            }

            return empleado switch
            {
                EmpleadoAsalariado asalariado => new EmpleadoDetalleDto(
                    asalariado.Id, "Asalariado", asalariado.PrimerNombre, asalariado.ApellidoPaterno,
                    asalariado.NumeroSeguroSocial, asalariado.Departamento, asalariado.Estado,
                    asalariado.SalarioSemanal, null, null, null, null, null),

                EmpleadoPorHora porHora => new EmpleadoDetalleDto(
                    porHora.Id, "PorHora", porHora.PrimerNombre, porHora.ApellidoPaterno,
                    porHora.NumeroSeguroSocial, porHora.Departamento, porHora.Estado,
                    null, porHora.SueldoPorHora, porHora.HorasTrabajadas, null, null, null),

                EmpleadoPorComision porComision => new EmpleadoDetalleDto(
                    porComision.Id, "PorComision", porComision.PrimerNombre, porComision.ApellidoPaterno,
                    porComision.NumeroSeguroSocial, porComision.Departamento, porComision.Estado,
                    null, null, null, porComision.VentasBrutas, porComision.TarifaComision, null),

                EmpleadoAsalariadoComision asalariadoComision => new EmpleadoDetalleDto(
                    asalariadoComision.Id, "AsalariadoComision", asalariadoComision.PrimerNombre, asalariadoComision.ApellidoPaterno,
                    asalariadoComision.NumeroSeguroSocial, asalariadoComision.Departamento, asalariadoComision.Estado,
                    null, null, null, asalariadoComision.VentasBrutas, asalariadoComision.TarifaComision, asalariadoComision.SalarioBase),

                _ => null
            };
        }

        public async Task<bool> ActualizarAsalariadoComisionAsync(int id, ActualizarEmpleadoAsalariadoComisionDto dto)
        {
            var empleado = await _empleadoRepository.ObtenerPorIdAsync(id);
            if (empleado is not EmpleadoAsalariadoComision asalariadoComision)
            {
                return false;
            }

            asalariadoComision.VentasBrutas = dto.VentasBrutas;
            asalariadoComision.TarifaComision = dto.TarifaComision;
            asalariadoComision.SalarioBase = dto.SalarioBase;
            await _empleadoRepository.ActualizarAsync(asalariadoComision);
            await _empleadoRepository.GuardarCambiosAsync();
            _logger.LogInformation("Empleado asalariado por comisión {Id} actualizado", id);
            return true;
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