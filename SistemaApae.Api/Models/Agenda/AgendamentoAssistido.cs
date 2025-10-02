using Supabase.Postgrest.Attributes;
using System.ComponentModel.DataAnnotations;

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

}
