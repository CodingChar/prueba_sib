namespace SB.Management.Application.DTOs
{
    public record EmpleadoResponseDto(
        int Id,
        string Tipo,
        string? PrimerNombre,
        string ApellidoPaterno,
        string NumeroSeguroSocial,
        string Departamento,
        string Estado,
        decimal PagoCalculado);

    public record ReporteSemanalDto(
        System.DateOnly FechaInicio,
        System.DateOnly FechaFin,
        System.Collections.Generic.List<EmpleadoResponseDto> Empleados,
        decimal TotalNomina);
}