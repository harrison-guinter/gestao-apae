using SistemaApae.Api.Models.Agreements;

namespace SistemaApae.Api.Repositories.Agreements;

/// <summary>
/// Interface para repositório de convênios
/// </summary>
public interface IConvenioRepository
{
    /// <summary>
    /// Lista convênios por filtros de pesquisa
    /// </summary>
    /// <returns> Lista de Convenio dos filtros de pesquisa </returns>
    Task<IEnumerable<Convenio>> GetByFiltersAsync(ConvenioFiltroRequest filters);

    /// <summary>
    /// Lista todos os convênios
    /// </summary>
    /// <returns> Lista de Convenio </returns>
    Task<IEnumerable<Convenio>> GetAllAsync();

    /// <summary>
    /// Cria um novo convênio
    /// </summary>
    /// <returns> Convenio criado </returns>
    Task<Convenio> CreateAsync(Convenio agreement);

    /// <summary>
    /// Atualiza um convênio existente
    /// </summary>
    /// <returns> Convenio atualizado </returns>
    Task<Convenio> UpdateAsync(Convenio agreement);
}