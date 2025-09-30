using SistemaApae.Api.Models.Agreements;
using SistemaApae.Api.Services;
using Supabase.Postgrest;

namespace SistemaApae.Api.Repositories.Agreements;

/// <summary>
/// Repositório de convênios
/// </summary>
public class ConvenioRepository : IConvenioRepository
{
    private readonly ISupabaseService _supabaseService;
    private readonly ILogger<ConvenioRepository> _logger;

    /// <summary>
    /// Inicializa uma nova instância do ConvenioRepository
    /// </summary>
    public ConvenioRepository(ISupabaseService supabaseService, ILogger<ConvenioRepository> logger)
    {
        _supabaseService = supabaseService;
        _logger = logger;
    }

    /// <summary>
    /// Lista convênios por filtros de pesquisa
    /// </summary>
    /// <returns> Lista de Convenio dos filtros de pesquisa </returns>
    public async Task<IEnumerable<Convenio>> GetByFiltersAsync(ConvenioFiltroRequest filters)
    {
        try
        {
            var query = _supabaseService.Client
                .From<Convenio>()
                .Select("");

            if (!string.IsNullOrEmpty(filters.Nome))
                query = query.Filter(u => u.Nome, Constants.Operator.ILike, $"%{filters.Nome}%");


            var response = await query.Get();

            return response.Models;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar convênios por filtros de pesquisa");
            throw;
        }
    }

    /// <summary>
    /// Lista todos os convênios
    /// </summary>
    /// <returns> Lista de Convenio </returns>
    public async Task<IEnumerable<Convenio>> GetAllAsync()
    {
        try
        {
            var response = await _supabaseService.Client
                .From<Convenio>()
                .Get();

            return response.Models;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar convênios");
            throw;
        }
    }

    /// <summary>
    /// Cria um novo convênio
    /// </summary>
    /// <returns> Convenio criado </returns>
    public async Task<Convenio> CreateAsync(Convenio agreement)
    {
        try
        {
            agreement.Id = Guid.NewGuid();

            var response = await _supabaseService.Client
                .From<Convenio>()
                .Insert(agreement);

            return response.Models.First();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar convênio: {Id}", agreement.Id);
            throw;
        }
    }

    /// <summary>
    /// Atualiza um convênio existente
    /// </summary>
    /// <returns> Convenio atualizado </returns>
    public async Task<Convenio> UpdateAsync(Convenio agreement)
    {
        try
        {
            var response = await _supabaseService.Client
                .From<Convenio>()
                .Where(u => u.Id == agreement.Id)
                .Update(agreement);

            return response.Models.First();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar convênio: {Id}", agreement.Id);
            throw;
        }
    }
}

