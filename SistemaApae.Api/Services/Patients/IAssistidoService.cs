using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Patients;

namespace SistemaApae.Api.Services.Patients;

/// <summary>
/// Interface para serviço de Assistido
/// </summary>
public interface IAssistidoService
{
    /// <summary>
    /// Lista assistido por filtros de pesquisa
    /// </summary>
    /// <returns> Lista de Assistido dos filtros de pesquisa </returns>
    Task<ApiResponse<IEnumerable<AssistidoDto>>> GetByFilters(AssistidoFiltroRequest filters);

    /// <summary>
    /// Busca um assistido por Id
    /// </summary>
    /// <returns> Assistido do id </returns>
    Task<ApiResponse<AssistidoDto>> GetAssistidoById(Guid idAssistido);

    /// <summary>
    /// Lista todos os Assistidos
    /// </summary>
    /// <returns> Lista de Assistidos </returns>
    Task<ApiResponse<IEnumerable<AssistidoDto>>> GetAllAssistidos();

    /// <summary>
    /// Cria um novo Assistido
    /// </summary>
    /// <returns> Assistido criado </returns>
    Task<ApiResponse<AssistidoDto>> Create(Assistido request);

    /// <summary>
    /// Atualiza um Assistido existente
    /// </summary>
    /// <returns> Assistido atualizado </returns>
    Task<ApiResponse<AssistidoDto>> Update(Assistido request);

    /// <summary>
    /// Inativa um Assistido existente
    /// </summary>
    /// <returns> Assistido inativado </returns>
    Task<ApiResponse<AssistidoDto>> Delete(Guid idAssistido);
}
