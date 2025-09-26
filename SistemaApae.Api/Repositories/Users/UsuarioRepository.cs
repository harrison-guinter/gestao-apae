using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Users;
using SistemaApae.Api.Services;
using Supabase.Postgrest;

namespace SistemaApae.Api.Repositories.Users;

/// <summary>
/// Repositório de usuários
/// </summary>
public class UsuarioRepository : IUsuarioRepository
{
    private readonly ISupabaseService _supabaseService;
    private readonly ILogger<UsuarioRepository> _logger;

    /// <summary>
    /// Inicializa uma nova instância do UsuarioRepository
    /// </summary>
    /// <param name="supabaseService">Serviço do Supabase</param>
    /// <param name="logger">Logger para registro de eventos</param>
    public UsuarioRepository(ISupabaseService supabaseService, ILogger<UsuarioRepository> logger)
    {
        _supabaseService = supabaseService;
        _logger = logger;
    }

    /// <summary>
    /// Busca um usuário por email
    /// </summary>
    /// <returns> Usuario do email ou nulo </returns>
    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        try
        {
            var usuario = await _supabaseService.Client
                .From<Usuario>()
                .Where(u => u.Email == email)
                .Single();

            return usuario;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuário por email: {Email}", email);
            throw;
        }
    }

    /// <summary>
    /// Lista usuários por filtros de pesquisa
    /// </summary>
    /// <returns> Lista de Usuario dos filtros de pesquisa </returns>
    public async Task<IEnumerable<Usuario>> GetByFiltersAsync(ConvenioFiltroRequest filters)
    {
        try
        {
            var query = _supabaseService.Client
                .From<Usuario>()
                .Where(u => u.Status == filters.Status)
                .Select("");

            if (!string.IsNullOrEmpty(filters.Email))
                query = query.Filter(u => u.Email, Constants.Operator.ILike, $"%{filters.Email}%");

            if (!string.IsNullOrEmpty(filters.Nome))
                query = query.Filter(u => u.Nome, Constants.Operator.ILike, $"%{filters.Nome}%");

            if (filters.Perfil != null)
                query = query.Where(u => u.Perfil == filters.Perfil);

            var response = await query.Get();

            return response.Models;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuários por filtros de pesquisa");
            throw;
        }
    }

    /// <summary>
    /// Busca um usuário por id
    /// </summary>
    /// <returns> Usuario do id ou nulo </returns>
    public async Task<Usuario?> GetByIdAsync(Guid idUser)
    {
        try
        {
            var usuario = await _supabaseService.Client
                .From<Usuario>()
                .Where(u => u.Id == idUser)
                .Single();

            return usuario;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuário por Id: {Id}", idUser);
            throw;
        }
    }

    /// <summary>
    /// Lista todos os usuários
    /// </summary>
    /// <returns> Lista de Usuario </returns>
    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        try
        {
            var response = await _supabaseService.Client
                .From<Usuario>()
                .Get();

            return response.Models;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar usuários");
            throw;
        }
    }

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    /// <returns> Usuario criado </returns>
    public async Task<Usuario> CreateAsync(Usuario user)
    {
        try
        {
            user.Id = Guid.NewGuid();
            user.UpdatedAt = DateTime.UtcNow;

            var response = await _supabaseService.Client
                .From<Usuario>()
                .Insert(user);

            return response.Models.First();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usuário: {Id}", user.Id);
            throw;
        }
    }

    /// <summary>
    /// Atualiza um usuário existente
    /// </summary>
    /// <returns> Usuario atualizado </returns>
    public async Task<Usuario> UpdateAsync(Usuario user)
    {
        try
        {
            user.UpdatedAt = DateTime.UtcNow;

            var response = await _supabaseService.Client
                .From<Usuario>()
                .Where(u => u.Id == user.Id)
                .Update(user);

            return response.Models.First();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar usuário: {Id}", user.Id);
            throw;
        }
    }
}
