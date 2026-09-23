using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantenimientoTempoModels.DTOs.Mantenimientos
{
    public class ProgramarMantenimientoDto
    {
        public int MaquinaId { get; set; }
        public int? PlanesMantenimientoId { get; set; }
        public int UsuarioId { get; set; }
        public string TipoMantenimiento { get; set; } = "PREVENTIVO"; // PREVENTIVO o CORRECTIVO
        public DateTime FechaProgramada { get; set; } = DateTime.Now;
        public string? Observaciones { get; set; }
    }
}