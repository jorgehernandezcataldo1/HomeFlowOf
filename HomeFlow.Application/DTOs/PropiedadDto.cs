using HomeFlow.Domain.Enums;

namespace HomeFlow.Application.DTOs;

/// <summary>
/// DTO para crear/actualizar una Propiedad
/// </summary>
public class PropiedadCreateUpdateDto
{
    public int PropietarioId { get; set; }

    public int TipoPropiedadCatalogoId { get; set; }

    public TipoOperacion TipoOperacion { get; set; }

    // Ubicación
    public string Direccion { get; set; } = string.Empty;

    public string? Comuna { get; set; }

    public string? Region { get; set; }

    public int? Piso { get; set; }

    public string? Torre { get; set; }

    public string? NumeroDepartamento { get; set; }

    public decimal? DistanciaMetroMetros { get; set; }

    public string? NombreMetroCercano { get; set; }

    public decimal? DistanciaColegioMetros { get; set; }

    public string? NombreColegioCercano { get; set; }

    // Características
    public int Habitaciones { get; set; }

    public int Banos { get; set; }

    public bool TieneBodega { get; set; }

    public bool TieneEstacionamiento { get; set; }

    public int NumeroEstacionamientos { get; set; }

    public bool EsAmoblada { get; set; }

    public bool AceptaMascotas { get; set; }

    public bool TieneCondominio { get; set; }

    public decimal? MetrosCuadrados { get; set; }

    public int? AntiguedadAnos { get; set; }

    public string? Descripcion { get; set; }

    // Costos
    public decimal? GastosComunes { get; set; }

    public decimal? GastosBasicosEstimados { get; set; }

    public bool PagaContribuciones { get; set; }

    public decimal? PrecioArriendo { get; set; }

    public decimal? PrecioVenta { get; set; }
}

/// <summary>
/// DTO para retornar datos de Propiedad
/// </summary>
public class PropiedadDto
{
    public int IdPropiedad { get; set; }

    public int PropietarioId { get; set; }

    public string? NombrePropietario { get; set; }

    public int TipoPropiedadCatalogoId { get; set; }

    public string? TipoPropiedad { get; set; }

    public TipoOperacion TipoOperacion { get; set; }

    public EstadoPropiedad Estado { get; set; }

    // Ubicación
    public string Direccion { get; set; } = string.Empty;

    public string? Comuna { get; set; }

    public string? Region { get; set; }

    // Características
    public int Habitaciones { get; set; }

    public int Banos { get; set; }

    public decimal? MetrosCuadrados { get; set; }

    public bool TieneEstacionamiento { get; set; }

    public decimal? PrecioArriendo { get; set; }

    public decimal? PrecioVenta { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }
}

/// <summary>
/// DTO resumido para listar propiedades
/// </summary>
public class PropiedadListDto
{
    public int IdPropiedad { get; set; }

    public string Direccion { get; set; } = string.Empty;

    public string TipoPropiedad { get; set; } = string.Empty;

    public int Habitaciones { get; set; }

    public int Banos { get; set; }

    public string Estado { get; set; } = string.Empty;

    public decimal? Precio { get; set; }

    public DateTime FechaCreacion { get; set; }
}
