using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Filters;

namespace SistemaApae.Api.Models.Appointment;

/// <summary>
/// Modelo para requisição de atendimento por filtros de pesquisa
/// </summary>
public class AtendimentoFiltroRequest : BaseFilter
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
    public StatusAtendimentoEnum? Status { get; set; }
}