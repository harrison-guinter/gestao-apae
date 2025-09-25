using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using SistemaApae.Api.Models.Enums;

namespace SistemaApae.Api.Models.Agenda;

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

    /// <summary>
    /// Indica se o atendimento está ativo/inativo
    /// </summary>
    [Column("status")]
    public StatusEntidadeEnum Status { get; set; }

    // Navigation properties
    /// <summary>
    /// Agendamento associado ao atendimento
    /// </summary>
    public Agendamento? Agendamento { get; set; }
}
