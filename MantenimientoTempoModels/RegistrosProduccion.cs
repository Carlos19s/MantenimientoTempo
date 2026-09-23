using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantenimientoTempoModels
{
    public class RegistrosProduccion
    {
        [Key]
        public int Id { get; set; }

        public int MaquinaId { get; set; }

        [ForeignKey(nameof(MaquinaId))]
        public Maquinas? Maquina { get; set; }

        public DateTime FechaRegistro { get; set; }
        public decimal KilosTurno { get; set; }
        public string? NombreArchivoExcel { get; set; }
        public DateTime FechaImportacion { get; set; } = DateTime.Now;
    }
}
