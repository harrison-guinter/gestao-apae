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

    public async Task<ApiResponse<IEnumerable<MunicipioDto>>> GetAll()
    {
        try
        {
            var municipios = await _municipioRepository.GetAllAsync();
            if (municipios == null || !municipios.Any())
                return ApiResponse<IEnumerable<MunicipioDto>>.ErrorResponse("Municípios não foram encontrados");

            return ApiResponse<IEnumerable<MunicipioDto>>.SuccessResponse(municipios.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar municípios");
            return ApiResponse<IEnumerable<MunicipioDto>>.ErrorResponse("Erro interno ao listar municípios");
        }
    }

    public async Task<ApiResponse<MunicipioDto>> GetById(Guid id)
    {
        try
        {
            var municipio = await _municipioRepository.GetByIdAsync(id);
            if (municipio == null)
                return ApiResponse<MunicipioDto>.ErrorResponse("Município não foi encontrado");

            return ApiResponse<MunicipioDto>.SuccessResponse(municipio.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar município por id: {Id}", id);
            return ApiResponse<MunicipioDto>.ErrorResponse("Erro interno ao buscar município");
        }
    }

    public async Task<ApiResponse<IEnumerable<MunicipioDto>>> GetByName(string nome)
    {
        try
        {
            var municipios = await _municipioRepository.GetByNameAsync(nome);
            if (municipios == null || !municipios.Any())
                return ApiResponse<IEnumerable<MunicipioDto>>.ErrorResponse("Municípios não foram encontrados");

            return ApiResponse<IEnumerable<MunicipioDto>>.SuccessResponse(municipios.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar municípios por nome: {Nome}", nome);
            return ApiResponse<IEnumerable<MunicipioDto>>.ErrorResponse("Erro interno ao buscar municípios");
        }
    }
}


