using Microsoft.AspNetCore.Mvc;
using HomeFlow.Application.Interfaces;
using HomeFlow.Shared.DTOs.Clients;

namespace HomeFlow.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(IClientService clientService, ILogger<ClientsController> logger)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // ========== PROPIETARIOS ==========

        /// <summary>
        /// Crear propietario
        /// </summary>
        [HttpPost("propietarios")]
        public async Task<IActionResult> CreatePropietario([FromBody] CreatePropietarioRequest request)
        {
            try
            {
                var result = await _clientService.CreatePropietarioAsync(request);

                if (!result.Success)
                    return BadRequest(result);

                return CreatedAtAction(nameof(GetPropietario), new { id = result.Data.PropietarioId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando propietario");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener propietario por ID
        /// </summary>
        [HttpGet("propietarios/{id}")]
        public async Task<IActionResult> GetPropietario(int id)
        {
            try
            {
                var result = await _clientService.GetPropietarioByIdAsync(id);

                if (!result.Success)
                    return NotFound(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo propietario");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener todos los propietarios
        /// </summary>
        [HttpGet("propietarios")]
        public async Task<IActionResult> GetAllPropietarios()
        {
            try
            {
                var result = await _clientService.GetAllPropietariosAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo propietarios");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualizar propietario
        /// </summary>
        [HttpPut("propietarios/{id}")]
        public async Task<IActionResult> UpdatePropietario(int id, [FromBody] CreatePropietarioRequest request)
        {
            try
            {
                var result = await _clientService.UpdatePropietarioAsync(id, request);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando propietario");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Eliminar propietario
        /// </summary>
        [HttpDelete("propietarios/{id}")]
        public async Task<IActionResult> DeletePropietario(int id)
        {
            try
            {
                var result = await _clientService.DeletePropietarioAsync(id);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando propietario");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        // ========== ARRENDATARIOS ==========

        /// <summary>
        /// Crear arrendatario
        /// </summary>
        [HttpPost("arrendatarios")]
        public async Task<IActionResult> CreateArrendatario([FromBody] CreateArrendatarioRequest request)
        {
            try
            {
                var result = await _clientService.CreateArrendatarioAsync(request);

                if (!result.Success)
                    return BadRequest(result);

                return CreatedAtAction(nameof(GetArrendatario), new { id = result.Data.ArrendatarioId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando arrendatario");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener arrendatario por ID
        /// </summary>
        [HttpGet("arrendatarios/{id}")]
        public async Task<IActionResult> GetArrendatario(int id)
        {
            try
            {
                var result = await _clientService.GetArrendatarioByIdAsync(id);

                if (!result.Success)
                    return NotFound(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo arrendatario");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener todos los arrendatarios
        /// </summary>
        [HttpGet("arrendatarios")]
        public async Task<IActionResult> GetAllArrendatarios()
        {
            try
            {
                var result = await _clientService.GetAllArrendatariosAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo arrendatarios");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualizar arrendatario
        /// </summary>
        [HttpPut("arrendatarios/{id}")]
        public async Task<IActionResult> UpdateArrendatario(int id, [FromBody] CreateArrendatarioRequest request)
        {
            try
            {
                var result = await _clientService.UpdateArrendatarioAsync(id, request);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando arrendatario");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Eliminar arrendatario
        /// </summary>
        [HttpDelete("arrendatarios/{id}")]
        public async Task<IActionResult> DeleteArrendatario(int id)
        {
            try
            {
                var result = await _clientService.DeleteArrendatarioAsync(id);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando arrendatario");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Buscar arrendatarios por requisitos
        /// </summary>
        [HttpPost("arrendatarios/buscar")]
        public async Task<IActionResult> BuscarArrendatarios(decimal? maxArriendo, int? habitacionesMinimas, int? comunaId)
        {
            try
            {
                var result = await _clientService.BuscarArrendatariosPorRequisitosAsync(maxArriendo, habitacionesMinimas, comunaId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en búsqueda");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }
    }
}
