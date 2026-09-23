using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantenimientoTempoModels
{
    public class Maquinas
    {
        [Key]
        public int Id { get; set; }

        public string CodigoMaquina { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string? Modelo { get; set; }
        public string? NumeroSerie { get; set; }
        public string? FotoRuta { get; set; }

        public int? Galga { get; set; }
        public int? DiametroPulgadas { get; set; }
        public int? NumeroAlimentadores { get; set; }

        public string TipoAgujaActual { get; set; } = string.Empty;
        public DateTime FechaMontajeAguja { get; set; } = DateTime.Now;

        public decimal KilosAcumuladosAguja { get; set; } = 0;
        public decimal LimiteKilosAguja { get; set; } = 15000;

        public decimal KilosMetaPedido { get; set; } = 0;
        public decimal KilosTejidosPedido { get; set; } = 0;

        public EstadoSemaforo EstadoSemaforo { get; set; } = EstadoSemaforo.Verde;

        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? ProximaFechaMantenimiento { get; set; }
        public string? ProximaFrecuenciaMantenimiento { get; set; }

        // Relaciones con List<T>
        public List<FotosMaquinas> FotosPiezas { get; set; } = new List<FotosMaquinas>();
        public List<PlanesMantenimiento> PlanesMantenimiento { get; set; } = new List<PlanesMantenimiento>();
        public List<Mantenimientos> HistorialMantenimientos { get; set; } = new List<Mantenimientos>();
        public List<RegistrosProduccion> RegistrosProduccion { get; set; } = new List<RegistrosProduccion>();
    }
}
