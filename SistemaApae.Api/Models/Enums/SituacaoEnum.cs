namespace SistemaApae.Api.Models.Enums;

/// <summary>
/// Enum para situação do assistido
/// NOTA: Valores precisam ser definidos conforme regras de negócio
/// </summary>
public enum SituacaoEnum
{
    /// <summary>
    /// Ativo - assistido em atendimento
    /// </summary>
    ATIVO = 1,
    
    /// <summary>
    /// Inativo - assistido sem atendimento
    /// </summary>
    INATIVO = 2,
    
    /// <summary>
    /// Aguardando - na fila de espera
    /// </summary>
    AGUARDANDO = 3,
    
    /// <summary>
    /// Suspenso - temporariamente sem atendimento
    /// </summary>
    SUSPENSO = 4
}
