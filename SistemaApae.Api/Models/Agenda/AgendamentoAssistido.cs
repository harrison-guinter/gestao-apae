using Supabase.Postgrest.Attributes;
using System.ComponentModel.DataAnnotations;
using SistemaApae.Api.Models.Patients;

namespace SistemaApae.Api.Models.Agenda;

/// <summary>
/// Modelo de relacionamento entre agendamento e assistido (tabela de junção)
/// </summary>
[Table("agendamento_assistido")]
public class AgendamentoAssistido : ApiBaseModel
{

    /// <summary>
    /// ID do agendamento
    /// </summary>
    [Required]
    [Column("id_agendamento")]
    public Guid IdAgendamento { get; set; }

    /// <summary>
    /// ID do assistido
    /// </summary>
    [Required]
    [Column("id_assistido")]
    public Guid IdAssistido { get; set; }

    /// <summary>
    /// Navegação do agendamento (embed via PostgREST)
    /// </summary>
    [Reference(typeof(Agendamento), includeInQuery: true)]
    public Agendamento? Agendamento { get; set; }

    /// <summary>
    /// Navegação do assistido (embed via PostgREST)
    /// </summary>
    [Reference(typeof(Assistido), includeInQuery: true)]
    public Assistido? Assistido { get; set; }

}
