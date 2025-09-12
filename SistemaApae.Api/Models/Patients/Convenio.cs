using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace SistemaApae.Api.Models.Patients;

/// <summary>
/// Modelo de convênio do sistema
/// </summary>
[Table("convenio")]
public class Convenio : BaseModel
{
    /// <summary>
    /// ID único do convênio
    /// </summary>
    [Column("id_convenio")]
    public Guid IdConvenio { get; set; }

    /// <summary>
    /// Nome do convênio
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Data de início do convênio
    /// </summary>
    [Column("data_inicio")]
    public DateTime? DataInicio { get; set; }

    /// <summary>
    /// Data de fim do convênio
    /// </summary>
    [Column("data_fim")]
    public DateTime? DataFim { get; set; }

    /// <summary>
    /// Status do convênio (ativo/inativo)
    /// </summary>
    [Column("status")]
    public bool Status { get; set; } = true;

    /// <summary>
    /// Cidade do convênio
    /// </summary>
    [MaxLength(50)]
    [Column("cidade")]
    public string? Cidade { get; set; }

    /// <summary>
    /// Observações sobre o convênio
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
}
