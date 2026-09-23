using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantenimientoTempoModels
{
    public class RolPermisos
    {
        [Key]
        public int Id { get; set; }

        public int RolId { get; set; }

        [ForeignKey(nameof(RolId))]
        public Roles? Rol { get; set; }

        public int PermisoId { get; set; }

        [ForeignKey(nameof(PermisoId))]
        public Permisos? Permiso { get; set; }
    }
}