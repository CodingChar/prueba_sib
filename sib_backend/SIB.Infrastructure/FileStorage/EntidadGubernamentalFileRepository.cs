using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SB.Management.Application.Interfaces;
using SB.Management.Domain.Entities;

namespace SB.Management.Infrastructure.FileStorage
{
    public class EntidadGubernamentalFileRepository : IEntidadGubernamentalRepository
    {
        private readonly string _rutaArchivo;

        private static readonly JsonSerializerOptions OpcionesJson = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public EntidadGubernamentalFileRepository(IConfiguration configuration)
        {
            var rutaRelativa = configuration["AlmacenamientoArchivos:EntidadesGubernamentales"]
                ?? throw new InvalidOperationException("Falta la configuración 'AlmacenamientoArchivos:EntidadesGubernamentales' en appsettings.json");

            _rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), rutaRelativa);

            var carpeta = Path.GetDirectoryName(_rutaArchivo);
            if (!string.IsNullOrEmpty(carpeta) && !Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }
        }

        public async Task<List<EntidadGubernamental>> ObtenerTodasAsync()
        {
            if (!File.Exists(_rutaArchivo))
            {
                return new List<EntidadGubernamental>();
            }

            var json = await File.ReadAllTextAsync(_rutaArchivo);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<EntidadGubernamental>();
            }

            return JsonSerializer.Deserialize<List<EntidadGubernamental>>(json, OpcionesJson)
                   ?? new List<EntidadGubernamental>();
        }

        public async Task<EntidadGubernamental?> ObtenerPorIdAsync(int id)
        {
            var entidades = await ObtenerTodasAsync();
            return entidades.FirstOrDefault(e => e.Id == id);
        }

        public async Task AgregarAsync(EntidadGubernamental entidad)
        {
            var entidades = await ObtenerTodasAsync();
            var siguienteId = entidades.Count == 0 ? 1 : entidades.Max(e => e.Id) + 1;
            entidad.Id = siguienteId;
            entidades.Add(entidad);
            await GuardarTodasAsync(entidades);
        }

        public async Task ActualizarAsync(EntidadGubernamental entidad)
        {
            var entidades = await ObtenerTodasAsync();
            var indice = entidades.FindIndex(e => e.Id == entidad.Id);
            if (indice >= 0)
            {
                entidades[indice] = entidad;
                await GuardarTodasAsync(entidades);
            }
        }

        public async Task EliminarAsync(int id)
        {
            var entidades = await ObtenerTodasAsync();
            entidades.RemoveAll(e => e.Id == id);
            await GuardarTodasAsync(entidades);
        }

        private async Task GuardarTodasAsync(List<EntidadGubernamental> entidades)
        {
            var json = JsonSerializer.Serialize(entidades, OpcionesJson);
            await File.WriteAllTextAsync(_rutaArchivo, json);
        }
    }
}