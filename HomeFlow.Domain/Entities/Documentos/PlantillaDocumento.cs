using HomeFlow.Domain.Entities.Common;
using HomeFlow.Domain.Enums;

namespace HomeFlow.Domain.Entities.Documentos;

// Plantilla con placeholders (ej. {{NombreCliente}}, {{DireccionPropiedad}})
// que permite generar la Orden de Visita o el Contrato "con un click",
// reemplazando los marcadores por los datos reales antes de exportar a PDF.
public class PlantillaDocumento : EntidadEmpresaBase
{
    public int IdPlantillaDocumento { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public TipoDocumento TipoDocumento { get; set; }

    public string ContenidoHtml { get; set; } = string.Empty;
}
