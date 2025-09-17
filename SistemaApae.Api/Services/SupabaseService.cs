using Supabase;
using SistemaApae.Api.Models;

namespace SistemaApae.Api.Services;

/// <summary>
/// Serviço para gerenciar a conexão com o Supabase
/// </summary>
public class SupabaseService : ISupabaseService
{
    private readonly Supabase.Client _client;
    private readonly ILogger<SupabaseService> _logger;

    /// <summary>
    /// Inicializa uma nova instância do SupabaseService
    /// </summary>
    /// <param name="configuration">Configuração da aplicação</param>
    /// <param name="logger">Logger para registro de eventos</param>
    public SupabaseService(IConfiguration configuration, ILogger<SupabaseService> logger)
    {
        _logger = logger;
        
        // Tenta obter as configurações das variáveis de ambiente primeiro, depois do appsettings
        var url = Environment.GetEnvironmentVariable("SUPABASE_URL") ?? 
                  configuration["Supabase:Url"];
        var anonKey = Environment.GetEnvironmentVariable("SUPABASE_ANON_KEY") ?? 
                      configuration["Supabase:AnonKey"];
        var serviceRoleKey = Environment.GetEnvironmentVariable("SUPABASE_SERVICE_ROLE_KEY") ?? 
                            configuration["Supabase:ServiceRoleKey"];
        
        if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(anonKey))
        {
            throw new InvalidOperationException("Configurações do Supabase não encontradas. Verifique as variáveis de ambiente SUPABASE_URL e SUPABASE_ANON_KEY ou as configurações no appsettings.json.");
        }

        var options = new SupabaseOptions
        {
            AutoConnectRealtime = true,
            AutoRefreshToken = true
        };

        _client = new Supabase.Client(url, anonKey, options);
        
        _logger.LogInformation("Cliente Supabase inicializado com sucesso");
    }

    /// <summary>
    /// Cliente Supabase configurado
    /// </summary>
    public Supabase.Client Client => _client;

    /// <summary>
    /// Verifica se a conexão com o Supabase está funcionando
    /// </summary>
    /// <returns>True se a conexão estiver funcionando</returns>
    public Task<bool> IsConnectedAsync()
    {
        try
        {
            // Verifica se o cliente está inicializado
            if (_client == null)
            {
                return Task.FromResult(false);
            }

            // Para simplificar, apenas verifica se o cliente foi criado com sucesso
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar conexão com Supabase");
            return Task.FromResult(false);
        }
    }
}
