using SistemaApae.Api.Models.Filters;

namespace SistemaApae.Api.Models.Patients;

/// <summary>
/// Modelo para requisição de usuário por filtros de pesquisa
/// </summary>
public class AssistidoFiltroRequest : BaseFilter
{
    /// <summary>
    /// Nome do Assistido
    /// </summary>
    public string? Nome { get; set; }

    /// <summary>
    /// CPF do Assistido
    /// </summary>

    public string? CPF { get; set; }

}

