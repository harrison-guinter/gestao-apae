namespace SistemaApae.Api.Models.Agreements;

/// <summary>
/// Modelo para requisição de convênios por filtros de pesquisa
/// </summary>
public class ConvenioFiltroRequest
{
    /// <summary>
    /// Nome do convênio
    /// </summary>
    public string? Nome { get; set; }

    /// <summary>
    /// Nome do munícipio do convênio
    /// </summary>
    public string? Municipio { get; set; }
}

