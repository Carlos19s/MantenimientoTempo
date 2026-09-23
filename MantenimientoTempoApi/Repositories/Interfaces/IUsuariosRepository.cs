using MantenimientoTempoModels.DTOs.Auth;

namespace MantenimientoTempoApi.Repositories.Interfaces
{
    public interface IUsuariosRepository
    {
        Task<(int Id, string NombreCompleto, string Username, string PasswordHash, bool Activo, string RolNombre, int RolId)?> ObtenerPorUsernameAsync(string username);
        Task<List<string>> ObtenerPermisosPorRolIdAsync(int rolId);
        Task<bool> ExisteUsernameAsync(string username);
        Task<bool> CrearUsuarioAsync(int rolId, string nombreCompleto, string username, string passwordHash, string? email);
        Task<List<UsuarioDto>> ListarUsuariosAsync();
        Task<bool> CambiarEstadoUsuarioAsync(int usuarioId, bool activo);
        Task<List<RolDto>> ListarRolesAsync();
        Task<List<PermisoDto>> ListarPermisosAsync();
    }
}
