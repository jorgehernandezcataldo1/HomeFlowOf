using FluentValidation;
using HomeFlow.Application.DTOs;
using HomeFlow.Shared.DTOs.Clients;
using System.Text.RegularExpressions;

namespace HomeFlow.Application.Validators
{
    /// <summary>
    /// Validador para creación de Cliente
    /// </summary>
    public class CreateClienteRequestValidator : AbstractValidator<CreateClienteRequest>
    {
        public CreateClienteRequestValidator()
        {
            RuleFor(x => x.Rut)
                .NotEmpty().WithMessage("El RUT es requerido")
                .Matches(@"^\d{7,8}-[0-9K]$", RegexOptions.IgnoreCase)
                .WithMessage("El RUT debe estar en formato correcto");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es requerido")
                .MaximumLength(255).WithMessage("El nombre no puede exceder 255 caracteres");

            RuleFor(x => x.Apellido)
                .NotEmpty().WithMessage("El apellido es requerido")
                .MaximumLength(255).WithMessage("El apellido no puede exceder 255 caracteres");

            RuleFor(x => x.Correo)
                .EmailAddress().WithMessage("El correo debe ser válido")
                .When(x => !string.IsNullOrEmpty(x.Correo));

            RuleFor(x => x.Telefono)
                .Matches(@"^\d{9,15}$", RegexOptions.IgnoreCase)
                .WithMessage("El teléfono debe contener solo números (9-15 dígitos)")
                .When(x => !string.IsNullOrEmpty(x.Telefono));

            RuleFor(x => x.Direccion)
                .MaximumLength(255).WithMessage("La dirección no puede exceder 255 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Direccion));
        }
    }

    /// <summary>
    /// Validador para creación de Propietario
    /// </summary>
    public class CreatePropietarioRequestValidator : AbstractValidator<CreatePropietarioRequest>
    {
        public CreatePropietarioRequestValidator()
        {
            RuleFor(x => x.Cliente)
                .NotNull().WithMessage("Los datos del cliente son requeridos")
                .SetValidator(new CreateClienteRequestValidator());

            RuleFor(x => x.CuentaBancaria)
                .NotEmpty().WithMessage("La cuenta bancaria es requerida")
                .MaximumLength(20).WithMessage("La cuenta bancaria no puede exceder 20 caracteres");

            RuleFor(x => x.BancoPreferido)
                .NotEmpty().WithMessage("El banco preferido es requerido")
                .MaximumLength(100).WithMessage("El banco no puede exceder 100 caracteres");

            RuleFor(x => x.TipoCuenta)
                .MaximumLength(20).WithMessage("El tipo de cuenta no puede exceder 20 caracteres")
                .When(x => !string.IsNullOrEmpty(x.TipoCuenta));
        }
    }

    /// <summary>
    /// Validador para creación de Arrendatario
    /// </summary>
    public class CreateArrendatarioRequestValidator : AbstractValidator<CreateArrendatarioRequest>
    {
        public CreateArrendatarioRequestValidator()
        {
            RuleFor(x => x.Cliente)
                .NotNull().WithMessage("Los datos del cliente son requeridos")
                .SetValidator(new CreateClienteRequestValidator());

            RuleFor(x => x.LiquidoMensual)
                .GreaterThan(0).WithMessage("El líquido mensual debe ser mayor a 0")
                .When(x => x.LiquidoMensual.HasValue);

            RuleFor(x => x.Empleador)
                .MaximumLength(255).WithMessage("El empleador no puede exceder 255 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Empleador));

            RuleFor(x => x.AntiguedadLaboral)
                .GreaterThanOrEqualTo(0).WithMessage("La antigüedad laboral no puede ser negativa")
                .When(x => x.AntiguedadLaboral.HasValue);

            RuleFor(x => x.NumeroHijos)
                .GreaterThanOrEqualTo(0).WithMessage("El número de hijos no puede ser negativo")
                .LessThanOrEqualTo(20).WithMessage("El número de hijos no puede exceder 20");

            RuleFor(x => x.TipoMascota)
                .MaximumLength(100).WithMessage("El tipo de mascota no puede exceder 100 caracteres")
                .When(x => !string.IsNullOrEmpty(x.TipoMascota));
        }
    }

    public class ClienteCreateUpdateDtoValidator : AbstractValidator<ClienteCreateUpdateDto>
    {
        public ClienteCreateUpdateDtoValidator()
        {
            RuleFor(x => x.Rut).NotEmpty()
                .Matches(@"^\d{7,8}-[0-9K]$", RegexOptions.IgnoreCase).WithMessage("RUT en formato 12345678-9");
            RuleFor(x => x.Nombres).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Apellidos).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Correo).NotEmpty().EmailAddress();
            RuleFor(x => x.Telefono).Matches(@"^\d{9,15}$").When(x => !string.IsNullOrEmpty(x.Telefono));
            RuleFor(x => x)
                .Must(x => x.EsPropietario || x.EsArrendatarioComprador)
                .WithMessage("El cliente debe ser Propietario y/o Arrendatario/Comprador.");
        }
    }
}
