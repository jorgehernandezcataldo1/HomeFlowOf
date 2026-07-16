namespace HomeFlow.Shared.DTOs.References
{
    /// <summary>
    /// DTO para Estado de Propiedad
    /// </summary>
    public class EstadoPropiedadDto
    {
        public int EstadoPropiedadId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool EsActivo { get; set; }
    }

    /// <summary>
    /// DTO para Categoría de Propiedad
    /// </summary>
    public class CategoriaPropiedadDto
    {
        public int CategoriaPropiedadId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool EsActivo { get; set; }
    }

    /// <summary>
    /// DTO para Tipo de Propiedad
    /// </summary>
    public class TipoPropiedadDto
    {
        public int TipoPropiedadId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool EsActivo { get; set; }
    }

    /// <summary>
    /// DTO para Estado Civil
    /// </summary>
    public class EstadoCivilDto
    {
        public int EstadoCivilId { get; set; }
        public string Nombre { get; set; }
        public bool EsActivo { get; set; }
    }

    /// <summary>
    /// DTO para Región
    /// </summary>
    public class RegionDto
    {
        public int RegionId { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public bool EsActivo { get; set; }
    }

    /// <summary>
    /// DTO para Comuna
    /// </summary>
    public class ComunaDto
    {
        public int ComunaId { get; set; }
        public int RegionId { get; set; }
        public string RegionNombre { get; set; }
        public string Nombre { get; set; }
        public bool EsActivo { get; set; }
    }

    /// <summary>
    /// DTO para Tipo de Documento
    /// </summary>
    public class TipoDocumentoDto
    {
        public int TipoDocumentoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string PlantillaHtml { get; set; }
        public bool EsActivo { get; set; }
    }
}
