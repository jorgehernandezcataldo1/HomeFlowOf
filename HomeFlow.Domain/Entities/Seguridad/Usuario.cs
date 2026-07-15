using System;
using System.Collections.Generic;
using System.Text;

using HomeFlow.Domain.Entities.Common;

namespace HomeFlow.Domain.Entities.Seguridad;

public class Usuario : EntidadEmpresaBase
{
    public int IdUsuario { get; set; }

    public string Rut { get; set; } = string.Empty;

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public string PasswordSalt { get; set; } = string.Empty;

    public DateTime? UltimoIngreso { get; set; }

    public bool Bloqueado { get; set; }

    // Navegación
    public Empresa? Empresa { get; set; }

    //public ICollection<UsuarioRol> Roles { get; set; } = new List<UsuarioRol>();
}