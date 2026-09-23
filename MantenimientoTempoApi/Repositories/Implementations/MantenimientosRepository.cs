using System.Globalization;
using MantenimientoTempoApi.Data;
using MantenimientoTempoApi.Repositories.Interfaces;
using MantenimientoTempoModels.DTOs.Mantenimientos;

namespace MantenimientoTempoApi.Repositories.Implementations
{
    public class MantenimientosRepository : IMantenimientosRepository
    {
        private readonly TursoDatabaseContext _context;

        public MantenimientosRepository(TursoDatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<MantenimientoResumenDto>> ListarAsync(string? estado)
        {
            using var client = await _context.GetClientAsync();
            Libsql.Client.IResultSet result;

            if (!string.IsNullOrWhiteSpace(estado))
            {
                const string sqlConFiltro = @"
                    SELECT m.Id, m.MaquinaId, maq.CodigoMaquina, maq.Marca, u.NombreCompleto as Tecnico,
                           m.TipoMantenimiento, m.Estado, m.FechaProgramada, m.FechaEjecucion,
                           m.KilosMaquinaALaFecha, m.SeCambiaronAgujas, m.CantidadAgujasCambiadas, m.Observaciones
                    FROM Mantenimientos m
                    INNER JOIN Maquinas maq ON m.MaquinaId = maq.Id
                    INNER JOIN Usuarios u ON m.UsuarioId = u.Id
                    WHERE UPPER(m.Estado) = UPPER(?)
                    ORDER BY m.FechaProgramada DESC;";
                result = await client.Execute(sqlConFiltro, estado.Trim());
            }
            else
            {
                const string sqlTodo = @"
                    SELECT m.Id, m.MaquinaId, maq.CodigoMaquina, maq.Marca, u.NombreCompleto as Tecnico,
                           m.TipoMantenimiento, m.Estado, m.FechaProgramada, m.FechaEjecucion,
                           m.KilosMaquinaALaFecha, m.SeCambiaronAgujas, m.CantidadAgujasCambiadas, m.Observaciones
                    FROM Mantenimientos m
                    INNER JOIN Maquinas maq ON m.MaquinaId = maq.Id
                    INNER JOIN Usuarios u ON m.UsuarioId = u.Id
                    ORDER BY m.FechaProgramada DESC;";
                result = await client.Execute(sqlTodo);
            }

            var lista = new List<MantenimientoResumenDto>();
            foreach (var row in result.Rows)
            {
                var cols = row.ToList();
                lista.Add(new MantenimientoResumenDto
                {
                    Id = int.Parse(cols[0]?.ToString() ?? "0"),
                    MaquinaId = int.Parse(cols[1]?.ToString() ?? "0"),
                    CodigoMaquina = cols[2]?.ToString() ?? "",
                    MarcaMaquina = cols[3]?.ToString() ?? "",
                    TecnicoNombre = cols[4]?.ToString() ?? "",
                    TipoMantenimiento = cols[5]?.ToString() ?? "",
                    Estado = cols[6]?.ToString() ?? "",
                    FechaProgramada = DateTime.TryParse(cols[7]?.ToString(), out var fp) ? fp : DateTime.Now,
                    FechaEjecucion = DateTime.TryParse(cols[8]?.ToString(), out var fe) ? fe : null,
                    KilosMaquinaALaFecha = decimal.Parse(cols[9]?.ToString() ?? "0", CultureInfo.InvariantCulture),
                    SeCambiaronAgujas = (cols[10]?.ToString() ?? "0") == "1",
                    CantidadAgujasCambiadas = int.Parse(cols[11]?.ToString() ?? "0"),
                    Observaciones = cols[12]?.ToString()
                });
            }
            return lista;
        }

        public async Task<bool> ProgramarAsync(ProgramarMantenimientoDto dto, decimal kilosActuales)
        {
            using var client = await _context.GetClientAsync();
            const string sql = @"
                INSERT INTO Mantenimientos (
                    MaquinaId, PlanesMantenimientoId, UsuarioId, TipoMantenimiento,
                    Estado, FechaProgramada, KilosMaquinaALaFecha, SeCambiaronAgujas,
                    CantidadAgujasCambiadas, Observaciones
                ) VALUES (?, ?, ?, ?, 'PROGRAMADO', ?, ?, 0, 0, ?);";

            await client.Execute(
                sql,
                dto.MaquinaId,
                (object?)dto.PlanesMantenimientoId ?? DBNull.Value,
                dto.UsuarioId,
                dto.TipoMantenimiento.ToUpper(),
                dto.FechaProgramada.ToString("yyyy-MM-dd HH:mm:ss"),
                (double)kilosActuales,
                (object?)dto.Observaciones ?? DBNull.Value
            );
            return true;
        }

        public async Task<int?> ObtenerMaquinaIdPorMantenimientoAsync(int mantenimientoId)
        {
            using var client = await _context.GetClientAsync();
            var res = await client.Execute("SELECT MaquinaId FROM Mantenimientos WHERE Id = ?;", mantenimientoId);
            var row = res.Rows.FirstOrDefault();
            return row != null ? int.Parse(row.ToList()[0]?.ToString() ?? "0") : null;
        }

        public async Task<bool> CompletarMantenimientoAsync(int mantenimientoId, string fechaEjecucion, bool seCambiaronAgujas, string? tipoAguja, int cantidadAgujas, string? observaciones)
        {
            using var client = await _context.GetClientAsync();
            const string sql = @"
                UPDATE Mantenimientos 
                SET Estado = 'COMPLETADO',
                    FechaEjecucion = ?,
                    SeCambiaronAgujas = ?,
                    TipoAgujaInstalada = ?,
                    CantidadAgujasCambiadas = ?,
                    Observaciones = ?
                WHERE Id = ?;";

            await client.Execute(
                sql,
                fechaEjecucion,
                seCambiaronAgujas ? 1 : 0,
                (object?)tipoAguja ?? DBNull.Value,
                cantidadAgujas,
                (object?)observaciones ?? DBNull.Value,
                mantenimientoId
            );
            return true;
        }

        public async Task<bool> RegistrarDetalleActividadAsync(int mantenimientoId, int? planActividadId, string descripcion, bool completada, string? observaciones)
        {
            using var client = await _context.GetClientAsync();
            const string sql = @"
                INSERT INTO MantenimientoDetalles (
                    MantenimientoId, PlanActividadId, DescripcionActividad, Completada, Observaciones
                ) VALUES (?, ?, ?, ?, ?);";

            await client.Execute(
                sql,
                mantenimientoId,
                (object?)planActividadId ?? DBNull.Value,
                descripcion,
                completada ? 1 : 0,
                (object?)observaciones ?? DBNull.Value
            );
            return true;
        }
    }
}
