using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace SistemaApae.Api.Models.Administrative;

/// <summary>
/// Modelo de município do sistema
/// </summary>
[Table("municipio")]
public class Municipio : BaseModel
{
    /// <summary>
    /// ID único do município
    /// </summary>
    [Column("id_municipio")]
    public Guid IdMunicipio { get; set; }

    /// <summary>
    /// Nome do município
    /// </summary>
    [Required]
    [MaxLength(150)]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// CEP inicial do município
    /// </summary>
    [MaxLength(8)]
    [Column("cep_inicio")]
    public string? CepInicio { get; set; }

    /// <summary>
    /// CEP final do município
    /// </summary>
    [MaxLength(8)]
    [Column("cep_fim")]
    public string? CepFim { get; set; }

    /// <summary>
    /// UF do município
    /// </summary>
    [Required]
    [MaxLength(2)]
    [Column("uf")]
    public string Uf { get; set; } = string.Empty;
}
