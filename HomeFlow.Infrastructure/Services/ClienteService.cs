using HomeFlow.Application.DTOs;
using HomeFlow.Domain.Entities.Clientes;
using HomeFlow.Infrastructure.Repositories;

namespace HomeFlow.Application.Services;

/// <summary>
/// Servicio para gestionar operaciones de Clientes
/// </summary>
public interface IClienteService
{
    Task<ClienteDto?> GetClienteAsync(int id);
    Task<List<ClienteListDto>> GetClientesAsync(int page, int pageSize);
    Task<ClienteDto?> GetClienteByRutAsync(string rut);
    Task<ClienteDto> CrearClienteAsync(ClienteCreateUpdateDto dto);
    Task<ClienteDto> ActualizarClienteAsync(int id, ClienteCreateUpdateDto dto);
    Task<bool> EliminarClienteAsync(int id);
}

/// <summary>
/// Implementación del servicio de Clientes
/// </summary>
public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repository;

    public ClienteService(IClienteRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<ClienteDto?> GetClienteAsync(int id)
    {
        var cliente = await _repository.GetByIdAsync(id);
        return MapToDto(cliente);
    }

    public async Task<List<ClienteListDto>> GetClientesAsync(int page, int pageSize)
    {
        var (clientes, total) = await _repository.GetPaginatedAsync(page, pageSize);
        return clientes.Select(c => new ClienteListDto
        {
            IdCliente = c.IdCliente,
            Rut = c.Rut,
            NombreCompleto = $"{c.Nombres} {c.Apellidos}",
            Correo = c.Correo,
            Telefono = c.Telefono,
            EsPropietario = c.EsPropietario,
            EsArrendatarioComprador = c.EsArrendatarioComprador,
            FechaCreacion = c.FechaCreacion
        }).ToList();
    }

    public async Task<ClienteDto?> GetClienteByRutAsync(string rut)
    {
        var cliente = await _repository.GetByRutAsync(rut);
        return MapToDto(cliente);
    }

    public async Task<ClienteDto> CrearClienteAsync(ClienteCreateUpdateDto dto)
    {
        var cliente = new Cliente
        {
            Rut = dto.Rut,
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            Correo = dto.Correo,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion,
            EstadoCivil = dto.EstadoCivil,
            TelefonoEmergencia = dto.TelefonoEmergencia,
            Notas = dto.Notas,
            EsPropietario = dto.EsPropietario,
            EsArrendatarioComprador = dto.EsArrendatarioComprador,
            Activo = true
        };

        await _repository.AddAsync(cliente);
        await _repository.SaveChangesAsync();

        return MapToDto(cliente)!;
    }

    public async Task<ClienteDto> ActualizarClienteAsync(int id, ClienteCreateUpdateDto dto)
    {
        var cliente = await _repository.GetByIdAsync(id);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente con ID {id} no encontrado");

        cliente.Nombres = dto.Nombres;
        cliente.Apellidos = dto.Apellidos;
        cliente.Correo = dto.Correo;
        cliente.Telefono = dto.Telefono;
        cliente.Direccion = dto.Direccion;
        cliente.EstadoCivil = dto.EstadoCivil;
        cliente.TelefonoEmergencia = dto.TelefonoEmergencia;
        cliente.Notas = dto.Notas;
        cliente.EsPropietario = dto.EsPropietario;
        cliente.EsArrendatarioComprador = dto.EsArrendatarioComprador;
        cliente.FechaModificacion = DateTime.Now;

        await _repository.UpdateAsync(cliente);
        await _repository.SaveChangesAsync();

        return MapToDto(cliente)!;
    }

    public async Task<bool> EliminarClienteAsync(int id)
    {
        var cliente = await _repository.GetByIdAsync(id);
        if (cliente == null)
            return false;

        cliente.Activo = false;
        await _repository.UpdateAsync(cliente);
        await _repository.SaveChangesAsync();

        return true;
    }

    private static ClienteDto? MapToDto(Cliente? cliente)
    {
        if (cliente == null) return null;

        return new ClienteDto
        {
            IdCliente = cliente.IdCliente,
            Rut = cliente.Rut,
            Nombres = cliente.Nombres,
            Apellidos = cliente.Apellidos,
            Correo = cliente.Correo,
            Telefono = cliente.Telefono,
            Direccion = cliente.Direccion,
            FotoCarnetUrl = cliente.FotoCarnetUrl,
            EstadoCivil = cliente.EstadoCivil,
            EsPropietario = cliente.EsPropietario,
            EsArrendatarioComprador = cliente.EsArrendatarioComprador,
            FechaCreacion = cliente.FechaCreacion,
            FechaModificacion = cliente.FechaModificacion,
            Activo = cliente.Activo
        };
    }
}
