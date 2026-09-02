namespace SB.Management.Application.DTOs
{
    public record ActualizarEmpleadoAsalariadoDto(decimal SalarioSemanal);

    public record ActualizarEmpleadoPorHoraDto(decimal SueldoPorHora, decimal HorasTrabajadas);

    public record ActualizarEmpleadoPorComisionDto(decimal VentasBrutas, decimal TarifaComision);

    public record ActualizarEmpleadoAsalariadoComisionDto(decimal VentasBrutas, decimal TarifaComision, decimal SalarioBase);
}