using SistemaApae.Api.Models.Patients;

namespace SistemaApae.Api.Repositories.Patients;

/// <summary>
/// Interface para repositório de assistidos
/// </summary>
public interface IAssistidoRepository
{
    /// <summary>
    /// Busca um Assistido por email
    /// </summary>
    /// <param name="email">Email do Assistido</param>
    /// <returns> Assistido do email ou nulo </returns>
    Task<Assistido?> GetByEmailAsync(string email);

    /// <summary>
    /// Lista Assistidos por filtros de pesquisa
    /// </summary>
    /// <returns> Lista de Assistidos dos filtros de pesquisa </returns>
    Task<IEnumerable<Assistido>> GetByFiltersAsync(AssistidoFiltroRequest filtros);

    /// <summary>
    /// Busca um Assistido por id
    /// </summary>
    /// <param name="idAssistido">ID do Assistido</param>
    /// <returns> Assistido do id ou nulo </returns>
    Task<Assistido?> GetByIdAsync(Guid idAssistido);

    /// <summary>
    /// Lista todos os usuários
    /// </summary>
    /// <returns> Lista de Usuario </returns>
    Task<IEnumerable<Assistido>> GetAllAsync();

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    /// <param name="usuario">Dados do usuário</param>
    /// <returns> Usuario criado </returns>
    Task<Assistido> CreateAsync(Assistido assistido);

    /// <summary>
    /// Atualiza um Assistido existente
    /// </summary>
    /// <param name="assistido">Dados do Assistido</param>
    /// <returns> Assistido atualizado </returns>
    Task<Assistido> UpdateAsync(Assistido assistido);

    /// <summary>
    /// Inativa um Assistido
    /// </summary>
    /// <param name="idAssistido">ID do Assistido</param>
    /// <returns> Assistido inativado </returns>
    Task<Assistido> DeleteAsync(Guid idAssistido);
}
