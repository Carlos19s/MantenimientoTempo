using MantenimientoTempoApi.Data;
using MantenimientoTempoApi.Repositories.Interfaces;
using MantenimientoTempoModels.DTOs.Auth;

namespace MantenimientoTempoApi.Repositories.Implementations
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly TursoDatabaseContext _context;

        public UsuariosRepository(TursoDatabaseContext context)
        {
            _context = context;
        }

        public async Task<(int Id, string NombreCompleto, string Username, string PasswordHash, bool Activo, string RolNombre, int RolId)?> ObtenerPorUsernameAsync(string username)
        {
            using var client = await _context.GetClientAsync();
            const string sql = @"
                SELECT u.Id, u.NombreCompleto, u.Username, u.PasswordHash, u.Activo, r.Nombre as RolNombre, r.Id as RolId
                FROM Usuarios u
                INNER JOIN Roles r ON u.RolId = r.Id
                WHERE u.Username = ? 
                LIMIT 1;";

            var result = await client.Execute(sql, username.Trim());
            var row = result.Rows.FirstOrDefault();
            if (row == null) return null;

            var cols = row.ToList();
            return (
                int.Parse(cols[0]?.ToString() ?? "0"),
                cols[1]?.ToString() ?? "",
                cols[2]?.ToString() ?? "",
                cols[3]?.ToString() ?? "",
                (cols[4]?.ToString() ?? "0") == "1",
                cols[5]?.ToString() ?? "",
                int.Parse(cols[6]?.ToString() ?? "0")
            );
        }

        public async Task<List<string>> ObtenerPermisosPorRolIdAsync(int rolId)
        {
            using var client = await _context.GetClientAsync();
            const string sql = @"
                SELECT p.Codigo 
                FROM Permisos p
                INNER JOIN RolPermisos rp ON p.Id = rp.PermisoId
                WHERE rp.RolId = ?;";

            var result = await client.Execute(sql, rolId);
            return result.Rows
                .Select(r => r.FirstOrDefault()?.ToString() ?? "")
                .Where(p => !string.IsNullOrEmpty(p))
                .ToList();
        }

        public async Task<bool> ExisteUsernameAsync(string username)
        {
            using var client = await _context.GetClientAsync();
            var res = await client.Execute("SELECT 1 FROM Usuarios WHERE Username = ? LIMIT 1;", username.Trim());
            return res.Rows.Any();
        }

        public async Task<bool> CrearUsuarioAsync(int rolId, string nombreCompleto, string username, string passwordHash, string? email)
        {
            using var client = await _context.GetClientAsync();
            const string sql = @"
                INSERT INTO Usuarios (RolId, NombreCompleto, Username, PasswordHash, Email, Activo)
                VALUES (?, ?, ?, ?, ?, 1);";

            await client.Execute(
                sql,
                rolId,
                nombreCompleto.Trim(),
                username.Trim(),
                passwordHash,
                (object?)email?.Trim() ?? DBNull.Value
            );
            return true;
        }

        public async Task<List<UsuarioDto>> ListarUsuariosAsync()
        {
            using var client = await _context.GetClientAsync();
            const string sql = @"
                SELECT u.Id, u.RolId, r.Nombre as RolNombre, u.NombreCompleto, u.Username, u.Email, u.Activo, u.FechaCreacion
                FROM Usuarios u
                INNER JOIN Roles r ON u.RolId = r.Id
                ORDER BY u.NombreCompleto ASC;";

            var result = await client.Execute(sql);
            var lista = new List<UsuarioDto>();

            foreach (var row in result.Rows)
            {
                var cols = row.ToList();
                lista.Add(new UsuarioDto
                {
                    Id = int.Parse(cols[0]?.ToString() ?? "0"),
                    RolId = int.Parse(cols[1]?.ToString() ?? "0"),
                    RolNombre = cols[2]?.ToString() ?? "",
                    NombreCompleto = cols[3]?.ToString() ?? "",
                    Username = cols[4]?.ToString() ?? "",
                    Email = cols[5]?.ToString(),
                    Activo = (cols[6]?.ToString() ?? "0") == "1",
                    FechaCreacion = DateTime.TryParse(cols[7]?.ToString(), out var fc) ? fc : DateTime.Now
                });
            }
            return lista;
        }

        public async Task<bool> CambiarEstadoUsuarioAsync(int usuarioId, bool activo)
        {
            using var client = await _context.GetClientAsync();
            const string sql = "UPDATE Usuarios SET Activo = ? WHERE Id = ?;";
            await client.Execute(sql, activo ? 1 : 0, usuarioId);
            return true;
        }

        public async Task<List<RolDto>> ListarRolesAsync()
        {
            using var client = await _context.GetClientAsync();
            const string sql = "SELECT Id, Nombre, Descripcion, Activo FROM Roles WHERE Activo = 1 ORDER BY Id ASC;";
            var result = await client.Execute(sql);
            var roles = new List<RolDto>();

            foreach (var row in result.Rows)
            {
                var cols = row.ToList();
                roles.Add(new RolDto
                {
                    Id = int.Parse(cols[0]?.ToString() ?? "0"),
                    Nombre = cols[1]?.ToString() ?? "",
                    Descripcion = cols[2]?.ToString(),
                    Activo = (cols[3]?.ToString() ?? "0") == "1"
                });
            }
            return roles;
        }

        public async Task<List<PermisoDto>> ListarPermisosAsync()
        {
            using var client = await _context.GetClientAsync();
            const string sql = "SELECT Id, Codigo, Modulo, Descripcion FROM Permisos ORDER BY Modulo, Codigo ASC;";
            var result = await client.Execute(sql);
            var permisos = new List<PermisoDto>();

            foreach (var row in result.Rows)
            {
                var cols = row.ToList();
                permisos.Add(new PermisoDto
                {
                    Id = int.Parse(cols[0]?.ToString() ?? "0"),
                    Codigo = cols[1]?.ToString() ?? "",
                    Modulo = cols[2]?.ToString() ?? "",
                    Descripcion = cols[3]?.ToString() ?? ""
                });
            }
            return permisos;
        }
    }
}
