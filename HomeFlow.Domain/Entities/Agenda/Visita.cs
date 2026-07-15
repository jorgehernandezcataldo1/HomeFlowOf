using HomeFlow.Domain.Entities.Clientes;
using HomeFlow.Domain.Entities.Common;
using HomeFlow.Domain.Entities.Propiedades;
using HomeFlow.Domain.Enums;

namespace HomeFlow.Domain.Entities.Agenda;

public class Visita : EntidadEmpresaBase
{
    public int IdVisita { get; set; }

    public int PropiedadId { get; set; }

    public int ClienteId { get; set; }

    public int UsuarioCorredorId { get; set; }

    public DateTime FechaHora { get; set; }

    public EstadoVisita Estado { get; set; } = EstadoVisita.Programada;

    public string? Observaciones { get; set; }

    public int? DocumentoGeneradoId { get; set; }

    // Navegacion
    public Propiedad? Propiedad { get; set; }

    public Cliente? Cliente { get; set; }
}
