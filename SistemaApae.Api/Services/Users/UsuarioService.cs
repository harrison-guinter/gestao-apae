using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Users;
using SistemaApae.Api.Repositories.Users;

namespace SistemaApae.Api.Services.Users;
/// <summary>
/// Serviço de usuários
/// </summary>
public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UsuarioService> _logger;

    /// <summary>
    /// Inicializa uma nova instância do UsuarioService
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
    /// Lista usuários por filtros de pesquisa
    /// </summary>
    /// <returns> Lista de Usuario dos filtros de pesquisa </returns>
    public async Task<ApiResponse<IEnumerable<UsuarioDto>>> GetUserByFilters(UsuarioFiltroRequest filters)
    {
        try
        {
            // Busca registro na entidade Usuario com parâmetros do filtro de pesquisa
            var response = await _usuarioRepository.GetByFiltersAsync(filters);

            if (response == null)
            {
                _logger.LogWarning("Usuário não encontrado por filtros de pesquisa");
                return ApiResponse<IEnumerable<UsuarioDto>>.ErrorResponse("Usuário não foi encontrado");
            }

            return ApiResponse<IEnumerable<UsuarioDto>>.SuccessResponse(response.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuário por filtros de pesquisa");
            return ApiResponse<IEnumerable<UsuarioDto>>.ErrorResponse("Erro interno ao buscar usuário");
        }
    }

    /// <summary>
    /// Busca um usuário por id
    /// </summary>
    /// <returns> Usuario do id </returns>
    public async Task<ApiResponse<UsuarioDto>> GetUserById(Guid idUsuario)
    {
        try
        {
            // Busca registro na entidade Usuario com parâmetro Id
            var response = await _usuarioRepository.GetByIdAsync(idUsuario);

            if (response == null)
            {
                _logger.LogWarning("Usuário não encontrado por id: {Id}", idUsuario);
                return ApiResponse<UsuarioDto>.ErrorResponse("Usuário não foi encontrado");
            }

            return ApiResponse<UsuarioDto>.SuccessResponse(response.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuário por id: {Id}", idUsuario);
            return ApiResponse<UsuarioDto>.ErrorResponse("Erro interno ao buscar usuário");
        }
    }

    /// <summary>
    /// Lista todos os usuários
    /// </summary>
    /// <returns> Lista de Usuario </returns>
    public async Task<ApiResponse<IEnumerable<UsuarioDto>>> GetAllUsers()
    {
        try
        {
            // Busca todos os registro na entidade Usuario
            var response = await _usuarioRepository.GetAllAsync();

            if (response == null)
            {
                _logger.LogWarning("Usuário não encontrados");
                return ApiResponse<IEnumerable<UsuarioDto>>.ErrorResponse("Usuário não foram encontrados");
            }

            return ApiResponse<IEnumerable<UsuarioDto>>.SuccessResponse(response.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar usuários");
            return ApiResponse<IEnumerable<UsuarioDto>>.ErrorResponse("Erro interno ao listar usuários");
        }
    }

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    /// <returns> Usuario criado </returns>
    public async Task<ApiResponse<UsuarioDto>> CreateUser(Usuario user)
    {
        try
        {
            // Insere novo registro na entidade Usuario
            var response = await _usuarioRepository.CreateAsync(user);

            if (response == null)
            {
                _logger.LogWarning("Usuário não adicionado: {Nome}", user.Nome);
                return ApiResponse<UsuarioDto>.ErrorResponse("Usuário não foi adicionado");
            }

            return ApiResponse<UsuarioDto>.SuccessResponse(response.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar usuário: {Nome}", user.Nome);
            return ApiResponse<UsuarioDto>.ErrorResponse("Erro interno ao adicionar usuário");
        }
    }

    /// <summary>
    /// Atualiza um novo usuário existente
    /// </summary>
    /// <returns> Usuario atualizado </returns>
    public async Task<ApiResponse<UsuarioDto>> UpdateUser(Usuario user)
    {
        try
        {
            // Atualiza registro existente na entidade Usuario
            var response = await _usuarioRepository.UpdateAsync(user);

            if (response == null)
            {
                _logger.LogWarning("Usuário não atualizado: {Nome}", user.Nome);
                return ApiResponse<UsuarioDto>.ErrorResponse("Usuário não foi atualizado");
            }

            return ApiResponse<UsuarioDto>.SuccessResponse(response.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar usuário: {Nome}", user.Nome);
            return ApiResponse<UsuarioDto>.ErrorResponse("Erro interno ao atualizar usuário");
        }
    }

    /// <summary>
    /// Inativa um novo usuário existente
    /// </summary>
    /// <returns> Usuario inativado </returns>
    public async Task<ApiResponse<UsuarioDto>> DeleteUser(Guid idUsuario)
    {
        try
        {
            // Inativa registro existente na entidade Usuario
            var response = await _usuarioRepository.DeleteAsync(idUsuario);

            if (response == null)
            {
                _logger.LogWarning("Usuário não inativado: {Id}", idUsuario);
                return ApiResponse<UsuarioDto>.ErrorResponse("Usuário não foi inativado");
            }

            return ApiResponse<UsuarioDto>.SuccessResponse(response.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao inativar usuário: {Id}", idUsuario);
            return ApiResponse<UsuarioDto>.ErrorResponse("Erro interno ao inativar usuário");
        }
    }
}