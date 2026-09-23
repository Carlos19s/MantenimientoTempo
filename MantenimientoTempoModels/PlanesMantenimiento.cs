using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantenimientoTempoModels
{
    public class PlanesMantenimiento
    {
        [Key]
        public int Id { get; set; }

        public int MaquinaId { get; set; }

        [ForeignKey(nameof(MaquinaId))]
        public Maquinas? Maquina { get; set; }

        public string Frecuencia { get; set; } = "MENSUAL";
        public int IntervaloDias { get; set; }
        public DateTime ProximaFechaEstimada { get; set; }
        public string NivelPrioridad { get; set; } = "MEDIA";
        public bool Activo { get; set; } = true;

        public List<PlanActividades> Actividades { get; set; } = new List<PlanActividades>();
    }
}
