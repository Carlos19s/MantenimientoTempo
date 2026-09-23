using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantenimientoTempoModels
{
    public class MantenimientoDetalles
    {
        [Key]
        public int Id { get; set; }

        public int MantenimientoId { get; set; }

        [ForeignKey(nameof(MantenimientoId))]
        public Mantenimientos? Mantenimiento { get; set; }

        public int? PlanActividadId { get; set; }

        [ForeignKey(nameof(PlanActividadId))]
        public PlanActividades? PlanActividad { get; set; }

        public string DescripcionActividad { get; set; } = string.Empty;
        public bool Completada { get; set; } = false;
        public string? Observaciones { get; set; }
    }
}
