using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Patients;

namespace SistemaApae.Api.Services.Patients
{
    public class AssistidoService : IAssistidoService
    {
        public Task<ApiResponse<Assistido>> Create(Assistido request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<Assistido>> Delete(Guid idAssistido)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<IEnumerable<Assistido>>> GetAllAssistidos()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<Assistido>> GetAssistidoById(Guid idAssistido)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<IEnumerable<Assistido>>> GetByFilters(AssistidoFiltroRequest filters)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<Assistido>> Update(Assistido request)
        {
            throw new NotImplementedException();
        }
    }
}
