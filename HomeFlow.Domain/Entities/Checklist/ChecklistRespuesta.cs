using HomeFlow.Domain.Entities.Common;

namespace HomeFlow.Domain.Entities.Checklist;

// Una "corrida" concreta de un checklist sobre un Cliente o una Propiedad.
public class ChecklistRespuesta : EntidadEmpresaBase
{
    public int IdChecklistRespuesta { get; set; }

    public int ChecklistPlantillaId { get; set; }

    public int? ClienteId { get; set; }

    public int? PropiedadId { get; set; }

    public int UsuarioEvaluadorId { get; set; }

    public DateTime FechaEvaluacion { get; set; } = DateTime.Now;

    public bool Aprobado { get; set; }

    public string? Observaciones { get; set; }

    // Navegacion
    public ChecklistPlantilla? ChecklistPlantilla { get; set; }

    public ICollection<ChecklistRespuestaDetalle> Detalles { get; set; } = new List<ChecklistRespuestaDetalle>();
}
