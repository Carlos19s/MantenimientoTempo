using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantenimientoTempoModels
{
    public class Usuarios
    {
        [Key]
        public int Id { get; set; }

        public int RolId { get; set; }

        [ForeignKey(nameof(RolId))]
        public Roles? Rol { get; set; }

        public string NombreCompleto { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Email { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public List<Mantenimientos> MantenimientosAsignados { get; set; } = new List<Mantenimientos>();
    }
}