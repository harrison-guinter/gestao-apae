using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Users;
using Supabase.Postgrest.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SistemaApae.Api.Models.Agenda;

/// <summary>
/// Modelo de agendamento do sistema
/// </summary>
[Table("agendamento")]
public class Agendamento : ApiBaseModel
{

    /// <summary>
    /// ID do profissional responsável
    /// </summary>
    [Required]
    [Column("id_profissional")]
    public Guid IdProfissional { get; set; }

    /// <summary>
    /// Navegação do profissional (embed via PostgREST)
    /// </summary>
    [Reference(typeof(Usuario), includeInQuery: true)]
    public Usuario? Profissional { get; set; }

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
    /// Dia da semana do agendamento
    /// </summary>
    [Column("dia_semana")]
    public DiaSemanaEnum? DiaSemana { get; set; }

    /// <summary>
    /// Observações sobre o agendamento
    /// </summary>
    [Column("observacao")]
    public string? Observacao { get; set; }

}
