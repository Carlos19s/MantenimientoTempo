using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantenimientoTempoModels.DTOs.Produccion
{
    public class RegistrarProduccionDto
    {
        public int MaquinaId { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public decimal KilosTurno { get; set; }
        public string? NombreArchivoExcel { get; set; }
    }
}
