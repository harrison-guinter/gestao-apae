using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Filters;

namespace SistemaApae.Api.Models.Agenda;

/// <summary>
/// Filtros para busca de relacionamentos AgendamentoAssistido
/// </summary>
public class AgendamentoAssistidoFilterRequest : BaseFilter
{
    /// <summary>
    /// ID do agendamento para filtrar
    /// </summary>
    public Guid IdAgendamento { get; set; } = Guid.Empty;

    /// <summary>
    /// ID do assistido para filtrar
    /// </summary>
    public Guid IdAssistido { get; set; } = Guid.Empty;

    /// <summary>
    /// Status do relacionamento (Ativo/Inativo)
    /// </summary>
    public StatusEntidadeEnum? Status { get; set; } = StatusEntidadeEnum.Ativo;
}
