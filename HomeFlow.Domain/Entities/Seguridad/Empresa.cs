using System;
using System.Collections.Generic;
using System.Text;

using HomeFlow.Domain.Entities.Common;

namespace HomeFlow.Domain.Entities.Seguridad;

public class Empresa : EntidadBase
{
    public int IdEmpresa { get; set; }

    public string Rut { get; set; } = string.Empty;

    public string RazonSocial { get; set; } = string.Empty;

    public string? NombreFantasia { get; set; }

    public string Correo { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public string? Logo { get; set; }

    public string Plan { get; set; } = "FREE";
}