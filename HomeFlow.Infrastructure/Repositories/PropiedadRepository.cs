using Microsoft.EntityFrameworkCore;
using HomeFlow.Domain.Entities.Propiedades;
using HomeFlow.Domain.Enums;
using HomeFlow.Infrastructure.Persistence;
using HomeFlow.Infrastructure.Repositories.Base;

namespace HomeFlow.Infrastructure.Repositories;

/// <summary>
/// Repositorio específico para Propiedades con métodos personalizados
/// </summary>
public interface IPropiedadRepository : IRepository<Propiedad>
{
    /// <summary>
    /// Obtiene las propiedades de un propietario
    /// </summary>
    Task<IEnumerable<Propiedad>> GetByPropietarioAsync(int propietarioId);

    /// <summary>
    /// Obtiene propiedades disponibles (no arrendadas ni vendidas)
    /// </summary>
    Task<IEnumerable<Propiedad>> GetDisponiblesAsync();

    /// <summary>
    /// Obtiene propiedades por tipo de operación
    /// </summary>
    Task<IEnumerable<Propiedad>> GetByTipoOperacionAsync(TipoOperacion tipoOperacion);

    /// <summary>
    /// Obtiene propiedades por estado
    /// </summary>
    Task<IEnumerable<Propiedad>> GetByEstadoAsync(EstadoPropiedad estado);

    /// <summary>
    /// Obtiene propiedad con todas sus relaciones navegacionales
    /// </summary>
    Task<Propiedad?> GetWithDetailsAsync(int propiedadId);

    /// <summary>
    /// Busca propiedades por ubicación
    /// </summary>
    Task<IEnumerable<Propiedad>> SearchByUbicacionAsync(string? comuna, string? region);

    /// <summary>
    /// Obtiene propiedades que coinciden con requerimientos de búsqueda
    /// </summary>
    Task<IEnumerable<Propiedad>> GetMatchingPropertiesAsync(int requeriemientoId);

    /// <summary>
    /// Obtiene propiedades paginadas
    /// </summary>
    Task<(IEnumerable<Propiedad> Propiedades, int Total)> GetPaginatedAsync(int page, int pageSize, EstadoPropiedad? filtroEstado = null);
}

/// <summary>
/// Implementación del repositorio de Propiedades
/// </summary>
public class PropiedadRepository : Repository<Propiedad>, IPropiedadRepository
{
    private readonly HomeFlowDbContext _homeFlowContext;

    public PropiedadRepository(HomeFlowDbContext context) : base(context)
    {
        _homeFlowContext = context;
    }

    public async Task<IEnumerable<Propiedad>> GetByPropietarioAsync(int propietarioId)
    {
        return await _dbSet
            .Where(p => p.PropietarioId == propietarioId && p.Activo)
            .OrderByDescending(p => p.FechaCreacion)
            .ToListAsync();
    }

    public async Task<IEnumerable<Propiedad>> GetDisponiblesAsync()
    {
        return await _dbSet
            .Where(p => p.Estado != EstadoPropiedad.Arrendado &&
                       p.Estado != EstadoPropiedad.Vendido &&
                       p.Estado != EstadoPropiedad.Inactivo &&
                       p.Activo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Propiedad>> GetByTipoOperacionAsync(TipoOperacion tipoOperacion)
    {
        return await _dbSet
            .Where(p => p.TipoOperacion == tipoOperacion && p.Activo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Propiedad>> GetByEstadoAsync(EstadoPropiedad estado)
    {
        return await _dbSet
            .Where(p => p.Estado == estado && p.Activo)
            .ToListAsync();
    }

    public async Task<Propiedad?> GetWithDetailsAsync(int propiedadId)
    {
        return await _dbSet
            .Include(p => p.Propietario)
            .Include(p => p.TipoPropiedadCatalogo)
            .FirstOrDefaultAsync(p => p.IdPropiedad == propiedadId && p.Activo);
    }

    public async Task<IEnumerable<Propiedad>> SearchByUbicacionAsync(string? comuna, string? region)
    {
        var query = _dbSet.Where(p => p.Activo);

        if (!string.IsNullOrEmpty(comuna))
            query = query.Where(p => p.Comuna != null && p.Comuna.Contains(comuna));

        if (!string.IsNullOrEmpty(region))
            query = query.Where(p => p.Region != null && p.Region.Contains(region));

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Propiedad>> GetMatchingPropertiesAsync(int requeriemientoId)
    {
        var requerimiento = await _homeFlowContext.Requerimientos
            .FirstOrDefaultAsync(r => r.IdRequerimientoBusqueda == requeriemientoId);

        // Por ahora devolvemos una lista vacía, la lógica de matching será implementada después
        return new List<Propiedad>();
    }

    public async Task<(IEnumerable<Propiedad> Propiedades, int Total)> GetPaginatedAsync(int page, int pageSize, EstadoPropiedad? filtroEstado = null)
    {
        var query = _dbSet.Where(p => p.Activo);

        if (filtroEstado.HasValue)
            query = query.Where(p => p.Estado == filtroEstado);

        var total = await query.CountAsync();
        var propiedades = await query
            .OrderByDescending(p => p.FechaCreacion)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(p => p.Propietario)
            .Include(p => p.TipoPropiedadCatalogo)
            .ToListAsync();

        return (propiedades, total);
    }
}
