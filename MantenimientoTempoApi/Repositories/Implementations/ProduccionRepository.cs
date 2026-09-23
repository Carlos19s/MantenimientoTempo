using MantenimientoTempoApi.Data;
using MantenimientoTempoApi.Repositories.Interfaces;
using MantenimientoTempoModels.DTOs.Produccion;

namespace MantenimientoTempoApi.Repositories.Implementations
{
    public class ProduccionRepository : IProduccionRepository
    {
        private readonly TursoDatabaseContext _context;

        public ProduccionRepository(TursoDatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> RegistrarProduccionAsync(RegistrarProduccionDto dto)
        {
            using var client = await _context.GetClientAsync();
            const string sql = @"
                INSERT INTO RegistrosProduccion (MaquinaId, FechaRegistro, KilosTurno, NombreArchivoExcel)
                VALUES (?, ?, ?, ?);";

            await client.Execute(
                sql,
                dto.MaquinaId,
                dto.FechaRegistro.ToString("yyyy-MM-dd HH:mm:ss"),
                (double)dto.KilosTurno,
                (object?)dto.NombreArchivoExcel?.Trim() ?? DBNull.Value
            );
            return true;
        }
    }
}