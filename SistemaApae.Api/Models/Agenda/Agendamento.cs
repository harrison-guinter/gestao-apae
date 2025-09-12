using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Users;
using SistemaApae.Api.Models.Patients;

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
    /// Indica se o agendamento está ativo
    /// </summary>
    [Column("ativo")]
    public bool Ativo { get; set; } = true;

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
    /// Profissional responsável pelo agendamento
    /// </summary>
    public Profissional? Profissional { get; set; }

    /// <summary>
    /// Lista de assistidos associados ao agendamento
    /// </summary>
    public List<AgendamentoAssistido>? AgendamentoAssistidos { get; set; }

    /// <summary>
    /// Lista de atendimentos realizados
    /// </summary>
    public List<Atendimento>? Atendimentos { get; set; }
}
