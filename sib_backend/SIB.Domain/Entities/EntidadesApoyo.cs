using System;
using System.Collections.Generic;

namespace SB.Management.Domain.Entities
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int RolId { get; set; }
        public Rol? Rol { get; set; }
    }

    public class Pago
    {
        public int Id { get; set; }
        public int EmpleadoId { get; set; }
        public Empleado? Empleado { get; set; }
        public DateOnly FechaPago { get; set; }
        public decimal MontoCalculado { get; set; }
        public string? DetalleCalculo { get; set; }
        public DateTime FechaGeneracion { get; set; } = DateTime.UtcNow;
    }
}