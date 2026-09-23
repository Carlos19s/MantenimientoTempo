using Microsoft.AspNetCore.Mvc;
using MantenimientoTempoApi.Data;

namespace MantenimientoTempoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquiposController : ControllerBase
    {
        private readonly TursoDatabaseContext _context;

        public EquiposController(TursoDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("validar/{hardwareId}")]
        public async Task<IActionResult> ValidarEquipo(string hardwareId)
        {
            if (string.IsNullOrWhiteSpace(hardwareId))
                return BadRequest(new { autorizado = false, mensaje = "Hardware ID requerido." });

            try
            {
                using var client = await _context.GetClientAsync();
                const string sql = "SELECT NombreEquipo, Activo FROM EquiposAutorizados WHERE HardwareId = ? LIMIT 1;";
                var res = await client.Execute(sql, hardwareId.Trim());

                var row = res.Rows.FirstOrDefault();
                if (row == null)
                {
                    return Ok(new { autorizado = false, registrado = false, mensaje = "Equipo no registrado en el sistema." });
                }

                var cols = row.ToList();
                string nombre = cols[0]?.ToString() ?? "Equipo";
                bool activo = (cols[1]?.ToString() ?? "0") == "1";

                if (!activo)
                {
                    return Ok(new { autorizado = false, registrado = true, mensaje = "Este equipo ha sido revocado o suspendido." });
                }

                return Ok(new { autorizado = true, registrado = true, nombreEquipo = nombre });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { autorizado = false, mensaje = $"Error al validar equipo: {ex.Message}" });
            }
        }

        // Endpoint administrativo para dar de alta un equipo
        [HttpPost("autorizar")]
        public async Task<IActionResult> AutorizarEquipo([FromBody] AutorizarEquipoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.HardwareId))
                return BadRequest("El HardwareId es obligatorio.");

            try
            {
                using var client = await _context.GetClientAsync();
                const string sql = @"
                    INSERT INTO EquiposAutorizados (HardwareId, NombreEquipo, Activo, FechaAutorizacion)
                    VALUES (?, ?, 1, datetime('now'))
                    ON CONFLICT(HardwareId) DO UPDATE SET Activo = 1, NombreEquipo = excluded.NombreEquipo;";

                await client.Execute(sql, request.HardwareId.Trim(), request.NombreEquipo.Trim());
                return Ok(new { mensaje = "Equipo autorizado correctamente en Turso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al registrar: {ex.Message}" });
            }
        }
    }

    public class AutorizarEquipoRequest
    {
        public string HardwareId { get; set; } = string.Empty;
        public string NombreEquipo { get; set; } = string.Empty;
    }
}
