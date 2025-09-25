namespace SistemaApae.Api.Models.Patients;

/// <summary>
/// Extensões para conversão entre Assistido e AssistidoDto
/// </summary>
public static class AssistidoExtensions
{
    /// <summary>
    /// Converte Assistido para AssistidoDto
    /// </summary>
    /// <param name="assistido">Modelo Assistido</param>
    /// <returns>DTO UsuarioDto</returns>
    public static AssistidoDto ToDto(this Assistido assistido)
    {
        return new AssistidoDto
        {
            IdUsuario = assistido.IdAssistido,
            Nome = assistido.Nome
        };
    }

    /// <summary>
    /// Converte lista de Assistido para lista de AssistidoDto
    /// </summary>
    /// <param name="assistidos">Lista de Assistido</param>
    /// <returns>Lista de AssistidoDto</returns>
    public static IEnumerable<AssistidoDto> ToDto(this IEnumerable<Assistido> assistidos)
    {
        return assistidos.Select(u => u.ToDto());
    }
}
