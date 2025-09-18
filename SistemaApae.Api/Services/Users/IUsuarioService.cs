using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Users;

namespace SistemaApae.Api.Services.Users
{
    public interface IUsuarioService
    {
        Task<ApiResponse<Usuario>> CreateUser(Usuario request);
    }
}