using HomeFlow.Domain.Entities.Common;
using HomeFlow.Domain.Enums;

namespace HomeFlow.Domain.Entities.Clientes;

// Representa a una persona natural. Una misma persona puede ser
// propietaria de una propiedad y, en paralelo, estar buscando
// arrendar/comprar otra: por eso NO se modela como dos tablas separadas
// (Propietario / Arrendatario) sino como roles sobre un mismo Cliente.
public class Cliente : EntidadEmpresaBase
{
    public int IdCliente { get; set; }

    public string Rut { get; set; } = string.Empty;

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public EstadoCivil EstadoCivil { get; set; }

    public string? FotoCarnetUrl { get; set; }

    public bool EsPropietario { get; set; }

    public bool EsArrendatarioComprador { get; set; }

    // Navegacion
    public InformacionArrendatario? InformacionArrendatario { get; set; }

    public ICollection<RequerimientoBusqueda> Requerimientos { get; set; } = new List<RequerimientoBusqueda>();
}
