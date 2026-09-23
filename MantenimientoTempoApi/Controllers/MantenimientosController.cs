using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MantenimientoTempoApi.Repositories.Interfaces;
using MantenimientoTempoModels.DTOs.Mantenimientos;

namespace MantenimientoTempoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MantenimientosController : ControllerBase
    {
        private readonly IMantenimientosRepository _mantenimientosRepo;
        private readonly IMaquinasRepository _maquinasRepo;

        public MantenimientosController(IMantenimientosRepository mantenimientosRepo, IMaquinasRepository maquinasRepo)
        {
            _mantenimientosRepo = mantenimientosRepo;
            _maquinasRepo = maquinasRepo;
        }

        [HttpGet]
        public async Task<IActionResult> ListarMantenimientos([FromQuery] string? estado = null)
        {
            try
            {
                var mantenimientos = await _mantenimientosRepo.ListarAsync(estado);
                return Ok(mantenimientos);
            }
            catch
            {
                return StatusCode(500, new { mensaje = "Error al obtener la lista de mantenimientos." });
            }
        }

        [HttpPost("programar")]
        public async Task<IActionResult> Programar([FromBody] ProgramarMantenimientoDto dto)
        {
            if (dto.MaquinaId <= 0 || dto.UsuarioId <= 0)
                return BadRequest(new { mensaje = "Debe especificar una máquina y técnico válidos." });

            try
            {
                var kilosData = await _maquinasRepo.ObtenerKilosAsync(dto.MaquinaId);
                decimal kilosActuales = kilosData?.KilosAcumulados ?? 0;

                await _mantenimientosRepo.ProgramarAsync(dto, kilosActuales);
                return Ok(new { mensaje = "Mantenimiento programado correctamente." });
            }
            catch
            {
                return StatusCode(500, new { mensaje = "Error al programar el mantenimiento." });
            }
        }

        [HttpPost("ejecutar")]
        public async Task<IActionResult> Ejecutar([FromBody] EjecutarMantenimientoDto dto)
        {
            if (dto.MantenimientoId <= 0)
                return BadRequest(new { mensaje = "ID de mantenimiento inválido." });

            try
            {
                var maquinaId = await _mantenimientosRepo.ObtenerMaquinaIdPorMantenimientoAsync(dto.MantenimientoId);
                if (!maquinaId.HasValue)
                    return NotFound(new { mensaje = "Mantenimiento no encontrado." });

                string ahora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                await _mantenimientosRepo.CompletarMantenimientoAsync(
                    dto.MantenimientoId,
                    ahora,
                    dto.SeCambiaronAgujas,
                    dto.TipoAgujaInstalada,
                    dto.CantidadAgujasCambiadas,
                    dto.Observaciones
                );

                foreach (var act in dto.Actividades)
                {
                    await _mantenimientosRepo.RegistrarDetalleActividadAsync(
                        dto.MantenimientoId,
                        act.PlanActividadId,
                        act.DescripcionActividad,
                        act.Completada,
                        act.Observaciones
                    );
                }

                if (dto.SeCambiaronAgujas)
                {
                    await _maquinasRepo.ResetearContadorAgujasAsync(maquinaId.Value, dto.TipoAgujaInstalada, ahora);
                }

                return Ok(new { mensaje = "Mantenimiento ejecutado y registrado correctamente." });
            }
            catch
            {
                return StatusCode(500, new { mensaje = "Error al registrar la ejecución del mantenimiento." });
            }
        }
    }
}
