using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MantenimientoTempoApi.Repositories.Interfaces;
using MantenimientoTempoModels.DTOs.Produccion;

namespace MantenimientoTempoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProduccionController : ControllerBase
    {
        private readonly IProduccionRepository _produccionRepo;
        private readonly IMaquinasRepository _maquinasRepo;

        public ProduccionController(IProduccionRepository produccionRepo, IMaquinasRepository maquinasRepo)
        {
            _produccionRepo = produccionRepo;
            _maquinasRepo = maquinasRepo;
        }

        [HttpPost("registrar-kilos")]
        public async Task<IActionResult> RegistrarKilos([FromBody] RegistrarProduccionDto dto)
        {
            if (dto.MaquinaId <= 0 || dto.KilosTurno <= 0)
                return BadRequest(new { mensaje = "Máquina y kilos deben ser válidos y mayores a 0." });

            try
            {
                var maqData = await _maquinasRepo.ObtenerKilosAsync(dto.MaquinaId);
                if (maqData == null)
                    return NotFound(new { mensaje = "Máquina no encontrada o inactiva." });

                decimal kilosActuales = maqData.Value.KilosAcumulados;
                decimal limiteKilos = maqData.Value.LimiteKilos;
                decimal nuevosKilos = kilosActuales + dto.KilosTurno;

                int nuevoSemaforo = 0;
                if (limiteKilos > 0)
                {
                    decimal ratio = nuevosKilos / limiteKilos;
                    if (ratio >= 0.90m) nuevoSemaforo = 2;
                    else if (ratio >= 0.75m) nuevoSemaforo = 1;
                }

                await _produccionRepo.RegistrarProduccionAsync(dto);
                await _maquinasRepo.ActualizarDesgasteYSemaforoAsync(dto.MaquinaId, nuevosKilos, nuevoSemaforo);

                return Ok(new
                {
                    mensaje = "Producción registrada y desgaste recalculado.",
                    kilosPrevios = kilosActuales,
                    nuevosKilosAcumulados = nuevosKilos,
                    nuevoEstadoSemaforo = nuevoSemaforo
                });
            }
            catch
            {
                return StatusCode(500, new { mensaje = "Error al procesar el registro de producción." });
            }
        }
    }
}