using HomeFlow.Domain.Entities.Common;
using HomeFlow.Domain.Entities.Propiedades;
using HomeFlow.Domain.Enums;

namespace HomeFlow.Domain.Entities.Clientes;

// Lo que un Cliente esta buscando. Esta tabla es la base del motor de
// coincidencias: cuando se ingresa una Propiedad nueva, se compara contra
// los RequerimientoBusqueda activos para sugerir clientes compatibles,
// y viceversa (ver seccion "Motor de coincidencias" en el documento adjunto).
public class RequerimientoBusqueda : EntidadEmpresaBase
{
    public int IdRequerimientoBusqueda { get; set; }

    public int ClienteId { get; set; }

    public TipoOperacion TipoOperacion { get; set; }

    public int? TipoPropiedadCatalogoId { get; set; }

    public string? Comuna { get; set; }

    public decimal? PresupuestoMinimo { get; set; }

    public decimal? PresupuestoMaximo { get; set; }

    public int? HabitacionesMinimas { get; set; }

    public int? BanosMinimos { get; set; }

    public bool RequiereEstacionamiento { get; set; }

    public bool RequiereBodega { get; set; }

    public bool TieneMascota { get; set; }

    public bool Activo { get; set; } = true;

    // Navegacion
    public Cliente? Cliente { get; set; }

    public TipoPropiedadCatalogo? TipoPropiedadCatalogo { get; set; }
}
