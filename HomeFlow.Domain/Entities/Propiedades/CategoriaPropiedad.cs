using HomeFlow.Domain.Entities.Common;

namespace HomeFlow.Domain.Entities.Propiedades;

// Catalogo configurable (no enum) para que el corredor pueda agregar
// categorias nuevas sin tocar codigo. Ej: Residencial, Comercial, Industrial.
public class CategoriaPropiedad : EntidadEmpresaBase
{
    public int IdCategoriaPropiedad { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }
}
