using SistemaApae.Api.Models.Administrative;

namespace SistemaApae.Api.Repositories.Administrative;

/// <summary>
/// Interface para repositório de municípios
/// </summary>
public interface IMunicipioRepository
{
    /// <summary>
    /// Busca um município por id
    /// </summary>
    Task<Municipio?> GetByIdAsync(Guid id);

    /// <summary>
    /// Lista todos os municípios
    /// </summary>
    /// <returns>Lista de municípios</returns>
    Task<IEnumerable<Municipio>> GetAllAsync();

    /// <summary>
    /// Busca municípios pelo nome (case-insensitive, parcial)
    /// </summary>
    Task<IEnumerable<Municipio>> GetByNameAsync(string nome);
}
