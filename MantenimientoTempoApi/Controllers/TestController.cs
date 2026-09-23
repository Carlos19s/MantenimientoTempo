using Microsoft.AspNetCore.Mvc;
using MantenimientoTempoApi.Data;

namespace MantenimientoTempoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly TursoDatabaseContext _context;

        public TestController(TursoDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("conexion")]
        public async Task<IActionResult> ProbarConexion()
        {
            try
            {
                using var client = await _context.GetClientAsync();

                // Ejecuta la consulta de comprobación
                var resultSet = await client.Execute("SELECT 1 AS Conectado;");

                // Obtenemos la primera fila si existe
                var filas = resultSet.Rows.ToList();
                string valor = "Sin filas";

                if (filas.Count > 0)
                {
                    // Obtiene el primer valor de la primera fila
                    var primeraFila = filas[0];
                    var primerValor = primeraFila.FirstOrDefault();
                    valor = primerValor?.ToString() ?? "OK";
                }

                return Ok(new
                {
                    mensaje = "Conexión a Turso exitosa",
                    resultado = valor
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Error al conectar con Turso",
                    detalle = ex.Message
                });
            }
        }
    }
}