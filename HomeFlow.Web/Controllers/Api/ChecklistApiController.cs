using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeFlow.Application.Interfaces;
using HomeFlow.Domain.Enums;
using HomeFlow.Shared.DTOs.Checklists;

namespace HomeFlow.Web.Controllers.Api
{
    [Route("api/checklist")]
    [Authorize]
    public class ChecklistApiController(IChecklistService checklistService, ICurrentUserService currentUser) : ApiControllerBase
    {
        [HttpPost("plantillas")]
        public async Task<IActionResult> CrearPlantilla([FromBody] CrearChecklistPlantillaRequest request, CancellationToken ct)
        {
            var result = await checklistService.CrearPlantillaAsync(currentUser.EmpresaId, request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("plantilla/{tipo}")]
        public async Task<IActionResult> ObtenerPlantilla(TipoChecklist tipo, CancellationToken ct)
        {
            var result = await checklistService.ObtenerPlantillaAsync(currentUser.EmpresaId, tipo, ct);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost("respuestas")]
        public async Task<IActionResult> RegistrarRespuesta([FromBody] CreateChecklistRespuestaRequest request, CancellationToken ct)
        {
            var result = await checklistService.RegistrarRespuestaAsync(currentUser.EmpresaId, currentUser.UsuarioId, request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}