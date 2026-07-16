using HomeFlow.Domain.Enums;

namespace HomeFlow.Application.DTOs;

/// <summary>
/// DTO para crear/actualizar un Cliente
/// </summary>
public class ClienteCreateUpdateDto
{
    public string Rut { get; set; } = string.Empty;

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public EstadoCivil EstadoCivil { get; set; }

    public string? TelefonoEmergencia { get; set; }

    public string? Notas { get; set; }

    public bool EsPropietario { get; set; }

    public bool EsArrendatarioComprador { get; set; }
}

/// <summary>
/// DTO para retornar datos de Cliente
/// </summary>
public class ClienteDto
{
    public int IdCliente { get; set; }

    public string Rut { get; set; } = string.Empty;

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string NombreCompleto => $"{Nombres} {Apellidos}";

    public string Correo { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? FotoCarnetUrl { get; set; }

    public EstadoCivil EstadoCivil { get; set; }

    public bool EsPropietario { get; set; }

    public bool EsArrendatarioComprador { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public bool Activo { get; set; }
}

/// <summary>
/// DTO para listar clientes con información resumida
/// </summary>
public class ClienteListDto
{
    public int IdCliente { get; set; }

    public string CodCliente => $"{Rut}-{IdCliente}";

    public string Rut { get; set; } = string.Empty;

    public string NombreCompleto { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public bool EsPropietario { get; set; }

    public bool EsArrendatarioComprador { get; set; }

    public DateTime FechaCreacion { get; set; }
}
