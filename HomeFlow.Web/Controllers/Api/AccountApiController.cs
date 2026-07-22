using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeFlow.Application.Interfaces;
using HomeFlow.Shared.DTOs.Auth;

namespace HomeFlow.Web.Controllers.Api
{
    [Route("api/account")]
    public class AccountApiController : ApiControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AccountApiController> _logger;

        public AccountApiController(IAuthService authService, ILogger<AccountApiController> logger)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _authService.LoginAsync(request);
                if (!result.Success) return Unauthorized(result);

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, result.Data!.UsuarioId.ToString()),
                    new("EmpresaId", result.Data.EmpresaId.ToString()),
                    new(ClaimTypes.GivenName, result.Data.Nombre),
                    new(ClaimTypes.Email, result.Data.Correo)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties { IsPersistent = request.RecordarMe });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en login");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterCorredorRequest request)
        {
            var result = await _authService.RegisterCorredorAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _authService.ChangePasswordAsync(usuarioId, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("me")]
        public IActionResult Me() => Ok(new
        {
            nombre = User.FindFirst(ClaimTypes.GivenName)?.Value,
            correo = User.FindFirst(ClaimTypes.Email)?.Value
        });

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { success = true, message = "Sesión cerrada" });
        }
    }
}