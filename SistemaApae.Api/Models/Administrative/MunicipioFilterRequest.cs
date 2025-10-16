using SistemaApae.Api.Models.Filters;

namespace SistemaApae.Api.Models.Administrative;

/// <summary>
/// Modelo para requisição de Municipio por filtros de pesquisa
/// </summary>
public class MunicipioFilterRequest : BaseFilter
{
    /// <summary>
    /// Nome do Municipio
    /// </summary>
    public string? Nome { get; set; }

}

