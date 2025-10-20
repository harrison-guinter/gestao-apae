using SistemaApae.Api.Models;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Filters;
using SistemaApae.Api.Repositories;
using Supabase.Postgrest.Exceptions;

namespace SistemaApae.Api.Services;

/// <summary>
/// Implementação genérica de serviço de aplicação com ApiResponse
/// </summary>
public class Service<TEntity, TFilter> : IService<TEntity, TFilter>
    where TEntity : ApiBaseModel, new()
{
    private readonly IRepository<TEntity, TFilter> _repository;
    private readonly ILogger<Service<TEntity, TFilter>> _logger;

    /// <summary>
    /// Inicializa uma nova instância do serviço genérico
    /// </summary>
    public Service(IRepository<TEntity, TFilter> repository, ILogger<Service<TEntity, TFilter>> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <inheritdoc />
    public virtual async Task<ApiResponse<IEnumerable<TEntity>>> GetByFilters(TFilter filtros)
    {
        try
        {
            var result = await _repository.GetByFiltersAsync(filtros);
            if (!result.Any())
                return ApiResponse<IEnumerable<TEntity>>.ErrorResponse("Registros não foram encontrados");

            var resp = ApiResponse<IEnumerable<TEntity>>.SuccessResponse(result);

            if (filtros is IBaseFilter paged)
            {
                resp.Limit = paged.Limit ?? 50;
                resp.Skip = paged.Skip ?? 0;
            }
            else
            {
                resp.Limit = 50;
                resp.Skip = 0;
            }

            return resp;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar por filtros");
            return ApiResponse<IEnumerable<TEntity>>.ErrorResponse("Erro interno ao buscar por filtros");
        }
    }

    /// <inheritdoc />
    public virtual async Task<ApiResponse<TEntity>> GetById(Guid id)
    {
        try
        {
            var result = await _repository.GetByIdAsync(id);
            if (result is null)
                return ApiResponse<TEntity>.ErrorResponse("Registro não foi encontrado");
            return ApiResponse<TEntity>.SuccessResponse(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar por id: {Id}", id);
            return ApiResponse<TEntity>.ErrorResponse("Erro interno ao buscar por id");
        }
    }

    /// <inheritdoc />
    public virtual async Task<ApiResponse<TEntity>> Create(TEntity entity)
    {
        try
        {
            var result = await _repository.CreateAsync(entity);
            if (result is null)
                return ApiResponse<TEntity>.ErrorResponse("Registro não foi adicionado");
            return ApiResponse<TEntity>.SuccessResponse(result);
        }
        catch (PostgrestException ex)
        {
            if (ex.Message.Contains("23505"))
            {
                _logger.LogError(ex, "Erro de duplicidade de registro");
                return ApiResponse<TEntity>.ErrorResponse("Erro de duplicidade de registro");
            }

            _logger.LogError(ex, "Erro ao adicionar registro");
            return ApiResponse<TEntity>.ErrorResponse("Erro interno ao adicionar registro");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar registro");
            return ApiResponse<TEntity>.ErrorResponse("Erro interno ao adicionar registro");
        }
    }

    /// <inheritdoc />
    public virtual async Task<ApiResponse<TEntity>> Update(TEntity entity)
    {
        try
        {
            var result = await _repository.UpdateAsync(entity);
            if (result is null)
                return ApiResponse<TEntity>.ErrorResponse("Registro não foi atualizado");
            return ApiResponse<TEntity>.SuccessResponse(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar registro");
            return ApiResponse<TEntity>.ErrorResponse("Erro interno ao atualizar registro");
        }
    }
}


