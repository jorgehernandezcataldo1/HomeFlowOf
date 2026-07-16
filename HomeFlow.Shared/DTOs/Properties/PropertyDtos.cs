namespace HomeFlow.Shared.DTOs.Properties
{
    /// <summary>
    /// DTO para Propiedad
    /// </summary>
    public class PropiedadDto
    {
        public int PropiedadId { get; set; }
        public int PropietarioId { get; set; }
        public string PropietarioNombre { get; set; }
        public int TipoPropiedadId { get; set; }
        public string TipoPropiedadNombre { get; set; }
        public int CategoriaPropiedadId { get; set; }
        public string CategoriaPropiedadNombre { get; set; }
        public int EstadoPropiedadId { get; set; }
        public string EstadoPropiedadNombre { get; set; }
        public string Direccion { get; set; }
        public int ComunaId { get; set; }
        public string ComunaNombre { get; set; }
        public int? Piso { get; set; }
        public string Torre { get; set; }
        public string CodigoPostal { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public string DescripcionGeneral { get; set; }
        public decimal? PrecioArriendo { get; set; }
        public decimal? PrecioVenta { get; set; }
        public int Habitaciones { get; set; }
        public int Banos { get; set; }
        public decimal? MetrosCuadrados { get; set; }
        public bool Bodega { get; set; }
        public int Estacionamiento { get; set; }
        public bool Terraza { get; set; }
        public bool Jardin { get; set; }
        public string EquipamientoKitchen { get; set; }
        public string Ventanas { get; set; }
        public bool Condominio { get; set; }
        public decimal? GastosComunes { get; set; }
        public decimal? GastoAgua { get; set; }
        public decimal? GastoLuz { get; set; }
        public decimal? GastoGas { get; set; }
        public bool ContribucionesAFecto { get; set; }
        public decimal? MontoContribuciones { get; set; }
        public bool PermiteArriendo { get; set; }
        public bool PermiteVenta { get; set; }
        public string InfoMetro { get; set; }
        public int? DistanciaMetro { get; set; }
        public string InfoColegio { get; set; }
        public int? DistanciaColegio { get; set; }
        public string InfoSupermercado { get; set; }
        public int? DistanciaSupermercado { get; set; }
        public bool RequiereHipoteca { get; set; }
        public int CorredorAsignadoId { get; set; }
        public string CorredorAsignadoNombre { get; set; }
        public string FotoUrl { get; set; }
        public string FotosCasaUrl { get; set; }
        public string DocumentosUrl { get; set; }
        public string Notas { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public decimal GastoTotalMensualEstimado { get; set; }
        public decimal CostoTotalArriendoEstimado { get; set; }
        public bool EstaDisponibleArriendo { get; set; }
        public string DireccionCompleta { get; set; }
    }

    /// <summary>
    /// DTO para crear Propiedad
    /// </summary>
    public class CreatePropiedadRequest
    {
        public int PropietarioId { get; set; }
        public int TipoPropiedadId { get; set; }
        public int CategoriaPropiedadId { get; set; }
        public string Direccion { get; set; }
        public int ComunaId { get; set; }
        public int? Piso { get; set; }
        public string Torre { get; set; }
        public string CodigoPostal { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public string DescripcionGeneral { get; set; }
        public decimal? PrecioArriendo { get; set; }
        public decimal? PrecioVenta { get; set; }
        public int Habitaciones { get; set; } = 1;
        public int Banos { get; set; } = 1;
        public decimal? MetrosCuadrados { get; set; }
        public bool Bodega { get; set; }
        public int Estacionamiento { get; set; }
        public bool Terraza { get; set; }
        public bool Jardin { get; set; }
        public string EquipamientoKitchen { get; set; }
        public string Ventanas { get; set; }
        public bool Condominio { get; set; }
        public decimal? GastosComunes { get; set; }
        public decimal? GastoAgua { get; set; }
        public decimal? GastoLuz { get; set; }
        public decimal? GastoGas { get; set; }
        public bool ContribucionesAFecto { get; set; }
        public decimal? MontoContribuciones { get; set; }
        public bool PermiteArriendo { get; set; } = true;
        public bool PermiteVenta { get; set; } = true;
        public string InfoMetro { get; set; }
        public int? DistanciaMetro { get; set; }
        public string InfoColegio { get; set; }
        public int? DistanciaColegio { get; set; }
        public string InfoSupermercado { get; set; }
        public int? DistanciaSupermercado { get; set; }
        public bool RequiereHipoteca { get; set; }
        public int CorredorAsignadoId { get; set; }
        public string FotoUrl { get; set; }
        public string Notas { get; set; }
    }

    /// <summary>
    /// DTO para editar Propiedad
    /// </summary>
    public class UpdatePropiedadRequest : CreatePropiedadRequest
    {
        public int PropiedadId { get; set; }
    }

    /// <summary>
    /// DTO para buscar y filtrar propiedades
    /// </summary>
    public class BuscarPropiedadRequest
    {
        public int? ComunaId { get; set; }
        public int? TipoPropiedadId { get; set; }
        public int? CategoriaPropiedadId { get; set; }
        public decimal? PrecioMinimo { get; set; }
        public decimal? PrecioMaximo { get; set; }
        public int? HabitacionesMinimas { get; set; }
        public int? BanosMinimos { get; set; }
        public bool? TieneGaraje { get; set; }
        public bool? TieneTerraza { get; set; }
        public bool? TieneCondominio { get; set; }
        public string Estado { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    /// <summary>
    /// DTO para respuesta paginada de propiedades
    /// </summary>
    public class PropiedadListaResponse
    {
        public int TotalRegistros { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public List<PropiedadDto> Propiedades { get; set; } = new List<PropiedadDto>();
    }
}
