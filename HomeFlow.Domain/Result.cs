namespace HomeFlow.Domain
{
    /// <summary>
    /// Clase de resultado para operaciones de negocio
    /// Implementa el patrón Result para enviar respuestas JSON
    /// </summary>
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public Result()
        {
            Errors = new Dictionary<string, string>();
        }

        public static Result Ok(string message = "Operación completada exitosamente", object data = null)
        {
            return new Result 
            { 
                Success = true, 
                Message = message, 
                Data = data 
            };
        }

        public static Result Fail(string message = "Error en la operación")
        {
            return new Result 
            { 
                Success = false, 
                Message = message 
            };
        }

        public static Result ValidationError(Dictionary<string, string> errors)
        {
            return new Result 
            { 
                Success = false, 
                Message = "Errores de validación", 
                Errors = errors 
            };
        }
    }

    /// <summary>
    /// Clase de resultado genérica para operaciones con datos
    /// </summary>
    /// <typeparam name="T">Tipo de dato a retornar</typeparam>
    public class Result<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public Result()
        {
            Errors = new Dictionary<string, string>();
        }

        public static Result<T> Ok(T data, string message = "Operación completada exitosamente")
        {
            return new Result<T> 
            { 
                Success = true, 
                Message = message, 
                Data = data 
            };
        }

        public static Result<T> Fail(string message = "Error en la operación")
        {
            return new Result<T> 
            { 
                Success = false, 
                Message = message 
            };
        }

        public static Result<T> ValidationError(Dictionary<string, string> errors)
        {
            return new Result<T> 
            { 
                Success = false, 
                Message = "Errores de validación", 
                Errors = errors 
            };
        }
    }
}
