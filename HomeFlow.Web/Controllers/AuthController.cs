using System.Security.Claims;
using HomeFlow.Application.Interfaces;
using HomeFlow.Shared.DTOs.Auth;
using HomeFlow.Web.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeFlow.Web.Controllers;

/// <summary>
/// Controlador MVC encargado de las vistas de autenticación (login/logout).
///
/// No confundir con Controllers/AccountController.cs: ese es un controlador
/// API (api/account) pensado para clientes externos (apps móviles, SPA,
/// Postman, etc.) que devuelve JSON. Este controlador es el que usa el
/// navegador y el que arma la cookie de sesión configurada en Program.cs
/// (CookieAuthenticationDefaults). Ambos reutilizan el mismo IAuthService,
/// así que la lógica de negocio vive en un solo lugar.
/// </summary>
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Muestra el formulario de inicio de sesión.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToLocal(returnUrl);

        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    /// <summary>
    /// Procesa el inicio de sesión. Delega toda la validación de
    /// credenciales en AuthService (hash de contraseña, usuario bloqueado,
    /// activo, etc.) y solo arma la cookie de sesión si el servicio
    /// confirma que las credenciales son válidas.
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null, CancellationToken cancellationToken = default)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        var result = await _authService.LoginAsync(new LoginRequest
        {
            Correo = model.Correo.Trim(),
            Password = model.Password,
            RecordarMe = model.RecordarMe
        }, cancellationToken);

        if (!result.Success || result.Data is null)
        {
            _logger.LogWarning("Intento de login fallido para {Correo}", model.Correo);
            ModelState.AddModelError(string.Empty, result.Message ?? "Correo o contraseña incorrectos.");
            return View(model);
        }

        await FirmarSesionAsync(result.Data, model.RecordarMe);

        _logger.LogInformation("Usuario {Correo} (Id {UsuarioId}) inició sesión.", result.Data.Correo, result.Data.UsuarioId);

        return RedirectToLocal(returnUrl);
    }

    /// <summary>
    /// Cierra la sesión actual y vuelve al login.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        var correo = User.Identity?.Name;
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        _logger.LogInformation("Usuario {Correo} cerró sesión.", correo);
        return RedirectToAction(nameof(Login));
    }

    /// <summary>
    /// Página mostrada cuando un usuario autenticado intenta acceder a
    /// algo para lo que no tiene permiso (útil una vez que se agreguen
    /// roles/policies más adelante).
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied() => View();

    private async Task FirmarSesionAsync(LoginResponse usuario, bool recordarMe)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
            new(ClaimTypes.Name, usuario.Correo),
            new(ClaimTypes.Email, usuario.Correo),
            new(ClaimTypes.GivenName, usuario.Nombre),
            new(ClaimTypes.Surname, usuario.Apellido),
            // Claim propio de HomeFlow: permite filtrar todo por empresa
            // (multi-tenant) sin volver a consultar la BD en cada request.
            new("EmpresaId", usuario.EmpresaId.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
        {
            IsPersistent = recordarMe,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
            AllowRefresh = true
        });
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        // Ajusta esto si tu página de inicio post-login no es Dashboard/Index.
        return RedirectToAction("Index", "Dashboard");
    }
}
