namespace HomeFlow.Domain.Enums;

// Flujo sugerido: Pendiente -> Publicado -> EnTramite -> Arrendado/Vendido
// (se agregaron Publicado e Inactivo respecto a lo solicitado original,
// para distinguir "recien ingresada" de "visible en el sitio/portal")
public enum EstadoPropiedad
{
    Pendiente = 1,
    Publicado = 2,
    EnTramite = 3,
    Arrendado = 4,
    Vendido = 5,
    Inactivo = 6
}
