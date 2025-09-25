using SistemaApae.Api.Models.Administrative;
using SistemaApae.Api.Models.Auth;

namespace SistemaApae.Api.Services.Administrative;

public interface IMunicipioService
{
    Task<ApiResponse<IEnumerable<Municipio>>> GetAll();
    Task<ApiResponse<Municipio>> GetById(Guid id);
    Task<ApiResponse<IEnumerable<Municipio>>> GetByName(string nome);
}


