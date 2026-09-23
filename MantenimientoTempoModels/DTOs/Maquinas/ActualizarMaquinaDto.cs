using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantenimientoTempoModels.DTOs.Maquinas
{
    public class ActualizarMaquinaDto
    {
        public string Marca { get; set; } = string.Empty;
        public string? Modelo { get; set; }
        public string? NumeroSerie { get; set; }
        public string? FotoRuta { get; set; }
        public int? Galga { get; set; }
        public int? DiametroPulgadas { get; set; }
        public int? NumeroAlimentadores { get; set; }
        public string TipoAgujaActual { get; set; } = string.Empty;
        public decimal LimiteKilosAguja { get; set; }
        public decimal KilosMetaPedido { get; set; }
        public DateTime? ProximaFechaMantenimiento { get; set; }
        public string? ProximaFrecuenciaMantenimiento { get; set; }
    }
}
