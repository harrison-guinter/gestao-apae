namespace SistemaApae.Api.Models.Enums;

/// <summary>
/// Enum para status do atendimento
/// </summary>
public enum StatusAtendimentoEnum
{
    /// <summary>
    /// Presença confirmada
    /// </summary>
    PRESENCA = 1,
    
    /// <summary>
    /// Falta não justificada
    /// </summary>
    FALTA = 2,
    
    /// <summary>
    /// Falta justificada
    /// </summary>
    JUSTIFICADA = 3
}
