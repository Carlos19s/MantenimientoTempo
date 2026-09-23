using Microsoft.AspNetCore.Mvc;

namespace MantenimientoTempoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActualizacionesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private const string VERSION_ACTUAL_SERVIDOR = "1.0.0";

        public ActualizacionesController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet("version")]
        public IActionResult ObtenerUltimaVersion()
        {
            return Ok(new
            {
                version = VERSION_ACTUAL_SERVIDOR,
                esObligatoria = true,
                mensaje = "Actualización de mantenimiento y mejoras del sistema."
            });
        }

        [HttpGet("descargar")]
        public IActionResult DescargarNuevaVersion()
        {
            // Busca el ejecutable publicado dentro de wwwroot/updates/
            string rutaArchivo = Path.Combine(_env.ContentRootPath, "Updates", "MantenimientoTempo.exe");

            if (!System.IO.File.Exists(rutaArchivo))
                return NotFound(new { mensaje = "El archivo de actualización no está disponible en el servidor." });

            byte[] bytes = System.IO.File.ReadAllBytes(rutaArchivo);
            return File(bytes, "application/octet-stream", "MantenimientoTempo_Update.exe");
        }
    }
}