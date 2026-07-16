namespace HomeFlow.Domain.Entities.Properties
{
    using References;
    using Users;
    using Clients;
    using Processes;

    /// <summary>
    /// Entidad Propiedad - Representa un inmueble (departamento, casa, etc.)
    /// </summary>
    public class Propiedad : EntityBase
    {
        public int PropietarioId { get; set; }
        public int TipoPropiedadId { get; set; }
        public int CategoriaPropiedadId { get; set; }
        public int EstadoPropiedadId { get; set; }
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
        public bool Bodega { get; set; } = false;
        public int Estacionamiento { get; set; } = 0;
        public bool Terraza { get; set; } = false;
        public bool Jardin { get; set; } = false;
        public string EquipamientoKitchen { get; set; }
        public string Ventanas { get; set; }
        public bool Condominio { get; set; } = false;
        public decimal? GastosComunes { get; set; }
        public decimal? GastoAgua { get; set; }
        public decimal? GastoLuz { get; set; }
        public decimal? GastoGas { get; set; }
        public bool ContribucionesAFecto { get; set; } = false;
        public decimal? MontoContribuciones { get; set; }
        public bool PermiteArriendo { get; set; } = true;
        public bool PermiteVenta { get; set; } = true;
        public string InfoMetro { get; set; }
        public int? DistanciaMetro { get; set; } // en metros
        public string InfoColegio { get; set; }
        public int? DistanciaColegio { get; set; } // en metros
        public string InfoSupermercado { get; set; }
        public int? DistanciaSupermercado { get; set; } // en metros
        public bool RequiereHipoteca { get; set; } = false;
        public int CorredorAsignadoId { get; set; }
        public string FotoUrl { get; set; }
        public string FotosCasaUrl { get; set; } // JSON array
        public string DocumentosUrl { get; set; } // JSON array
        public string Notas { get; set; }
        public DateTime? FechaPublicacion { get; set; }

        // Relaciones
        public Propietario Propietario { get; set; }
        public TipoPropiedad TipoPropiedad { get; set; }
        public CategoriaPropiedad CategoriaPropiedad { get; set; }
        public EstadoPropiedad EstadoPropiedad { get; set; }
        public Comuna Comuna { get; set; }
        public Corredor CorredorAsignado { get; set; }
        public ChecklistPropiedad? ChecklistPropiedad { get; set; }
        public ICollection<OrdenVisita> OrdenesVisita { get; set; } = new List<OrdenVisita>();
        public ICollection<ContratoArriendo> Contratos { get; set; } = new List<ContratoArriendo>();
        public ICollection<MatchingClientePropiedad> Matchings { get; set; } = new List<MatchingClientePropiedad>();

        /// <summary>
        /// Obtiene el gasto total mensual (servicios + condominio)
        /// </summary>
        public decimal GastoTotalMensualEstimado =>
            (GastosComunes ?? 0) + (GastoAgua ?? 0) + (GastoLuz ?? 0) + (GastoGas ?? 0) + (MontoContribuciones ?? 0);

        /// <summary>
        /// Obtiene el costo total de arriendo incluyendo gastos
        /// </summary>
        public decimal CostoTotalArriendoEstimado =>
            (PrecioArriendo ?? 0) + GastoTotalMensualEstimado;

        /// <summary>
        /// Indica si la propiedad está disponible para arriendo
        /// </summary>
        public bool EstaDisponibleArriendo =>
            PermiteArriendo && 
            EstadoPropiedad?.Nombre == "Disponible" &&
            FechaPublicacion.HasValue;

        /// <summary>
        /// Obtiene dirección completa
        /// </summary>
        public string DireccionCompleta
        {
            get
            {
                var partes = new List<string> { Direccion };
                if (!string.IsNullOrEmpty(Piso?.ToString()))
                    partes.Add($"Piso {Piso}");
                if (!string.IsNullOrEmpty(Torre))
                    partes.Add($"Torre {Torre}");
                if (Comuna != null)
                    partes.Add(Comuna.Nombre);
                return string.Join(", ", partes);
            }
        }
    }
}
