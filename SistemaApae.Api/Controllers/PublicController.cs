using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using SistemaApae.Api.Services;
using System.ComponentModel.DataAnnotations;

namespace SistemaApae.Api.Controllers;

/// <summary>
/// Controller público com endpoints básicos para monitoramento e informações da API
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PublicController : ControllerBase
{
    private readonly ISupabaseService _supabaseService;
    private readonly ILogger<PublicController> _logger;
    private readonly IAuthService _authService;

    /// <summary>
    /// Inicializa uma nova instância do PublicController
    /// </summary>
    /// <param name="supabaseService">Serviço do Supabase</param>
    /// <param name="logger">Logger para registro de eventos</param>
    /// <param name="authService">Serviço de autenticação</param>
    public PublicController(ISupabaseService supabaseService, ILogger<PublicController> logger, IAuthService authService)
    {
        _supabaseService = supabaseService;
        _logger = logger;
        _authService = authService;
    }
    /// <summary>
    /// Verifica se a API está funcionando corretamente
    /// </summary>
    /// <remarks>
    /// Endpoint de health check que retorna o status atual da API.
    /// Útil para monitoramento e verificação de disponibilidade.
    /// </remarks>
    /// <returns>Status da API com timestamp</returns>
    /// <response code="200">API está funcionando normalmente</response>
    [HttpGet("ping")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<object> Ping()
    {
        return Ok(new
        {
            Status = "OK",
            Message = "Sistema APAE API está funcionando",
            Timestamp = DateTime.Now
        });
    }

    /// <summary>
    /// Obtém informações básicas sobre a versão da API
    /// </summary>
    /// <remarks>
    /// Retorna informações sobre a aplicação, incluindo versão atual e status.
    /// A versão é obtida automaticamente do assembly da aplicação.
    /// </remarks>
    /// <returns>Informações da aplicação e versão</returns>
    /// <response code="200">Informações da API retornadas com sucesso</response>
    [HttpGet("version")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<object> Version()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version?.ToString() ?? "1.0.0";
        
        return Ok(new
        {
            Application = "Sistema APAE API",
            Version = version,
            Status = "Online"
        });
    }

    /// <summary>
    /// Verifica a conexão com o Supabase
    /// </summary>
    /// <remarks>
    /// Endpoint para verificar se a conexão com o banco de dados Supabase está funcionando.
    /// Útil para monitoramento da integração com o banco de dados.
    /// </remarks>
    /// <returns>Status da conexão com Supabase</returns>
    /// <response code="200">Status da conexão retornado com sucesso</response>
    /// <response code="500">Erro interno ao verificar conexão</response>
    [HttpGet("supabase-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<object>> SupabaseStatus()
    {
        try
        {
            var isConnected = await _supabaseService.IsConnectedAsync();

            return Ok(new 
            { 
                Connected = isConnected,
                Message = isConnected ? "Conexão com Supabase funcionando" : "Erro na conexão com Supabase",
                Timestamp = DateTime.Now
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar status do Supabase");
            return StatusCode(500, new 
            { 
                Connected = false,
                Message = "Erro interno ao verificar conexão com Supabase",
                Error = ex.Message,
                Timestamp = DateTime.Now
            });
        }
    }
}
