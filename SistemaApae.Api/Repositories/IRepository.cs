namespace SistemaApae.Api.Repositories;

using SistemaApae.Api.Models;

/// <summary>
/// Interface genérica para repositórios
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade</typeparam>
/// <typeparam name="TFilter">Tipo do filtro de pesquisa</typeparam>
public interface IRepository<TEntity, TFilter>
    where TEntity : ApiBaseModel, new()
{
    /// <summary>
    /// Busca uma entidade por ID
    /// </summary>
    Task<TEntity?> GetByIdAsync(Guid id);

    /// <summary>
    /// Lista entidades por filtros de pesquisa
    /// </summary>
    Task<IEnumerable<TEntity>> GetByFiltersAsync(TFilter filtros);

    /// <summary>
    /// Lista todas as entidades
    /// </summary>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Cria uma nova entidade
    /// </summary>
    Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    /// Atualiza uma entidade existente
    /// </summary>
    Task<TEntity> UpdateAsync(TEntity entity);
}

/// <summary>
/// Estratégia para aplicar filtros em consultas de uma entidade
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade</typeparam>
/// <typeparam name="TFilter">Tipo do filtro</typeparam>
public interface IRepositoryFilter<TEntity, TFilter>
    where TEntity : ApiBaseModel, new()
{
    /// <summary>
    /// Aplica os filtros informados à consulta base da entidade
    /// </summary>
    /// <param name="query">Consulta base (tabela) para a entidade</param>
    /// <param name="filtros">Objeto com os filtros de pesquisa</param>
    /// <returns>Consulta com filtros aplicados</returns>
    Supabase.Postgrest.Interfaces.IPostgrestTable<TEntity> Apply(
        Supabase.Postgrest.Interfaces.IPostgrestTable<TEntity> query,
        TFilter filtros
    );
}
