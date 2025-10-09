using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Filters;

namespace SistemaApae.Api.Models.Users;

/// <summary>
/// Modelo para requisição de usuário por filtros de pesquisa
/// </summary>
public class UsuarioFiltroRequest : BaseFilter
{
    /// <summary>
    /// Nome do usuário
    /// </summary>
    public string? Nome { get; set; }

    /// <summary>
    /// Email do usuário
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Perfil do usuário
    /// </summary>
    public PerfilEnum? Perfil { get; set; }

    /// <summary>
    /// Status do usuário (ativo/inativo)
    /// </summary>
    public StatusEntidadeEnum Status { get; set; } = StatusEntidadeEnum.Ativo;
}

