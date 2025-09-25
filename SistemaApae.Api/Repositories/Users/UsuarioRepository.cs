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
    public async Task<IEnumerable<Usuario>> GetByFiltersAsync(UsuarioFiltroRequest filtros)
    {
        try
        {
            var query = _supabaseService.Client
                .From<Usuario>()
                .Where(u => u.Status == filtros.Status)
                .Select("");

            if (!string.IsNullOrEmpty(filtros.Email))
                query = query.Filter(u => u.Email, Constants.Operator.ILike, $"%{filtros.Email}%");

            if (!string.IsNullOrEmpty(filtros.Nome))
                query = query.Filter(u => u.Nome, Constants.Operator.ILike, $"%{filtros.Nome}%");

            if (filtros.Perfil != null)
                query = query.Filter(u => u.Perfil, Constants.Operator.Equals, filtros.Perfil);

            var response = await query.Get();

            return response.Models;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuários por filtros");
            throw;
        }
    }

    /// <summary>
    /// Busca um usuário por id
    /// </summary>
    /// <returns> Usuario do id ou nulo </returns>
    public async Task<Usuario?> GetByIdAsync(Guid idUsuario)
    {
        try
        {
            var usuario = await _supabaseService.Client
                .From<Usuario>()
                .Where(u => u.Id == idUsuario)
                .Single();

            return usuario;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuário por Id: {Id}", idUsuario);
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
    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        try
        {
            usuario.Id = Guid.NewGuid();
            usuario.UpdatedAt = DateTime.UtcNow;

            var response = await _supabaseService.Client
                .From<Usuario>()
                .Insert(usuario);

            return response.Models.First();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usuário: {Email}", usuario.Email);
            throw;
        }
    }

    /// <summary>
    /// Atualiza um usuário existente
    /// </summary>
    /// <returns> Usuario atualizado </returns>
    public async Task<Usuario> UpdateAsync(Usuario usuario)
    {
        try
        {
            usuario.UpdatedAt = DateTime.UtcNow;

            var response = await _supabaseService.Client
                .From<Usuario>()
                .Where(u => u.Id == usuario.Id)
                .Set(u => u.Nome, usuario.Nome)
                .Set(u => u.Email, usuario.Email)
                .Set(u => u.Telefone!, usuario.Telefone ?? string.Empty)
                .Set(u => u.Senha, usuario.Senha)
                .Set(u => u.Status, usuario.Status)
                .Set(u => u.Observacao!, usuario.Observacao ?? string.Empty)
                .Set(u => u.UpdatedAt, usuario.UpdatedAt)
                .Update();

            return response.Models.First();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar usuário: {Id}", usuario.Id);
            throw;
        }
    }

    /// <summary>
    /// Inativa um usuário
    /// </summary>
    /// <returns> Usuario inativado </returns>
    public async Task<Usuario> DeleteAsync(Guid idUsuario)
    {
        try
        {
            var response = await _supabaseService.Client
                .From<Usuario>()
                .Where(u => u.Id == idUsuario)
                .Set(u => u.Status, false)
                .Update();

            return response.Models.First();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao inativar usuário: {Id}", idUsuario);
            throw;
        }
    }
}
