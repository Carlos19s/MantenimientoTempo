using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantenimientoTempoModels
{
    public class FotosMaquinas
    {
        [Key]
        public int Id { get; set; }

        public int MaquinaId { get; set; }

        [ForeignKey(nameof(MaquinaId))]
        public Maquinas? Maquina { get; set; }

        public string NombrePieza { get; set; } = string.Empty;
        public string FotoRuta { get; set; } = string.Empty;
        public string? DescripcionTecnica { get; set; }
        public DateTime FechaSubida { get; set; } = DateTime.Now;
    }
}
