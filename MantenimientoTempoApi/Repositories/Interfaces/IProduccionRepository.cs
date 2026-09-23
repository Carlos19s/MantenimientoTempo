using MantenimientoTempoModels.DTOs.Produccion;

namespace MantenimientoTempoApi.Repositories.Interfaces
{
    public interface IProduccionRepository
    {
        Task<bool> RegistrarProduccionAsync(RegistrarProduccionDto dto);
    }
}
