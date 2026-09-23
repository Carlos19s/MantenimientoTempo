using System.Net.Http.Headers;
using MantenimientoTempoModels.DTOs.Auth;

namespace MantenimientoTempoForms.Services
{
    public static class ApiClient
    {
        // Puerto donde corre tu Web API
        private const string BaseUrl = "https://localhost:7247/";

        public static HttpClient Http { get; } = new HttpClient { BaseAddress = new Uri(BaseUrl) };

        // Sesion activa en memoria
        public static string Token { get; private set; } = string.Empty;
        public static int UsuarioId { get; private set; }
        public static string NombreCompleto { get; private set; } = string.Empty;
        public static string Username { get; private set; } = string.Empty;
        public static string Rol { get; private set; } = string.Empty;
        public static List<string> Permisos { get; private set; } = new List<string>();

        public static void SetSession(LoginResponseDto dto)
        {
            Token = dto.Token;
            UsuarioId = dto.UsuarioId;
            NombreCompleto = dto.NombreCompleto;
            Username = dto.Username;
            Rol = dto.Rol;
            Permisos = dto.Permisos ?? new List<string>();

            // Configura el Bearer token para todas las peticiones posteriores
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        }

        public static void ClearSession()
        {
            Token = string.Empty;
            UsuarioId = 0;
            NombreCompleto = string.Empty;
            Username = string.Empty;
            Rol = string.Empty;
            Permisos.Clear();
            Http.DefaultRequestHeaders.Authorization = null;
        }

        public static bool TienePermiso(string codigoPermiso)
        {
            if (Rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase)) return true;
            return Permisos.Contains(codigoPermiso, StringComparer.OrdinalIgnoreCase);
        }
    }
}
