using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantenimientoTempoModels
{
    public class Permisos
    {
        [Key]
        public int Id { get; set; }

        public string Codigo { get; set; } = string.Empty;
        public string Modulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public List<RolPermisos> RolPermisos { get; set; } = new List<RolPermisos>();
    }
}