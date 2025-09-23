using Supabase;
using SistemaApae.Api.Models.Users;
using SistemaApae.Api.Services;

namespace SistemaApae.Api.Repositories.Users;

/// <summary>
/// Repositório de usuários usando Supabase como banco de dados
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
    /// <returns> Usuário do email </returns>
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
            return null;
        }
    }

    /// <summary>
    /// Busca um usuário por id
    /// </summary>
    /// <returns> Usuário do id </returns>
    public async Task<Usuario?> GetByIdAsync(Guid id)
    {
        try
        {
            var usuario = await _supabaseService.Client
                .From<Usuario>()
                .Where(u => u.IdUsuario == id)
                .Single();

            return usuario;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuário por Id: {Id}", id);
            return null;
        }
    }

    /// <summary>
    /// Lista todos os usuários
    /// </summary>
    /// <returns> Lista de usuários </returns>
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
            return new List<Usuario>();
        }
    }

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    /// <returns> Usuário criado </returns>
    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        try
        {
            usuario.IdUsuario = Guid.NewGuid();
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
    public async Task<Usuario> UpdateAsync(Usuario usuario)
    {
        try
        {
            usuario.UpdatedAt = DateTime.UtcNow;

            var response = await _supabaseService.Client
                .From<Usuario>()
                .Where(x => x.IdUsuario == usuario.IdUsuario)
                .Set(x => x.Nome, usuario.Nome)
                .Set(x => x.Email, usuario.Email)
                .Set(x => x.Telefone, usuario.Telefone ?? string.Empty)
                .Set(x => x.Senha, usuario.Senha)
                .Set(x => x.Status, usuario.Status)
                .Set(x => x.Observacao, usuario.Observacao ?? string.Empty)
                .Set(x => x.UpdatedAt, usuario.UpdatedAt)
                .Update();

            return response.Models.First();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar usuário: {Id}", usuario.IdUsuario);
            throw;
        }
    }

    /// <summary>
    /// Remove um usuário
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            await _supabaseService.Client
                .From<Usuario>()
                .Where(x => x.IdUsuario == id)
                .Delete();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover usuário: {Id}", id);
            return false;
        }
    }
}
