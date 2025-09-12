using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace SistemaApae.Api.Models.Users;

/// <summary>
/// Modelo de profissional do sistema
/// </summary>
[Table("profissional")]
public class Profissional : BaseModel
{
    /// <summary>
    /// ID único do profissional
    /// </summary>
    [Column("id_profissional")]
    public Guid IdProfissional { get; set; }

    /// <summary>
    /// ID do usuário associado
    /// </summary>
    [Required]
    [Column("id_usuario")]
    public Guid IdUsuario { get; set; }

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
    /// Usuário associado ao profissional
    /// </summary>
    public Usuario? Usuario { get; set; }
}
