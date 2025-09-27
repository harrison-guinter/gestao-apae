using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Users;
using SistemaApae.Api.Repositories.Users;

namespace SistemaApae.Api.Services.Users;
/// <summary>
/// Serviço de usuários
/// </summary>
public class UsuarioService : IUsuarioService
{
    private readonly IAuthService _authService;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UsuarioService> _logger;

    /// <summary>
    /// Inicializa uma nova instância do UsuarioService
    /// </summary>
    public UsuarioService(
        IAuthService authService,
        IUsuarioRepository usuarioRepository,
        IConfiguration configuration,
        ILogger<UsuarioService> logger
    )
    {
        _authService = authService;
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Lista usuários por filtros de pesquisa
    /// </summary>
    /// <returns> Lista de Usuario dos filtros de pesquisa </returns>
    public async Task<ApiResponse<IEnumerable<Usuario>>> GetUserByFilters(UsuarioFiltroRequest filters)
    {
        try
        {
            // Busca registro na entidade Usuario com parâmetros do filtro de pesquisa
            var response = await _usuarioRepository.GetByFiltersAsync(filters);

            if (!response.Any())
            {
                _logger.LogWarning("Usuário não encontrado por filtros de pesquisa");
                return ApiResponse<IEnumerable<Usuario>>.ErrorResponse("Usuário não foi encontrado");
            }

            var responseUpdated = response.Select(u =>
            {
                u.Senha = null;
                return u;
            });

            return ApiResponse<IEnumerable<Usuario>>.SuccessResponse(responseUpdated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuário por filtros de pesquisa");
            return ApiResponse<IEnumerable<Usuario>>.ErrorResponse("Erro interno ao buscar usuário");
        }
    }

    /// <summary>
    /// Busca um usuário por id
    /// </summary>
    /// <returns> Usuario do id </returns>
    public async Task<ApiResponse<Usuario>> GetUserById(Guid idUsuario)
    {
        try
        {
            // Busca registro na entidade Usuario com parâmetro Id
            var response = await _usuarioRepository.GetByIdAsync(idUsuario);

            if (response == null)
            {
                _logger.LogWarning("Usuário não encontrado por id: {Id}", idUsuario);
                return ApiResponse<Usuario>.ErrorResponse("Usuário não foi encontrado");
            }

            response.Senha = null;

            return ApiResponse<Usuario>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuário por id: {Id}", idUsuario);
            return ApiResponse<Usuario>.ErrorResponse("Erro interno ao buscar usuário");
        }
    }

    /// <summary>
    /// Lista todos os usuários
    /// </summary>
    /// <returns> Lista de Usuario </returns>
    public async Task<ApiResponse<IEnumerable<Usuario>>> GetAllUsers()
    {
        try
        {
            // Busca todos os registros na entidade Usuario
            var response = await _usuarioRepository.GetAllAsync();

            if (!response.Any())
            {
                _logger.LogWarning("Usuários não encontrados");
                return ApiResponse<IEnumerable<Usuario>>.ErrorResponse("Usuários não foram encontrados");
            }

            var responseUpdated = response.Select(u =>
            {
                u.Senha = null;
                return u;
            });

            return ApiResponse<IEnumerable<Usuario>>.SuccessResponse(responseUpdated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar usuários");
            return ApiResponse<IEnumerable<Usuario>>.ErrorResponse("Erro interno ao listar usuários");
        }
    }

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    /// <returns> Usuario criado </returns>
    public async Task<ApiResponse<Usuario>> CreateUser(Usuario user)
    {
        try
        {
            var newPassword = _authService.GenerateRandomPassword();

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            // Insere novo registro na entidade Usuario
            var response = await _usuarioRepository.CreateAsync(user, hashedPassword);

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
    /// Atualiza um novo usuário existente
    /// </summary>
    /// <returns> Usuario atualizado </returns>
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