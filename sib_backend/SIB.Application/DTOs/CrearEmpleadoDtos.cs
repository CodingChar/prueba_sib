namespace SB.Management.Application.DTOs
{
    public record CrearEmpleadoAsalariadoDto(
        string? PrimerNombre,
        string ApellidoPaterno,
        string NumeroSeguroSocial,
        string Departamento,
        decimal SalarioSemanal);

    public record CrearEmpleadoPorHoraDto(
        string ApellidoPaterno,
        string NumeroSeguroSocial,
        string Departamento,
        decimal SueldoPorHora,
        decimal HorasTrabajadas);

    public record CrearEmpleadoPorComisionDto(
        string? PrimerNombre,
        string ApellidoPaterno,
        string NumeroSeguroSocial,
        string Departamento,
        decimal VentasBrutas,
        decimal TarifaComision);

    public record CrearEmpleadoAsalariadoComisionDto(
        string? PrimerNombre,
        string ApellidoPaterno,
        string NumeroSeguroSocial,
        string Departamento,
        decimal VentasBrutas,
        decimal TarifaComision,
        decimal SalarioBase);
}