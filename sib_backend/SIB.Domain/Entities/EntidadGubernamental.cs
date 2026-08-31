using System;

namespace SB.Management.Domain.Entities
{
    public class EntidadGubernamental
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string PoderDelEstado { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
    }
}