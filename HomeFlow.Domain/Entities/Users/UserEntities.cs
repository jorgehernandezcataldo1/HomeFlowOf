namespace HomeFlow.Domain.Entities.Users
{
    using References;
    using Properties;

    /// <summary>
    /// Entidad Corredor - Usuario del sistema (gestor de propiedades)
    /// </summary>
    public class Corredor : EntityBase
    {
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string Licencia { get; set; }
        public string FotoPerfilUrl { get; set; }
        public bool EsActivo { get; set; } = true;
        public bool EsAdmin { get; set; } = false;

        // Relaciones
        public ICollection<Propietario> PropietariosAsignados { get; set; } = new List<Propietario>();
        public ICollection<Arrendatario> ArrendatariosAsignados { get; set; } = new List<Arrendatario>();
        public ICollection<Propiedad> PropiedadesAsignadas { get; set; } = new List<Propiedad>();
        public ICollection<OrdenVisita> OrdenesVisita { get; set; } = new List<OrdenVisita>();
        public ICollection<ContratoArriendo> Contratos { get; set; } = new List<ContratoArriendo>();
        public ICollection<LogAuditoria> LogsAuditoria { get; set; } = new List<LogAuditoria>();

        /// <summary>
        /// Obtiene nombre completo del corredor
        /// </summary>
        public string NombreCompleto => $"{Nombre} {Apellido}";
    }

    /// <summary>
    /// Entidad Cliente base - Representa propietarios y arrendatarios
    /// </summary>
    public class Cliente : EntityBase
    {
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public int? EstadoCivilId { get; set; }
        public string Direccion { get; set; }
        public int? ComunaId { get; set; }
        public string FotoCarnetUrl { get; set; }
        public string Notas { get; set; }
        public bool EsActivo { get; set; } = true;

        // Relaciones
        public EstadoCivil? EstadoCivil { get; set; }
        public Comuna? Comuna { get; set; }
        public Propietario? Propietario { get; set; }
        public Arrendatario? Arrendatario { get; set; }

        /// <summary>
        /// Obtiene nombre completo del cliente
        /// </summary>
        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}
