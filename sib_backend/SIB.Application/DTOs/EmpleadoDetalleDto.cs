namespace SB.Management.Application.DTOs
{
    public record EmpleadoDetalleDto(
        int Id,
        string Tipo,
        string? PrimerNombre,
        string ApellidoPaterno,
        string NumeroSeguroSocial,
        string Departamento,
        string Estado,
        decimal? SalarioSemanal,
        decimal? SueldoPorHora,
        decimal? HorasTrabajadas,
        decimal? VentasBrutas,
        decimal? TarifaComision,
        decimal? SalarioBase);
}