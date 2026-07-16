namespace HomeFlow.Infrastructure.Repositories.Base;

/// <summary>
/// Interfaz base para repositorios genéricos
/// </summary>
/// <typeparam name="T">Tipo de entidad</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Obtiene todas las entidades
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Obtiene una entidad por su ID
    /// </summary>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Agrega una nueva entidad
    /// </summary>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Actualiza una entidad existente
    /// </summary>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Elimina una entidad
    /// </summary>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Verifica si una entidad existe
    /// </summary>
    Task<bool> ExistsAsync(int id);

    /// <summary>
    /// Ejecuta una consulta personalizada
    /// </summary>
    IQueryable<T> Query();

    /// <summary>
    /// Guarda los cambios en la base de datos
    /// </summary>
    Task<int> SaveChangesAsync();
}
