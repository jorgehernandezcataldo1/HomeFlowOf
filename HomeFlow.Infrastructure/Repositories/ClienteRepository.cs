using Microsoft.EntityFrameworkCore;
using HomeFlow.Domain.Entities.Clientes;
using HomeFlow.Infrastructure.Persistence;
using HomeFlow.Infrastructure.Repositories.Base;

namespace HomeFlow.Infrastructure.Repositories;

/// <summary>
/// Repositorio específico para Clientes con métodos personalizados
/// </summary>
public interface IClienteRepository : IRepository<Cliente>
{
    /// <summary>
    /// Busca un cliente por RUT
    /// </summary>
    Task<Cliente?> GetByRutAsync(string rut);

    /// <summary>
    /// Busca un cliente por correo
    /// </summary>
    Task<Cliente?> GetByCorreoAsync(string correo);

    /// <summary>
    /// Obtiene todos los clientes propietarios
    /// </summary>
    Task<IEnumerable<Cliente>> GetPropietariosAsync();

    /// <summary>
    /// Obtiene todos los clientes que buscan arrendar/comprar
    /// </summary>
    Task<IEnumerable<Cliente>> GetArrendatariosAsync();

    /// <summary>
    /// Obtiene un cliente con su información de arrendatario
    /// </summary>
    Task<Cliente?> GetWithArrendatarioInfoAsync(int clienteId);

    /// <summary>
    /// Obtiene un cliente con sus propiedades
    /// </summary>
    Task<Cliente?> GetWithPropiedadesAsync(int clienteId);

    /// <summary>
    /// Busca clientes activos por término de búsqueda
    /// </summary>
    Task<IEnumerable<Cliente>> SearchAsync(string searchTerm);

    /// <summary>
    /// Obtiene clientes paginados
    /// </summary>
    Task<(IEnumerable<Cliente> Clientes, int Total)> GetPaginatedAsync(int page, int pageSize);
}

/// <summary>
/// Implementación del repositorio de Clientes
/// </summary>
public class ClienteRepository : Repository<Cliente>, IClienteRepository
{
    private readonly HomeFlowDbContext _homeFlowContext;

    public ClienteRepository(HomeFlowDbContext context) : base(context)
    {
        _homeFlowContext = context;
    }

    public async Task<Cliente?> GetByRutAsync(string rut)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Rut == rut && c.Activo);
    }

    public async Task<Cliente?> GetByCorreoAsync(string correo)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Correo == correo && c.Activo);
    }

    public async Task<IEnumerable<Cliente>> GetPropietariosAsync()
    {
        return await _dbSet
            .Where(c => c.EsPropietario && c.Activo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Cliente>> GetArrendatariosAsync()
    {
        return await _dbSet
            .Where(c => c.EsArrendatarioComprador && c.Activo)
            .ToListAsync();
    }

    public async Task<Cliente?> GetWithArrendatarioInfoAsync(int clienteId)
    {
        return await _dbSet
            .Include(c => c.InformacionArrendatario)
            .FirstOrDefaultAsync(c => c.IdCliente == clienteId && c.Activo);
    }

    public async Task<Cliente?> GetWithPropiedadesAsync(int clienteId)
    {
        return await _dbSet
            .Include(c => c.Propiedades)
            .FirstOrDefaultAsync(c => c.IdCliente == clienteId && c.Activo);
    }

    public async Task<IEnumerable<Cliente>> SearchAsync(string searchTerm)
    {
        return await _dbSet
            .Where(c => (c.Nombres.Contains(searchTerm) ||
                        c.Apellidos.Contains(searchTerm) ||
                        c.Rut.Contains(searchTerm) ||
                        c.Correo.Contains(searchTerm)) &&
                       c.Activo)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Cliente> Clientes, int Total)> GetPaginatedAsync(int page, int pageSize)
    {
        var query = _dbSet.Where(c => c.Activo);
        var total = await query.CountAsync();
        var clientes = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (clientes, total);
    }
}
