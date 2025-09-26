using SistemaApae.Api.Models.Agreements;
using SistemaApae.Api.Models.Auth;

namespace SistemaApae.Api.Services.Agreements;

/// <summary>
/// Interface para serviço de convênios
/// </summary>
public interface IConvenioService
{
    /// <summary>
    /// Lista convênios por filtros de pesquisa
    /// </summary>
    /// <returns> Lista de Convenio dos filtros de pesquisa </returns>
    Task<ApiResponse<IEnumerable<Convenio>>> GetAgreementByFilters(ConvenioFiltroRequest request);

    /// <summary>
    /// Lista todos os convênios
    /// </summary>
    /// <returns> Lista de Convenio </returns>
    Task<ApiResponse<IEnumerable<Convenio>>> GetAllAgreements();

    /// <summary>
    /// Cria um novo convênio
    /// </summary>
    /// <returns> Convenio criado </returns>
    Task<ApiResponse<Convenio>> CreateAgreement(Convenio agreement);

    /// <summary>
    /// Atualiza um novo convênio existente
    /// </summary>
    /// <returns> Convenio atualizado </returns>
    Task<ApiResponse<Convenio>> UpdateAgreement(Convenio agreement);
}