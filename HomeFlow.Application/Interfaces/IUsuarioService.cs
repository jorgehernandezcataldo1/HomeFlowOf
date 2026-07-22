using HomeFlow.Domain;
using HomeFlow.Shared.DTOs.Usuarios;

namespace HomeFlow.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<Result<UsuarioDto>> CrearUsuarioAsync(int empresaId, CreateUsuarioRequest request, CancellationToken cancellationToken = default);
        Task<Result<UsuarioDto>> ActualizarUsuarioAsync(int empresaId, UpdateUsuarioRequest request, CancellationToken cancellationToken = default);
        Task<Result<UsuarioDto>> ObtenerPorIdAsync(int empresaId, int idUsuario, CancellationToken cancellationToken = default);
        Task<Result<List<UsuarioListDto>>> ListarPorEmpresaAsync(int empresaId, CancellationToken cancellationToken = default);
        Task<Result> DesactivarUsuarioAsync(int empresaId, int idUsuario, CancellationToken cancellationToken = default);
        Task<Result> BloquearUsuarioAsync(int empresaId, int idUsuario, bool bloquear, CancellationToken cancellationToken = default);
    }
}