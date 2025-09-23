using SistemaApae.Api.Models.Enums;

namespace SistemaApae.Api.Models.Users;

/// <summary>
/// DTO para resposta da API de usuário (sem propriedades internas do Supabase)
/// </summary>
public class UsuarioDto
{
    /// <summary>
    /// ID único do usuário
    /// </summary>
    public Guid IdUsuario { get; set; }

    /// <summary>
    /// Nome do usuário
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Email do usuário
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Telefone do usuário
    /// </summary>
    public string? Telefone { get; set; }

    /// <summary>
    /// Perfil do usuário
    /// </summary>
    public PerfilEnum Perfil { get; set; }

    /// <summary>
    /// Status do usuário (ativo/inativo)
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// Registro profissional
    /// </summary>
    public string? RegistroProfissional { get; set; }

    /// <summary>
    /// Especialidade do profissional
    /// </summary>
    public string? Especialidade { get; set; }

    /// <summary>
    /// Observações sobre o usuário
    /// </summary>
    public string? Observacao { get; set; }
}
