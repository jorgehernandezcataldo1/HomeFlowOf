using System.Security.Claims;
using HomeFlow.Application.Interfaces;

namespace HomeFlow.Web.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    public int UsuarioId => int.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
    public int EmpresaId => int.TryParse(User?.FindFirst("EmpresaId")?.Value, out var id) ? id : 0;
}