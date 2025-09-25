using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using SistemaApae.Api.Models.Enums;
using System.Text.Json.Serialization;

namespace SistemaApae.Api.Models.Users;

/// <summary>
/// Modelo de usuário do sistema
/// </summary>
[Table("usuario")]
public class Usuario : ApiBaseModel
{
    /// <summary>
    /// Nome do usuário
    /// </summary>
    [Required]
    [MaxLength(150)]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Email do usuário (único)
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Telefone do usuário
    /// </summary>
    [MaxLength(20)]
    [Column("telefone")]
    public string? Telefone { get; set; }

    /// <summary>
    /// Senha hasheada do usuário
    /// </summary>
    [Required]
    [MaxLength(150)]
    [Column("senha")]
    public string Senha { get; set; } = string.Empty;

    /// <summary>
    /// Perfil usado no código
    /// </summary>
    [JsonIgnore]
    public PerfilEnum Perfil { get; set; }

    /// <summary>
    /// Perfil do usuário no banco de dados
    /// </summary>
    [Required]
    [Column("perfil")]
    public string PerfilDb 
    {
        get => Perfil.ToString();
        set 
        {
            if (Enum.TryParse<PerfilEnum>(value, out var parsed))
                Perfil = parsed;
        }
    }

    /// <summary>
    /// Indica se o usuário está ativo/inativo
    /// </summary>
    [Column("status")]
    public StatusEntidadeEnum Status { get; set; }

    /// <summary>
    /// Registro profissional
    /// </summary>
    [MaxLength(50)]
    [Column("registro_profissional")]
    public string? RegistroProfissional { get; set; }

    /// <summary>
    /// Especialidade do profissional
    /// </summary>
    [MaxLength(50)]
    [Column("especialidade")]
    public string? Especialidade { get; set; }

    /// <summary>
    /// Observações sobre o usuário
    /// </summary>
    [Column("observacao")]
    public string? Observacao { get; set; }

    /// <summary>
    /// Data da última atualização
    /// </summary>
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
