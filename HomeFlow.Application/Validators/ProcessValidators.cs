using HomeFlow.Shared.DTOs.Processes;
using FluentValidation;

namespace HomeFlow.Application.Validators
{
    /// <summary>
    /// Validador para creación de Orden de Visita
    /// </summary>
    public class CreateOrdenVisitaRequestValidator : AbstractValidator<CreateOrdenVisitaRequest>
    {
        public CreateOrdenVisitaRequestValidator()
        {
            RuleFor(x => x.PropiedadId)
                .GreaterThan(0).WithMessage("La propiedad es requerida");

            RuleFor(x => x.ArrendatarioId)
                .GreaterThan(0).WithMessage("El arrendatario es requerido");

            RuleFor(x => x.CorredorId)
                .GreaterThan(0).WithMessage("El corredor es requerido");

            RuleFor(x => x.FechaVisita)
                .NotEmpty().WithMessage("La fecha de visita es requerida")
                .GreaterThan(DateTime.UtcNow).WithMessage("La fecha de visita debe ser futura");

            RuleFor(x => x.HoraInicio)
                .NotNull().WithMessage("La hora de inicio es requerida")
                .When(x => x.HoraInicio.HasValue);

            RuleFor(x => x.HoraFin)
                .GreaterThan(x => x.HoraInicio ?? DateTime.MinValue)
                .WithMessage("La hora de fin debe ser posterior a la hora de inicio")
                .When(x => x.HoraFin.HasValue && x.HoraInicio.HasValue);

            RuleFor(x => x.Observaciones)
                .MaximumLength(1000).WithMessage("Las observaciones no pueden exceder 1000 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Observaciones));
        }
    }

    /// <summary>
    /// Validador para creación de Checklist de Arrendatario
    /// </summary>
    public class CreateChecklistArrendatarioRequestValidator : AbstractValidator<CreateChecklistArrendatarioRequest>
    {
        public CreateChecklistArrendatarioRequestValidator()
        {
            RuleFor(x => x.OrdenVisitaId)
                .GreaterThan(0).WithMessage("La orden de visita es requerida");

            RuleFor(x => x.ArrendatarioId)
                .GreaterThan(0).WithMessage("El arrendatario es requerido");

            RuleFor(x => x.Comentarios)
                .MaximumLength(1000).WithMessage("Los comentarios no pueden exceder 1000 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Comentarios));
        }
    }

    /// <summary>
    /// Validador para creación de Contrato de Arriendo
    /// </summary>
    public class CreateContratoArriendoRequestValidator : AbstractValidator<CreateContratoArriendoRequest>
    {
        public CreateContratoArriendoRequestValidator()
        {
            RuleFor(x => x.PropiedadId)
                .GreaterThan(0).WithMessage("La propiedad es requerida");

            RuleFor(x => x.PropietarioId)
                .GreaterThan(0).WithMessage("El propietario es requerido");

            RuleFor(x => x.ArrendatarioId)
                .GreaterThan(0).WithMessage("El arrendatario es requerido");

            RuleFor(x => x.CorredorId)
                .GreaterThan(0).WithMessage("El corredor es requerido");

            RuleFor(x => x.FechaInicio)
                .NotEmpty().WithMessage("La fecha de inicio es requerida")
                .GreaterThan(DateTime.UtcNow.AddDays(-1)).WithMessage("La fecha de inicio debe ser hoy o posterior");

            RuleFor(x => x.FechaTermino)
                .GreaterThan(x => x.FechaInicio)
                .WithMessage("La fecha de término debe ser posterior a la fecha de inicio");

            RuleFor(x => x.MontoMensual)
                .GreaterThan(0).WithMessage("El monto mensual debe ser mayor a 0");

            RuleFor(x => x.DepositoGarantia)
                .GreaterThanOrEqualTo(0).WithMessage("El depósito de garantía no puede ser negativo")
                .When(x => x.DepositoGarantia.HasValue);

            RuleFor(x => x.DiasPago)
                .GreaterThan(0).WithMessage("El día de pago debe ser mayor a 0")
                .LessThanOrEqualTo(31).WithMessage("El día de pago no puede exceder 31");

            RuleFor(x => x.Comentarios)
                .MaximumLength(1000).WithMessage("Los comentarios no pueden exceder 1000 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Comentarios));
        }
    }
}
