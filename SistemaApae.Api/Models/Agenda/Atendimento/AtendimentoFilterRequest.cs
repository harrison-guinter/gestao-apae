using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Filters;

namespace SistemaApae.Api.Models.Agenda.Atendimento;

/// <summary>
/// Modelo para requisição de Atendimento por filtros de pesquisa
/// </summary>
public class AtendimentoFilterRequest : BaseFilter
{
    /// <summary>
    /// ID do agendamento
    /// </summary>
    public Guid IdAgendamento { get; set; } = Guid.Empty;

    /// <summary>
    /// ID do assistido
    /// </summary>
    public Guid IdAssistido { get; set; } = Guid.Empty;

    /// <summary>
    /// Data inicial do atendimento
    /// </summary>
    public DateTime? DataAtendimentoInicio { get; set; }

    /// <summary>
    /// Data final do atendimento
    /// </summary>
    public DateTime? DataAtendimentoFim { get; set; }

    /// <summary>
    /// Status do atendimento (presença)
    /// </summary>
    public StatusAtendimentoEnum? Presenca { get; set; }
}

