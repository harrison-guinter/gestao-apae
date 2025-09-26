using Microsoft.AspNetCore.Mvc;
using SistemaApae.Api.Models.Agreements;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Services;
using SistemaApae.Api.Services.Agreements;

namespace SistemaApae.Api.Controllers;

/// <summary>
/// Controller publico com endpoints de CRUD da entidade Convenio
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ConvenioController : ControllerBase
{
    private readonly ISupabaseService _supabaseService;
    private readonly ILogger<ConvenioController> _logger;
    private readonly IConvenioService _convenioService;

    /// <summary>
    /// Inicializa uma nova instância do ConvenioController
    /// </summary>
    public ConvenioController(ISupabaseService supabaseService, ILogger<ConvenioController> logger, IConvenioService convenioService)
    {
        _supabaseService = supabaseService;
        _logger = logger;
        _convenioService = convenioService;
    }

    /// <summary>
    /// Lista convênos por filtros de pesquisa
    /// </summary>
    /// <returns> Lista de Convenio dos filtros de pesquisa </returns>
    [HttpGet("filter")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<Convenio>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<Convenio>>> GetAgreementByFilters([FromQuery] ConvenioFiltroRequest request)
    {
        var result = await _convenioService.GetAgreementByFilters(request);

        if (!result.Success)
        {
            if (result.Message.Contains("Convênio não foi encontrado"))
                return NotFound();

            return StatusCode(500, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Lista todos os convênios
    /// </summary>
    /// <returns> Lista de Convenio </returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<Convenio>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<Convenio>>>> GetAllAgreements()
    {
        var result = await _convenioService.GetAllAgreements();

        if (!result.Success)
        {
            if (result.Message.Contains("Convênios não foram encontrados"))
                return NotFound();

            return StatusCode(500, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Criar um convênio
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> CreateAgreement([FromBody] Convenio agreement)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos", errors));
        }

        var result = await _convenioService.CreateAgreement(agreement);

        if (!result.Success)
        {
            if (result.Message.Contains("Convênio não foi adicionado"))
                return NoContent();

            return StatusCode(500, result);
        }

        return Created();
    }

    /// <summary>
    /// Atualiza um convênio existente
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> UpdateAgreement([FromBody] Convenio agreement)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos", errors));
        }

        var result = await _convenioService.UpdateAgreement(agreement);

        if (!result.Success)
        {
            if (result.Message.Contains("Convênio não foi atualizado"))
                return NoContent();

            return StatusCode(500, result);
        }

        return Ok();
    }
}
