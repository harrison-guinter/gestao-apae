using Supabase;
using SistemaApae.Api.Models.Users;
using SistemaApae.Api.Services;

namespace SistemaApae.Api.Repositories;

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
    /// Busca usuário por email usando filtro PostgREST
    /// </summary>
    /// <param name="email">Email do usuário</param>
    /// <returns>Usuário encontrado ou null</returns>
    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        try
        {
            _logger.LogInformation("Buscando usuário por email: {Email}", email);

            var usuario = await _supabaseService.Client
                .From<Usuario>()
                .Where(u => u.Email == email)
                .Single();

            if (usuario == null)
            {
                _logger.LogWarning("Usuário não encontrado para o email: {Email}", email);                
            }
            else
            {
                _logger.LogInformation("Usuário encontrado: {Email} - {Nome} - {Status}", usuario.Email, usuario.Nome, usuario.Status);
            }

            return usuario;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuário por email: {Email}", email);
            return null;
        }
    }

    /// <summary>
    /// Busca usuário por ID
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <returns>Usuário encontrado ou null</returns>
    public async Task<Usuario?> GetByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Buscando usuário por ID: {Id}", id);

            var usuario = await _supabaseService.Client
                .From<Usuario>()
                .Where(u => u.IdUsuario == id)
                .Single();

            if (usuario == null)
            {
                _logger.LogWarning("Usuário não encontrado para o ID: {Id}", id);
            }
            else
            {
                _logger.LogInformation("Usuário encontrado: {Email} - {Nome} - {Status}", usuario.Email, usuario.Nome, usuario.Status);
            }

            return usuario;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuário por ID: {Id}", id);
            return null;
        }
    }

    /// <summary>
    /// Lista todos os usuários
    /// </summary>
    /// <returns>Lista de usuários</returns>
    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        try
        {
            var response = await _supabaseService.Client
                .From<Usuario>()
                .Get();

            _logger.LogInformation("Total de usuários encontrados: {Count}", response.Models.Count);
            foreach (var user in response.Models)
            {
                _logger.LogInformation("Usuário: {Email} - {Nome} - {Status}", user.Email, user.Nome, user.Status);
            }

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
    /// <param name="usuario">Dados do usuário</param>
    /// <returns>Usuário criado</returns>
    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        try
        {
            usuario.IdUsuario = Guid.NewGuid();
            usuario.CreatedAt = DateTime.UtcNow;
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
    /// <param name="usuario">Dados do usuário</param>
    /// <returns>Usuário atualizado</returns>
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
    /// <param name="id">ID do usuário</param>
    /// <returns>True se removido com sucesso</returns>
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
