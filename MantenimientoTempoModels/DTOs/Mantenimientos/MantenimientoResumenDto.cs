using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantenimientoTempoModels.DTOs.Mantenimientos
{
    public class MantenimientoResumenDto
    {
        public int Id { get; set; }
        public int MaquinaId { get; set; }
        public string CodigoMaquina { get; set; } = string.Empty;
        public string MarcaMaquina { get; set; } = string.Empty;
        public string TecnicoNombre { get; set; } = string.Empty;
        public string TipoMantenimiento { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaProgramada { get; set; }
        public DateTime? FechaEjecucion { get; set; }
        public decimal KilosMaquinaALaFecha { get; set; }
        public bool SeCambiaronAgujas { get; set; }
        public int CantidadAgujasCambiadas { get; set; }
        public string? Observaciones { get; set; }
    }
}