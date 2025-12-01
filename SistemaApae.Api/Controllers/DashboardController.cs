using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Dashboard;
using SistemaApae.Api.Services;

namespace SistemaApae.Api.Controllers;

/// <summary>
/// Controller com endpoints para o dashboard do sistema
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _dashboardService;

    /// <summary>
    /// Inicializa uma nova instância do DashboardController
    /// </summary>
    public DashboardController(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    /// <summary>
	/// Obtém as estatísticas mensais do dashboard
    /// </summary>
	/// <param name="year">Ano alvo (opcional, padrão: ano atual)</param>
	/// <param name="month">Mês alvo 1-12 (opcional, padrão: mês atual)</param>
    /// <returns>Estatísticas do dashboard incluindo totais de assistidos, atendimentos, usuários e agendamentos</returns>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(ApiResponse<DashboardStatsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse<DashboardStatsDto>>> GetDashboardStats([FromQuery] int? year, [FromQuery] int? month)
    {
		var result = await _dashboardService.GetDashboardStats(year, month);
        
        if (!result.Success)
        {
            return StatusCode(500, result);
        }
        
        return Ok(result);
    }
}
