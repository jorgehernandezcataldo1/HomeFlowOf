namespace HomeFlow.Application.Interfaces;

public interface ICurrentUserService
{
    int UsuarioId { get; }
    int EmpresaId { get; }
    bool IsAuthenticated { get; }
}