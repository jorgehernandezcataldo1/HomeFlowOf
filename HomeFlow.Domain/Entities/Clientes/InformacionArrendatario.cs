using HomeFlow.Domain.Entities.Common;
using HomeFlow.Domain.Enums;

namespace HomeFlow.Domain.Entities.Clientes;

// Datos adicionales que solo aplican cuando el Cliente busca arrendar/comprar
// (relacion 1 a 1 con Cliente). Se separa de Cliente porque un propietario
// puro no necesita estos datos, y evita columnas nulas innecesarias.
public class InformacionArrendatario : EntidadEmpresaBase
{
    public int IdInformacionArrendatario { get; set; }

    public int ClienteId { get; set; }

    public TipoTrabajo TipoTrabajo { get; set; }

    public decimal? IngresoLiquido { get; set; }

    public int? AntiguedadLaboralMeses { get; set; }

    public bool TieneHijos { get; set; }

    public int? NumeroHijos { get; set; }

    public bool TieneMascota { get; set; }

    public string? TipoMascota { get; set; }

    public string? Observaciones { get; set; }

    // Navegacion
    public Cliente? Cliente { get; set; }
}
