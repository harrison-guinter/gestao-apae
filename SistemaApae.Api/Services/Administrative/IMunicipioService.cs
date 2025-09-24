using SistemaApae.Api.Models.Administrative;
using SistemaApae.Api.Models.Auth;

namespace SistemaApae.Api.Services.Administrative;

public interface IMunicipioService
{
    Task<ApiResponse<IEnumerable<MunicipioDto>>> GetAll();
    Task<ApiResponse<MunicipioDto>> GetById(Guid id);
    Task<ApiResponse<IEnumerable<MunicipioDto>>> GetByName(string nome);
}


