namespace HomeFlow.Domain.Entities.Audit
{
    using Users;

    /// <summary>
    /// Entidad Log de Auditoría - Registro de cambios en el sistema
    /// </summary>
    public class LogAuditoria : EntityBase
    {
        public int? CorredorId { get; set; }
        public string Accion { get; set; } // CREATE, UPDATE, DELETE
        public string Tabla { get; set; }
        public int? RegistroId { get; set; }
        public string ValoresAnteriores { get; set; } // JSON
        public string ValoresNuevos { get; set; } // JSON
        public string DireccionIP { get; set; }

        // Relaciones
        public Corredor? Corredor { get; set; }

        /// <summary>
        /// Obtiene la descripción de la acción
        /// </summary>
        public string ObtenerDescripcionAccion()
        {
            return Accion switch
            {
                "CREATE" => "Creación",
                "UPDATE" => "Actualización",
                "DELETE" => "Eliminación",
                _ => "Modificación"
            };
        }
    }

    /// <summary>
    /// Clase base para todas las entidades
    /// </summary>
    public abstract class EntityBase
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;
    }
}
