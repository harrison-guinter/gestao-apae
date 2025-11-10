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
    /// Dados do Assistido associado
    /// </summary>
    public AssistidoAtendimentoDto Assistido { get; set; }

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

public class AssistidoAtendimentoDto
{
    public AssistidoAtendimentoDto(Guid id, string nome)
    {
        IdAssistido = id;
        Nome = nome;
    }

    /// <summary>
    /// ID do Assistido associado
    /// </summary>
    public Guid IdAssistido { get; set; }

    /// <summary>
    /// Nome do assistido
    /// </summary>
    public string? Nome { get; set; } = string.Empty;
}