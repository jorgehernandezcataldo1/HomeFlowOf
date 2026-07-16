using System.Security.Cryptography;
using System.Text;
using HomeFlow.Application.Interfaces;
using HomeFlow.Domain;
using HomeFlow.Domain.Entities.Users;
using HomeFlow.Domain.Interfaces;
using HomeFlow.Shared.DTOs.Auth;
using FluentValidation;

namespace HomeFlow.Application.Services
{
    /// <summary>
    /// Servicio de autenticación de corredores
    /// Maneja login, registro y gestión de contraseñas
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IValidator<RegisterCorredorRequest> _registerValidator;
        private readonly IValidator<ChangePasswordRequest> _changePasswordValidator;

        public AuthService(
            IUnitOfWork unitOfWork,
            IValidator<LoginRequest> loginValidator,
            IValidator<RegisterCorredorRequest> registerValidator,
            IValidator<ChangePasswordRequest> changePasswordValidator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _loginValidator = loginValidator ?? throw new ArgumentNullException(nameof(loginValidator));
            _registerValidator = registerValidator ?? throw new ArgumentNullException(nameof(registerValidator));
            _changePasswordValidator = changePasswordValidator ?? throw new ArgumentNullException(nameof(changePasswordValidator));
        }

        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                // Validar solicitud
                var validationResult = await _loginValidator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage);
                    return Result<LoginResponse>.ValidationError(errors);
                }

                // Buscar corredor por correo
                var corredor = await _unitOfWork.Repository<Corredor>()
                    .FirstOrDefaultAsync(c => c.Correo.ToLower() == request.Correo.ToLower(), cancellationToken);

                if (corredor == null)
                    return Result<LoginResponse>.Fail("Correo o contraseña incorrectos");

                // Verificar que esté activo
                if (!corredor.EsActivo)
                    return Result<LoginResponse>.Fail("La cuenta ha sido desactivada. Contacte al administrador.");

                // Verificar contraseña
                if (!VerifyPassword(request.Password, corredor.PasswordHash))
                    return Result<LoginResponse>.Fail("Correo o contraseña incorrectos");

                // Crear respuesta
                var response = new LoginResponse
                {
                    CorredorId = corredor.CorredorId,
                    Nombre = corredor.Nombre,
                    Apellido = corredor.Apellido,
                    Correo = corredor.Correo,
                    EsAdmin = corredor.EsAdmin
                };

                return Result<LoginResponse>.Ok(response, "Autenticación exitosa");
            }
            catch (Exception ex)
            {
                return Result<LoginResponse>.Fail($"Error durante la autenticación: {ex.Message}");
            }
        }

        public async Task<Result> RegisterCorredorAsync(RegisterCorredorRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                // Validar solicitud
                var validationResult = await _registerValidator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage);
                    return Result.ValidationError(errors);
                }

                // Verificar si el RUT ya existe
                var existeRut = await _unitOfWork.Repository<Corredor>()
                    .AnyAsync(c => c.Rut == request.Rut, cancellationToken);

                if (existeRut)
                    return Result.Fail("Ya existe un corredor con este RUT");

                // Verificar si el correo ya existe
                var existeCorreo = await _unitOfWork.Repository<Corredor>()
                    .AnyAsync(c => c.Correo.ToLower() == request.Correo.ToLower(), cancellationToken);

                if (existeCorreo)
                    return Result.Fail("Ya existe un corredor con este correo");

                // Crear nuevo corredor
                var corredor = new Corredor
                {
                    Rut = request.Rut,
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    Correo = request.Correo,
                    Telefono = request.Telefono,
                    Licencia = request.Licencia,
                    EsActivo = true,
                    EsAdmin = false,
                    FechaCreacion = DateTime.UtcNow,
                    FechaActualizacion = DateTime.UtcNow
                };

                // Hash y salt de la contraseña
                var (passwordHash, passwordSalt) = GeneratePasswordHash(request.Password);
                corredor.PasswordHash = passwordHash;
                corredor.PasswordSalt = passwordSalt;

                // Agregar al repositorio
                await _unitOfWork.Repository<Corredor>().AddAsync(corredor, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Ok("Corredor registrado exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error al registrar corredor: {ex.Message}");
            }
        }

        public async Task<Result> ChangePasswordAsync(int corredorId, ChangePasswordRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                // Validar solicit ud
                var validationResult = await _changePasswordValidator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage);
                    return Result.ValidationError(errors);
                }

                // Obtener corredor
                var corredor = await _unitOfWork.Repository<Corredor>()
                    .GetByIdAsync(corredorId, cancellationToken);

                if (corredor == null)
                    return Result.Fail("Corredor no encontrado");

                // Verificar contraseña actual
                if (!VerifyPassword(request.PasswordActual, corredor.PasswordHash))
                    return Result.Fail("La contraseña actual es incorrecta");

                // Actualizar contraseña
                var (passwordHash, passwordSalt) = GeneratePasswordHash(request.PasswordNueva);
                corredor.PasswordHash = passwordHash;
                corredor.PasswordSalt = passwordSalt;
                corredor.FechaActualizacion = DateTime.UtcNow;

                await _unitOfWork.Repository<Corredor>().UpdateAsync(corredor, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Ok("Contraseña actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error al cambiar contraseña: {ex.Message}");
            }
        }

        public async Task<Result> ValidateCredentialsAsync(string correo, string password, CancellationToken cancellationToken = default)
        {
            try
            {
                var corredor = await _unitOfWork.Repository<Corredor>()
                    .FirstOrDefaultAsync(c => c.Correo.ToLower() == correo.ToLower(), cancellationToken);

                if (corredor == null || !corredor.EsActivo)
                    return Result.Fail("Credenciales inválidas");

                if (!VerifyPassword(password, corredor.PasswordHash))
                    return Result.Fail("Credenciales inválidas");

                return Result.Ok("Credenciales válidas");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error al validar credenciales: {ex.Message}");
            }
        }

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            var (hash, _) = GeneratePasswordHash(password);
            return hash;
        }

        public bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
                return false;

            var (computedHash, _) = GeneratePasswordHash(password);
            return computedHash == hash;
        }

        /// <summary>
        /// Genera hash y salt de contraseña usando PBKDF2
        /// </summary>
        private (string hash, string salt) GeneratePasswordHash(string password)
        {
            // Generar salt
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);

            // Generar hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(20);
            string hashString = Convert.ToBase64String(hash);

            return (hashString, salt);
        }
    }
}
