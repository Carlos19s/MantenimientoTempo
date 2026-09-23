using MantenimientoTempoModels.DTOs.Mantenimientos;

namespace MantenimientoTempoApi.Repositories.Interfaces
{
    public interface IMantenimientosRepository
    {
        Task<List<MantenimientoResumenDto>> ListarAsync(string? estado);
        Task<bool> ProgramarAsync(ProgramarMantenimientoDto dto, decimal kilosActuales);
        Task<int?> ObtenerMaquinaIdPorMantenimientoAsync(int mantenimientoId);
        Task<bool> CompletarMantenimientoAsync(int mantenimientoId, string fechaEjecucion, bool seCambiaronAgujas, string? tipoAguja, int cantidadAgujas, string? observaciones);
        Task<bool> RegistrarDetalleActividadAsync(int mantenimientoId, int? planActividadId, string descripcion, bool completada, string? observaciones);
    }
}
