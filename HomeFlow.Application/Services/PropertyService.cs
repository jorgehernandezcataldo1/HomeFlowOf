using AutoMapper;
using HomeFlow.Application.Interfaces;
using HomeFlow.Domain;
using HomeFlow.Domain.Entities.Properties;
using HomeFlow.Domain.Entities.References;
using HomeFlow.Domain.Interfaces;
using HomeFlow.Shared.DTOs.Properties;
using FluentValidation;

namespace HomeFlow.Application.Services
{
    /// <summary>
    /// Servicio para gestión de propiedades
    /// </summary>
    public class PropertyService : IPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreatePropiedadRequest> _createPropertyValidator;
        private readonly IValidator<BuscarPropiedadRequest> _searchValidator;

        public PropertyService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreatePropiedadRequest> createPropertyValidator,
            IValidator<BuscarPropiedadRequest> searchValidator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _createPropertyValidator = createPropertyValidator;
            _searchValidator = searchValidator;
        }

        public async Task<Result<PropiedadDto>> CreatePropertyAsync(CreatePropiedadRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var validationResult = await _createPropertyValidator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage);
                    return Result<PropiedadDto>.ValidationError(errors);
                }

                // Verificar que propietario existe
                var propietario = await _unitOfWork.Repository<Domain.Entities.Clients.Propietario>()
                    .GetByIdAsync(request.PropietarioId, cancellationToken);
                if (propietario == null)
                    return Result<PropiedadDto>.Fail("Propietario no encontrado");

                var propiedad = _mapper.Map<Propiedad>(request);
                propiedad.EstadoPropiedadId = 1; // Pendiente por defecto
                propiedad.FechaCreacion = DateTime.UtcNow;
                propiedad.FechaActualizacion = DateTime.UtcNow;

                await _unitOfWork.Repository<Propiedad>().AddAsync(propiedad, cancellationToken);

                // Crear checklist automáticamente
                var checklist = new ChecklistPropiedad
                {
                    PropiedadId = propiedad.PropiedadId,
                    FechaCreacion = DateTime.UtcNow,
                    FechaActualizacion = DateTime.UtcNow
                };
                await _unitOfWork.Repository<ChecklistPropiedad>().AddAsync(checklist, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<PropiedadDto>(propiedad);
                return Result<PropiedadDto>.Ok(dto, "Propiedad creada exitosamente");
            }
            catch (Exception ex)
            {
                return Result<PropiedadDto>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<Result<PropiedadDto>> GetPropertyByIdAsync(int propiedadId, CancellationToken cancellationToken = default)
        {
            try
            {
                var propiedad = await _unitOfWork.Repository<Propiedad>().GetByIdAsync(propiedadId, cancellationToken);
                if (propiedad == null)
                    return Result<PropiedadDto>.Fail("Propiedad no encontrada");

                var dto = _mapper.Map<PropiedadDto>(propiedad);
                return Result<PropiedadDto>.Ok(dto);
            }
            catch (Exception ex)
            {
                return Result<PropiedadDto>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<PropiedadDto>>> GetAllPropertiesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var propiedades = await _unitOfWork.Repository<Propiedad>().GetAllAsync(cancellationToken);
                var dtos = _mapper.Map<IEnumerable<PropiedadDto>>(propiedades);
                return Result<IEnumerable<PropiedadDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PropiedadDto>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<Result<PropiedadListaResponse>> SearchPropertiesAsync(BuscarPropiedadRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var validationResult = await _searchValidator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage);
                    return Result<PropiedadListaResponse>.ValidationError(errors);
                }

                var predicate = new Func<Propiedad, bool>(p =>
                    (request.ComunaId == null || p.ComunaId == request.ComunaId) &&
                    (request.TipoPropiedadId == null || p.TipoPropiedadId == request.TipoPropiedadId) &&
                    (request.CategoriaPropiedadId == null || p.CategoriaPropiedadId == request.CategoriaPropiedadId) &&
                    (request.PrecioMinimo == null || p.PrecioArriendo >= request.PrecioMinimo) &&
                    (request.PrecioMaximo == null || p.PrecioArriendo <= request.PrecioMaximo) &&
                    (request.HabitacionesMinimas == null || p.Habitaciones >= request.HabitacionesMinimas) &&
                    (request.BanosMinimos == null || p.Banos >= request.BanosMinimos) &&
                    (request.TieneGaraje == null || (request.TieneGaraje == true ? p.Estacionamiento > 0 : true)) &&
                    (request.TieneTerraza == null || (request.TieneTerraza == true ? p.Terraza : true)) &&
                    (request.TieneCondominio == null || (request.TieneCondominio == true ? p.Condominio : true)) &&
                    (string.IsNullOrEmpty(request.Estado) || p.EstadoPropiedad.Nombre == request.Estado)
                );

                var propiedades = await _unitOfWork.Repository<Propiedad>().FindAsync(predicate, cancellationToken);
                var dto = _mapper.Map<IEnumerable<PropiedadDto>>(propiedades);

                var totalRegistros = dto.Count();
                var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)request.PageSize);

                var result = new PropiedadListaResponse
                {
                    TotalRegistros = totalRegistros,
                    PaginaActual = request.PageNumber,
                    TotalPaginas = totalPaginas,
                    Propiedades = dto.Skip((request.PageNumber - 1) * request.PageSize)
                                      .Take(request.PageSize)
                                      .ToList()
                };

                return Result<PropiedadListaResponse>.Ok(result);
            }
            catch (Exception ex)
            {
                return Result<PropiedadListaResponse>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<Result<PropiedadDto>> UpdatePropertyAsync(int propiedadId, UpdatePropiedadRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var validationResult = await _createPropertyValidator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage);
                    return Result<PropiedadDto>.ValidationError(errors);
                }

                var propiedad = await _unitOfWork.Repository<Propiedad>().GetByIdAsync(propiedadId, cancellationToken);
                if (propiedad == null)
                    return Result<PropiedadDto>.Fail("Propiedad no encontrada");

                _mapper.Map(request, propiedad);
                propiedad.FechaActualizacion = DateTime.UtcNow;

                await _unitOfWork.Repository<Propiedad>().UpdateAsync(propiedad, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<PropiedadDto>(propiedad);
                return Result<PropiedadDto>.Ok(dto, "Propiedad actualizada");
            }
            catch (Exception ex)
            {
                return Result<PropiedadDto>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<Result> DeletePropertyAsync(int propiedadId, CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.Repository<Propiedad>().DeleteByIdAsync(propiedadId, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Ok("Propiedad eliminada");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<PropiedadDto>>> GetPropertiesByPropietarioAsync(int propietarioId, CancellationToken cancellationToken = default)
        {
            try
            {
                var propiedades = await _unitOfWork.Repository<Propiedad>()
                    .FindAsync(p => p.PropietarioId == propietarioId, cancellationToken);
                var dtos = _mapper.Map<IEnumerable<PropiedadDto>>(propiedades);
                return Result<IEnumerable<PropiedadDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PropiedadDto>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<PropiedadDto>>> GetAvailablePropertiesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var propiedades = await _unitOfWork.Repository<Propiedad>()
                    .FindAsync(p => p.EstaDisponibleArriendo, cancellationToken);
                var dtos = _mapper.Map<IEnumerable<PropiedadDto>>(propiedades);
                return Result<IEnumerable<PropiedadDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PropiedadDto>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<Result> PublishPropertyAsync(int propiedadId, CancellationToken cancellationToken = default)
        {
            try
            {
                var propiedad = await _unitOfWork.Repository<Propiedad>().GetByIdAsync(propiedadId, cancellationToken);
                if (propiedad == null)
                    return Result.Fail("Propiedad no encontrada");

                // Cambiar estado a Disponible
                propiedad.EstadoPropiedadId = 3; // Disponible
                propiedad.FechaPublicacion = DateTime.UtcNow;
                propiedad.FechaActualizacion = DateTime.UtcNow;

                await _unitOfWork.Repository<Propiedad>().UpdateAsync(propiedad, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Ok("Propiedad publicada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error: {ex.Message}");
            }
        }
    }
}
