using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Filters;

namespace SistemaApae.Api.Models.Appointment;

/// <summary>
/// Modelo para requisição de atendimento por filtros de pesquisa
/// </summary>
public class AtendimentoFilterRequest : BaseFilter
{
    /// <summary>
    /// Lista de IDs de agendamentos para busca em lote
    /// </summary>
    public List<Guid>? IdsAgendamento { get; set; }

    /// <summary>
    /// ID do agendamento
    /// </summary>
    public Guid IdAgendamento { get; set; } = Guid.Empty;

    /// <summary>
    /// ID do assistido
    /// </summary>
    public Guid IdAssistido { get; set; } = Guid.Empty;

    /// <summary>
    /// Data início do atendimento
    /// </summary>
    public DateTime? DataInicioAtendimento { get; set; }

    /// <summary>
    /// Data fim do atendimento
    /// </summary>
    public DateTime? DataFimAtendimento { get; set; }

    /// <summary>
    /// Status do atendimento (presença/falta/justificada)
    /// </summary>
    public StatusAtendimentoEnum? Presenca { get; set; }

    /// <summary>
    /// Lista de status de presença para filtro (permite múltiplos valores)
    /// </summary>
    public List<StatusAtendimentoEnum>? Presencas { get; set; }

    /// <summary>
    /// ID do profissional (filtra via relacionamento do agendamento)
    /// </summary>
    public Guid IdProfissional { get; set; } = Guid.Empty;

    /// <summary>
    /// ID do município (filtra via relacionamento do assistido)
    /// </summary>
    public Guid IdMunicipio { get; set; } = Guid.Empty;

    /// <summary>
    /// ID do Convênio (filtra via relacionamento do assistido)
    /// </summary>
    public Guid IdConvenio { get; set; } = Guid.Empty;
}