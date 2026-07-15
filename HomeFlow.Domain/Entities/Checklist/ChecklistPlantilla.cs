using HomeFlow.Domain.Entities.Common;
using HomeFlow.Domain.Enums;

namespace HomeFlow.Domain.Entities.Checklist;

// Plantilla configurable de checklist (para Cliente o para Propiedad).
// Al no ser un checklist fijo en codigo, el corredor puede ajustar los
// requisitos sin necesitar un cambio de sistema.
public class ChecklistPlantilla : EntidadEmpresaBase
{
    public int IdChecklistPlantilla { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public TipoChecklist TipoChecklist { get; set; }

    // Navegacion
    public ICollection<ChecklistItem> Items { get; set; } = new List<ChecklistItem>();
}
