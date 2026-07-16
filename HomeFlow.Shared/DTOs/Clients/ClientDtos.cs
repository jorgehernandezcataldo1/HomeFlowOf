namespace HomeFlow.Shared.DTOs.Clients
{
    /// <summary>
    /// DTO base para Cliente
    /// </summary>
    public class ClienteDto
    {
        public int ClienteId { get; set; }
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public int? EstadoCivilId { get; set; }
        public string EstadoCivilNombre { get; set; }
        public string Direccion { get; set; }
        public int? ComunaId { get; set; }
        public string ComunaNombre { get; set; }
        public string FotoCarnetUrl { get; set; }
        public string Notas { get; set; }
        public bool EsActivo { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }

    /// <summary>
    /// DTO para crear cliente
    /// </summary>
    public class CreateClienteRequest
    {
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public int? EstadoCivilId { get; set; }
        public string Direccion { get; set; }
        public int? ComunaId { get; set; }
        public string Notas { get; set; }
    }

    /// <summary>
    /// DTO para editar cliente
    /// </summary>
    public class UpdateClienteRequest : CreateClienteRequest
    {
        public int ClienteId { get; set; }
    }

    /// <summary>
    /// DTO para Propietario
    /// </summary>
    public class PropietarioDto
    {
        public int PropietarioId { get; set; }
        public ClienteDto Cliente { get; set; }
        public string BancoPreferido { get; set; }
        public string CuentaBancaria { get; set; }
        public string TipoCuenta { get; set; }
        public bool DocumentoIdentidadVerificado { get; set; }
        public DateTime? FechaVerificacion { get; set; }
        public bool RequisitosCompletos { get; set; }
        public DateTime? FechaRequisitosCompletos { get; set; }
        public int? CorredorAsignadoId { get; set; }
        public string CorredorAsignadoNombre { get; set; }
        public bool CumpleLosRequisitos { get; set; }

        public string NombreCompleto => Cliente?.NombreCompleto ?? "";
        public string Rut => Cliente?.Rut ?? "";
    }

    /// <summary>
    /// DTO para crear Propietario
    /// </summary>
    public class CreatePropietarioRequest
    {
        public ClienteDto Cliente { get; set; }
        public string BancoPreferido { get; set; }
        public string CuentaBancaria { get; set; }
        public string TipoCuenta { get; set; }
        public int? CorredorAsignadoId { get; set; }
    }

    /// <summary>
    /// DTO para Arrendatario
    /// </summary>
    public class ArrendatarioDto
    {
        public int ArrendatarioId { get; set; }
        public ClienteDto Cliente { get; set; }
        public decimal? LiquidoMensual { get; set; }
        public string Empleador { get; set; }
        public int? AntiguedadLaboral { get; set; }
        public bool TieneHijos { get; set; }
        public int NumeroHijos { get; set; }
        public bool TieneMascota { get; set; }
        public string TipoMascota { get; set; }
        public bool PreAprobacionCredito { get; set; }
        public bool DocumentacionCompleta { get; set; }
        public bool RequisitosCompletos { get; set; }
        public DateTime? FechaRequisitosCompletos { get; set; }
        public int? CorredorAsignadoId { get; set; }
        public string CorredorAsignadoNombre { get; set; }
        public decimal CapacidadArriendoTeorida { get; set; }
        public bool PuedeOptar { get; set; }

        public string NombreCompleto => Cliente?.NombreCompleto ?? "";
        public string Rut => Cliente?.Rut ?? "";
    }

    /// <summary>
    /// DTO para crear Arrendatario
    /// </summary>
    public class CreateArrendatarioRequest
    {
        public ClienteDto Cliente { get; set; }
        public decimal? LiquidoMensual { get; set; }
        public string Empleador { get; set; }
        public int? AntiguedadLaboral { get; set; }
        public bool TieneHijos { get; set; }
        public int NumeroHijos { get; set; }
        public bool TieneMascota { get; set; }
        public string TipoMascota { get; set; }
        public int? CorredorAsignadoId { get; set; }
    }
}
