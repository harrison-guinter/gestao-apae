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
    public string? Nome { get; set; } = string.Empty;

    /// <summary>
    /// Email do usuário (único)
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// Telefone do usuário
    /// </summary>
    public string? Telefone { get; set; }

    /// <summary>
    /// Perfil do usuário
    /// </summary>
    public PerfilEnum Perfil { get; set; }

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

    /// <summary>
    /// Indica se a entidade está ativa/inativa
    /// </summary>
    public StatusEntidadeEnum Status { get; set; }

}

