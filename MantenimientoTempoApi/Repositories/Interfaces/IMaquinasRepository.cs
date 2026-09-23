
using MantenimientoTempoModels.DTOs.Maquinas;

namespace MantenimientoTempoApi.Repositories.Interfaces
{
    public interface IMaquinasRepository
    {
        Task<List<MaquinaDto>> ObtenerTodasActivasAsync();
        Task<MaquinaDto?> ObtenerPorIdAsync(int id);
        Task<bool> ExisteCodigoAsync(string codigo);
        Task<bool> CrearMaquinaAsync(CrearMaquinaDto dto);
        Task<bool> ActualizarMaquinaAsync(int id, ActualizarMaquinaDto dto);
        Task<(decimal KilosAcumulados, decimal LimiteKilos)?> ObtenerKilosAsync(int maquinaId);
        Task<bool> ActualizarDesgasteYSemaforoAsync(int maquinaId, decimal nuevosKilos, int nuevoSemaforo);
        Task<bool> ResetearContadorAgujasAsync(int maquinaId, string? nuevoTipoAguja, string fechaMontaje);
    }
}
