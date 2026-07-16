namespace HomeFlow.Application.DTOs;

/// <summary>
/// DTO para respuestas genéricas de la API
/// </summary>
/// <typeparam name="T">Tipo de datos en la respuesta</typeparam>
public class ApiResponse<T>
{
    public bool Exitoso { get; set; }

    public string? Mensaje { get; set; }

    public T? Datos { get; set; }

    public List<string>? Errores { get; set; }

    public static ApiResponse<T> Success(T dato, string mensaje = "Operación exitosa")
    {
        return new ApiResponse<T>
        {
            Exitoso = true,
            Mensaje = mensaje,
            Datos = dato
        };
    }

    public static ApiResponse<T> Error(string mensaje, List<string>? errores = null)
    {
        return new ApiResponse<T>
        {
            Exitoso = false,
            Mensaje = mensaje,
            Errores = errores ?? new List<string>()
        };
    }
}

/// <summary>
/// DTO para respuestas paginadas
/// </summary>
/// <typeparam name="T">Tipo de datos en la respuesta</typeparam>
public class ApiPaginatedResponse<T>
{
    public bool Exitoso { get; set; }

    public string? Mensaje { get; set; }

    public List<T> Datos { get; set; } = new List<T>();

    public int TotalRegistros { get; set; }

    public int Pagina { get; set; }

    public int TamanoPagina { get; set; }

    public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / TamanoPagina);

    public List<string>? Errores { get; set; }

    public static ApiPaginatedResponse<T> Success(List<T> datos, int totalRegistros, int pagina, int tamanoPagina, string mensaje = "Operación exitosa")
    {
        return new ApiPaginatedResponse<T>
        {
            Exitoso = true,
            Mensaje = mensaje,
            Datos = datos,
            TotalRegistros = totalRegistros,
            Pagina = pagina,
            TamanoPagina = tamanoPagina
        };
    }

    public static ApiPaginatedResponse<T> Error(string mensaje, List<string>? errores = null)
    {
        return new ApiPaginatedResponse<T>
        {
            Exitoso = false,
            Mensaje = mensaje,
            Errores = errores ?? new List<string>()
        };
    }
}
