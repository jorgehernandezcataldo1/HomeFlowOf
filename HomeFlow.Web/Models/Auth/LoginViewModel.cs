using System.ComponentModel.DataAnnotations;

namespace HomeFlow.Web.Models.Auth;

/// <summary>
/// Modelo de la vista de inicio de sesión. Solo valida el formato de lo
/// que la persona escribió (correo, largo mínimo, etc.). Las reglas de
/// negocio reales -si el usuario existe, si la contraseña coincide, si
/// está bloqueado, etc.- se validan en HomeFlow.Application.Services.AuthService,
/// nunca aquí ni en el cliente.
/// </summary>
public class LoginViewModel
{
    [Required(ErrorMessage = "Ingresa tu correo electrónico.")]
    [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
    [Display(Name = "Correo electrónico")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa tu contraseña.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Mantener sesión iniciada")]
    public bool RecordarMe { get; set; }
}
