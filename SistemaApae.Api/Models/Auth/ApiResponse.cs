namespace SistemaApae.Api.Models.Auth;

/// <summary>
/// Modelo genérico para respostas da API
/// </summary>
/// <typeparam name="T">Tipo dos dados da resposta</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indica se a operação foi bem-sucedida
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Mensagem da resposta
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Dados da resposta
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Lista de erros (se houver)
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// Timestamp da resposta
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Cria uma resposta de sucesso
    /// </summary>
    /// <param name="data">Dados da resposta</param>
    /// <param name="message">Mensagem de sucesso</param>
    /// <returns>Resposta de sucesso</returns>
    public static ApiResponse<T> SuccessResponse(T data, string message = "Operação realizada com sucesso")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// Cria uma resposta de erro
    /// </summary>
    /// <param name="message">Mensagem de erro</param>
    /// <param name="errors">Lista de erros específicos</param>
    /// <returns>Resposta de erro</returns>
    public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}
