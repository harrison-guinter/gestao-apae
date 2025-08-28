using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace SistemaApae.Api.Controllers;

/// <summary>
/// Controller público com endpoints básicos para monitoramento e informações da API
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PublicController : ControllerBase
{
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
            Timestamp = DateTime.UtcNow
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
}
