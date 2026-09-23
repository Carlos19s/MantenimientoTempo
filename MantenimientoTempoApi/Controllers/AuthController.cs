using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MantenimientoTempoApi.Repositories.Interfaces;
using MantenimientoTempoModels.DTOs.Auth;

namespace MantenimientoTempoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuariosRepository _usuariosRepo;
        private readonly IConfiguration _config;

        public AuthController(IUsuariosRepository usuariosRepo, IConfiguration config)
        {
            _usuariosRepo = usuariosRepo;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new LoginResponseDto { Exito = false, Mensaje = "Credenciales incompletas." });

            try
            {
                var usuario = await _usuariosRepo.ObtenerPorUsernameAsync(dto.Username);
                if (usuario == null)
                    return Unauthorized(new LoginResponseDto { Exito = false, Mensaje = "Usuario o contraseña incorrectos." });

                var u = usuario.Value;
                if (!u.Activo)
                    return Unauthorized(new LoginResponseDto { Exito = false, Mensaje = "El usuario se encuentra inactivo." });

                if (!BCrypt.Net.BCrypt.Verify(dto.Password, u.PasswordHash))
                    return Unauthorized(new LoginResponseDto { Exito = false, Mensaje = "Usuario o contraseña incorrectos." });

                var permisos = await _usuariosRepo.ObtenerPermisosPorRolIdAsync(u.RolId);
                string token = GenerarJwtToken(u.Id, u.Username, u.RolNombre, permisos);

                return Ok(new LoginResponseDto
                {
                    Exito = true,
                    Mensaje = "Inicio de sesión correcto.",
                    Token = token,
                    UsuarioId = u.Id,
                    NombreCompleto = u.NombreCompleto,
                    Username = u.Username,
                    Rol = u.RolNombre,
                    Permisos = permisos
                });
            }
            catch
            {
                return StatusCode(500, new LoginResponseDto { Exito = false, Mensaje = "Error interno de autenticación." });
            }
        }

        [HttpPost("crear-usuario-inicial")]
        public async Task<IActionResult> CrearUsuarioInicial(string username, string password, string nombreCompleto, int rolId = 1)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(nombreCompleto))
                return BadRequest(new { mensaje = "Datos obligatorios faltantes." });

            try
            {
                if (await _usuariosRepo.ExisteUsernameAsync(username))
                    return Conflict(new { mensaje = "El nombre de usuario ya existe." });

                string hash = BCrypt.Net.BCrypt.HashPassword(password);
                // Línea corregida:
                await _usuariosRepo.CrearUsuarioAsync(rolId, nombreCompleto, username, hash, null);


                return Ok(new { mensaje = "Usuario creado correctamente." });
            }
            catch
            {
                return StatusCode(500, new { mensaje = "Error al registrar usuario." });
            }
        }

        private string GenerarJwtToken(int userId, string username, string rol, List<string> permisos)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, rol)
            };

            foreach (var permiso in permisos) claims.Add(new Claim("permiso", permiso));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            int horas = int.TryParse(_config["Jwt:ExpiracionHoras"], out int h) ? h : 12;

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(horas),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}