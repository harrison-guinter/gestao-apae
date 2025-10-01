using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SistemaApae.Api.Models.Administrative;
using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Patients;
using Supabase.Postgrest.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SistemaApae.Api.Models.Agreements;

/// <summary>
/// Modelo de Convênio do sistema
/// </summary>
[Table("convenio")]
public class Convenio : ApiBaseModel
{
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
    public string Nome { get; set; }

    /// <summary>
    /// Indica se o convênio está ativo/inativo
    /// </summary>
    [Column("status")]
    public StatusEntidadeEnum Status { get; set; } = StatusEntidadeEnum.Ativo;

    /// <summary>
    /// Observações sobre o convênio
    /// </summary>
    [Column("observacao")]
    public string? Observacao { get; set; }

    /// <summary>
    /// Tipo do convênio CAS
    /// </summary>
    [Column("tipo_convenio_cas")]
    public TipoConvenioEnum? TipoConvenio { get; set; }

}
