using AutoMapper;
using HomeFlow.Domain.Entities.Clients;
using HomeFlow.Domain.Entities.Properties;
using HomeFlow.Domain.Entities.References;
using HomeFlow.Domain.Entities.Processes;
using HomeFlow.Shared.DTOs.Clients;
using HomeFlow.Shared.DTOs.Properties;
using HomeFlow.Shared.DTOs.References;
using HomeFlow.Shared.DTOs.Processes;

namespace HomeFlow.Application.Mappings
{
    /// <summary>
    /// Perfiles de AutoMapper para mapeo de entidades a DTOs
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Cliente <-> ClienteDto
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<CreateClienteRequest, Cliente>()
                .ForMember(d => d.ClienteId, o => o.Ignore())
                .ForMember(d => d.FechaCreacion, o => o.Ignore())
                .ForMember(d => d.FechaActualizacion, o => o.Ignore());

            // Propietario <-> PropietarioDto
            CreateMap<Propietario, PropietarioDto>()
                .ForMember(d => d.Cliente, o => o.MapFrom(s => s.Cliente))
                .ReverseMap();
            CreateMap<CreatePropietarioRequest, Propietario>()
                .ForMember(d => d.PropietarioId, o => o.Ignore())
                .ForMember(d => d.FechaCreacion, o => o.Ignore())
                .ForMember(d => d.FechaActualizacion, o => o.Ignore());

            // Arrendatario <-> ArrendatarioDto
            CreateMap<Arrendatario, ArrendatarioDto>()
                .ForMember(d => d.Cliente, o => o.MapFrom(s => s.Cliente))
                .ForMember(d => d.CapacidadArriendoTeorida, o => o.MapFrom(s => s.CapacidadArriendoTeorida))
                .ForMember(d => d.PuedeOptar, o => o.MapFrom(s => s.PuedeOptar))
                .ReverseMap();
            CreateMap<CreateArrendatarioRequest, Arrendatario>()
                .ForMember(d => d.ArrendatarioId, o => o.Ignore())
                .ForMember(d => d.FechaCreacion, o => o.Ignore())
                .ForMember(d => d.FechaActualizacion, o => o.Ignore());

            // Propiedad <-> PropiedadDto
            CreateMap<Propiedad, PropiedadDto>()
                .ForMember(d => d.GastoTotalMensualEstimado, o => o.MapFrom(s => s.GastoTotalMensualEstimado))
                .ForMember(d => d.CostoTotalArriendoEstimado, o => o.MapFrom(s => s.CostoTotalArriendoEstimado))
                .ForMember(d => d.EstaDisponibleArriendo, o => o.MapFrom(s => s.EstaDisponibleArriendo))
                .ForMember(d => d.DireccionCompleta, o => o.MapFrom(s => s.DireccionCompleta))
                .ReverseMap();
            CreateMap<CreatePropiedadRequest, Propiedad>()
                .ForMember(d => d.PropiedadId, o => o.Ignore())
                .ForMember(d => d.EstadoPropiedadId, o => o.Ignore())
                .ForMember(d => d.FechaCreacion, o => o.Ignore())
                .ForMember(d => d.FechaActualizacion, o => o.Ignore());
            CreateMap<UpdatePropiedadRequest, Propiedad>()
                .ForMember(d => d.PropiedadId, o => o.Ignore())
                .ForMember(d => d.FechaCreacion, o => o.Ignore())
                .ForMember(d => d.FechaActualizacion, o => o.Ignore());

            // Referencias
            CreateMap<EstadoPropiedad, EstadoPropiedadDto>().ReverseMap();
            CreateMap<CategoriaPropiedad, CategoriaPropiedadDto>().ReverseMap();
            CreateMap<TipoPropiedad, TipoPropiedadDto>().ReverseMap();
            CreateMap<EstadoCivil, EstadoCivilDto>().ReverseMap();
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<Comuna, ComunaDto>()
                .ForMember(d => d.RegionNombre, o => o.MapFrom(s => s.Region.Nombre))
                .ReverseMap();

            // Procesos
            CreateMap<ChecklistPropietario, ChecklistPropietarioDto>()
                .ForMember(d => d.PorcentajeAvance, o => o.MapFrom(s => s.ObtenerPorcentajeAvance()))
                .ReverseMap();

            CreateMap<ChecklistPropiedad, ChecklistPropiedadDto>()
                .ForMember(d => d.PorcentajeAvance, o => o.MapFrom(s => s.ObtenerPorcentajeAvance()))
                .ReverseMap();

            CreateMap<OrdenVisita, OrdenVisitaDto>()
                .ForMember(d => d.EstadoDescriptivo, o => o.MapFrom(s => s.ObtenerEstadoDescriptivo()))
                .ReverseMap();
            CreateMap<CreateOrdenVisitaRequest, OrdenVisita>()
                .ForMember(d => d.OrdenVisitaId, o => o.Ignore())
                .ForMember(d => d.FechaCreacion, o => o.Ignore())
                .ForMember(d => d.FechaActualizacion, o => o.Ignore());

            CreateMap<ChecklistArrendatario, ChecklistArrendatarioDto>()
                .ForMember(d => d.TieneInteresContratar, o => o.MapFrom(s => s.TieneInteresContratar))
                .ReverseMap();

            CreateMap<ContratoArriendo, ContratoArriendoDto>()
                .ForMember(d => d.VigenciaEnMeses, o => o.MapFrom(s => s.ObtenerVigenciaEnMeses()))
                .ForMember(d => d.IngresoTotal, o => o.MapFrom(s => s.ObtenerIngresoTotal()))
                .ForMember(d => d.EstaVigente, o => o.MapFrom(s => s.EstaVigente))
                .ForMember(d => d.DiasRestantes, o => o.MapFrom(s => s.ObtenerDiasRestantes()))
                .ReverseMap();
            CreateMap<CreateContratoArriendoRequest, ContratoArriendo>()
                .ForMember(d => d.ContratoArriendoId, o => o.Ignore())
                .ForMember(d => d.FechaCreacion, o => o.Ignore())
                .ForMember(d => d.FechaActualizacion, o => o.Ignore());

            CreateMap<MatchingClientePropiedad, MatchingClientePropiedadDto>()
                .ForMember(d => d.CalidadMatch, o => o.MapFrom(s => s.ObtenerCalidadMatch()))
                .ReverseMap();
        }
    }
}
