using System.Text.RegularExpressions;
using HomeFlow.Shared.DTOs.Auth;
using FluentValidation;

namespace HomeFlow.Application.Validators
{
    /// <summary>
    /// Validador para solicitud de login
    /// </summary>
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Correo)
                .NotEmpty().WithMessage("El correo es requerido")
                .EmailAddress().WithMessage("El correo debe ser válido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es requerida")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres");
        }
    }

    /// <summary>
    /// Validador para solicitud de cambio de contraseña
    /// </summary>
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.PasswordActual)
                .NotEmpty().WithMessage("La contraseña actual es requerida");

            RuleFor(x => x.PasswordNueva)
                .NotEmpty().WithMessage("La nueva contraseña es requerida")
                .MinimumLength(6).WithMessage("La nueva contraseña debe tener al menos 6 caracteres")
                .NotEqual(x => x.PasswordActual).WithMessage("La nueva contraseña debe ser diferente a la actual");

            RuleFor(x => x.ConfirmarPassword)
                .Equal(x => x.PasswordNueva).WithMessage("Las contraseñas no coinciden");
        }
    }

    /// <summary>
    /// Validador para registro de Corredor
    /// </summary>
    public class RegisterCorredorRequestValidator : AbstractValidator<RegisterCorredorRequest>
    {
        public RegisterCorredorRequestValidator()
        {
            RuleFor(x => x.Rut)
                .NotEmpty().WithMessage("El RUT es requerido")
                .Matches(@"^\d{7,8}-[0-9K]$", RegexOptions.IgnoreCase)
                .WithMessage("El RUT debe estar en formato XX.XXX.XXX-X o sin puntos XXXXXXXX-X");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es requerido")
                .MaximumLength(255).WithMessage("El nombre no puede exceder 255 caracteres");

            RuleFor(x => x.Apellido)
                .NotEmpty().WithMessage("El apellido es requerido")
                .MaximumLength(255).WithMessage("El apellido no puede exceder 255 caracteres");

            RuleFor(x => x.Correo)
                .NotEmpty().WithMessage("El correo es requerido")
                .EmailAddress().WithMessage("El correo debe ser válido");

            RuleFor(x => x.Telefono)
                .MaximumLength(15).WithMessage("El teléfono no puede exceder 15 caracteres");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es requerida")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres");
                //.Matches(@"[A-Z]", "La contraseña debe contener al menos una letra mayúscula")
                //.Matches(@"[a-z]", "La contraseña debe contener al menos una letra minúscula")
                //.Matches(@"[0-9]", "La contraseña debe contener al menos un número");

            RuleFor(x => x.ConfirmarPassword)
                .Equal(x => x.Password).WithMessage("Las contraseñas no coinciden");
        }
    }
}
