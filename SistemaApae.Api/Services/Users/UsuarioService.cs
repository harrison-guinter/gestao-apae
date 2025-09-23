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
    /// Busca um usuario por e-mail
    /// </summary>
    /// <returns> Usuário do email </returns>
    public async Task<ApiResponse<Usuario>> GetUserByEmail(string email)
    {
        try
        {
            // Busca registro na entidade Usuario com parâmetro Email
            var response = await _usuarioRepository.GetByEmailAsync(email);

            if (response == null)
            {
                _logger.LogWarning("Usuário não encontrado por e-mail: {Email}", email);
                return ApiResponse<Usuario>.ErrorResponse("Usuário não foi encontrado");
            }

            return ApiResponse<Usuario>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuário por e-mail: {Email}", email);
            return ApiResponse<Usuario>.ErrorResponse("Erro interno ao buscar usuário");
        }
    }

    /// <summary>
    /// Busca um usuario por id
    /// </summary>
    /// <returns> Usuário do id </returns>
    public async Task<ApiResponse<Usuario>> GetUserById(Guid id)
    {
        try
        {
            // Busca registro na entidade Usuario com parâmetro Id
            var response = await _usuarioRepository.GetByIdAsync(id);

            if (response == null)
            {
                _logger.LogWarning("Usuário não encontrado por id: {Id}", id);
                return ApiResponse<Usuario>.ErrorResponse("Usuário não foi encontrado");
            }

            return ApiResponse<Usuario>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuário por id: {Id}", id);
            return ApiResponse<Usuario>.ErrorResponse("Erro interno ao buscar usuário");
        }
    }

    /// <summary>
    /// Lista todos os usuários
    /// </summary>
    /// <returns> Lista de usuários </returns>
    public async Task<ApiResponse<IEnumerable<Usuario>>> GetAllUsers()
    {
        try
        {
            // Busca todos os registro na entidade Usuario
            var response = await _usuarioRepository.GetAllAsync();

            if (response == null)
            {
                _logger.LogWarning("Usuário não encontrados");
                return ApiResponse<IEnumerable<Usuario>>.ErrorResponse("Usuário não foram encontrados");
            }

            return ApiResponse<IEnumerable<Usuario>>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar usuários");
            return ApiResponse<IEnumerable<Usuario>>.ErrorResponse("Erro interno ao listar usuários");
        }
    }

    /// <summary>
    /// Cria um novo usuario
    /// </summary>
    /// <returns> Usuário criado </returns>
    public async Task<ApiResponse<Usuario>> CreateUser(Usuario user)
    {
        try
        {
            // Insere novo registro na entidade Usuario
            var response = await _usuarioRepository.CreateAsync(user);

            if (response == null)
            {
                _logger.LogWarning("Usuário não adicionado: {Nome}", user.Nome);
                return ApiResponse<Usuario>.ErrorResponse("Usuário não foi adicionado");
            }

            return ApiResponse<Usuario>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar usuário: {Nome}", user.Nome);
            return ApiResponse<Usuario>.ErrorResponse("Erro interno ao adicionar usuário");
        }
    }

    /// <summary>
    /// Atualiza um novo usuario existente
    /// </summary>
    public async Task<ApiResponse<Usuario>> UpdateUser(Usuario user)
    {
        try
        {
            // Atualiza registro existente na entidade Usuario
            var response = await _usuarioRepository.UpdateAsync(user);

            if (response == null)
            {
                _logger.LogWarning("Usuário não atualizado: {Nome}", user.Nome);
                return ApiResponse<Usuario>.ErrorResponse("Usuário não foi atualizado");
            }

            return ApiResponse<Usuario>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar usuário: {Nome}", user.Nome);
            return ApiResponse<Usuario>.ErrorResponse("Erro interno ao atualizar usuário");
        }
    }
}