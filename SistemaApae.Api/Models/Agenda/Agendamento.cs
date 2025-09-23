using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Users;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace SistemaApae.Api.Models.Agenda;

/// <summary>
/// Modelo de agendamento do sistema
/// </summary>
[Table("agendamento")]
public class Agendamento : BaseModel
{
    /// <summary>
    /// ID único do agendamento
    /// </summary>
    [Column("id_agendamento")]
    public Guid IdAgendamento { get; set; }

    /// <summary>
    /// ID do profissional responsável
    /// </summary>
    [Required]
    [Column("id_profissional")]
    public Guid IdProfissional { get; set; }

    /// <summary>
    /// Tipo de recorrência do agendamento
    /// </summary>
    [Column("tipo_recorrencia")]
    public TipoRecorrenciaEnum? TipoRecorrencia { get; set; }

    /// <summary>
    /// Horário do agendamento
    /// </summary>
    [Column("horario_agendamento")]
    public TimeOnly? HorarioAgendamento { get; set; }

    /// <summary>
    /// Data do agendamento
    /// </summary>
    [Column("data_agendamento")]
    public DateOnly? DataAgendamento { get; set; }

    /// <summary>
    /// Observações sobre o agendamento
    /// </summary>
    [Column("observacao")]
    public string? Observacao { get; set; }

    /// <summary>
    /// Indica se o agendamento está ativo/inativo
    /// </summary>
    [Column("status")]
    public bool Status { get; set; } = true;

    /// <summary>
    /// Lista de assistidos associados ao agendamento
    /// </summary>
    public List<AgendamentoAssistido>? AgendamentoAssistidos { get; set; }

    /// <summary>
    /// Lista de atendimentos realizados
    /// </summary>
    public List<Atendimento>? Atendimentos { get; set; }
}
