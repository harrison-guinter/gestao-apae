using SistemaApae.Api.Models.Administrative;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Repositories.Administrative;

namespace SistemaApae.Api.Services.Administrative;

/// <summary>
/// Serviço de leitura de municípios
/// </summary>
public class MunicipioService : IMunicipioService
{
    private readonly IMunicipioRepository _municipioRepository;
    private readonly ILogger<MunicipioService> _logger;

    public MunicipioService(IMunicipioRepository municipioRepository, ILogger<MunicipioService> logger)
    {
        _municipioRepository = municipioRepository;
        _logger = logger;
    }

    public async Task<ApiResponse<IEnumerable<Municipio>>> GetAll()
    {
        try
        {
            var municipios = await _municipioRepository.GetAllAsync();
            if (municipios == null || !municipios.Any())
                return ApiResponse<IEnumerable<Municipio>>.ErrorResponse("Municípios não foram encontrados");

            return ApiResponse<IEnumerable<Municipio>>.SuccessResponse(municipios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar municípios");
            return ApiResponse<IEnumerable<Municipio>>.ErrorResponse("Erro interno ao listar municípios");
        }
    }

    public async Task<ApiResponse<Municipio>> GetById(Guid id)
    {
        try
        {
            var municipio = await _municipioRepository.GetByIdAsync(id);
            if (municipio == null)
                return ApiResponse<Municipio>.ErrorResponse("Município não foi encontrado");

            return ApiResponse<Municipio>.SuccessResponse(municipio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar município por id: {Id}", id);
            return ApiResponse<Municipio>.ErrorResponse("Erro interno ao buscar município");
        }
    }

    public async Task<ApiResponse<IEnumerable<Municipio>>> GetByName(string nome)
    {
        try
        {
            var municipios = await _municipioRepository.GetByNameAsync(nome);
            if (municipios == null || !municipios.Any())
                return ApiResponse<IEnumerable<Municipio>>.ErrorResponse("Municípios não foram encontrados");

            return ApiResponse<IEnumerable<Municipio>>.SuccessResponse(municipios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar municípios por nome: {Nome}", nome);
            return ApiResponse<IEnumerable<Municipio>>.ErrorResponse("Erro interno ao buscar municípios");
        }
    }
}


