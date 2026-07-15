using HomeFlow.Domain.Entities.Common;

namespace HomeFlow.Domain.Entities.Checklist;

public class ChecklistItem : EntidadBase
{
    public int IdChecklistItem { get; set; }

    public int ChecklistPlantillaId { get; set; }

    public string Descripcion { get; set; } = string.Empty;

    public int Orden { get; set; }

    public bool Obligatorio { get; set; } = true;

    // Navegacion
    public ChecklistPlantilla? ChecklistPlantilla { get; set; }
}
