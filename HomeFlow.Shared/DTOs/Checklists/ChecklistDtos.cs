using System;
using System.Collections.Generic;
using System.Text;
using HomeFlow.Domain;

namespace HomeFlow.Shared.DTOs.Checklists
{
    public class ChecklistItemDto
    {
        public int IdChecklistItem { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int Orden { get; set; }
        public bool Obligatorio { get; set; }
    }

    public class ChecklistPlantillaDto
    {
        public int IdChecklistPlantilla { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public List<ChecklistItemDto> Items { get; set; } = new();
    }

    public class CrearChecklistItemRequest
    {
        public string Descripcion { get; set; } = string.Empty;
        public int Orden { get; set; }
        public bool Obligatorio { get; set; } = true;
    }

    public class CrearChecklistPlantillaRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public HomeFlow.Domain.Enums.TipoChecklist TipoChecklist { get; set; }
        public List<CrearChecklistItemRequest> Items { get; set; } = new();
    }

    public class ChecklistRespuestaDetalleRequest
    {
        public int ChecklistItemId { get; set; }
        public bool Cumple { get; set; }
        public string? Comentario { get; set; }
    }

    public class CreateChecklistRespuestaRequest
    {
        public int ChecklistPlantillaId { get; set; }
        public int? ClienteId { get; set; }
        public int? PropiedadId { get; set; }
        public string? Observaciones { get; set; }
        public List<ChecklistRespuestaDetalleRequest> Detalles { get; set; } = new();
    }

    public class ChecklistRespuestaDto
    {
        public int IdChecklistRespuesta { get; set; }
        public int ChecklistPlantillaId { get; set; }
        public string ChecklistNombre { get; set; } = string.Empty;
        public int? ClienteId { get; set; }
        public int? PropiedadId { get; set; }
        public DateTime FechaEvaluacion { get; set; }
        public bool Aprobado { get; set; }
        public int PorcentajeAvance { get; set; }
        public string? Observaciones { get; set; }
    }
}
