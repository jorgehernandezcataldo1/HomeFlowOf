using System;
using System.Collections.Generic;
using System.Text;

// HomeFlow.Shared/DTOs/Usuarios/UsuarioDtos.cs
namespace HomeFlow.Shared.DTOs.Usuarios
{
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public string Rut { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string NombreCompleto => $"{Nombres} {Apellidos}";
        public string Correo { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public bool Bloqueado { get; set; }
        public DateTime? UltimoIngreso { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class UsuarioListDto
    {
        public int IdUsuario { get; set; }
        public string Rut { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public bool Bloqueado { get; set; }
    }

    public class CreateUsuarioRequest
    {
        public string Rut { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmarPassword { get; set; } = string.Empty;
    }

    public class UpdateUsuarioRequest
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
    }
}
