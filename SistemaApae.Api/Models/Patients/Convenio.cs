using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace SistemaApae.Api.Models.Patients;

/// <summary>
/// Modelo de convênio CAS do sistema
/// </summary>
[Table("convenio_cas")]
public class ConvenioCas : BaseModel
{
    /// <summary>
    /// ID único do convênio CAS
    /// </summary>
    [Column("id_convenio_cas")]
    public Guid IdConvenioCas { get; set; }

    /// <summary>
    /// ID da secretaria prefeitura associada
    /// </summary>
    [Column("id_secretaria_prefeitura")]
    public Guid? IdSecretariaPrefeitura { get; set; }

    /// <summary>
    /// Nome do convênio
    /// </summary>
    [Required]
    [MaxLength(150)]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Status do convênio (ativo/inativo)
    /// </summary>
    [Column("status")]
    public bool Status { get; set; } = true;

    /// <summary>
    /// Indica se é um convênio CAS
    /// </summary>
    [Column("eh_cas")]
    public bool EhCas { get; set; } = true;

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

    // Navigation properties
    /// <summary>
    /// Secretaria prefeitura associada
    /// </summary>
    public Administrative.SecretariaPrefeitura? SecretariaPrefeitura { get; set; }

    /// <summary>
    /// Lista de assistidos associados
    /// </summary>
    public List<Assistido>? Assistidos { get; set; }
}
