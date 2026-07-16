namespace HomeFlow.Domain.Entities.Clients
{
    using References;
    using Users;
    using Properties;

    /// <summary>
    /// Entidad Propietario - Cliente que posee propiedades
    /// </summary>
    public class Propietario : EntityBase
    {
        public int ClienteId { get; set; }
        public string BancoPreferido { get; set; }
        public string CuentaBancaria { get; set; }
        public string TipoCuenta { get; set; }
        public bool DocumentoIdentidadVerificado { get; set; } = false;
        public DateTime? FechaVerificacion { get; set; }
        public bool RequisitosCompletos { get; set; } = false;
        public DateTime? FechaRequisitosCompletos { get; set; }
        public int? CorredorAsignadoId { get; set; }

        // Relaciones
        public Cliente Cliente { get; set; }
        public Corredor? CorredorAsignado { get; set; }
        public ICollection<Propiedad> Propiedades { get; set; } = new List<Propiedad>();
        public ChecklistPropietario? ChecklistPropietario { get; set; }
        public ICollection<ContratoArriendo> Contratos { get; set; } = new List<ContratoArriendo>();

        /// <summary>
        /// Indica si el propietario cumple todos los requisitos
        /// </summary>
        public bool CumplelRequisitos => 
            DocumentoIdentidadVerificado && 
            RequisitosCompletos && 
            !string.IsNullOrEmpty(CuentaBancaria);

        public string NombreCompleto => Cliente?.NombreCompleto ?? "Sin Nombre";
        public string Rut => Cliente?.Rut ?? "";
    }

    /// <summary>
    /// Entidad Arrendatario - Cliente que busca arrendar propiedades
    /// </summary>
    public class Arrendatario : EntityBase
    {
        public int ClienteId { get; set; }
        public decimal? LiquidoMensual { get; set; }
        public string Empleador { get; set; }
        public int? AntiguedadLaboral { get; set; } // en meses
        public bool TieneHijos { get; set; } = false;
        public int NumeroHijos { get; set; } = 0;
        public bool TieneMascota { get; set; } = false;
        public string TipoMascota { get; set; }
        public bool PreAprobacionCredito { get; set; } = false;
        public bool DocumentacionCompleta { get; set; } = false;
        public bool RequisitosCompletos { get; set; } = false;
        public DateTime? FechaRequisitosCompletos { get; set; }
        public int? CorredorAsignadoId { get; set; }

        // Relaciones
        public Cliente Cliente { get; set; }
        public Corredor? CorredorAsignado { get; set; }
        public ICollection<OrdenVisita> OrdenesVisita { get; set; } = new List<OrdenVisita>();
        public ICollection<ChecklistArrendatario> ChecklistsArrendatario { get; set; } = new List<ChecklistArrendatario>();
        public ICollection<ContratoArriendo> Contratos { get; set; } = new List<ContratoArriendo>();
        public ICollection<MatchingClientePropiedad> Matchings { get; set; } = new List<MatchingClientePropiedad>();

        /// <summary>
        /// Indica si el arrendatario puede optar por arriendo (capacidad de pago y requisitoS)
        /// </summary>
        public bool PuedeOptar => 
            LiquidoMensual.HasValue && 
            LiquidoMensual.Value > 0 && 
            RequisitosCompletos && 
            DocumentacionCompleta;

        /// <summary>
        /// Capacidad de gasto teórica (30% del líquido = recomendación)
        /// </summary>
        public decimal CapacidadArriendoTeorida => 
            LiquidoMensual.HasValue ? Math.Round(LiquidoMensual.Value * 0.30m, 2) : 0;

        public string NombreCompleto => Cliente?.NombreCompleto ?? "Sin Nombre";
        public string Rut => Cliente?.Rut ?? "";
    }
}
