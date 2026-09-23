using System.Diagnostics;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json.Nodes;

namespace MantenimientoTempoForms.Services
{
    public static class ActualizadorService
    {
        // Obtiene la versión del ensamblado actual del cliente
        public static string ObtenerVersionLocal()
        {
            Version? ver = Assembly.GetExecutingAssembly().GetName().Version;
            return ver != null ? $"{ver.Major}.{ver.Minor}.{ver.Build}" : "1.0.0";
        }

        public static async Task<bool> ComprobarYActualizarAsync()
        {
            try
            {
                // 1. Consultar a la API la última versión disponible
                var respuesta = await ApiClient.Http.GetFromJsonAsync<JsonObject>("api/Actualizaciones/version");
                if (respuesta == null) return false;

                string versionServidorStr = respuesta["version"]?.ToString() ?? "1.0.0";
                string versionLocalStr = ObtenerVersionLocal();

                Version versionServidor = new Version(versionServidorStr);
                Version versionLocal = new Version(versionLocalStr);

                // Si el servidor no tiene una versión más nueva, continúa normal
                if (versionServidor <= versionLocal)
                    return false;

                var confirmacion = MessageBox.Show(
                    $"Existe una nueva versión disponible ({versionServidorStr}).\nVersión actual: {versionLocalStr}\n\n¿Desea actualizar ahora?",
                    "Actualización Disponible",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information
                );

                if (confirmacion != DialogResult.Yes)
                    return false;

                // 2. Descargar el binario actualizado
                string rutaTemporal = Path.Combine(Path.GetTempPath(), "MantenimientoTempo_Update.exe");
                byte[] binario = await ApiClient.Http.GetByteArrayAsync("api/Actualizaciones/descargar");
                await File.WriteAllBytesAsync(rutaTemporal, binario);

                // 3. Ejecutar el reemplazo del archivo
                EjecutarScriptReemplazo(rutaTemporal);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo comprobar la actualización: {ex.Message}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        private static void EjecutarScriptReemplazo(string rutaDescarga)
        {
            string ejecutableActual = Environment.ProcessPath!;
            string scriptPath = Path.Combine(Path.GetTempPath(), "update_tempo.bat");

            // Script por lotes que espera a que el proceso actual se cierre, reemplaza el .exe y lo vuelve a abrir
            string contenidoBat = $@"
@echo off
timeout /t 2 /nobreak > nul
move /y ""{rutaDescarga}"" ""{ejecutableActual}""
start """" ""{ejecutableActual}""
del ""%~f0""
";
            File.WriteAllText(scriptPath, contenidoBat);

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = scriptPath,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process.Start(psi);
            Environment.Exit(0); // Cierra la aplicación actual para liberar el bloqueo del .exe
        }
    }
}
