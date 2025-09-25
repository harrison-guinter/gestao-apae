namespace SistemaApae.Api.Repositories;

/// <summary>
/// Interface genérica para repositórios
/// </summary>
/// <typeparam name="T">Tipo da entidade</typeparam>
/// <typeparam name="TFilter">Tipo do filtro de pesquisa</typeparam>
public interface IRepository<T, TFilter> where T : class
{
    /// <summary>
    /// Busca uma entidade por ID
    /// </summary>
    /// <param name="id">ID da entidade</param>
    /// <returns>Entidade ou nulo</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Lista entidades por filtros de pesquisa
    /// </summary>
    /// <param name="filtros">Filtros de pesquisa</param>
    /// <returns>Lista de entidades</returns>
    Task<IEnumerable<T>> GetByFiltersAsync(TFilter filtros);

    /// <summary>
    /// Lista todas as entidades
    /// </summary>
    /// <returns>Lista de entidades</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Cria uma nova entidade
    /// </summary>
    /// <param name="entity">Dados da entidade</param>
    /// <returns>Entidade criada</returns>
    Task<T> CreateAsync(T entity);

    /// <summary>
    /// Atualiza uma entidade existente
    /// </summary>
    /// <param name="entity">Dados da entidade</param>
    /// <returns>Entidade atualizada</returns>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Deleta ou inativa uma entidade por ID
    /// </summary>
    /// <param name="id">ID da entidade</param>
    /// <returns>Entidade deletada/inativada</returns>
    Task<T> DeleteAsync(Guid id);
}
