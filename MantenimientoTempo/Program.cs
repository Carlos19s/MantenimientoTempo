using MantenimientoTempoForms.Services;

namespace MantenimientoTempo
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // 1. Candado DPAPI + Hardware ID contra Turso
            var licencia = Task.Run(() => HardwareLicenciaService.ValidarConServidorTursoAsync()).GetAwaiter().GetResult();
            if (!licencia.Exito)
            {
                string hwid = HardwareLicenciaService.ObtenerHardwareId();
                MessageBox.Show(
                    $"ACCESO NO AUTORIZADO\n\n{licencia.Mensaje}\n\nHardware ID:\n{hwid}",
                    "Equipo No Autorizado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop
                );
                return;
            }

            // 2. Comprobar si hay una versión superior en la API
            bool actualizando = Task.Run(() => ActualizadorService.ComprobarYActualizarAsync()).GetAwaiter().GetResult();
            if (actualizando)
            {
                // La aplicación se cerrará por el script del actualizador
                return;
            }

            // 3. Flujo normal hacia el Login
            Application.Run(new FormLogin());
        }
    }
}
