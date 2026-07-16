namespace HomeFlow.Domain.Specifications;

/// <summary>
/// Interfaz base para especificaciones de dominio (usada para lógica de negocio compleja)
/// </summary>
public interface IDomainSpecification<T> where T : class
{
    IQueryable<T> Apply(IQueryable<T> query);
}
