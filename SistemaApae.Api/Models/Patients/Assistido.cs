using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace SistemaApae.Api.Models.Patients;

/// <summary>
/// Modelo de assistido do sistema
/// </summary>
[Table("assistido")]
public class Assistido : BaseModel
{
    /// <summary>
    /// ID único do assistido
    /// </summary>
    [Column("id_assistido")]
    public Guid IdAssistido { get; set; }

    /// <summary>
    /// ID do convênio CAS associado
    /// </summary>
    [Column("id_convenio_cas")]
    public Guid? IdConvenioCas { get; set; }

    /// <summary>
    /// Nome do assistido
    /// </summary>
    [Required]
    [MaxLength(150)]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// CPF do assistido (único)
    /// </summary>
    [MaxLength(11)]
    [Column("cpf")]
    public string? Cpf { get; set; }

    /// <summary>
    /// Data de nascimento do assistido
    /// </summary>
    [Column("data_nascimento")]
    public DateTime? DataNascimento { get; set; }

    /// <summary>
    /// Endereço do assistido
    /// </summary>
    [MaxLength(255)]
    [Column("endereco")]
    public string? Endereco { get; set; }

    /// <summary>
    /// Telefone do assistido
    /// </summary>
    [MaxLength(20)]
    [Column("telefone")]
    public string? Telefone { get; set; }

    /// <summary>
    /// Nome e telefone de contato
    /// </summary>
    [MaxLength(150)]
    [Column("nome_telefone")]
    public string? NomeTelefone { get; set; }

    /// <summary>
    /// Condição do assistido
    /// </summary>
    [MaxLength(150)]
    [Column("condicao")]
    public string? Condicao { get; set; }

    /// <summary>
    /// Número do SUS
    /// </summary>
    [MaxLength(15)]
    [Column("num_sus")]
    public string? NumSus { get; set; }

    /// <summary>
    /// Observações sobre o assistido
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
    /// Convênio CAS associado ao assistido
    /// </summary>
    public ConvenioCas? ConvenioCas { get; set; }
}
