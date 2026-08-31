using System;

namespace SB.Management.Domain.Entities
{
    public class EmpleadoAsalariado : Empleado
    {
        public decimal SalarioSemanal { get; set; }

        public override decimal CalcularPago() => SalarioSemanal;
    }

    public class EmpleadoPorHora : Empleado
    {
        private const decimal HORAS_SEMANALES_REGULARES = 40m;
        private const decimal FACTOR_HORAS_EXTRA = 1.5m;

        public decimal SueldoPorHora { get; set; }
        public decimal HorasTrabajadas { get; set; }

        public override decimal CalcularPago()
        {
            if (HorasTrabajadas <= HORAS_SEMANALES_REGULARES)
                return SueldoPorHora * HorasTrabajadas;

            var horasExtra = HorasTrabajadas - HORAS_SEMANALES_REGULARES;
            return (SueldoPorHora * HORAS_SEMANALES_REGULARES)
                 + (SueldoPorHora * FACTOR_HORAS_EXTRA * horasExtra);
        }
    }

    public class EmpleadoPorComision : Empleado
    {
        public decimal VentasBrutas { get; set; }
        public decimal TarifaComision { get; set; }

        public override decimal CalcularPago() => VentasBrutas * TarifaComision;
    }

    public class EmpleadoAsalariadoComision : Empleado
    {
        private const decimal FACTOR_BONIFICACION_SALARIO_BASE = 0.10m;

        public decimal VentasBrutas { get; set; }
        public decimal TarifaComision { get; set; }
        public decimal SalarioBase { get; set; }

        public override decimal CalcularPago() =>
            (VentasBrutas * TarifaComision)
            + SalarioBase
            + (SalarioBase * FACTOR_BONIFICACION_SALARIO_BASE);
    }
}