using SistemaApae.Api.Models.Enums;

namespace SistemaApae.Api.Models.Appointment;

public class AtendimentoDto
{

    /// <summary>
    /// ID do atendimento
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// ID do agendamento associado
    /// </summary>
    public Guid IdAgendamento { get; set; }

    /// <summary>
    /// ID do Assistido associado
    /// </summary>
    public Guid IdAssistido { get; set; }

    /// <summary>
    /// Data e hora do atendimento
    /// </summary>
    public DateTime? DataAtendimento { get; set; }

    /// <summary>
    /// Status do atendimento
    /// </summary>
    public StatusAtendimentoEnum? Presenca { get; set; }

    /// <summary>
    /// Avaliação do atendimento
    /// </summary>
    public string? Avaliacao { get; set; }

    /// <summary>
    /// Observações sobre o atendimento
    /// </summary>
    public string? Observacao { get; set; }

}
