using HomeFlow.Domain.Entities.Common;

namespace HomeFlow.Domain.Entities.Propiedades;

// Catalogo configurable de tipos de inmueble: Casa, Departamento, Oficina,
// Bodega, Terreno, Edificio, etc. Al ser tabla (y no enum) el sistema
// escala sin recompilar cuando aparezcan nuevos tipos de propiedad.
public class TipoPropiedadCatalogo : EntidadEmpresaBase
{
    public int IdTipoPropiedadCatalogo { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public int CategoriaPropiedadId { get; set; }

    // Navegacion
    public CategoriaPropiedad? CategoriaPropiedad { get; set; }
}
