using HomeFlow.Domain.Entities.Common;
using HomeFlow.Domain.Enums;

namespace HomeFlow.Domain.Entities.Notificaciones;

public class Notificacion : EntidadEmpresaBase
{
    public int IdNotificacion { get; set; }

    public int? UsuarioDestinatarioId { get; set; } // null = visible para toda la empresa

    public int? ClienteId { get; set; }

    public int? PropiedadId { get; set; }

    public TipoNotificacion Tipo { get; set; }

    public string Mensaje { get; set; } = string.Empty;

    public bool Leida { get; set; }

    public DateTime? FechaProgramada { get; set; }
}
