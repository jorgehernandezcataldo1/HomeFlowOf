using HomeFlow.Domain.Entities.Common;
using HomeFlow.Domain.Entities.Propiedades;
using HomeFlow.Domain.Enums;

namespace HomeFlow.Domain.Entities.Clientes;

/// <summary>
/// Representa a una persona natural. Una misma persona puede ser
/// propietaria de una propiedad y, en paralelo, estar buscando
/// arrendar/comprar otra: por eso NO se modela como dos tablas separadas
/// (Propietario / Arrendatario) sino como roles sobre un mismo Cliente.
/// </summary>
public class Cliente : EntidadEmpresaBase
{
    public int IdCliente { get; set; }

    /// <summary>
    /// RUT del cliente (formato: 12345678-9)
    /// </summary>
    public string Rut { get; set; } = string.Empty;

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    /// <summary>
    /// Dirección del domicilio actual
    /// </summary>
    public string? Direccion { get; set; }

    public EstadoCivil EstadoCivil { get; set; }

    /// <summary>
    /// URL o ruta de archivo de foto del carnet
    /// </summary>
    public string? FotoCarnetUrl { get; set; }

    /// <summary>
    /// Indica si pueden ser propietarios (arrendadores/vendedores)
    /// </summary>
    public bool EsPropietario { get; set; }

    /// <summary>
    /// Indica si buscan arrendar/comprar
    /// </summary>
    public bool EsArrendatarioComprador { get; set; }

    /// <summary>
    /// Teléfono emergencia
    /// </summary>
    public string? TelefonoEmergencia { get; set; }

    /// <summary>
    /// Notas o comentarios adicionales
    /// </summary>
    public string? Notas { get; set; }

    // Navegación
    /// <summary>
    /// Información extendida como arrendatario
    /// </summary>
    public InformacionArrendatario? InformacionArrendatario { get; set; }

    /// <summary>
    /// Propiedades de las que es propietario
    /// </summary>
    public ICollection<Propiedad> Propiedades { get; set; } = new List<Propiedad>();

    /// <summary>
    /// Requerimientos de búsqueda (filtros para matching)
    /// </summary>
    public ICollection<RequerimientoBusqueda> Requerimientos { get; set; } = new List<RequerimientoBusqueda>();

    public string NombreCompleto => $"{Nombres} {Apellidos}";
}
