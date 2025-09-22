using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using SistemaApae.Api.Models.Patients;

namespace SistemaApae.Api.Models.Agenda;

/// <summary>
/// Modelo de relacionamento entre agendamento e assistido (tabela de junção)
/// </summary>
[Table("agendamento_assistido")]
public class AgendamentoAssistido : BaseModel
{
    /// <summary>
    /// ID único do relacionamento
    /// </summary>
    [Column("id_agendamento_assistido")]
    public Guid IdAgendamentoAssistido { get; set; }

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

    // Navigation properties
    /// <summary>
    /// Agendamento associado
    /// </summary>
    public Agendamento? Agendamento { get; set; }

    /// <summary>
    /// Assistido associado
    /// </summary>
    public Assistido? Assistido { get; set; }
}
