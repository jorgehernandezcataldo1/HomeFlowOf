using Microsoft.AspNetCore.Mvc;
using HomeFlow.Application.Interfaces;
using HomeFlow.Shared.DTOs.Properties;

namespace HomeFlow.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        private readonly ILogger<PropertiesController> _logger;

        public PropertiesController(IPropertyService propertyService, ILogger<PropertiesController> logger)
        {
            _propertyService = propertyService ?? throw new ArgumentNullException(nameof(propertyService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Crear propiedad
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateProperty([FromBody] CreatePropiedadRequest request)
        {
            try
            {
                var result = await _propertyService.CreatePropertyAsync(request);

                if (!result.Success)
                    return BadRequest(result);

                return CreatedAtAction(nameof(GetProperty), new { id = result.Data.PropiedadId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando propiedad");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener propiedad por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProperty(int id)
        {
            try
            {
                var result = await _propertyService.GetPropertyByIdAsync(id);

                if (!result.Success)
                    return NotFound(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo propiedad");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener todas las propiedades
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllProperties()
        {
            try
            {
                var result = await _propertyService.GetAllPropertiesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo propiedades");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Buscar propiedades con filtros
        /// </summary>
        [HttpPost("search")]
        public async Task<IActionResult> SearchProperties([FromBody] BuscarPropiedadRequest request)
        {
            try
            {
                var result = await _propertyService.SearchPropertiesAsync(request);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en búsqueda");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualizar propiedad
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProperty(int id, [FromBody] UpdatePropiedadRequest request)
        {
            try
            {
                var result = await _propertyService.UpdatePropertyAsync(id, request);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando propiedad");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Eliminar propiedad
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            try
            {
                var result = await _propertyService.DeletePropertyAsync(id);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando propiedad");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener propiedades de un propietario
        /// </summary>
        [HttpGet("propietario/{propietarioId}")]
        public async Task<IActionResult> GetPropertiesByPropietario(int propietarioId)
        {
            try
            {
                var result = await _propertyService.GetPropertiesByPropietarioAsync(propietarioId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo propiedades");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener propiedades disponibles
        /// </summary>
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableProperties()
        {
            try
            {
                var result = await _propertyService.GetAvailablePropertiesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo propiedades disponibles");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Publicar propiedad
        /// </summary>
        [HttpPost("{id}/publish")]
        public async Task<IActionResult> PublishProperty(int id)
        {
            try
            {
                var result = await _propertyService.PublishPropertyAsync(id);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publicando propiedad");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }
    }
}
