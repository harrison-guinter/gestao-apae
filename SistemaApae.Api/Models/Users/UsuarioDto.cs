using SistemaApae.Api.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SistemaApae.Api.Models.Users;
public class UsuarioDto
{

    /// <summary>
    /// ID único da entidade
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nome do usuário
    /// </summary>
    [Required]
    [MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Email do usuário (único)
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Telefone do usuário
    /// </summary>
    [MaxLength(20)]
    public string? Telefone { get; set; }

    /// <summary>
    /// Perfil do usuário
    /// </summary>
    public PerfilEnum Perfil { get; set; }

    /// <summary>
    /// Registro profissional
    /// </summary>
    [MaxLength(50)]
    public string? RegistroProfissional { get; set; }

    /// <summary>
    /// Especialidade do profissional
    /// </summary>
    [MaxLength(50)]
    public string? Especialidade { get; set; }

    /// <summary>
    /// Observações sobre o usuário
    /// </summary>
    public string? Observacao { get; set; }

    /// <summary>
    /// Indica se a entidade está ativa/inativa
    /// </summary>
    public StatusEntidadeEnum Status { get; set; }

}

