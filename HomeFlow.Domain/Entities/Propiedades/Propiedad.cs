using HomeFlow.Domain.Entities.Clientes;
using HomeFlow.Domain.Entities.Common;
using HomeFlow.Domain.Enums;

namespace HomeFlow.Domain.Entities.Propiedades;

/// <summary>
/// Representa una propiedad (casa, departamento, edificio, etc.)
/// </summary>
public class Propiedad : EntidadEmpresaBase
{
    public int IdPropiedad { get; set; }

    public int PropietarioId { get; set; }

    public int TipoPropiedadCatalogoId { get; set; }

    public TipoOperacion TipoOperacion { get; set; }

    public EstadoPropiedad Estado { get; set; } = EstadoPropiedad.Pendiente;

    // =================== UBICACIÓN ===================
    public string Direccion { get; set; } = string.Empty;

    public string? Comuna { get; set; }

    public string? Region { get; set; }

    /// <summary>
    /// Piso (para departamentos)
    /// </summary>
    public int? Piso { get; set; }

    /// <summary>
    /// Torre (para edificios)
    /// </summary>
    public string? Torre { get; set; }

    /// <summary>
    /// Número de departamento
    /// </summary>
    public string? NumeroDepartamento { get; set; }

    // Proximidad a servicios
    public decimal? DistanciaMetroMetros { get; set; }

    public string? NombreMetroCercano { get; set; }

    public decimal? DistanciaColegioMetros { get; set; }

    public string? NombreColegioCercano { get; set; }

    // =================== CARACTERÍSTICAS ===================
    public int Habitaciones { get; set; }

    public int Banos { get; set; }

    public bool TieneBodega { get; set; }

    public bool TieneEstacionamiento { get; set; }

    public int NumeroEstacionamientos { get; set; }

    public bool EsAmoblada { get; set; }

    public bool AceptaMascotas { get; set; }

    public bool TieneCondominio { get; set; }

    /// <summary>
    /// Metros cuadrados terrestures
    /// </summary>
    public decimal? MetrosCuadrados { get; set; }

    /// <summary>
    /// Antigüedad de la construcción en años
    /// </summary>
    public int? AntiguedadAnos { get; set; }

    /// <summary>
    /// Descripción adicional
    /// </summary>
    public string? Descripcion { get; set; }

    // =================== COSTOS ===================
    /// <summary>
    /// Gasto mensual de cuota condominio
    /// </summary>
    public decimal? GastosComunes { get; set; }

    /// <summary>
    /// Estimado de gastos básicos (agua, luz, gas)
    /// </summary>
    public decimal? GastosBasicosEstimados { get; set; }

    public bool PagaContribuciones { get; set; }

    /// <summary>
    /// Valor mensual de arriendo
    /// </summary>
    public decimal? PrecioArriendo { get; set; }

    /// <summary>
    /// Precio de venta
    /// </summary>
    public decimal? PrecioVenta { get; set; }

    // =================== NAVEGACIÓN ===================
    public Cliente? Propietario { get; set; }

    public TipoPropiedadCatalogo? TipoPropiedadCatalogo { get; set; }

    /// <summary>
    /// Visitas programadas a esta propiedad
    /// </summary>
    public ICollection<Agenda.Visita> Visitas { get; set; } = new List<Agenda.Visita>();

    /// <summary>
    /// Requerimientos de búsqueda que coinciden (para matching automático)
    /// </summary>
    public ICollection<RequerimientoBusqueda> RequerimientosCoincidentes { get; set; } = new List<RequerimientoBusqueda>();
}
