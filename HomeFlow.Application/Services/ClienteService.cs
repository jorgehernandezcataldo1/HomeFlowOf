using HomeFlow.Application.DTOs;
using HomeFlow.Application.Interfaces;
using HomeFlow.Domain;
using HomeFlow.Domain.Entities.Clientes;
using HomeFlow.Domain.Interfaces;

namespace HomeFlow.Application.Services;

public sealed class ClienteService(IUnitOfWork unitOfWork) : IClienteService
{
    public async Task<Result<ClienteDto>> CrearAsync(int empresaId, int usuarioCreadorId, ClienteCreateUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var clientes = unitOfWork.Repository<Cliente>();
        var correo = dto.Correo.Trim().ToUpperInvariant();

        // Rut y Correo tienen índice único global en la BD -> se valida igual, sin filtrar por empresa
        if (await clientes.AnyAsync(c => c.Rut == dto.Rut || c.Correo.ToUpper() == correo, cancellationToken))
            return Result<ClienteDto>.Fail("Ya existe un cliente registrado con ese RUT o correo.");

        var cliente = new Cliente
        {
            EmpresaId = empresaId,
            Rut = dto.Rut.Trim(),
            Nombres = dto.Nombres.Trim(),
            Apellidos = dto.Apellidos.Trim(),
            Correo = dto.Correo.Trim(),
            Telefono = dto.Telefono,
            Direccion = dto.Direccion,
            EstadoCivil = dto.EstadoCivil,
            TelefonoEmergencia = dto.TelefonoEmergencia,
            Notas = dto.Notas,
            EsPropietario = dto.EsPropietario,
            EsArrendatarioComprador = dto.EsArrendatarioComprador,
            Activo = true,
            UsuarioCreacion = usuarioCreadorId
        };

        await clientes.AddAsync(cliente, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ClienteDto>.Ok(MapToDto(cliente), "Cliente creado exitosamente.");
    }

    public async Task<Result<ClienteDto>> ActualizarAsync(int empresaId, int idCliente, ClienteCreateUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var clientes = unitOfWork.Repository<Cliente>();
        var cliente = await clientes.FirstOrDefaultAsync(c => c.IdCliente == idCliente && c.EmpresaId == empresaId, cancellationToken);
        if (cliente is null) return Result<ClienteDto>.Fail("Cliente no encontrado.");

        var correo = dto.Correo.Trim().ToUpperInvariant();
        if (await clientes.AnyAsync(c => c.IdCliente != idCliente && (c.Rut == dto.Rut || c.Correo.ToUpper() == correo), cancellationToken))
            return Result<ClienteDto>.Fail("Otro cliente ya usa ese RUT o correo.");

        cliente.Nombres = dto.Nombres.Trim();
        cliente.Apellidos = dto.Apellidos.Trim();
        cliente.Correo = dto.Correo.Trim();
        cliente.Telefono = dto.Telefono;
        cliente.Direccion = dto.Direccion;
        cliente.EstadoCivil = dto.EstadoCivil;
        cliente.TelefonoEmergencia = dto.TelefonoEmergencia;
        cliente.Notas = dto.Notas;
        cliente.EsPropietario = dto.EsPropietario;
        cliente.EsArrendatarioComprador = dto.EsArrendatarioComprador;
        cliente.FechaModificacion = DateTime.Now;

        await clientes.UpdateAsync(cliente, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ClienteDto>.Ok(MapToDto(cliente), "Cliente actualizado.");
    }

    public async Task<Result<ClienteDto>> ObtenerPorIdAsync(int empresaId, int idCliente, CancellationToken cancellationToken = default)
    {
        var cliente = await unitOfWork.Repository<Cliente>()
            .FirstOrDefaultAsync(c => c.IdCliente == idCliente && c.EmpresaId == empresaId, cancellationToken);

        return cliente is null
            ? Result<ClienteDto>.Fail("Cliente no encontrado.")
            : Result<ClienteDto>.Ok(MapToDto(cliente));
    }

    public async Task<Result<List<ClienteListDto>>> ListarAsync(int empresaId, bool? soloPropietarios = null, CancellationToken cancellationToken = default)
    {
        var clientes = (await unitOfWork.Repository<Cliente>().FindAsync(c => c.EmpresaId == empresaId, cancellationToken))
            .Where(c => soloPropietarios == null || c.EsPropietario == soloPropietarios)
            .OrderBy(c => c.Nombres)
            .Select(c => new ClienteListDto
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

        return Result<List<ClienteListDto>>.Ok(clientes);
    }

    public async Task<Result> DesactivarAsync(int empresaId, int idCliente, CancellationToken cancellationToken = default)
    {
        var clientes = unitOfWork.Repository<Cliente>();
        var cliente = await clientes.FirstOrDefaultAsync(c => c.IdCliente == idCliente && c.EmpresaId == empresaId, cancellationToken);
        if (cliente is null) return Result.Fail("Cliente no encontrado.");

        cliente.Activo = false;
        cliente.FechaModificacion = DateTime.Now;
        await clientes.UpdateAsync(cliente, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Ok("Cliente desactivado.");
    }

    private static ClienteDto MapToDto(Cliente c) => new()
    {
        IdCliente = c.IdCliente,
        Rut = c.Rut,
        Nombres = c.Nombres,
        Apellidos = c.Apellidos,
        Correo = c.Correo,
        Telefono = c.Telefono,
        Direccion = c.Direccion,
        FotoCarnetUrl = c.FotoCarnetUrl,
        EstadoCivil = c.EstadoCivil,
        EsPropietario = c.EsPropietario,
        EsArrendatarioComprador = c.EsArrendatarioComprador,
        FechaCreacion = c.FechaCreacion,
        FechaModificacion = c.FechaModificacion,
        Activo = c.Activo
    };
}