using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MantenimientoTempoApi.Repositories.Interfaces;
using MantenimientoTempoModels.DTOs.Mantenimientos;

namespace MantenimientoTempoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlanesMantenimientoController : ControllerBase
    {
        private readonly IPlanesMantenimientoRepository _planesRepo;

        public PlanesMantenimientoController(IPlanesMantenimientoRepository planesRepo)
        {
            _planesRepo = planesRepo;
        }

        [HttpGet("maquina/{maquinaId}")]
        public async Task<IActionResult> ObtenerPorMaquina(int maquinaId)
        {
            try
            {
                var planes = await _planesRepo.ObtenerPlanesPorMaquinaAsync(maquinaId);
                return Ok(planes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al recuperar planes de mantenimiento: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearPlan([FromBody] CrearPlanMantenimientoDto dto)
        {
            if (dto.MaquinaId <= 0 || string.IsNullOrWhiteSpace(dto.Frecuencia))
                return BadRequest(new { mensaje = "Máquina y frecuencia son campos obligatorios." });

            try
            {
                var creado = await _planesRepo.CrearPlanConActividadesAsync(dto);
                if (creado)
                    return Ok(new { mensaje = "Plan de mantenimiento registrado exitosamente." });

                return StatusCode(500, new { mensaje = "No se pudo registrar el plan." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al registrar plan: {ex.Message}" });
            }
        }
    }
}