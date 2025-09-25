using Supabase;
using SistemaApae.Api.Models.Administrative;
using SistemaApae.Api.Services;

namespace SistemaApae.Api.Repositories.Administrative;

/// <summary>
/// Repositório de municípios usando Supabase como banco de dados
/// </summary>
public class MunicipioRepository : IMunicipioRepository
{
    private readonly ISupabaseService _supabaseService;
    private readonly ILogger<MunicipioRepository> _logger;

    public MunicipioRepository(ISupabaseService supabaseService, ILogger<MunicipioRepository> logger)
    {
        _supabaseService = supabaseService;
        _logger = logger;
    }

    public async Task<Municipio?> GetByIdAsync(Guid id)
    {
        try
        {
            var municipio = await _supabaseService.Client
                .From<Municipio>()
                .Where(m => m.Id == id)
                .Single();

            return municipio;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar município por Id: {Id}", id);
            return null;
        }
    }

    public async Task<IEnumerable<Municipio>> GetAllAsync()
    {
        try
        {
            var response = await _supabaseService.Client
                .From<Municipio>()
                .Get();

            return response.Models;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar municípios");
            return new List<Municipio>();
        }
    }

    public async Task<IEnumerable<Municipio>> GetByNameAsync(string nome)
    {
        try
        {
            // Usa ilike para busca case-insensitive no Supabase/Postgres
            var response = await _supabaseService.Client
                .From<Municipio>()
                .Filter("nome", Supabase.Postgrest.Constants.Operator.ILike, $"%{nome}%")
                .Get();

            return response.Models;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar municípios por nome: {Nome}", nome);
            return new List<Municipio>();
        }
    }
}


