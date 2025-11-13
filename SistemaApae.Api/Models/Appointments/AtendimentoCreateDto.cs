using SistemaApae.Api.Models.Agenda;
using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Patients;
using System.ComponentModel.DataAnnotations;

namespace SistemaApae.Api.Models.Appointments;
public class AtendimentoCreateDto
{
    /// <summary>
    /// Agendamento 
    /// </summary>
    [Required(ErrorMessage = "Agendamento é obrigatório")]
    public AgendamentoDto Agendamento { get; set; }

    /// <summary>
    /// Assistido
    /// </summary>
    [Required(ErrorMessage = "Assistido é obrigatório")]
    public AssistidoDto Assistido { get; set; }

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
