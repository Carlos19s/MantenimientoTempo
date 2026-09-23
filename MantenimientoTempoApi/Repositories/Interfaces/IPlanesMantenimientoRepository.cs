using MantenimientoTempoModels.DTOs.Mantenimientos;

namespace MantenimientoTempoApi.Repositories.Interfaces
{
    public interface IPlanesMantenimientoRepository
    {
        Task<List<PlanMantenimientoDto>> ObtenerPlanesPorMaquinaAsync(int maquinaId);
        Task<bool> CrearPlanConActividadesAsync(CrearPlanMantenimientoDto dto);
    }
}
