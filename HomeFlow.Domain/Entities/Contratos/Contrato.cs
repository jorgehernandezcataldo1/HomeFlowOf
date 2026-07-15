using HomeFlow.Domain.Entities.Clientes;
using HomeFlow.Domain.Entities.Common;
using HomeFlow.Domain.Entities.Propiedades;
using HomeFlow.Domain.Enums;

namespace HomeFlow.Domain.Entities.Contratos;

public class Contrato : EntidadEmpresaBase
{
    public int IdContrato { get; set; }

    public int PropiedadId { get; set; }

    public int PropietarioId { get; set; }

    public int ArrendatarioId { get; set; }

    public TipoOperacion TipoOperacion { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaTermino { get; set; }

    public decimal MontoMensual { get; set; }

    public int DiaPago { get; set; }

    public EstadoContrato Estado { get; set; } = EstadoContrato.Vigente;

    public int? DocumentoGeneradoId { get; set; }

    // Navegacion
    public Propiedad? Propiedad { get; set; }

    public Cliente? Propietario { get; set; }

    public Cliente? Arrendatario { get; set; }
}
