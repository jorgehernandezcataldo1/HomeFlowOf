using HomeFlow.Application.DTOs;
using HomeFlow.Domain;

namespace HomeFlow.Application.Interfaces
{
    public interface IClienteService
    {
        Task<Result<ClienteDto>> CrearAsync(int empresaId, int usuarioCreadorId, ClienteCreateUpdateDto dto, CancellationToken cancellationToken = default);
        Task<Result<ClienteDto>> ActualizarAsync(int empresaId, int idCliente, ClienteCreateUpdateDto dto, CancellationToken cancellationToken = default);
        Task<Result<ClienteDto>> ObtenerPorIdAsync(int empresaId, int idCliente, CancellationToken cancellationToken = default);
        Task<Result<List<ClienteListDto>>> ListarAsync(int empresaId, bool? soloPropietarios = null, CancellationToken cancellationToken = default);
        Task<Result> DesactivarAsync(int empresaId, int idCliente, CancellationToken cancellationToken = default);
    }
}