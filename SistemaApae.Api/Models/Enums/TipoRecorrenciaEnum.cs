namespace SistemaApae.Api.Models.Enums;

/// <summary>
/// Enum para tipo de recorrência de agendamento
/// </summary>
public enum TipoRecorrenciaEnum
{
    /// <summary>
    /// Nenhuma recorrência
    /// </summary>
    NENHUM = 0,
    
    /// <summary>
    /// Recorrência diária
    /// </summary>
    DIARIA = 1,
    
    /// <summary>
    /// Recorrência semanal
    /// </summary>
    SEMANAL = 2,
    
    /// <summary>
    /// Recorrência mensal
    /// </summary>
    MENSAL = 3
}
