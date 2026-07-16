namespace HomeFlow.Shared.DTOs.Processes
{
    /// <summary>
    /// DTO para Checklist de Propietario
    /// </summary>
    public class ChecklistPropietarioDto
    {
        public int ChecklistPropietarioId { get; set; }
        public int PropietarioId { get; set; }
        public bool DocumentoIdentidadVerificado { get; set; }
        public bool ComprobanteUbicacionVerificado { get; set; }
        public bool CuentaBancariaVerificada { get; set; }
        public bool AntecedentesLimpios { get; set; }
        public bool FotoCarnetSubida { get; set; }
        public bool EsCompleto { get; set; }
        public DateTime? FechaCompleto { get; set; }
        public string Comentarios { get; set; }
        public int PorcentajeAvance { get; set; }
    }

    /// <summary>
    /// DTO para Checklist de Propiedad
    /// </summary>
    public class ChecklistPropiedadDto
    {
        public int ChecklistPropiedadId { get; set; }
        public int PropiedadId { get; set; }
        public bool FotosCompletas { get; set; }
        public bool DocumentacionCompleta { get; set; }
        public bool ServiciosVerificados { get; set; }
        public bool EspaciosVerificados { get; set; }
        public bool CondicionesEstructurales { get; set; }
        public bool DisponiblePublicar { get; set; }
        public DateTime? FechaDisponiblePublicar { get; set; }
        public string Comentarios { get; set; }
        public int PorcentajeAvance { get; set; }
    }

    /// <summary>
    /// DTO para Orden de Visita
    /// </summary>
    public class OrdenVisitaDto
    {
        public int OrdenVisitaId { get; set; }
        public int PropiedadId { get; set; }
        public string PropiedadDireccion { get; set; }
        public int ArrendatarioId { get; set; }
        public string ArrendatariNombre { get; set; }
        public int CorredorId { get; set; }
        public string CorredorNombre { get; set; }
        public DateTime FechaVisita { get; set; }
        public string Direccion { get; set; }
        public DateTime? HoraInicio { get; set; }
        public DateTime? HoraFin { get; set; }
        public bool Notificado { get; set; }
        public DateTime? FechaNotificacion { get; set; }
        public string DocumentoUrl { get; set; }
        public string Estado { get; set; }
        public string EstadoDescriptivo { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    /// <summary>
    /// DTO para crear Orden de Visita
    /// </summary>
    public class CreateOrdenVisitaRequest
    {
        public int PropiedadId { get; set; }
        public int ArrendatarioId { get; set; }
        public int CorredorId { get; set; }
        public DateTime FechaVisita { get; set; }
        public DateTime? HoraInicio { get; set; }
        public DateTime? HoraFin { get; set; }
        public string Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para Checklist de Arrendatario
    /// </summary>
    public class ChecklistArrendatarioDto
    {
        public int ChecklistArrendatarioId { get; set; }
        public int OrdenVisitaId { get; set; }
        public int ArrendatarioId { get; set; }
        public bool? PropiedadGusta { get; set; }
        public bool ConformidadEspacios { get; set; }
        public bool ConformidadServicios { get; set; }
        public bool PreguntasRespondidas { get; set; }
        public bool InteresContratar { get; set; }
        public DateTime? FechaCompleto { get; set; }
        public string Comentarios { get; set; }
        public bool TieneInteresContratar { get; set; }
    }

    /// <summary>
    /// DTO para crear Checklist de Arrendatario
    /// </summary>
    public class CreateChecklistArrendatarioRequest
    {
        public int OrdenVisitaId { get; set; }
        public int ArrendatarioId { get; set; }
        public bool? PropiedadGusta { get; set; }
        public bool ConformidadEspacios { get; set; }
        public bool ConformidadServicios { get; set; }
        public bool PreguntasRespondidas { get; set; }
        public bool InteresContratar { get; set; }
        public string Comentarios { get; set; }
    }

    /// <summary>
    /// DTO para Contrato de Arriendo
    /// </summary>
    public class ContratoArriendoDto
    {
        public int ContratoArriendoId { get; set; }
        public int PropiedadId { get; set; }
        public string PropiedadDireccion { get; set; }
        public int PropietarioId { get; set; }
        public string PropietarioNombre { get; set; }
        public int ArrendatarioId { get; set; }
        public string ArrendatarioNombre { get; set; }
        public int CorredorId { get; set; }
        public string CorredorNombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTermino { get; set; }
        public decimal MontoMensual { get; set; }
        public decimal? DepositoGarantia { get; set; }
        public int DiasPago { get; set; }
        public string Estado { get; set; }
        public string DocumentoUrl { get; set; }
        public DateTime? FechaFirma { get; set; }
        public string Comentarios { get; set; }
        public int VigenciaEnMeses { get; set; }
        public decimal IngresoTotal { get; set; }
        public bool EstaVigente { get; set; }
        public int DiasRestantes { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    /// <summary>
    /// DTO para crear Contrato de Arriendo
    /// </summary>
    public class CreateContratoArriendoRequest
    {
        public int PropiedadId { get; set; }
        public int PropietarioId { get; set; }
        public int ArrendatarioId { get; set; }
        public int CorredorId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTermino { get; set; }
        public decimal MontoMensual { get; set; }
        public decimal? DepositoGarantia { get; set; }
        public int DiasPago { get; set; } = 5;
        public string Comentarios { get; set; }
    }

    /// <summary>
    /// DTO para Matching de Cliente-Propiedad
    /// </summary>
    public class MatchingClientePropiedadDto
    {
        public int MatchingClientePropiedadId { get; set; }
        public int ArrendatarioId { get; set; }
        public string ArrendatarioNombre { get; set; }
        public int PropiedadId { get; set; }
        public string PropiedadDireccion { get; set; }
        public int PorcentajeCoincidencia { get; set; }
        public string CalidadMatch { get; set; }
        public decimal? RequisitosMaxNivelArriendo { get; set; }
        public int? RequistosHabitaciones { get; set; }
        public string RequisitosZona { get; set; }
        public bool? RequistosCondominio { get; set; }
        public bool? RequistosGaraje { get; set; }
        public bool EsNotificado { get; set; }
        public DateTime? FechaNotificacion { get; set; }
    }
}
