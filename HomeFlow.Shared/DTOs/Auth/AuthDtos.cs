namespace HomeFlow.Shared.DTOs.Auth
{
    /// <summary>
    /// DTO para login de corredor
    /// </summary>
    public class LoginRequest
    {
        public string Correo { get; set; }
        public string Password { get; set; }
        public bool RecordarMe { get; set; }
    }

    /// <summary>
    /// DTO para respuesta de login
    /// </summary>
    public class LoginResponse
    {
        public int CorredorId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Token { get; set; } // Para future JWT implementation
        public bool EsAdmin { get; set; }
    }

    /// <summary>
    /// DTO para cambio de contraseña
    /// </summary>
    public class ChangePasswordRequest
    {
        public string PasswordActual { get; set; }
        public string PasswordNueva { get; set; }
        public string ConfirmarPassword { get; set; }
    }

    /// <summary>
    /// DTO para registro de corredor
    /// </summary>
    public class RegisterCorredorRequest
    {
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Licencia { get; set; }
        public string Password { get; set; }
        public string ConfirmarPassword { get; set; }
    }
}
