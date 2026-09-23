using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantenimientoTempoModels
{
    public class PlanActividades
    {
        [Key]
        public int Id { get; set; }

        public int PlanesMantenimientoId { get; set; }

        [ForeignKey(nameof(PlanesMantenimientoId))]
        public PlanesMantenimiento? PlanMantenimiento { get; set; }

        public string DescripcionTarea { get; set; } = string.Empty;
        public string? PiezaInvolucrada { get; set; }
        public string Accion { get; set; } = "LIMPIEZA"; // LIMPIEZA, ENGRASE, REVISION, CAMBIO
        public int Orden { get; set; } = 1;
    }
}
