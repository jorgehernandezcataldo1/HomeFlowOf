using HomeFlow.Shared.DTOs.Properties;
using FluentValidation;

namespace HomeFlow.Application.Validators
{
    /// <summary>
    /// Validador para creación de Propiedad
    /// </summary>
    public class CreatePropiedadRequestValidator : AbstractValidator<CreatePropiedadRequest>
    {
        public CreatePropiedadRequestValidator()
        {
            RuleFor(x => x.PropietarioId)
                .GreaterThan(0).WithMessage("El propietario es requerido");

            RuleFor(x => x.TipoPropiedadId)
                .GreaterThan(0).WithMessage("El tipo de propiedad es requerido");

            RuleFor(x => x.CategoriaPropiedadId)
                .GreaterThan(0).WithMessage("La categoría de propiedad es requerida");

            RuleFor(x => x.Direccion)
                .NotEmpty().WithMessage("La dirección es requerida")
                .MaximumLength(255).WithMessage("La dirección no puede exceder 255 caracteres");

            RuleFor(x => x.ComunaId)
                .GreaterThan(0).WithMessage("La comuna es requerida");

            RuleFor(x => x.Habitaciones)
                .GreaterThan(0).WithMessage("El número de habitaciones debe ser mayor a 0")
                .LessThanOrEqualTo(20).WithMessage("El número de habitaciones no puede exceder 20");

            RuleFor(x => x.Banos)
                .GreaterThan(0).WithMessage("El número de baños debe ser mayor a 0")
                .LessThanOrEqualTo(20).WithMessage("El número de baños no puede exceder 20");

            RuleFor(x => x.MetrosCuadrados)
                .GreaterThan(0).WithMessage("Los metros cuadrados deben ser mayor a 0")
                .When(x => x.MetrosCuadrados.HasValue);

            RuleFor(x => x.PrecioArriendo)
                .GreaterThan(0).WithMessage("El precio de arriendo debe ser mayor a 0")
                .When(x => x.PrecioArriendo.HasValue && x.PermiteArriendo);

            RuleFor(x => x.PrecioVenta)
                .GreaterThan(0).WithMessage("El precio de venta debe ser mayor a 0")
                .When(x => x.PrecioVenta.HasValue && x.PermiteVenta);

            RuleFor(x => x.GastosComunes)
                .GreaterThanOrEqualTo(0).WithMessage("Los gastos comunes no pueden ser negativos")
                .When(x => x.GastosComunes.HasValue);

            RuleFor(x => x.Estacionamiento)
                .GreaterThanOrEqualTo(0).WithMessage("El número de estacionamientos no puede ser negativo")
                .LessThanOrEqualTo(10).WithMessage("El número de estacionamientos no puede exceder 10");

            RuleFor(x => x.CorredorAsignadoId)
                .GreaterThan(0).WithMessage("El corredor asignado es requerido");
        }
    }

    /// <summary>
    /// Validador para búsqueda de Propiedades
    /// </summary>
    public class BuscarPropiedadRequestValidator : AbstractValidator<BuscarPropiedadRequest>
    {
        public BuscarPropiedadRequestValidator()
        {
            RuleFor(x => x.PrecioMinimo)
                .GreaterThanOrEqualTo(0).WithMessage("El precio mínimo no puede ser negativo")
                .When(x => x.PrecioMinimo.HasValue);

            RuleFor(x => x.PrecioMaximo)
                .GreaterThan(0).WithMessage("El precio máximo debe ser mayor a 0")
                .GreaterThanOrEqualTo(x => x.PrecioMinimo ?? 0)
                .WithMessage("El precio máximo debe ser mayor o igual al precio mínimo")
                .When(x => x.PrecioMaximo.HasValue);

            RuleFor(x => x.HabitacionesMinimas)
                .GreaterThan(0).WithMessage("Las habitaciones mínimas deben ser mayor a 0")
                .When(x => x.HabitacionesMinimas.HasValue);

            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("El número de página debe ser mayor a 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("El tamaño de página debe ser mayor a 0")
                .LessThanOrEqualTo(100).WithMessage("El tamaño de página no puede exceder 100");
        }
    }
}
