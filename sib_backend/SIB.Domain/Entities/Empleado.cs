using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SB.Management.Domain.Entities
{
    public abstract class Empleado
    {
        public int Id { get; set; }
        public string? PrimerNombre { get; set; }
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string NumeroSeguroSocial { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public string Estado { get; set; } = "Activo";
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public ICollection<Pago> Pagos { get; set; } = new List<Pago>();

        public abstract decimal CalcularPago();
    }
}
