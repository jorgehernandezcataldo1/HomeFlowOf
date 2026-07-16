namespace HomeFlow.Domain.Entities.References
{
    /// <summary>
    /// Entidad base para todas las entidades del dominio
    /// </summary>
    public abstract class EntityBase
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }

    /// <summary>
    /// Catálogo de Estados de Propiedad
    /// </summary>
    public class EstadoPropiedad : EntityBase
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool EsActivo { get; set; } = true;

        // Relaciones
        public ICollection<Propiedad> Propiedades { get; set; } = new List<Propiedad>();
    }

    /// <summary>
    /// Catálogo de Categorías de Propiedad (Departamento, Casa, Oficina, etc.)
    /// </summary>
    public class CategoriaPropiedad : EntityBase
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool EsActivo { get; set; } = true;

        // Relaciones
        public ICollection<Propiedad> Propiedades { get; set; } = new List<Propiedad>();
    }

    /// <summary>
    /// Catálogo de Tipos de Propiedad (Residencial, Comercial, Industrial, Mixto)
    /// </summary>
    public class TipoPropiedad : EntityBase
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool EsActivo { get; set; } = true;

        // Relaciones
        public ICollection<Propiedad> Propiedades { get; set; } = new List<Propiedad>();
    }

    /// <summary>
    /// Catálogo de Estados Civiles
    /// </summary>
    public class EstadoCivil : EntityBase
    {
        public string Nombre { get; set; }
        public bool EsActivo { get; set; } = true;

        // Relaciones
        public ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    }

    /// <summary>
    /// Catálogo de Tipos de Documento
    /// </summary>
    public class TipoDocumento : EntityBase
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string PlantillaHtml { get; set; }
        public bool EsActivo { get; set; } = true;
    }

    /// <summary>
    /// Entidad Región (División territorial de Chile)
    /// </summary>
    public class Region : EntityBase
    {
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public bool EsActivo { get; set; } = true;

        // Relaciones
        public ICollection<Comuna> Comunas { get; set; } = new List<Comuna>();
    }

    /// <summary>
    /// Entidad Comuna (División menor dentro de Región)
    /// </summary>
    public class Comuna : EntityBase
    {
        public int RegionId { get; set; }
        public string Nombre { get; set; }
        public bool EsActivo { get; set; } = true;

        // Relaciones
        public Region Region { get; set; }
        public ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
        public ICollection<Propiedad> Propiedades { get; set; } = new List<Propiedad>();
    }
}
