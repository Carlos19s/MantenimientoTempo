using System.Globalization;
using MantenimientoTempoApi.Data;
using MantenimientoTempoApi.Repositories.Interfaces;
using MantenimientoTempoModels;
using MantenimientoTempoModels.DTOs.Maquinas;

namespace MantenimientoTempoApi.Repositories.Implementations
{
    public class MaquinasRepository : IMaquinasRepository
    {
        private readonly TursoDatabaseContext _context;

        public MaquinasRepository(TursoDatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<MaquinaDto>> ObtenerTodasActivasAsync()
        {
            using var client = await _context.GetClientAsync();
            const string sql = @"
                SELECT Id, CodigoMaquina, Marca, Modelo, NumeroSerie, FotoRuta,
                       Galga, DiametroPulgadas, NumeroAlimentadores, TipoAgujaActual,
                       FechaMontajeAguja, KilosAcumuladosAguja, LimiteKilosAguja,
                       KilosMetaPedido, KilosTejidosPedido, EstadoSemaforo, Activo,
                       ProximaFechaMantenimiento, ProximaFrecuenciaMantenimiento
                FROM Maquinas
                WHERE Activo = 1
                ORDER BY CodigoMaquina ASC;";

            var result = await client.Execute(sql);
            var lista = new List<MaquinaDto>();

            foreach (var row in result.Rows)
            {
                var cols = row.ToList();
                decimal kilosAcum = decimal.Parse(cols[11]?.ToString() ?? "0", CultureInfo.InvariantCulture);
                decimal limiteKilos = decimal.Parse(cols[12]?.ToString() ?? "15000", CultureInfo.InvariantCulture);

                int semaforo = 0;
                if (limiteKilos > 0)
                {
                    decimal ratio = kilosAcum / limiteKilos;
                    if (ratio >= 0.90m) semaforo = 2;
                    else if (ratio >= 0.75m) semaforo = 1;
                }

                lista.Add(new MaquinaDto
                {
                    Id = int.Parse(cols[0]?.ToString() ?? "0"),
                    CodigoMaquina = cols[1]?.ToString() ?? "",
                    Marca = cols[2]?.ToString() ?? "",
                    Modelo = cols[3]?.ToString(),
                    NumeroSerie = cols[4]?.ToString(),
                    FotoRuta = cols[5]?.ToString(),
                    Galga = cols[6] != null ? int.Parse(cols[6]!.ToString()!) : null,
                    DiametroPulgadas = cols[7] != null ? int.Parse(cols[7]!.ToString()!) : null,
                    NumeroAlimentadores = cols[8] != null ? int.Parse(cols[8]!.ToString()!) : null,
                    TipoAgujaActual = cols[9]?.ToString() ?? "",
                    FechaMontajeAguja = DateTime.TryParse(cols[10]?.ToString(), out var fm) ? fm : DateTime.Now,
                    KilosAcumuladosAguja = kilosAcum,
                    LimiteKilosAguja = limiteKilos,
                    KilosMetaPedido = decimal.Parse(cols[13]?.ToString() ?? "0", CultureInfo.InvariantCulture),
                    KilosTejidosPedido = decimal.Parse(cols[14]?.ToString() ?? "0", CultureInfo.InvariantCulture),
                    EstadoSemaforo = (EstadoSemaforo)semaforo,
                    Activo = (cols[16]?.ToString() ?? "1") == "1",
                    ProximaFechaMantenimiento = DateTime.TryParse(cols[17]?.ToString(), out var fp) ? fp : null,
                    ProximaFrecuenciaMantenimiento = cols[18]?.ToString()
                });
            }

            return lista;
        }

        public async Task<bool> ExisteCodigoAsync(string codigo)
        {
            using var client = await _context.GetClientAsync();
            var res = await client.Execute("SELECT 1 FROM Maquinas WHERE CodigoMaquina = ? LIMIT 1;", codigo.Trim().ToUpper());
            return res.Rows.Any();
        }

        public async Task<bool> CrearMaquinaAsync(CrearMaquinaDto dto)
        {
            using var client = await _context.GetClientAsync();
            const string sql = @"
                INSERT INTO Maquinas (
                    CodigoMaquina, Marca, Modelo, NumeroSerie, FotoRuta,
                    Galga, DiametroPulgadas, NumeroAlimentadores, TipoAgujaActual,
                    LimiteKilosAguja, KilosMetaPedido, ProximaFechaMantenimiento,
                    ProximaFrecuenciaMantenimiento, Activo, EstadoSemaforo
                ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, 1, 0);";

            await client.Execute(
                sql,
                dto.CodigoMaquina.Trim().ToUpper(),
                dto.Marca.Trim(),
                (object?)dto.Modelo?.Trim() ?? DBNull.Value,
                (object?)dto.NumeroSerie?.Trim() ?? DBNull.Value,
                (object?)dto.FotoRuta?.Trim() ?? DBNull.Value,
                (object?)dto.Galga ?? DBNull.Value,
                (object?)dto.DiametroPulgadas ?? DBNull.Value,
                (object?)dto.NumeroAlimentadores ?? DBNull.Value,
                dto.TipoAgujaActual.Trim(),
                (double)dto.LimiteKilosAguja,
                (double)dto.KilosMetaPedido,
                dto.ProximaFechaMantenimiento.HasValue ? dto.ProximaFechaMantenimiento.Value.ToString("yyyy-MM-dd HH:mm:ss") : DBNull.Value,
                (object?)dto.ProximaFrecuenciaMantenimiento?.Trim() ?? "MENSUAL"
            );
            return true;
        }

        public async Task<(decimal KilosAcumulados, decimal LimiteKilos)?> ObtenerKilosAsync(int maquinaId)
        {
            using var client = await _context.GetClientAsync();
            var res = await client.Execute("SELECT KilosAcumuladosAguja, LimiteKilosAguja FROM Maquinas WHERE Id = ? AND Activo = 1;", maquinaId);
            var row = res.Rows.FirstOrDefault();
            if (row == null) return null;

            var cols = row.ToList();
            return (
                decimal.Parse(cols[0]?.ToString() ?? "0", CultureInfo.InvariantCulture),
                decimal.Parse(cols[1]?.ToString() ?? "15000", CultureInfo.InvariantCulture)
            );
        }

        public async Task<bool> ActualizarDesgasteYSemaforoAsync(int maquinaId, decimal nuevosKilos, int nuevoSemaforo)
        {
            using var client = await _context.GetClientAsync();
            const string sql = "UPDATE Maquinas SET KilosAcumuladosAguja = ?, EstadoSemaforo = ? WHERE Id = ?;";
            await client.Execute(sql, (double)nuevosKilos, nuevoSemaforo, maquinaId);
            return true;
        }

        public async Task<bool> ResetearContadorAgujasAsync(int maquinaId, string? nuevoTipoAguja, string fechaMontaje)
        {
            using var client = await _context.GetClientAsync();
            if (!string.IsNullOrWhiteSpace(nuevoTipoAguja))
            {
                const string sql = @"
                    UPDATE Maquinas 
                    SET KilosAcumuladosAguja = 0, FechaMontajeAguja = ?, EstadoSemaforo = 0, TipoAgujaActual = ?
                    WHERE Id = ?;";
                await client.Execute(sql, fechaMontaje, nuevoTipoAguja.Trim(), maquinaId);
            }
            else
            {
                const string sql = @"
                    UPDATE Maquinas 
                    SET KilosAcumuladosAguja = 0, FechaMontajeAguja = ?, EstadoSemaforo = 0
                    WHERE Id = ?;";
                await client.Execute(sql, fechaMontaje, maquinaId);
            }
            return true;
        }
            public async Task<MaquinaDto?> ObtenerPorIdAsync(int id)
        {
            using var client = await _context.GetClientAsync();
            const string sql = @"
                SELECT Id, CodigoMaquina, Marca, Modelo, NumeroSerie, FotoRuta,
                       Galga, DiametroPulgadas, NumeroAlimentadores, TipoAgujaActual,
                       FechaMontajeAguja, KilosAcumuladosAguja, LimiteKilosAguja,
                       KilosMetaPedido, KilosTejidosPedido, EstadoSemaforo, Activo,
                       ProximaFechaMantenimiento, ProximaFrecuenciaMantenimiento
                FROM Maquinas
                WHERE Id = ? AND Activo = 1
                LIMIT 1;";

            var result = await client.Execute(sql, id);
            var row = result.Rows.FirstOrDefault();
            if (row == null) return null;

            var cols = row.ToList();
            decimal kilosAcum = decimal.Parse(cols[11]?.ToString() ?? "0", CultureInfo.InvariantCulture);
            decimal limiteKilos = decimal.Parse(cols[12]?.ToString() ?? "15000", CultureInfo.InvariantCulture);

            int semaforo = 0;
            if (limiteKilos > 0)
            {
                decimal ratio = kilosAcum / limiteKilos;
                if (ratio >= 0.90m) semaforo = 2;
                else if (ratio >= 0.75m) semaforo = 1;
            }

            return new MaquinaDto
            {
                Id = int.Parse(cols[0]?.ToString() ?? "0"),
                CodigoMaquina = cols[1]?.ToString() ?? "",
                Marca = cols[2]?.ToString() ?? "",
                Modelo = cols[3]?.ToString(),
                NumeroSerie = cols[4]?.ToString(),
                FotoRuta = cols[5]?.ToString(),
                Galga = cols[6] != null ? int.Parse(cols[6]!.ToString()!) : null,
                DiametroPulgadas = cols[7] != null ? int.Parse(cols[7]!.ToString()!) : null,
                NumeroAlimentadores = cols[8] != null ? int.Parse(cols[8]!.ToString()!) : null,
                TipoAgujaActual = cols[9]?.ToString() ?? "",
                FechaMontajeAguja = DateTime.TryParse(cols[10]?.ToString(), out var fm) ? fm : DateTime.Now,
                KilosAcumuladosAguja = kilosAcum,
                LimiteKilosAguja = limiteKilos,
                KilosMetaPedido = decimal.Parse(cols[13]?.ToString() ?? "0", CultureInfo.InvariantCulture),
                KilosTejidosPedido = decimal.Parse(cols[14]?.ToString() ?? "0", CultureInfo.InvariantCulture),
                EstadoSemaforo = (EstadoSemaforo)semaforo,
                Activo = (cols[16]?.ToString() ?? "1") == "1",
                ProximaFechaMantenimiento = DateTime.TryParse(cols[17]?.ToString(), out var fp) ? fp : null,
                ProximaFrecuenciaMantenimiento = cols[18]?.ToString()
            };
        }

        public async Task<bool> ActualizarMaquinaAsync(int id, ActualizarMaquinaDto dto)
        {
            using var client = await _context.GetClientAsync();
            const string sql = @"
                UPDATE Maquinas SET
                    Marca = ?, Modelo = ?, NumeroSerie = ?, FotoRuta = ?,
                    Galga = ?, DiametroPulgadas = ?, NumeroAlimentadores = ?,
                    TipoAgujaActual = ?, LimiteKilosAguja = ?, KilosMetaPedido = ?,
                    ProximaFechaMantenimiento = ?, ProximaFrecuenciaMantenimiento = ?
                WHERE Id = ? AND Activo = 1;";

            await client.Execute(
                sql,
                dto.Marca.Trim(),
                (object?)dto.Modelo?.Trim() ?? DBNull.Value,
                (object?)dto.NumeroSerie?.Trim() ?? DBNull.Value,
                (object?)dto.FotoRuta?.Trim() ?? DBNull.Value,
                (object?)dto.Galga ?? DBNull.Value,
                (object?)dto.DiametroPulgadas ?? DBNull.Value,
                (object?)dto.NumeroAlimentadores ?? DBNull.Value,
                dto.TipoAgujaActual.Trim(),
                (double)dto.LimiteKilosAguja,
                (double)dto.KilosMetaPedido,
                dto.ProximaFechaMantenimiento.HasValue ? dto.ProximaFechaMantenimiento.Value.ToString("yyyy-MM-dd HH:mm:ss") : DBNull.Value,
                (object?)dto.ProximaFrecuenciaMantenimiento?.Trim() ?? "MENSUAL",
                id
            );
            return true;
        }

    }
}
