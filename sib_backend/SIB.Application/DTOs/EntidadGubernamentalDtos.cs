namespace SB.Management.Application.DTOs
{
    public record CrearEntidadGubernamentalDto(
        string Nombre,
        string Categoria,
        string PoderDelEstado,
        string Sector);

    public record EntidadGubernamentalResponseDto(
        int Id,
        string Nombre,
        string Categoria,
        string PoderDelEstado,
        string Sector);
}