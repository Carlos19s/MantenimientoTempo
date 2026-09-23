using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantenimientoTempoModels
{
    public class Mantenimientos
    {
        [Key]
        public int Id { get; set; }

        public int MaquinaId { get; set; }

        [ForeignKey(nameof(MaquinaId))]
        public Maquinas? Maquina { get; set; }

        public int? PlanesMantenimientoId { get; set; }

        [ForeignKey(nameof(PlanesMantenimientoId))]
        public PlanesMantenimiento? PlanMantenimiento { get; set; }

        public int UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public Usuarios? Tecnico { get; set; }

        public string TipoMantenimiento { get; set; } = "PREVENTIVO";
        public string Estado { get; set; } = "PROGRAMADO";
        public DateTime FechaProgramada { get; set; } = DateTime.Now;
        public DateTime? FechaEjecucion { get; set; }

        public decimal KilosMaquinaALaFecha { get; set; }
        public bool SeCambiaronAgujas { get; set; }
        public string? TipoAgujaInstalada { get; set; }
        public int CantidadAgujasCambiadas { get; set; }
        public string? Observaciones { get; set; }

        public List<MantenimientoDetalles> Detalles { get; set; } = new List<MantenimientoDetalles>();
    }
}
