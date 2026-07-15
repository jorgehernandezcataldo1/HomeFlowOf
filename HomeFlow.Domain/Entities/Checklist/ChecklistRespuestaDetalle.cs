namespace HomeFlow.Domain.Entities.Checklist;

public class ChecklistRespuestaDetalle
{
    public int IdChecklistRespuestaDetalle { get; set; }

    public int ChecklistRespuestaId { get; set; }

    public int ChecklistItemId { get; set; }

    public bool Cumple { get; set; }

    public string? Comentario { get; set; }

    // Navegacion
    public ChecklistRespuesta? ChecklistRespuesta { get; set; }

    public ChecklistItem? ChecklistItem { get; set; }
}
