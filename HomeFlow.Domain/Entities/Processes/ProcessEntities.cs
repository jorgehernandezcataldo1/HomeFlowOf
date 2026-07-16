namespace HomeFlow.Domain.Entities.Processes
{
    using References;
    using Users;
    using Clients;
    using Properties;

    /// <summary>
    /// Entidad Checklist Propietario - Verificación de cumplimientos del propietario
    /// </summary>
    public class ChecklistPropietario : EntityBase
    {
        public int PropietarioId { get; set; }
        public bool DocumentoIdentidadVerificado { get; set; } = false;
        public bool ComprobanteUbicacionVerificado { get; set; } = false;
        public bool CuentaBancariaVerificada { get; set; } = false;
        public bool AntecedentesLimpios { get; set; } = false;
        public bool FotoCarnetSubida { get; set; } = false;
        public bool EsCompleto { get; set; } = false;
        public DateTime? FechaCompleto { get; set; }
        public string Comentarios { get; set; }

        // Relaciones
        public Propietario Propietario { get; set; }

        /// <summary>
        /// Calcula el porcentaje de cumplimiento
        /// </summary>
        public int ObtenerPorcentajeAvance()
        {
            var items = new[] 
            { 
                DocumentoIdentidadVerificado,
                ComprobanteUbicacionVerificado,
                CuentaBancariaVerificada,
                AntecedentesLimpios,
                FotoCarnetSubida
            };
            return (int)Math.Round((items.Count(x => x) / (decimal)items.Length) * 100);
        }
    }

    /// <summary>
    /// Entidad Checklist Propiedad - Verificación de cumplimientos de la propiedad
    /// </summary>
    public class ChecklistPropiedad : EntityBase
    {
        public int PropiedadId { get; set; }
        public bool FotosCompletas { get; set; } = false;
        public bool DocumentacionCompleta { get; set; } = false;
        public bool ServiciosVerificados { get; set; } = false;
        public bool EspaciosVerificados { get; set; } = false;
        public bool CondicionesEstructurales { get; set; } = false;
        public bool DisponiblePublicar { get; set; } = false;
        public DateTime? FechaDisponiblePublicar { get; set; }
        public string Comentarios { get; set; }

        // Relaciones
        public Propiedad Propiedad { get; set; }

        /// <summary>
        /// Calcula el porcentaje de cumplimiento
        /// </summary>
        public int ObtenerPorcentajeAvance()
        {
            var items = new[]
            {
                FotosCompletas,
                DocumentacionCompleta,
                ServiciosVerificados,
                EspaciosVerificados,
                CondicionesEstructurales
            };
            return (int)Math.Round((items.Count(x => x) / (decimal)items.Length) * 100);
        }
    }

    /// <summary>
    /// Entidad Orden de Visita - Agenda de visita de arrendatario a propiedad
    /// </summary>
    public class OrdenVisita : EntityBase
    {
        public int PropiedadId { get; set; }
        public int ArrendatarioId { get; set; }
        public int CorredorId { get; set; }
        public DateTime FechaVisita { get; set; }
        public string Direccion { get; set; }
        public DateTime? HoraInicio { get; set; }
        public DateTime? HoraFin { get; set; }
        public bool Notificado { get; set; } = false;
        public DateTime? FechaNotificacion { get; set; }
        public string DocumentoUrl { get; set; }
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Confirmada, Completada, Cancelada
        public string Observaciones { get; set; }

        // Relaciones
        public Propiedad Propiedad { get; set; }
        public Arrendatario Arrendatario { get; set; }
        public Corredor Corredor { get; set; }
        public ChecklistArrendatario? ChecklistArrendatario { get; set; }

        /// <summary>
        /// Obtiene el estado de la visita formateado
        /// </summary>
        public string ObtenerEstadoDescriptivo()
        {
            return Estado switch
            {
                "Pendiente" => "Pendiente de Confirmación",
                "Confirmada" => "Confirmada",
                "Completada" => "Visita Realizada",
                "Cancelada" => "Cancelada",
                _ => "Desconocido"
            };
        }
    }

    /// <summary>
    /// Entidad Checklist Arrendatario - Verificación después de visita a propiedad
    /// </summary>
    public class ChecklistArrendatario : EntityBase
    {
        public int OrdenVisitaId { get; set; }
        public int ArrendatarioId { get; set; }
        public bool? PropiedadGusta { get; set; }
        public bool ConformidadEspacios { get; set; } = false;
        public bool ConformidadServicios { get; set; } = false;
        public bool PreguntasRespondidas { get; set; } = false;
        public bool InteresContratar { get; set; } = false;
        public DateTime? FechaCompleto { get; set; }
        public string Comentarios { get; set; }

        // Relaciones
        public OrdenVisita OrdenVisita { get; set; }
        public Arrendatario Arrendatario { get; set; }

        /// <summary>
        /// Calcula si el arrendatario tiene interés real en contratar
        /// </summary>
        public bool TieneInteresContratar =>
            PropiedadGusta == true && 
            ConformidadEspacios && 
            ConformidadServicios && 
            InteresContratar;
    }

    /// <summary>
    /// Entidad Contrato de Arriendo
    /// </summary>
    public class ContratoArriendo : EntityBase
    {
        public int PropiedadId { get; set; }
        public int PropietarioId { get; set; }
        public int ArrendatarioId { get; set; }
        public int CorredorId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTermino { get; set; }
        public decimal MontoMensual { get; set; }
        public decimal? DepositoGarantia { get; set; }
        public int DiasPago { get; set; } = 5; // Día del mes para pago
        public string Estado { get; set; } = "Activo"; // Activo, Finalizado, Cancelado, Suspendido
        public string DocumentoUrl { get; set; }
        public DateTime? FechaFirma { get; set; }
        public string Comentarios { get; set; }

        // Relaciones
        public Propiedad Propiedad { get; set; }
        public Propietario Propietario { get; set; }
        public Arrendatario Arrendatario { get; set; }
        public Corredor Corredor { get; set; }

        /// <summary>
        /// Obtiene la vigencia del contrato en meses
        /// </summary>
        public int ObtenerVigenciaEnMeses()
        {
            return (int)Math.Round((FechaTermino - FechaInicio).TotalDays / 30.44);
        }

        /// <summary>
        /// Obtiene el ingreso total del contrato
        /// </summary>
        public decimal ObtenerIngresoTotal()
        {
            return MontoMensual * ObtenerVigenciaEnMeses();
        }

        /// <summary>
        /// Indica si el contrato está vigente
        /// </summary>
        public bool EstaVigente =>
            Estado == "Activo" && 
            DateTime.UtcNow >= FechaInicio && 
            DateTime.UtcNow <= FechaTermino;

        /// <summary>
        /// Obtiene los días restantes del contrato
        /// </summary>
        public int ObtenerDiasRestantes()
        {
            if (Estado != "Activo") return 0;
            var diasRestantes = (FechaTermino - DateTime.UtcNow).Days;
            return diasRestantes > 0 ? diasRestantes : 0;
        }
    }

    /// <summary>
    /// Entidad Matching Cliente-Propiedad - Coincidencia automática
    /// </summary>
    public class MatchingClientePropiedad : EntityBase
    {
        public int ArrendatarioId { get; set; }
        public int PropiedadId { get; set; }
        public int PorcentajeCoincidencia { get; set; } = 0; // 0-100
        public decimal? RequisitosMaxNivelArriendo { get; set; }
        public int? RequistosHabitaciones { get; set; }
        public string RequisitosZona { get; set; }
        public bool? RequistosCondominio { get; set; }
        public bool? RequistosGaraje { get; set; }
        public bool EsNotificado { get; set; } = false;
        public DateTime? FechaNotificacion { get; set; }

        // Relaciones
        public Arrendatario Arrendatario { get; set; }
        public Propiedad Propiedad { get; set; }

        /// <summary>
        /// Obtiene la calidad del match
        /// </summary>
        public string ObtenerCalidadMatch()
        {
            return PorcentajeCoincidencia switch
            {
                >= 85 => "Excelente",
                >= 70 => "Muy Bueno",
                >= 50 => "Bueno",
                >= 30 => "Regular",
                _ => "Bajo"
            };
        }
    }
}
