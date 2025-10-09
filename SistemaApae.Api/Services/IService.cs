using SistemaApae.Api.Models;
using SistemaApae.Api.Models.Auth;

namespace SistemaApae.Api.Services;

/// <summary>
/// Interface genérica para serviços de aplicação com padrão ApiResponse
/// </summary>
public interface IService<TEntity, TFilter>
    where TEntity : ApiBaseModel, new()
{
    /// <summary>
    /// Lista entidades por filtros de pesquisa
    /// </summary>
    /// <param name="filtros">Objeto de filtros aplicáveis à entidade</param>
    /// <returns>Resposta com a lista de entidades filtradas</returns>
    Task<ApiResponse<IEnumerable<TEntity>>> GetByFilters(TFilter filtros);

    /// <summary>
    /// Busca uma entidade por identificador
    /// </summary>
    /// <param name="id">Identificador da entidade</param>
    /// <returns>Resposta com a entidade encontrada</returns>
    Task<ApiResponse<TEntity>> GetById(Guid id);

    /// <summary>
    /// Lista todas as entidades
    /// </summary>
    /// <returns>Resposta com a lista de entidades</returns>
    Task<ApiResponse<IEnumerable<TEntity>>> GetAll();

    /// <summary>
    /// Cria uma nova entidade
    /// </summary>
    /// <param name="entity">Dados da entidade</param>
    /// <returns>Resposta com a entidade criada</returns>
    Task<ApiResponse<TEntity>> Create(TEntity entity);

    /// <summary>
    /// Atualiza uma entidade existente
    /// </summary>
    /// <param name="entity">Dados da entidade</param>
    /// <returns>Resposta com a entidade atualizada</returns>
    Task<ApiResponse<TEntity>> Update(TEntity entity);
}


