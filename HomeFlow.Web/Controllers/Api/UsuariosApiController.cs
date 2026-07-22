using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeFlow.Application.Interfaces;
using HomeFlow.Domain;
using HomeFlow.Shared.DTOs.Usuarios;

namespace HomeFlow.Web.Controllers.Api
{
    [Route("api/usuarios")]
    [Authorize]
    public class UsuariosApiController(
        IUsuarioService usuarioService,
        ICurrentUserService currentUser,
        IValidator<CreateUsuarioRequest> createValidator,
        IValidator<UpdateUsuarioRequest> updateValidator,
        ILogger<UsuariosApiController> logger) : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CreateUsuarioRequest request, CancellationToken ct)
        {
            var validacion = await createValidator.ValidateAsync(request, ct);
            if (!validacion.IsValid) return BadRequest(Result<UsuarioDto>.ValidationError(ToErrors(validacion)));

            try
            {
                var result = await usuarioService.CrearUsuarioAsync(currentUser.EmpresaId, request, ct);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creando usuario");
                return StatusCode(500, Result<UsuarioDto>.Fail("Error interno del servidor"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken ct) =>
            Ok(await usuarioService.ListarPorEmpresaAsync(currentUser.EmpresaId, ct));

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id, CancellationToken ct)
        {
            var result = await usuarioService.ObtenerPorIdAsync(currentUser.EmpresaId, id, ct);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateUsuarioRequest request, CancellationToken ct)
        {
            if (id != request.IdUsuario) return BadRequest(Result<UsuarioDto>.Fail("El id no coincide."));
            var validacion = await updateValidator.ValidateAsync(request, ct);
            if (!validacion.IsValid) return BadRequest(Result<UsuarioDto>.ValidationError(ToErrors(validacion)));

            var result = await usuarioService.ActualizarUsuarioAsync(currentUser.EmpresaId, request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> Desactivar(int id, CancellationToken ct)
        {
            var result = await usuarioService.DesactivarUsuarioAsync(currentUser.EmpresaId, id, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id}/bloqueo")]
        public async Task<IActionResult> CambiarBloqueo(int id, [FromQuery] bool bloquear, CancellationToken ct)
        {
            var result = await usuarioService.BloquearUsuarioAsync(currentUser.EmpresaId, id, bloquear, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}