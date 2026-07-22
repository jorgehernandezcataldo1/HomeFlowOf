// HomeFlow.Application/Services/UsuarioService.cs
using HomeFlow.Domain;
using HomeFlow.Domain.Entities.Seguridad;
using HomeFlow.Domain.Interfaces;
using HomeFlow.Application.Interfaces;
using HomeFlow.Shared.DTOs.Usuarios;
using Microsoft.AspNetCore.Identity;

namespace HomeFlow.Application.Services;

public sealed class UsuarioService(IUnitOfWork unitOfWork) : IUsuarioService
{
    private readonly PasswordHasher<Usuario> _hasher = new();

    public async Task<Result<UsuarioDto>> CrearUsuarioAsync(int empresaId, CreateUsuarioRequest request, CancellationToken cancellationToken = default)
    {
        var usuarios = unitOfWork.Repository<Usuario>();
        var correo = request.Correo.Trim().ToUpperInvariant();

        // Correo.Trim() tiene índice único global en la BD (login es por correo, sin empresa) -> validamos igual
        if (await usuarios.AnyAsync(u => u.Correo.ToUpper() == correo, cancellationToken))
            return Result<UsuarioDto>.Fail("Ya existe un usuario registrado con ese correo.");

        var usuario = new Usuario
        {
            EmpresaId = empresaId,
            Rut = request.Rut.Trim(),
            Nombres = request.Nombres.Trim(),
            Apellidos = request.Apellidos.Trim(),
            Correo = request.Correo.Trim(),
            Activo = true
        };
        usuario.PasswordHash = _hasher.HashPassword(usuario, request.Password);

        await usuarios.AddAsync(usuario, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UsuarioDto>.Ok(MapToDto(usuario), "Usuario creado exitosamente.");
    }

    public async Task<Result<UsuarioDto>> ActualizarUsuarioAsync(int empresaId, UpdateUsuarioRequest request, CancellationToken cancellationToken = default)
    {
        var usuarios = unitOfWork.Repository<Usuario>();
        var usuario = await usuarios.FirstOrDefaultAsync(u => u.IdUsuario == request.IdUsuario && u.EmpresaId == empresaId, cancellationToken);
        if (usuario is null) return Result<UsuarioDto>.Fail("Usuario no encontrado.");

        var correo = request.Correo.Trim().ToUpperInvariant();
        if (await usuarios.AnyAsync(u => u.IdUsuario != request.IdUsuario && u.Correo.ToUpper() == correo, cancellationToken))
            return Result<UsuarioDto>.Fail("Ya existe otro usuario con ese correo.");

        usuario.Nombres = request.Nombres.Trim();
        usuario.Apellidos = request.Apellidos.Trim();
        usuario.Correo = request.Correo.Trim();
        usuario.FechaModificacion = DateTime.Now;

        await usuarios.UpdateAsync(usuario, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UsuarioDto>.Ok(MapToDto(usuario), "Usuario actualizado.");
    }

    public async Task<Result<UsuarioDto>> ObtenerPorIdAsync(int empresaId, int idUsuario, CancellationToken cancellationToken = default)
    {
        var usuario = await unitOfWork.Repository<Usuario>()
            .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario && u.EmpresaId == empresaId, cancellationToken);

        return usuario is null
            ? Result<UsuarioDto>.Fail("Usuario no encontrado.")
            : Result<UsuarioDto>.Ok(MapToDto(usuario));
    }

    public async Task<Result<List<UsuarioListDto>>> ListarPorEmpresaAsync(int empresaId, CancellationToken cancellationToken = default)
    {
        var usuarios = (await unitOfWork.Repository<Usuario>().FindAsync(u => u.EmpresaId == empresaId, cancellationToken))
            .OrderBy(u => u.Nombres)
            .Select(u => new UsuarioListDto
            {
                IdUsuario = u.IdUsuario,
                Rut = u.Rut,
                NombreCompleto = $"{u.Nombres} {u.Apellidos}",
                Correo = u.Correo,
                Activo = u.Activo,
                Bloqueado = u.Bloqueado
            }).ToList();

        return Result<List<UsuarioListDto>>.Ok(usuarios);
    }

    public async Task<Result> DesactivarUsuarioAsync(int empresaId, int idUsuario, CancellationToken cancellationToken = default)
    {
        var usuarios = unitOfWork.Repository<Usuario>();
        var usuario = await usuarios.FirstOrDefaultAsync(u => u.IdUsuario == idUsuario && u.EmpresaId == empresaId, cancellationToken);
        if (usuario is null) return Result.Fail("Usuario no encontrado.");

        usuario.Activo = false;
        usuario.FechaModificacion = DateTime.Now;
        await usuarios.UpdateAsync(usuario, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Ok("Usuario desactivado.");
    }

    public async Task<Result> BloquearUsuarioAsync(int empresaId, int idUsuario, bool bloquear, CancellationToken cancellationToken = default)
    {
        var usuarios = unitOfWork.Repository<Usuario>();
        var usuario = await usuarios.FirstOrDefaultAsync(u => u.IdUsuario == idUsuario && u.EmpresaId == empresaId, cancellationToken);
        if (usuario is null) return Result.Fail("Usuario no encontrado.");

        usuario.Bloqueado = bloquear;
        usuario.FechaModificacion = DateTime.Now;
        await usuarios.UpdateAsync(usuario, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Ok(bloquear ? "Usuario bloqueado." : "Usuario desbloqueado.");
    }

    private static UsuarioDto MapToDto(Usuario u) => new()
    {
        IdUsuario = u.IdUsuario,
        Rut = u.Rut,
        Nombres = u.Nombres,
        Apellidos = u.Apellidos,
        Correo = u.Correo,
        Activo = u.Activo,
        Bloqueado = u.Bloqueado,
        UltimoIngreso = u.UltimoIngreso,
        FechaCreacion = u.FechaCreacion
    };
}