using System;
using System.Collections.Generic;
using System.Text;

namespace HomeFlow.Domain.Entities.Common;

public abstract class EntidadBase
{
    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public int? UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModificacion { get; set; }
}
