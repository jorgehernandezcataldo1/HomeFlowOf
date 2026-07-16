namespace HomeFlow.Application.Exceptions;

/// <summary>
/// Excepción base para excepciones de la capa de aplicación
/// </summary>
public class ApplicationException : Exception
{
    public ApplicationException(string message) : base(message) { }

    public ApplicationException(string message, Exception innerException) 
        : base(message, innerException) { }
}

/// <summary>
/// Excepción cuando un recurso no es encontrado
/// </summary>
public class NotFoundException : ApplicationException
{
    public NotFoundException(string resourceName, object id) 
        : base($"El recurso '{resourceName}' con ID '{id}' no fue encontrado.") { }

    public NotFoundException(string message) : base(message) { }
}

/// <summary>
/// Excepción cuando sucede un error de validación
/// </summary>
public class ValidationException : ApplicationException
{
    public ValidationException(string message) : base(message) { }

    public ValidationException(Dictionary<string, string[]> errors) 
        : base(FormatErrors(errors))
    {
        Errors = errors;
    }

    public Dictionary<string, string[]>? Errors { get; set; }

    private static string FormatErrors(Dictionary<string, string[]> errors)
    {
        var message = "Se encontraron errores de validación:\n";
        foreach (var error in errors)
        {
            message += $"- {error.Key}: {string.Join(", ", error.Value)}\n";
        }
        return message;
    }
}

/// <summary>
/// Excepción cuando sucede un conflicto de negocio
/// </summary>
public class BusinessException : ApplicationException
{
    public BusinessException(string message) : base(message) { }
}
