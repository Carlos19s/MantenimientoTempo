using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantenimientoTempoModels.DTOs.Mantenimientos
{
    public class ActividadPlanDto
    {
        public int Id { get; set; }
        public int PlanesMantenimientoId { get; set; }
        public string DescripcionTarea { get; set; } = string.Empty;
        public string? PiezaInvolucrada { get; set; }
        public string Accion { get; set; } = "LIMPIEZA";
        public int Orden { get; set; }
    }

    public class PlanMantenimientoDto
    {
        public int Id { get; set; }
        public int MaquinaId { get; set; }
        public string Frecuencia { get; set; } = "MENSUAL";
        public int IntervaloDias { get; set; }
        public DateTime ProximaFechaEstimada { get; set; }
        public string NivelPrioridad { get; set; } = "MEDIA";
        public bool Activo { get; set; }
        public List<ActividadPlanDto> Actividades { get; set; } = new List<ActividadPlanDto>();
    }

    public class CrearPlanMantenimientoDto
    {
        public int MaquinaId { get; set; }
        public string Frecuencia { get; set; } = "MENSUAL";
        public int IntervaloDias { get; set; } = 30;
        public DateTime ProximaFechaEstimada { get; set; } = DateTime.Now.AddDays(30);
        public string NivelPrioridad { get; set; } = "MEDIA";
        public List<CrearActividadPlanDto> Actividades { get; set; } = new List<CrearActividadPlanDto>();
    }

    public class CrearActividadPlanDto
    {
        public string DescripcionTarea { get; set; } = string.Empty;
        public string? PiezaInvolucrada { get; set; }
        public string Accion { get; set; } = "LIMPIEZA";
        public int Orden { get; set; } = 1;
    }
}
