// HomeFlow.Application/Validators/UsuarioValidators.cs
using System.Text.RegularExpressions;
using FluentValidation;
using HomeFlow.Shared.DTOs.Usuarios;

namespace HomeFlow.Application.Validators
{
    public class CreateUsuarioRequestValidator : AbstractValidator<CreateUsuarioRequest>
    {
        public CreateUsuarioRequestValidator()
        {
            RuleFor(x => x.Rut).NotEmpty()
                .Matches(@"^\d{7,8}-[0-9K]$", RegexOptions.IgnoreCase).WithMessage("RUT en formato 12345678-9");
            RuleFor(x => x.Nombres).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Apellidos).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Correo).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            RuleFor(x => x.ConfirmarPassword).Equal(x => x.Password).WithMessage("Las contraseñas no coinciden");
        }
    }

    public class UpdateUsuarioRequestValidator : AbstractValidator<UpdateUsuarioRequest>
    {
        public UpdateUsuarioRequestValidator()
        {
            RuleFor(x => x.IdUsuario).GreaterThan(0);
            RuleFor(x => x.Nombres).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Apellidos).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Correo).NotEmpty().EmailAddress();
        }
    }
}