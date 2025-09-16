using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace SistemaApae.Api.Models.Administrative;

/// <summary>
/// Modelo de prefeitura do sistema
/// </summary>
[Table("prefeitura")]
public class Prefeitura : BaseModel
{
    /// <summary>
    /// ID único da prefeitura
    /// </summary>
    [Column("id_prefeitura")]
    public Guid IdPrefeitura { get; set; }

    /// <summary>
    /// Nome da prefeitura
    /// </summary>
    [Required]
    [MaxLength(150)]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Status da prefeitura (ativo/inativo)
    /// </summary>
    [Column("status")]
    public bool Status { get; set; } = true;

    /// <summary>
    /// Telefone da prefeitura
    /// </summary>
    [MaxLength(20)]
    [Column("telefone")]
    public string? Telefone { get; set; }

    /// <summary>
    /// CEP da prefeitura
    /// </summary>
    [MaxLength(8)]
    [Column("cep")]
    public string? Cep { get; set; }

    /// <summary>
    /// Observações sobre a prefeitura
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
    /// Lista de secretarias associadas à prefeitura
    /// </summary>
    public List<SecretariaPrefeitura>? SecretariaPrefeituras { get; set; }
}
