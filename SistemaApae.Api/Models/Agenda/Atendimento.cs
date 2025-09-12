using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using SistemaApae.Api.Models.Enums;

namespace SistemaApae.Api.Models.Agenda;

/// <summary>
/// Modelo de atendimento do sistema
/// </summary>
[Table("atendimento")]
public class Atendimento : BaseModel
{
    /// <summary>
    /// ID único do atendimento
    /// </summary>
    [Column("id_atendimento")]
    public Guid IdAtendimento { get; set; }

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
    [Column("status")]
    public StatusAtendimentoEnum? Status { get; set; }

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
    /// Data de criação
    /// </summary>
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Data da última atualização
    /// </summary>
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    /// <summary>
    /// Agendamento associado ao atendimento
    /// </summary>
    public Agendamento? Agendamento { get; set; }
}
