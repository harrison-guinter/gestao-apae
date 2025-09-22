using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Administrative;

namespace SistemaApae.Api.Models.Patients;

/// <summary>
/// Modelo de Convênio do sistema
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
    /// ID do município associado
    /// </summary>
    [Column("id_municipio")]
    public Guid? IdMunicipio { get; set; }

    /// <summary>
    /// Nome do convênio
    /// </summary>
    [Required]
    [MaxLength(150)]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Indica se o convênio está ativo
    /// </summary>
    [Column("ativo")]
    public bool Ativo { get; set; } = true;

    /// <summary>
    /// Observações sobre o convênio
    /// </summary>
    [Column("observacao")]
    public string? Observacao { get; set; }

    /// <summary>
    /// Tipo do convênio CAS
    /// </summary>
    [Column("tipo_convenio")]
    public TipoConvenioEnum? TipoConvenio { get; set; }

    // Navigation properties
    /// <summary>
    /// Município associado
    /// </summary>
    public Municipio? Municipio { get; set; }

    /// <summary>
    /// Lista de assistidos associados
    /// </summary>
    public List<Assistido>? Assistidos { get; set; }
}
