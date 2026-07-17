using HomeFlow.Application.Interfaces;
using HomeFlow.Domain;
using HomeFlow.Domain.Entities.Seguridad;
using HomeFlow.Domain.Interfaces;
using HomeFlow.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Identity;

namespace HomeFlow.Application.Services;

public sealed class AuthService(IUnitOfWork unitOfWork) : IAuthService
{
    private readonly PasswordHasher<Usuario> _hasher = new();

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Correo) || string.IsNullOrWhiteSpace(request.Password))
            return Result<LoginResponse>.Fail("Correo y contraseña son obligatorios.");

        var correo = request.Correo.Trim().ToUpperInvariant();
        var usuario = await unitOfWork.Repository<Usuario>().FirstOrDefaultAsync(
            user => user.Correo.ToUpper() == correo, cancellationToken);

        if (usuario is null || !usuario.Activo || usuario.Bloqueado ||
            _hasher.VerifyHashedPassword(usuario, usuario.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            return Result<LoginResponse>.Fail("Correo o contraseña incorrectos.");

        usuario.UltimoIngreso = DateTime.UtcNow;
        await unitOfWork.Repository<Usuario>().UpdateAsync(usuario, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<LoginResponse>.Ok(new LoginResponse
        {
            UsuarioId = usuario.IdUsuario,
            EmpresaId = usuario.EmpresaId,
            Nombre = usuario.Nombres,
            Apellido = usuario.Apellidos,
            Correo = usuario.Correo
        }, "Autenticación exitosa.");
    }

    public async Task<Result> RegisterCorredorAsync(RegisterCorredorRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Password) || request.Password != request.ConfirmarPassword)
            return Result.Fail("Las contraseñas no coinciden.");

        var usuarios = unitOfWork.Repository<Usuario>();
        if (await usuarios.AnyAsync(u => u.Rut == request.Rut || u.Correo.ToUpper() == request.Correo.Trim().ToUpperInvariant(), cancellationToken))
            return Result.Fail("Ya existe un usuario con ese RUT o correo.");

        var usuario = new Usuario { Rut = request.Rut.Trim(), Nombres = request.Nombre.Trim(), Apellidos = request.Apellido.Trim(), Correo = request.Correo.Trim(), EmpresaId = request.EmpresaId, Activo = true };
        usuario.PasswordHash = _hasher.HashPassword(usuario, request.Password);
        await usuarios.AddAsync(usuario, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Ok("Usuario registrado exitosamente.");
    }

    public async Task<Result> ChangePasswordAsync(int usuarioId, ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        if (request.PasswordNueva != request.ConfirmarPassword) return Result.Fail("Las contraseñas no coinciden.");
        var usuario = await unitOfWork.Repository<Usuario>().GetByIdAsync(usuarioId, cancellationToken);
        if (_hasher.VerifyHashedPassword(usuario, usuario.PasswordHash, request.PasswordActual) == PasswordVerificationResult.Failed) return Result.Fail("La contraseña actual es incorrecta.");
        usuario.PasswordHash = _hasher.HashPassword(usuario, request.PasswordNueva);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Ok("Contraseña actualizada.");
    }

    public async Task<Result> ValidateCredentialsAsync(string correo, string password, CancellationToken cancellationToken = default) =>
        (await LoginAsync(new LoginRequest { Correo = correo, Password = password }, cancellationToken)).Success ? Result.Ok("Credenciales válidas.") : Result.Fail("Credenciales inválidas.");

    public string HashPassword(string password) => _hasher.HashPassword(new Usuario(), password);
    public bool VerifyPassword(string password, string hash) => _hasher.VerifyHashedPassword(new Usuario(), hash, password) != PasswordVerificationResult.Failed;
}
