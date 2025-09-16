using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace SistemaApae.Api.Models.Administrative;

/// <summary>
/// Modelo de relacionamento entre secretaria e prefeitura (tabela de junção)
/// </summary>
[Table("secretaria_prefeitura")]
public class SecretariaPrefeitura : BaseModel
{
    /// <summary>
    /// ID único do relacionamento
    /// </summary>
    [Column("id_secretaria_prefeitura")]
    public Guid IdSecretariaPrefeitura { get; set; }

    /// <summary>
    /// ID da secretaria
    /// </summary>
    [Required]
    [Column("id_secretaria")]
    public Guid IdSecretaria { get; set; }

    /// <summary>
    /// ID da prefeitura
    /// </summary>
    [Required]
    [Column("id_prefeitura")]
    public Guid IdPrefeitura { get; set; }

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
    /// Secretaria associada
    /// </summary>
    public Secretaria? Secretaria { get; set; }

    /// <summary>
    /// Prefeitura associada
    /// </summary>
    public Prefeitura? Prefeitura { get; set; }

    /// <summary>
    /// Lista de convênios CAS associados
    /// </summary>
    public List<Patients.ConvenioCas>? ConveniosCas { get; set; }
}
