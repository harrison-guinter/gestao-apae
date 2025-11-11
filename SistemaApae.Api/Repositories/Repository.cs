using SistemaApae.Api.Models;
using SistemaApae.Api.Services;
using Supabase.Postgrest;
using SistemaApae.Api.Models.Filters;
using System.Runtime.CompilerServices;

namespace SistemaApae.Api.Repositories;

/// <summary>
/// Implementação genérica de repositório usando Supabase
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade</typeparam>
/// <typeparam name="TFilter">Tipo do filtro</typeparam>
public class Repository<TEntity, TFilter> : IRepository<TEntity, TFilter>
    where TEntity : ApiBaseModel, new()
{
    protected readonly ISupabaseService _supabaseService;
    protected readonly IRepositoryFilter<TEntity, TFilter> _filter;

    public Repository(ISupabaseService supabaseService, IRepositoryFilter<TEntity, TFilter> filter)
    {
        _supabaseService = supabaseService;
        _filter = filter;
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        var entity = await _supabaseService.Client
            .From<TEntity>()
            .Where(e => e.Id == id)
            .Single();

        return entity;
    }

    public virtual async Task<IEnumerable<TEntity>> GetByFiltersAsync(TFilter filtros)
    {
        try
        {
            var table = _supabaseService.Client
                .From<TEntity>();

            var filtered = _filter.Apply(table, filtros);

            // Aplica paginação somente se o filtro suportar IBaseFilter e Limit tiver sido informado
            if (filtros is IBaseFilter paged && paged.Limit.HasValue)
            {
                var limit = paged.Limit.Value;
                var from = paged.Skip.GetValueOrDefault(0);
                var to = from + Math.Max(0, limit - 1);
                filtered = filtered.Range(from, to);
            }

            var response = await filtered.Get();
            return response.Models;
        }
        catch
        {
            return new List<TEntity>();
        }
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        if (entity.Id == Guid.Empty)
            entity.Id = Guid.NewGuid();

        var response = await _supabaseService.Client
            .From<TEntity>()
            .Insert(entity);

        return response.Models.First();
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var response = await _supabaseService.Client
            .From<TEntity>()
            .Where(e => e.Id == entity.Id)
            .Update(entity);

        return response.Models.First();
    }
}

/// <summary>
/// Filtro padrão (no-op) para repositórios genéricos
/// </summary>
public class DefaultRepositoryFilter<TEntity, TFilter> : IRepositoryFilter<TEntity, TFilter>
    where TEntity : ApiBaseModel, new()
{
    public Supabase.Postgrest.Interfaces.IPostgrestTable<TEntity> Apply(
        Supabase.Postgrest.Interfaces.IPostgrestTable<TEntity> query,
        TFilter filtros)
    {
        return query;
    }
}


