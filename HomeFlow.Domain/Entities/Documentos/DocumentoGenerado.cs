using HomeFlow.Domain.Entities.Common;

namespace HomeFlow.Domain.Entities.Documentos;

public class DocumentoGenerado : EntidadEmpresaBase
{
    public int IdDocumentoGenerado { get; set; }

    public int PlantillaDocumentoId { get; set; }

    public int? ClienteId { get; set; }

    public int? PropiedadId { get; set; }

    public int UsuarioGeneradorId { get; set; }

    public DateTime FechaGeneracion { get; set; } = DateTime.Now;

    public string RutaArchivoPdf { get; set; } = string.Empty;

    // Navegacion
    public PlantillaDocumento? PlantillaDocumento { get; set; }
}
