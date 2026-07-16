using HomeFlow.Application.DTOs;
using HomeFlow.Domain.Entities.Propiedades;
using HomeFlow.Domain.Enums;
using HomeFlow.Infrastructure.Repositories;

namespace HomeFlow.Application.Services;

/// <summary>
/// Servicio para gestionar operaciones de Propiedades
/// </summary>
public interface IPropiedadService
{
    Task<PropiedadDto?> GetPropiedadAsync(int id);
    Task<List<PropiedadListDto>> GetPropiedadesAsync(int page, int pageSize, EstadoPropiedad? filtroEstado = null);
    Task<List<PropiedadListDto>> GetPropiedadesPorPropietarioAsync(int propietarioId);
    Task<PropiedadDto> CrearPropiedadAsync(PropiedadCreateUpdateDto dto);
    Task<PropiedadDto> ActualizarPropiedadAsync(int id, PropiedadCreateUpdateDto dto);
    Task<bool> EliminarPropiedadAsync(int id);
    Task<bool> CambiarEstadoPropiedadAsync(int id, EstadoPropiedad nuevoEstado);
}

/// <summary>
/// Implementación del servicio de Propiedades
/// </summary>
public class PropiedadService : IPropiedadService
{
    private readonly IPropiedadRepository _repository;
    private readonly IClienteRepository _clienteRepository;

    public PropiedadService(IPropiedadRepository repository, IClienteRepository clienteRepository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
    }

    public async Task<PropiedadDto?> GetPropiedadAsync(int id)
    {
        var propiedad = await _repository.GetWithDetailsAsync(id);
        return MapToDto(propiedad);
    }

    public async Task<List<PropiedadListDto>> GetPropiedadesAsync(int page, int pageSize, EstadoPropiedad? filtroEstado = null)
    {
        var (propiedades, total) = await _repository.GetPaginatedAsync(page, pageSize, filtroEstado);
        return propiedades.Select(p => new PropiedadListDto
        {
            IdPropiedad = p.IdPropiedad,
            Direccion = p.Direccion,
            TipoPropiedad = p.TipoPropiedadCatalogo?.Nombre ?? "N/A",
            Habitaciones = p.Habitaciones,
            Banos = p.Banos,
            Estado = p.Estado.ToString(),
            Precio = p.TipoOperacion == TipoOperacion.Venta ? p.PrecioVenta : p.PrecioArriendo,
            FechaCreacion = p.FechaCreacion
        }).ToList();
    }

    public async Task<List<PropiedadListDto>> GetPropiedadesPorPropietarioAsync(int propietarioId)
    {
        var propiedades = await _repository.GetByPropietarioAsync(propietarioId);
        return propiedades.Select(p => new PropiedadListDto
        {
            IdPropiedad = p.IdPropiedad,
            Direccion = p.Direccion,
            TipoPropiedad = p.TipoPropiedadCatalogo?.Nombre ?? "N/A",
            Habitaciones = p.Habitaciones,
            Banos = p.Banos,
            Estado = p.Estado.ToString(),
            Precio = p.TipoOperacion == TipoOperacion.Venta ? p.PrecioVenta : p.PrecioArriendo,
            FechaCreacion = p.FechaCreacion
        }).ToList();
    }

    public async Task<PropiedadDto> CrearPropiedadAsync(PropiedadCreateUpdateDto dto)
    {
        // Validar que el propietario existe
        var propietario = await _clienteRepository.GetByIdAsync(dto.PropietarioId);
        if (propietario == null || !propietario.EsPropietario)
            throw new InvalidOperationException("El cliente no es un propietario válido");

        var propiedad = new Propiedad
        {
            PropietarioId = dto.PropietarioId,
            TipoPropiedadCatalogoId = dto.TipoPropiedadCatalogoId,
            TipoOperacion = dto.TipoOperacion,
            Direccion = dto.Direccion,
            Comuna = dto.Comuna,
            Region = dto.Region,
            Piso = dto.Piso,
            Torre = dto.Torre,
            NumeroDepartamento = dto.NumeroDepartamento,
            DistanciaMetroMetros = dto.DistanciaMetroMetros,
            NombreMetroCercano = dto.NombreMetroCercano,
            DistanciaColegioMetros = dto.DistanciaColegioMetros,
            NombreColegioCercano = dto.NombreColegioCercano,
            Habitaciones = dto.Habitaciones,
            Banos = dto.Banos,
            TieneBodega = dto.TieneBodega,
            TieneEstacionamiento = dto.TieneEstacionamiento,
            NumeroEstacionamientos = dto.NumeroEstacionamientos,
            EsAmoblada = dto.EsAmoblada,
            AceptaMascotas = dto.AceptaMascotas,
            TieneCondominio = dto.TieneCondominio,
            MetrosCuadrados = dto.MetrosCuadrados,
            AntiguedadAnos = dto.AntiguedadAnos,
            Descripcion = dto.Descripcion,
            GastosComunes = dto.GastosComunes,
            GastosBasicosEstimados = dto.GastosBasicosEstimados,
            PagaContribuciones = dto.PagaContribuciones,
            PrecioArriendo = dto.PrecioArriendo,
            PrecioVenta = dto.PrecioVenta,
            Estado = EstadoPropiedad.Pendiente,
            Activo = true
        };

        await _repository.AddAsync(propiedad);
        await _repository.SaveChangesAsync();

        return MapToDto(await _repository.GetWithDetailsAsync(propiedad.IdPropiedad))!;
    }

    public async Task<PropiedadDto> ActualizarPropiedadAsync(int id, PropiedadCreateUpdateDto dto)
    {
        var propiedad = await _repository.GetByIdAsync(id);
        if (propiedad == null)
            throw new KeyNotFoundException($"Propiedad con ID {id} no encontrada");

        propiedad.Direccion = dto.Direccion;
        propiedad.Comuna = dto.Comuna;
        propiedad.Region = dto.Region;
        propiedad.Habitaciones = dto.Habitaciones;
        propiedad.Banos = dto.Banos;
        propiedad.GastosComunes = dto.GastosComunes;
        propiedad.GastosBasicosEstimados = dto.GastosBasicosEstimados;
        propiedad.PrecioArriendo = dto.PrecioArriendo;
        propiedad.PrecioVenta = dto.PrecioVenta;
        propiedad.Descripcion = dto.Descripcion;
        propiedad.FechaModificacion = DateTime.Now;

        await _repository.UpdateAsync(propiedad);
        await _repository.SaveChangesAsync();

        return MapToDto(await _repository.GetWithDetailsAsync(id))!;
    }

    public async Task<bool> EliminarPropiedadAsync(int id)
    {
        var propiedad = await _repository.GetByIdAsync(id);
        if (propiedad == null) return false;

        propiedad.Activo = false;
        await _repository.UpdateAsync(propiedad);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CambiarEstadoPropiedadAsync(int id, EstadoPropiedad nuevoEstado)
    {
        var propiedad = await _repository.GetByIdAsync(id);
        if (propiedad == null) return false;

        propiedad.Estado = nuevoEstado;
        propiedad.FechaModificacion = DateTime.Now;
        await _repository.UpdateAsync(propiedad);
        await _repository.SaveChangesAsync();

        return true;
    }

    private static PropiedadDto? MapToDto(Propiedad? propiedad)
    {
        if (propiedad == null) return null;

        return new PropiedadDto
        {
            IdPropiedad = propiedad.IdPropiedad,
            PropietarioId = propiedad.PropietarioId,
            NombrePropietario = propiedad.Propietario != null ? $"{propiedad.Propietario.Nombres} {propiedad.Propietario.Apellidos}" : null,
            TipoPropiedadCatalogoId = propiedad.TipoPropiedadCatalogoId,
            TipoPropiedad = propiedad.TipoPropiedadCatalogo?.Nombre,
            TipoOperacion = propiedad.TipoOperacion,
            Estado = propiedad.Estado,
            Direccion = propiedad.Direccion,
            Comuna = propiedad.Comuna,
            Region = propiedad.Region,
            Habitaciones = propiedad.Habitaciones,
            Banos = propiedad.Banos,
            MetrosCuadrados = propiedad.MetrosCuadrados,
            TieneEstacionamiento = propiedad.TieneEstacionamiento,
            PrecioArriendo = propiedad.PrecioArriendo,
            PrecioVenta = propiedad.PrecioVenta,
            FechaCreacion = propiedad.FechaCreacion,
            FechaModificacion = propiedad.FechaModificacion
        };
    }
}
