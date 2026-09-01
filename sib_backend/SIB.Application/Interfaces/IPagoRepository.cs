using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SB.Management.Domain.Entities;

namespace SB.Management.Application.Interfaces
{
    public interface IPagoRepository
    {
        Task AgregarAsync(Pago pago);
        Task<List<Pago>> ObtenerPorPeriodoAsync(DateOnly fechaInicio, DateOnly fechaFin);
        Task GuardarCambiosAsync();
    }
}