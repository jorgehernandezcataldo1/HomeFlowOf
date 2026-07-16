using HomeFlow.Domain;
using HomeFlow.Shared.DTOs.Properties;

namespace HomeFlow.Application.Interfaces
{
    /// <summary>
    /// Interfaz para servicios de propiedades
    /// </summary>
    public interface IPropertyService
    {
        Task<Result<PropiedadDto>> CreatePropertyAsync(CreatePropiedadRequest request, CancellationToken cancellationToken = default);
        Task<Result<PropiedadDto>> GetPropertyByIdAsync(int propiedadId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<PropiedadDto>>> GetAllPropertiesAsync(CancellationToken cancellationToken = default);
        Task<Result<PropiedadListaResponse>> SearchPropertiesAsync(BuscarPropiedadRequest request, CancellationToken cancellationToken = default);
        Task<Result<PropiedadDto>> UpdatePropertyAsync(int propiedadId, UpdatePropiedadRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeletePropertyAsync(int propiedadId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<PropiedadDto>>> GetPropertiesByPropietarioAsync(int propietarioId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<PropiedadDto>>> GetAvailablePropertiesAsync(CancellationToken cancellationToken = default);
        Task<Result> PublishPropertyAsync(int propiedadId, CancellationToken cancellationToken = default);
    }
}
