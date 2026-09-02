using SB.Management.Domain.Entities;
using Xunit;

namespace SB.Management.Domain.Tests
{
    public class EmpleadoCalculoPagoTests
    {
        [Fact]
        public void CalcularPago_EmpleadoAsalariado_DevuelveElSalarioSemanal()
        {
            var empleado = new EmpleadoAsalariado { SalarioSemanal = 15000m };

            var resultado = empleado.CalcularPago();

            Assert.Equal(15000m, resultado);
        }

        [Fact]
        public void CalcularPago_EmpleadoPorHora_SinHorasExtra_MultiplicaSueldoPorHoras()
        {
            var empleado = new EmpleadoPorHora
            {
                SueldoPorHora = 200m,
                HorasTrabajadas = 40m
            };

            var resultado = empleado.CalcularPago();

            Assert.Equal(8000m, resultado);
        }

        [Fact]
        public void CalcularPago_EmpleadoPorHora_ConHorasExtra_AplicaFactor1Punto5()
        {
            var empleado = new EmpleadoPorHora
            {
                SueldoPorHora = 200m,
                HorasTrabajadas = 45m
            };

            // Esperado: (200 x 40) + (200 x 1.5 x 5) = 8000 + 1500 = 9500
            var resultado = empleado.CalcularPago();

            Assert.Equal(9500m, resultado);
        }

        [Fact]
        public void CalcularPago_EmpleadoPorHora_ExactamenteEnElLimiteDe40Horas_NoAplicaHorasExtra()
        {
            var empleado = new EmpleadoPorHora
            {
                SueldoPorHora = 150m,
                HorasTrabajadas = 40m
            };

            var resultado = empleado.CalcularPago();

            Assert.Equal(6000m, resultado);
        }

        [Fact]
        public void CalcularPago_EmpleadoPorComision_MultiplicaVentasPorTarifa()
        {
            var empleado = new EmpleadoPorComision
            {
                VentasBrutas = 100000m,
                TarifaComision = 0.05m
            };

            var resultado = empleado.CalcularPago();

            Assert.Equal(5000m, resultado);
        }

        [Fact]
        public void CalcularPago_EmpleadoAsalariadoComision_SumaComisionSalarioBaseYBonificacion()
        {
            var empleado = new EmpleadoAsalariadoComision
            {
                VentasBrutas = 50000m,
                TarifaComision = 0.10m,
                SalarioBase = 10000m
            };

            // Esperado: (50000 x 0.10) + 10000 + (10000 x 0.10) = 5000 + 10000 + 1000 = 16000
            var resultado = empleado.CalcularPago();

            Assert.Equal(16000m, resultado);
        }
    }
}