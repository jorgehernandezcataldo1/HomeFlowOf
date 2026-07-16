namespace HomeFlow.Shared.DTOs.Users
{
    /// <summary>
    /// DTO para información de Corredor
    /// </summary>
    public class CorredorDto
    {
        public int CorredorId { get; set; }
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Licencia { get; set; }
        public string FotoPerfilUrl { get; set; }
        public bool EsActivo { get; set; }
        public bool EsAdmin { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }

    /// <summary>
    /// DTO para crear/editar corredor
    /// </summary>
    public class CreateUpdateCorredorRequest
    {
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Licencia { get; set; }
        public string FotoPerfilUrl { get; set; }
    }
}
