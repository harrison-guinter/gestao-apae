using SistemaApae.Api.Models.Agreements;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Repositories.Agreements;

namespace SistemaApae.Api.Services.Agreements;

/// <summary>
/// Serviço de convênios
/// </summary>
public class ConvenioService : IConvenioService
{
    private readonly IConvenioRepository _convenioRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ConvenioService> _logger;

    /// <summary>
    /// Inicializa uma nova instância do ConvenioService
    /// </summary>
    public ConvenioService(
        IConvenioRepository convenioRepository,
        IConfiguration configuration,
        ILogger<ConvenioService> logger
    )
    {
        _convenioRepository = convenioRepository;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os convênios
    /// </summary>
    /// <returns> Lista de Convenio </returns>
    public async Task<ApiResponse<IEnumerable<Convenio>>> GetAllAgreements()
    {
        try
        {
            // Busca todos os registros na entidade Convenio
            var response = await _convenioRepository.GetAllAsync();

            if (!response.Any())
            {
                _logger.LogWarning("Convênios não encontrados");
                return ApiResponse<IEnumerable<Convenio>>.ErrorResponse("Convênios não foram encontrados");
            }

            return ApiResponse<IEnumerable<Convenio>>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar convênios");
            return ApiResponse<IEnumerable<Convenio>>.ErrorResponse("Erro interno ao listar convênios");
        }
    }

    /// <summary>
    /// Cria um novo convênio
    /// </summary>
    /// <returns> Convenio criado </returns>
    public async Task<ApiResponse<Convenio>> CreateAgreement(Convenio agreement)
    {
        try
        {
            // Insere novo registro na entidade Convenio
            var response = await _convenioRepository.CreateAsync(agreement);

            if (response == null)
            {
                _logger.LogWarning("Convênio não adicionado: {Nome}", agreement.Nome);
                return ApiResponse<Convenio>.ErrorResponse("Convênio não foi adicionado");
            }

            return ApiResponse<Convenio>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar convênio: {Nome}", agreement.Nome);
            return ApiResponse<Convenio>.ErrorResponse("Erro interno ao adicionar convênio");
        }
    }

    /// <summary>
    /// Atualiza um novo convênio existente
    /// </summary>
    /// <returns> Convenio atualizado </returns>
    public async Task<ApiResponse<Convenio>> UpdateAgreement(Convenio agreement)
    {
        try
        {
            // Atualiza registro existente na entidade Convenio
            var response = await _convenioRepository.UpdateAsync(agreement);

            if (response == null)
            {
                _logger.LogWarning("Convênio não atualizado: {Nome}", agreement.Nome);
                return ApiResponse<Convenio>.ErrorResponse("Convênio não foi atualizado");
            }

            return ApiResponse<Convenio>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar convênio: {Nome}", agreement.Nome);
            return ApiResponse<Convenio>.ErrorResponse("Erro interno ao atualizar convênio");
        }
    }
}