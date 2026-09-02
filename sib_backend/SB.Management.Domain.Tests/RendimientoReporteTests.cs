using System.Collections.Generic;
using System.Diagnostics;
using SB.Management.Domain.Entities;
using Xunit;

namespace SB.Management.Domain.Tests
{
    public class RendimientoReporteTests
    {
        private const int CANTIDAD_EMPLEADOS_PRUEBA = 1000;
        private const int LIMITE_MILISEGUNDOS = 2000;

        [Fact]
        public void CalcularPago_ParaMilEmpleados_TardaMenosDeDosSegundos()
        {
            var empleados = GenerarMilEmpleadosDeLosCuatroTipos();
            var cronometro = Stopwatch.StartNew();

            foreach (var empleado in empleados)
            {
                _ = empleado.CalcularPago();
            }

            cronometro.Stop();

            Assert.True(
                cronometro.ElapsedMilliseconds < LIMITE_MILISEGUNDOS,
                $"El cálculo de {CANTIDAD_EMPLEADOS_PRUEBA} empleados tardó {cronometro.ElapsedMilliseconds}ms, se esperaba menos de {LIMITE_MILISEGUNDOS}ms");
        }

        private static List<Empleado> GenerarMilEmpleadosDeLosCuatroTipos()
        {
            var empleados = new List<Empleado>();

            for (var i = 0; i < CANTIDAD_EMPLEADOS_PRUEBA; i++)
            {
                Empleado empleado = (i % 4) switch
                {
                    0 => new EmpleadoAsalariado { SalarioSemanal = 15000m },
                    1 => new EmpleadoPorHora { SueldoPorHora = 200m, HorasTrabajadas = 45m },
                    2 => new EmpleadoPorComision { VentasBrutas = 80000m, TarifaComision = 0.05m },
                    _ => new EmpleadoAsalariadoComision { VentasBrutas = 60000m, TarifaComision = 0.08m, SalarioBase = 12000m }
                };

                empleados.Add(empleado);
            }

            return empleados;
        }
    }
}