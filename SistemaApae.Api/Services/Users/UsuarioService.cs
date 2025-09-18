using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Users;
using SistemaApae.Api.Repositories.Users;

namespace SistemaApae.Api.Services.Users;
/// <summary>
/// Serviço de CRUD da entidade Usuario
/// </summary>
public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UsuarioService> _logger;

    /// <summary>
    /// Inicializa uma nova instancia do UsuarioService
    /// </summary>
    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        IConfiguration configuration,
        ILogger<UsuarioService> logger
    )
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo usuario
    /// </summary>
    public async Task<ApiResponse<Usuario>> CreateUser(Usuario request)
    {
        try
        {
            // Insere novo registro na entidade Usuario
            var response = await _usuarioRepository.InsertUser(request);

            if (response == null)
            {
                _logger.LogWarning("Usuário não adicionado: {Nome}", request.Nome);
                return ApiResponse<Usuario>.ErrorResponse("Usuário não foi adicionado");
            }

            return ApiResponse<Usuario>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar usuário: {Nome}", request.Nome);
            return ApiResponse<Usuario>.ErrorResponse("Erro interno ao adicionar usuário");
        }
    }
}