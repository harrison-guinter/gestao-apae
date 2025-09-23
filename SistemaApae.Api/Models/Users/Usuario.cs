using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using SistemaApae.Api.Models.Enums;

namespace SistemaApae.Api.Models.Users;

/// <summary>
/// Modelo de usuário do sistema
/// </summary>
[Table("usuario")]
public class Usuario : BaseModel
{
    /// <summary>
    /// ID único do usuário
    /// </summary>
    [Column("id_usuario")]
    public Guid IdUsuario { get; set; }

    /// <summary>
    /// Nome do usuário
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Email do usuário (único)
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(50)]
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
    /// Perfil do usuário
    /// </summary>
    [Required]
    [Column("perfil")]
    public PerfilEnum Perfil { get; set; }

    /// <summary>
    /// Indica se o usuário está ativo
    /// </summary>
    [Column("ativo")]
    public bool Ativo { get; set; } = true;

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
