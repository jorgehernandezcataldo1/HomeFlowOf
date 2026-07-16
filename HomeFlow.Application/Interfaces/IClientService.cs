using HomeFlow.Domain;
using HomeFlow.Shared.DTOs.Clients;

namespace HomeFlow.Application.Interfaces
{
    /// <summary>
    /// Interfaz para servicios de clientes (Propietarios y Arrendatarios)
    /// </summary>
    public interface IClientService
    {
        // Propietarios
        Task<Result<PropietarioDto>> CreatePropietarioAsync(CreatePropietarioRequest request, CancellationToken cancellationToken = default);
        Task<Result<PropietarioDto>> GetPropietarioByIdAsync(int propietarioId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<PropietarioDto>>> GetAllPropietariosAsync(CancellationToken cancellationToken = default);
        Task<Result<PropietarioDto>> UpdatePropietarioAsync(int propietarioId, CreatePropietarioRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeletePropietarioAsync(int propietarioId, CancellationToken cancellationToken = default);

        // Arrendatarios
        Task<Result<ArrendatarioDto>> CreateArrendatarioAsync(CreateArrendatarioRequest request, CancellationToken cancellationToken = default);
        Task<Result<ArrendatarioDto>> GetArrendatarioByIdAsync(int arrendatarioId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<ArrendatarioDto>>> GetAllArrendatariosAsync(CancellationToken cancellationToken = default);
        Task<Result<ArrendatarioDto>> UpdateArrendatarioAsync(int arrendatarioId, CreateArrendatarioRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeleteArrendatarioAsync(int arrendatarioId, CancellationToken cancellationToken = default);

        // Búsqueda
        Task<Result<IEnumerable<ArrendatarioDto>>> BuscarArrendatariosPorRequisitosAsync(decimal? maxArriendo, int? habitacionesMinimas, int? comunaId, CancellationToken cancellationToken = default);
    }
}
