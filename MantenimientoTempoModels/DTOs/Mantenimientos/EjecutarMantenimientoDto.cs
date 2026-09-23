using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantenimientoTempoModels.DTOs.Mantenimientos
{
    public class DetalleActividadEjecutadaDto
    {
        public int? PlanActividadId { get; set; }
        public string DescripcionActividad { get; set; } = string.Empty;
        public bool Completada { get; set; } = true;
        public string? Observaciones { get; set; }
    }

    public class EjecutarMantenimientoDto
    {
        public int MantenimientoId { get; set; }
        public bool SeCambiaronAgujas { get; set; } = false;
        public string? TipoAgujaInstalada { get; set; }
        public int CantidadAgujasCambiadas { get; set; } = 0;
        public string? Observaciones { get; set; }
        public List<DetalleActividadEjecutadaDto> Actividades { get; set; } = new List<DetalleActividadEjecutadaDto>();
    }
}