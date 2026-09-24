using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MantenimientoTempoApi.Repositories.Interfaces;
using MantenimientoTempoModels.DTOs.Auth;

namespace MantenimientoTempoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]//Problema encontrado
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosRepository _usuariosRepo;

        public UsuariosController(IUsuariosRepository usuariosRepo)
        {
            _usuariosRepo = usuariosRepo;
        }

        [HttpGet]
        public async Task<IActionResult> ListarUsuarios()
        {
            try
            {
                var usuarios = await _usuariosRepo.ListarUsuariosAsync();
                return Ok(usuarios);
            }
            catch
            {
                return StatusCode(500, new { mensaje = "Error al listar los usuarios." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] CrearUsuarioDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.NombreCompleto))
                return BadRequest(new { mensaje = "Nombre, usuario y contraseña son obligatorios." });

            if (dto.RolId <= 0)
                return BadRequest(new { mensaje = "Debe asignar un rol válido." });

            try
            {
                if (await _usuariosRepo.ExisteUsernameAsync(dto.Username))
                    return Conflict(new { mensaje = $"El usuario '{dto.Username}' ya existe." });

                string hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                await _usuariosRepo.CrearUsuarioAsync(dto.RolId, dto.NombreCompleto, dto.Username, hash, dto.Email);

                return Ok(new { mensaje = $"Usuario '{dto.Username}' creado con éxito." });
            }
            catch
            {
                return StatusCode(500, new { mensaje = "Error al registrar el usuario." });
            }
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromQuery] bool activo)
        {
            if (id <= 0) return BadRequest(new { mensaje = "ID de usuario inválido." });

            try
            {
                await _usuariosRepo.CambiarEstadoUsuarioAsync(id, activo);
                return Ok(new { mensaje = $"Usuario {(activo ? "activado" : "desactivado")} correctamente." });
            }
            catch
            {
                return StatusCode(500, new { mensaje = "Error al actualizar estado del usuario." });
            }
        }

        [HttpGet("roles")]
        public async Task<IActionResult> ListarRoles()
        {
            try
            {
                var roles = await _usuariosRepo.ListarRolesAsync();
                return Ok(roles);
            }
            catch
            {
                return StatusCode(500, new { mensaje = "Error al obtener roles." });
            }
        }

        [HttpGet("permisos")]
        public async Task<IActionResult> ListarPermisos()
        {
            try
            {
                var permisos = await _usuariosRepo.ListarPermisosAsync();
                return Ok(permisos);
            }
            catch
            {
                return StatusCode(500, new { mensaje = "Error al obtener permisos." });
            }
        }
    }
}
