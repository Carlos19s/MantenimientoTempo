using System.Management;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace MantenimientoTempoForms.Services
{
    public static class HardwareLicenciaService
    {
        private static readonly string RutaCarpeta = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "TempoIndustrial"
        );

        private static readonly string RutaArchivoLicencia = Path.Combine(RutaCarpeta, "license.dat");

        // 1. Obtiene el UUID único de la placa madre del equipo
        public static string ObtenerHardwareId()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT UUID FROM Win32_ComputerSystemProduct");
                foreach (var obj in searcher.Get())
                {
                    var uuid = obj["UUID"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(uuid) && uuid != "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF")
                        return uuid.Trim();
                }
            }
            catch
            {
                // Fallback por si WMI tiene restricciones en la máquina
            }

            return Environment.MachineName;
        }

        // 2. Entropía criptográfica basada en el hardware local
        private static byte[] GenerarEntropiaHardware()
        {
            string hwid = ObtenerHardwareId();
            return SHA256.HashData(Encoding.UTF8.GetBytes(hwid + "_TEMPO_TURSO_AUTH_2026"));
        }

        // 3. Valida contra Turso (a través de la API) y guarda la licencia en DPAPI
        public static async Task<(bool Exito, string Mensaje)> ValidarConServidorTursoAsync()
        {
            string hwid = ObtenerHardwareId();

            try
            {
                var respuesta = await ApiClient.Http.GetFromJsonAsync<JsonObject>($"api/Equipos/validar/{Uri.EscapeDataString(hwid)}");

                if (respuesta == null)
                    return (false, "Respuesta vacía del servidor.");

                bool autorizado = respuesta["autorizado"]?.GetValue<bool>() ?? false;
                string mensaje = respuesta["mensaje"]?.ToString() ?? "No autorizado.";

                if (autorizado)
                {
                    // Guardar localmente con DPAPI para validar la máquina
                    GuardarLicenciaLocal(hwid);
                    return (true, "Equipo verificado con éxito.");
                }

                // Si fue revocado en Turso, eliminamos la licencia local
                if (File.Exists(RutaArchivoLicencia))
                    File.Delete(RutaArchivoLicencia);

                return (false, mensaje);
            }
            catch (Exception ex)
            {
                // Si la API o internet está caído, probamos si tenía licencia local válida
                if (TieneLicenciaLocalValida())
                {
                    return (true, "Acceso local validado (Modo sin conexión).");
                }

                return (false, $"Error al comunicar con el servidor: {ex.Message}");
            }
        }

        private static void GuardarLicenciaLocal(string hwid)
        {
            if (!Directory.Exists(RutaCarpeta))
                Directory.CreateDirectory(RutaCarpeta);

            byte[] datos = Encoding.UTF8.GetBytes(hwid);
            byte[] entropia = GenerarEntropiaHardware();

            byte[] cifrado = ProtectedData.Protect(
                datos,
                entropia,
                DataProtectionScope.LocalMachine
            );

            File.WriteAllBytes(RutaArchivoLicencia, cifrado);
        }

        public static bool TieneLicenciaLocalValida()
        {
            if (!File.Exists(RutaArchivoLicencia))
                return false;

            try
            {
                byte[] cifrado = File.ReadAllBytes(RutaArchivoLicencia);
                byte[] entropia = GenerarEntropiaHardware();

                byte[] descifrado = ProtectedData.Unprotect(
                    cifrado,
                    entropia,
                    DataProtectionScope.LocalMachine
                );

                string hwidRecuperado = Encoding.UTF8.GetString(descifrado);
                return hwidRecuperado == ObtenerHardwareId();
            }
            catch
            {
                return false;
            }
        }
    }
}