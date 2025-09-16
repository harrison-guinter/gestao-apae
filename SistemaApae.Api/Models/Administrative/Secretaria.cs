using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace SistemaApae.Api.Models.Administrative;

/// <summary>
/// Modelo de secretaria do sistema
/// </summary>
[Table("secretaria")]
public class Secretaria : BaseModel
{
    /// <summary>
    /// ID único da secretaria
    /// </summary>
    [Column("id_secretaria")]
    public Guid IdSecretaria { get; set; }

    /// <summary>
    /// Nome da secretaria
    /// </summary>
    [Required]
    [MaxLength(150)]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Status da secretaria (ativo/inativo)
    /// </summary>
    [Column("status")]
    public bool Status { get; set; } = true;

    /// <summary>
    /// Observações sobre a secretaria
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
    /// Lista de prefeituras associadas à secretaria
    /// </summary>
    public List<SecretariaPrefeitura>? SecretariaPrefeituras { get; set; }
}
