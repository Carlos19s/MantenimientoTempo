using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MantenimientoTempoApi.Repositories.Interfaces;
using MantenimientoTempoModels.DTOs.Maquinas;

namespace MantenimientoTempoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MaquinasController : ControllerBase
    {
        private readonly IMaquinasRepository _maquinasRepo;

        public MaquinasController(IMaquinasRepository maquinasRepo)
        {
            _maquinasRepo = maquinasRepo;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerMaquinas()
        {
            try
            {
                var maquinas = await _maquinasRepo.ObtenerTodasActivasAsync();
                return Ok(maquinas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al recuperar catálogo: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var maquina = await _maquinasRepo.ObtenerPorIdAsync(id);
                if (maquina == null)
                    return NotFound(new { mensaje = $"No se encontró la máquina con ID {id}." });

                return Ok(maquina);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al obtener máquina: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearMaquina([FromBody] CrearMaquinaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CodigoMaquina) || string.IsNullOrWhiteSpace(dto.Marca))
                return BadRequest(new { mensaje = "Código de máquina y marca son obligatorios." });

            try
            {
                if (await _maquinasRepo.ExisteCodigoAsync(dto.CodigoMaquina))
                    return Conflict(new { mensaje = $"La máquina '{dto.CodigoMaquina.ToUpper()}' ya está registrada." });

                await _maquinasRepo.CrearMaquinaAsync(dto);
                return Ok(new { mensaje = $"Máquina {dto.CodigoMaquina.ToUpper()} registrada exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al registrar máquina: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarMaquina(int id, [FromBody] ActualizarMaquinaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Marca))
                return BadRequest(new { mensaje = "La marca es obligatoria." });

            try
            {
                var existe = await _maquinasRepo.ObtenerPorIdAsync(id);
                if (existe == null)
                    return NotFound(new { mensaje = $"Máquina con ID {id} no encontrada." });

                await _maquinasRepo.ActualizarMaquinaAsync(id, dto);
                return Ok(new { mensaje = "Máquina actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al actualizar máquina: {ex.Message}" });
            }
        }
    }
}