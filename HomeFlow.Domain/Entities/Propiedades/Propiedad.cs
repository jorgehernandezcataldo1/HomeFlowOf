using HomeFlow.Domain.Entities.Clientes;
using HomeFlow.Domain.Entities.Common;
using HomeFlow.Domain.Enums;

namespace HomeFlow.Domain.Entities.Propiedades;

public class Propiedad : EntidadEmpresaBase
{
    public int IdPropiedad { get; set; }

    public int PropietarioId { get; set; }

    public int TipoPropiedadCatalogoId { get; set; }

    public TipoOperacion TipoOperacion { get; set; }

    public EstadoPropiedad Estado { get; set; } = EstadoPropiedad.Pendiente;

    // Ubicacion
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

    // Caracteristicas
    public int Habitaciones { get; set; }

    public int Banos { get; set; }

    public bool TieneBodega { get; set; }

    public bool TieneEstacionamiento { get; set; }

    public int NumeroEstacionamientos { get; set; }

    public bool EsAmoblada { get; set; }

    public bool AceptaMascotas { get; set; }

    public bool TieneCondominio { get; set; }

    // Costos
    public decimal? GastosComunes { get; set; }

    public decimal? GastosBasicosEstimados { get; set; }

    public bool PagaContribuciones { get; set; }

    public decimal? PrecioArriendo { get; set; }

    public decimal? PrecioVenta { get; set; }

    // Navegacion
    public Cliente? Propietario { get; set; }

    public TipoPropiedadCatalogo? TipoPropiedadCatalogo { get; set; }
}
