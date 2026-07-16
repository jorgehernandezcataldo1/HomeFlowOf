using AutoMapper;
using HomeFlow.Application.Interfaces;
using HomeFlow.Domain;
using HomeFlow.Domain.Entities.Clients;
using HomeFlow.Domain.Entities.Users;
using HomeFlow.Domain.Interfaces;
using HomeFlow.Shared.DTOs.Clients;
using FluentValidation;

namespace HomeFlow.Application.Services
{
    /// <summary>
    /// Servicio para gestión de clientes (Propietarios y Arrendatarios)
    /// </summary>
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateClienteRequest> _clienteValidator;
        private readonly IValidator<CreatePropietarioRequest> _propietarioValidator;
        private readonly IValidator<CreateArrendatarioRequest> _arrendatarioValidator;

        public ClientService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateClienteRequest> clienteValidator,
            IValidator<CreatePropietarioRequest> propietarioValidator,
            IValidator<CreateArrendatarioRequest> arrendatarioValidator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _clienteValidator = clienteValidator;
            _propietarioValidator = propietarioValidator;
            _arrendatarioValidator = arrendatarioValidator;
        }

        // ========== PROPIETARIOS ==========

        public async Task<Result<PropietarioDto>> CreatePropietarioAsync(CreatePropietarioRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var validationResult = await _propietarioValidator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage);
                    return Result<PropietarioDto>.ValidationError(errors);
                }

                // Verificar RUT único
                var existeRut = await _unitOfWork.Repository<Cliente>()
                    .AnyAsync(c => c.Rut == request.Cliente.Rut, cancellationToken);
                if (existeRut)
                    return Result<PropietarioDto>.Fail("Ya existe un cliente con este RUT");

                // Crear cliente
                var cliente = _mapper.Map<Cliente>(request.Cliente);
                cliente.FechaCreacion = DateTime.UtcNow;
                cliente.FechaActualizacion = DateTime.UtcNow;
                await _unitOfWork.Repository<Cliente>().AddAsync(cliente, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Crear propietario
                var propietario = new Propietario
                {
                    ClienteId = cliente.ClienteId,
                    BancoPreferido = request.BancoPreferido,
                    CuentaBancaria = request.CuentaBancaria,
                    TipoCuenta = request.TipoCuenta,
                    CorredorAsignadoId = request.CorredorAsignadoId,
                    FechaCreacion = DateTime.UtcNow,
                    FechaActualizacion = DateTime.UtcNow
                };

                await _unitOfWork.Repository<Propietario>().AddAsync(propietario, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<PropietarioDto>(propietario);
                return Result<PropietarioDto>.Ok(dto, "Propietario creado exitosamente");
            }
            catch (Exception ex)
            {
                return Result<PropietarioDto>.Fail($"Error al crear propietario: {ex.Message}");
            }
        }

        public async Task<Result<PropietarioDto>> GetPropietarioByIdAsync(int propietarioId, CancellationToken cancellationToken = default)
        {
            try
            {
                var propietario = await _unitOfWork.Repository<Propietario>().GetByIdAsync(propietarioId, cancellationToken);
                if (propietario == null)
                    return Result<PropietarioDto>.Fail("Propietario no encontrado");

                var dto = _mapper.Map<PropietarioDto>(propietario);
                return Result<PropietarioDto>.Ok(dto);
            }
            catch (Exception ex)
            {
                return Result<PropietarioDto>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<PropietarioDto>>> GetAllPropietariosAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var propietarios = await _unitOfWork.Repository<Propietario>().GetAllAsync(cancellationToken);
                var dtos = _mapper.Map<IEnumerable<PropietarioDto>>(propietarios);
                return Result<IEnumerable<PropietarioDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PropietarioDto>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<Result<PropietarioDto>> UpdatePropietarioAsync(int propietarioId, CreatePropietarioRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var propietario = await _unitOfWork.Repository<Propietario>().GetByIdAsync(propietarioId, cancellationToken);
                if (propietario == null)
                    return Result<PropietarioDto>.Fail("Propietario no encontrado");

                propietario.BancoPreferido = request.BancoPreferido;
                propietario.CuentaBancaria = request.CuentaBancaria;
                propietario.TipoCuenta = request.TipoCuenta;
                propietario.CorredorAsignadoId = request.CorredorAsignadoId;
                propietario.FechaActualizacion = DateTime.UtcNow;

                await _unitOfWork.Repository<Propietario>().UpdateAsync(propietario, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<PropietarioDto>(propietario);
                return Result<PropietarioDto>.Ok(dto, "Propietario actualizado");
            }
            catch (Exception ex)
            {
                return Result<PropietarioDto>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<Result> DeletePropietarioAsync(int propietarioId, CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.Repository<Propietario>().DeleteByIdAsync(propietarioId, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Ok("Propietario eliminado");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error: {ex.Message}");
            }
        }

        // ========== ARRENDATARIOS ==========

        public async Task<Result<ArrendatarioDto>> CreateArrendatarioAsync(CreateArrendatarioRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var validationResult = await _arrendatarioValidator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage);
                    return Result<ArrendatarioDto>.ValidationError(errors);
                }

                // Verificar RUT único
                var existeRut = await _unitOfWork.Repository<Cliente>()
                    .AnyAsync(c => c.Rut == request.Cliente.Rut, cancellationToken);
                if (existeRut)
                    return Result<ArrendatarioDto>.Fail("Ya existe un cliente con este RUT");

                // Crear cliente
                var cliente = _mapper.Map<Cliente>(request.Cliente);
                cliente.FechaCreacion = DateTime.UtcNow;
                cliente.FechaActualizacion = DateTime.UtcNow;
                await _unitOfWork.Repository<Cliente>().AddAsync(cliente, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Crear arrendatario
                var arrendatario = new Arrendatario
                {
                    ClienteId = cliente.ClienteId,
                    LiquidoMensual = request.LiquidoMensual,
                    Empleador = request.Empleador,
                    AntiguedadLaboral = request.AntiguedadLaboral,
                    TieneHijos = request.TieneHijos,
                    NumeroHijos = request.NumeroHijos,
                    TieneMascota = request.TieneMascota,
                    TipoMascota = request.TipoMascota,
                    CorredorAsignadoId = request.CorredorAsignadoId,
                    FechaCreacion = DateTime.UtcNow,
                    FechaActualizacion = DateTime.UtcNow
                };

                await _unitOfWork.Repository<Arrendatario>().AddAsync(arrendatario, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<ArrendatarioDto>(arrendatario);
                return Result<ArrendatarioDto>.Ok(dto, "Arrendatario creado exitosamente");
            }
            catch (Exception ex)
            {
                return Result<ArrendatarioDto>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<Result<ArrendatarioDto>> GetArrendatarioByIdAsync(int arrendatarioId, CancellationToken cancellationToken = default)
        {
            try
            {
                var arrendatario = await _unitOfWork.Repository<Arrendatario>().GetByIdAsync(arrendatarioId, cancellationToken);
                if (arrendatario == null)
                    return Result<ArrendatarioDto>.Fail("Arrendatario no encontrado");

                var dto = _mapper.Map<ArrendatarioDto>(arrendatario);
                return Result<ArrendatarioDto>.Ok(dto);
            }
            catch (Exception ex)
            {
                return Result<ArrendatarioDto>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<ArrendatarioDto>>> GetAllArrendatariosAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var arrendatarios = await _unitOfWork.Repository<Arrendatario>().GetAllAsync(cancellationToken);
                var dtos = _mapper.Map<IEnumerable<ArrendatarioDto>>(arrendatarios);
                return Result<IEnumerable<ArrendatarioDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ArrendatarioDto>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<Result<ArrendatarioDto>> UpdateArrendatarioAsync(int arrendatarioId, CreateArrendatarioRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var arrendatario = await _unitOfWork.Repository<Arrendatario>().GetByIdAsync(arrendatarioId, cancellationToken);
                if (arrendatario == null)
                    return Result<ArrendatarioDto>.Fail("Arrendatario no encontrado");

                arrendatario.LiquidoMensual = request.LiquidoMensual;
                arrendatario.Empleador = request.Empleador;
                arrendatario.AntiguedadLaboral = request.AntiguedadLaboral;
                arrendatario.TieneHijos = request.TieneHijos;
                arrendatario.NumeroHijos = request.NumeroHijos;
                arrendatario.TieneMascota = request.TieneMascota;
                arrendatario.TipoMascota = request.TipoMascota;
                arrendatario.CorredorAsignadoId = request.CorredorAsignadoId;
                arrendatario.FechaActualizacion = DateTime.UtcNow;

                await _unitOfWork.Repository<Arrendatario>().UpdateAsync(arrendatario, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<ArrendatarioDto>(arrendatario);
                return Result<ArrendatarioDto>.Ok(dto, "Arrendatario actualizado");
            }
            catch (Exception ex)
            {
                return Result<ArrendatarioDto>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<Result> DeleteArrendatarioAsync(int arrendatarioId, CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.Repository<Arrendatario>().DeleteByIdAsync(arrendatarioId, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Ok("Arrendatario eliminado");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error: {ex.Message}");
            }
        }

        // ========== BÚSQUEDA ==========

        public async Task<Result<IEnumerable<ArrendatarioDto>>> BuscarArrendatariosPorRequisitosAsync(decimal? maxArriendo, int? habitacionesMinimas, int? comunaId, CancellationToken cancellationToken = default)
        {
            try
            {
                var predicate = new Func<Arrendatario, bool>(a =>
                    (maxArriendo == null || a.CapacidadArriendoTeorida >= maxArriendo) &&
                    (habitacionesMinimas == null) // Se buscaría en propiedades
                    // (comunaId == null || a.Cliente.ComunaId == comunaId) // Se validaría con propiedades
                );

                var arrendatarios = await _unitOfWork.Repository<Arrendatario>().FindAsync(predicate, cancellationToken);
                var dtos = _mapper.Map<IEnumerable<ArrendatarioDto>>(arrendatarios);
                return Result<IEnumerable<ArrendatarioDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ArrendatarioDto>>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
