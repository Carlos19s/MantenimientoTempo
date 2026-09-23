using MantenimientoTempoApi.Data;
using MantenimientoTempoApi.Repositories.Interfaces;
using MantenimientoTempoModels.DTOs.Mantenimientos;

namespace MantenimientoTempoApi.Repositories.Implementations
{
    public class PlanesMantenimientoRepository : IPlanesMantenimientoRepository
    {
        private readonly TursoDatabaseContext _context;

        public PlanesMantenimientoRepository(TursoDatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<PlanMantenimientoDto>> ObtenerPlanesPorMaquinaAsync(int maquinaId)
        {
            using var client = await _context.GetClientAsync();

            // 1. Obtener los planes de la máquina
            const string sqlPlanes = @"
                SELECT Id, MaquinaId, Frecuencia, IntervaloDias, ProximaFechaEstimada, NivelPrioridad, Activo
                FROM PlanesMantenimiento
                WHERE MaquinaId = ? AND Activo = 1
                ORDER BY IntervaloDias ASC;";

            var resPlanes = await client.Execute(sqlPlanes, maquinaId);
            var listaPlanes = new List<PlanMantenimientoDto>();

            foreach (var row in resPlanes.Rows)
            {
                var cols = row.ToList();
                listaPlanes.Add(new PlanMantenimientoDto
                {
                    Id = int.Parse(cols[0]?.ToString() ?? "0"),
                    MaquinaId = int.Parse(cols[1]?.ToString() ?? "0"),
                    Frecuencia = cols[2]?.ToString() ?? "MENSUAL",
                    IntervaloDias = int.Parse(cols[3]?.ToString() ?? "30"),
                    ProximaFechaEstimada = DateTime.TryParse(cols[4]?.ToString(), out var fe) ? fe : DateTime.Now,
                    NivelPrioridad = cols[5]?.ToString() ?? "MEDIA",
                    Activo = (cols[6]?.ToString() ?? "1") == "1",
                    Actividades = new List<ActividadPlanDto>()
                });
            }

            if (!listaPlanes.Any())
                return listaPlanes;

            // 2. Obtener las actividades de esos planes
            var planIds = string.Join(",", listaPlanes.Select(p => p.Id));
            string sqlActividades = $@"
                SELECT Id, PlanesMantenimientoId, DescripcionTarea, PiezaInvolucrada, Accion, Orden
                FROM PlanActividades
                WHERE PlanesMantenimientoId IN ({planIds})
                ORDER BY Orden ASC;";

            var resActividades = await client.Execute(sqlActividades);

            foreach (var row in resActividades.Rows)
            {
                var cols = row.ToList();
                int planId = int.Parse(cols[1]?.ToString() ?? "0");
                var plan = listaPlanes.FirstOrDefault(p => p.Id == planId);

                if (plan != null)
                {
                    plan.Actividades.Add(new ActividadPlanDto
                    {
                        Id = int.Parse(cols[0]?.ToString() ?? "0"),
                        PlanesMantenimientoId = planId,
                        DescripcionTarea = cols[2]?.ToString() ?? "",
                        PiezaInvolucrada = cols[3]?.ToString(),
                        Accion = cols[4]?.ToString() ?? "LIMPIEZA",
                        Orden = int.Parse(cols[5]?.ToString() ?? "1")
                    });
                }
            }

            return listaPlanes;
        }

        public async Task<bool> CrearPlanConActividadesAsync(CrearPlanMantenimientoDto dto)
        {
            using var client = await _context.GetClientAsync();

            const string sqlPlan = @"
                INSERT INTO PlanesMantenimiento (MaquinaId, Frecuencia, IntervaloDias, ProximaFechaEstimada, NivelPrioridad, Activo)
                VALUES (?, ?, ?, ?, ?, 1) RETURNING Id;";

            var resPlan = await client.Execute(
                sqlPlan,
                dto.MaquinaId,
                dto.Frecuencia.ToUpper(),
                dto.IntervaloDias,
                dto.ProximaFechaEstimada.ToString("yyyy-MM-dd HH:mm:ss"),
                dto.NivelPrioridad.ToUpper()
            );

            var firstRow = resPlan.Rows.FirstOrDefault();
            if (firstRow == null) return false;

            int nuevoPlanId = int.Parse(firstRow.ToList()[0]?.ToString() ?? "0");

            // Insertar actividades si vienen en la petición
            foreach (var act in dto.Actividades)
            {
                const string sqlActividad = @"
                    INSERT INTO PlanActividades (PlanesMantenimientoId, DescripcionTarea, PiezaInvolucrada, Accion, Orden)
                    VALUES (?, ?, ?, ?, ?);";

                await client.Execute(
                    sqlActividad,
                    nuevoPlanId,
                    act.DescripcionTarea.Trim(),
                    (object?)act.PiezaInvolucrada?.Trim() ?? DBNull.Value,
                    act.Accion.ToUpper(),
                    act.Orden
                );
            }

            return true;
        }
    }
}