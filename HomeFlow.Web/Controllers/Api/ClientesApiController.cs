using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeFlow.Application.DTOs;
using HomeFlow.Application.Interfaces;
using HomeFlow.Domain;

namespace HomeFlow.Web.Controllers.Api
{
    [Route("api/clientes")]
    [Authorize]
    public class ClientesApiController(
        IClienteService clienteService,
        ICurrentUserService currentUser,
        IValidator<ClienteCreateUpdateDto> validator) : ApiControllerBase
    {
        [HttpPost("propietarios")]
        public async Task<IActionResult> CrearPropietario([FromBody] ClienteCreateUpdateDto dto, CancellationToken ct)
        {
            dto.EsPropietario = true;
            var validacion = await validator.ValidateAsync(dto, ct);
            if (!validacion.IsValid) return BadRequest(Result<ClienteDto>.ValidationError(ToErrors(validacion)));

            var result = await clienteService.CrearAsync(currentUser.EmpresaId, currentUser.UsuarioId, dto, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] bool? soloPropietarios, CancellationToken ct) =>
            Ok(await clienteService.ListarAsync(currentUser.EmpresaId, soloPropietarios, ct));

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id, CancellationToken ct)
        {
            var result = await clienteService.ObtenerPorIdAsync(currentUser.EmpresaId, id, ct);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ClienteCreateUpdateDto dto, CancellationToken ct)
        {
            var validacion = await validator.ValidateAsync(dto, ct);
            if (!validacion.IsValid) return BadRequest(Result<ClienteDto>.ValidationError(ToErrors(validacion)));

            var result = await clienteService.ActualizarAsync(currentUser.EmpresaId, id, dto, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> Desactivar(int id, CancellationToken ct)
        {
            var result = await clienteService.DesactivarAsync(currentUser.EmpresaId, id, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}