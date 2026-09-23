using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantenimientoTempoModels.DTOs.Maquinas
{
    public class MaquinaDto
    {
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
        public DateTime FechaMontajeAguja { get; set; }
        public decimal KilosAcumuladosAguja { get; set; }
        public decimal LimiteKilosAguja { get; set; }
        public decimal KilosMetaPedido { get; set; }
        public decimal KilosTejidosPedido { get; set; }
        public EstadoSemaforo EstadoSemaforo { get; set; }
        public string EstadoSemaforoTexto => EstadoSemaforo.ToString();
        public decimal PorcentajeDesgasteAguja => LimiteKilosAguja > 0
            ? Math.Round((KilosAcumuladosAguja / LimiteKilosAguja) * 100, 2)
            : 0;
        public bool Activo { get; set; }
        public DateTime? ProximaFechaMantenimiento { get; set; }
        public string? ProximaFrecuenciaMantenimiento { get; set; }
    }
}

