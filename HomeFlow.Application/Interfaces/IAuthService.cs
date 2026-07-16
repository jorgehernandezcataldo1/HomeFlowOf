using HomeFlow.Domain;
using HomeFlow.Shared.DTOs.Auth;

namespace HomeFlow.Application.Interfaces
{
    /// <summary>
    /// Interfaz para servicios de autenticación
    /// </summary>
    public interface IAuthService
    {
        Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
        Task<Result> RegisterCorredorAsync(RegisterCorredorRequest request, CancellationToken cancellationToken = default);
        Task<Result> ChangePasswordAsync(int corredorId, ChangePasswordRequest request, CancellationToken cancellationToken = default);
        Task<Result> ValidateCredentialsAsync(string correo, string password, CancellationToken cancellationToken = default);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
