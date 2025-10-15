using SistemaApae.Api.Models.Enums;
using Supabase.Postgrest.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SistemaApae.Api.Models.Agenda.Atendimento;

/// <summary>
/// Modelo de atendimento do sistema
/// </summary>
[Table("atendimento")]
public class Atendimento : ApiBaseModel
{

    /// <summary>
    /// ID do agendamento associado
    /// </summary>
    [Required]
    [Column("id_agendamento")]
    public Guid IdAgendamento { get; set; }

    /// <summary>
    /// ID do Assistido associado
    /// </summary>
    [Required]
    [Column("id_assistido")]
    public Guid IdAssistido { get; set; }

    /// <summary>
    /// Data e hora do atendimento
    /// </summary>
    [Column("data_atendimento")]
    public DateTime? DataAtendimento { get; set; }

    /// <summary>
    /// Status do atendimento
    /// </summary>
    [Column("presenca")]
    public StatusAtendimentoEnum? Presenca { get; set; }

    /// <summary>
    /// Avaliação do atendimento
    /// </summary>
    [Column("avaliacao")]
    public string? Avaliacao { get; set; }

    /// <summary>
    /// Observações sobre o atendimento
    /// </summary>
    [Column("observacao")]
    public string? Observacao { get; set; }

}
