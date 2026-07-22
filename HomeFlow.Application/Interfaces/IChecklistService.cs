using HomeFlow.Domain;
using HomeFlow.Domain.Enums;
using HomeFlow.Shared.DTOs.Checklists;

namespace HomeFlow.Application.Interfaces
{
    public interface IChecklistService
    {
        Task<Result<ChecklistPlantillaDto>> CrearPlantillaAsync(int empresaId, CrearChecklistPlantillaRequest request, CancellationToken cancellationToken = default);
        Task<Result<ChecklistPlantillaDto>> ObtenerPlantillaAsync(int empresaId, TipoChecklist tipo, CancellationToken cancellationToken = default);
        Task<Result<ChecklistRespuestaDto>> RegistrarRespuestaAsync(int empresaId, int usuarioEvaluadorId, CreateChecklistRespuestaRequest request, CancellationToken cancellationToken = default);
    }
}