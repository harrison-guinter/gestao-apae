namespace SistemaApae.Api.Models.Filters;

/// <summary>
/// Contrato para filtros com paginação e ordenação
/// </summary>
public interface IBaseFilter
{
    /// <summary>
    /// Quantidade máxima de registros a retornar
    /// </summary>
    int? Limit { get; set; }

    /// <summary>
    /// Quantidade de registros a pular (offset)
    /// </summary>
    int? Skip { get; set; }

}

/// <summary>
/// Implementação padrão de filtro com paginação e ordenação
/// </summary>
public class BaseFilter : IBaseFilter
{
    public int? Limit { get; set; }
    public int? Skip { get; set; }

}


