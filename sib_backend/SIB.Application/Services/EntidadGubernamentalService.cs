using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SB.Management.Application.DTOs;
using SB.Management.Application.Interfaces;
using SB.Management.Domain.Entities;

namespace SB.Management.Application.Services
{
    public class EntidadGubernamentalService
    {
        private readonly IEntidadGubernamentalRepository _repositorio;
        private readonly ILogger<EntidadGubernamentalService> _logger;

        public EntidadGubernamentalService(
            IEntidadGubernamentalRepository repositorio,
            ILogger<EntidadGubernamentalService> logger)
        {
            _repositorio = repositorio;
            _logger = logger;
        }

        public async Task<List<EntidadGubernamentalResponseDto>> ObtenerTodasAsync()
        {
            var entidades = await _repositorio.ObtenerTodasAsync();
            return entidades.Select(MapearADto).ToList();
        }

        public async Task<EntidadGubernamentalResponseDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _repositorio.ObtenerPorIdAsync(id);
            return entidad is null ? null : MapearADto(entidad);
        }

        public async Task<int> CrearAsync(CrearEntidadGubernamentalDto dto)
        {
            var entidad = new EntidadGubernamental
            {
                Nombre = dto.Nombre,
                Categoria = dto.Categoria,
                PoderDelEstado = dto.PoderDelEstado,
                Sector = dto.Sector
            };

            await _repositorio.AgregarAsync(entidad);
            _logger.LogInformation("Entidad gubernamental '{Nombre}' creada con Id {Id}", entidad.Nombre, entidad.Id);
            return entidad.Id;
        }

        public async Task ActualizarAsync(int id, CrearEntidadGubernamentalDto dto)
        {
            var entidad = new EntidadGubernamental
            {
                Id = id,
                Nombre = dto.Nombre,
                Categoria = dto.Categoria,
                PoderDelEstado = dto.PoderDelEstado,
                Sector = dto.Sector
            };

            await _repositorio.ActualizarAsync(entidad);
            _logger.LogInformation("Entidad gubernamental {Id} actualizada", id);
        }

        public async Task EliminarAsync(int id)
        {
            await _repositorio.EliminarAsync(id);
            _logger.LogInformation("Entidad gubernamental {Id} eliminada", id);
        }

        private static EntidadGubernamentalResponseDto MapearADto(EntidadGubernamental entidad)
        {
            return new EntidadGubernamentalResponseDto(
                entidad.Id, entidad.Nombre, entidad.Categoria, entidad.PoderDelEstado, entidad.Sector);
        }
    }
}