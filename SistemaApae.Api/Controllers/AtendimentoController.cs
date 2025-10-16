using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaApae.Api.Models.Appointment;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Services;
using SistemaApae.Api.Services.Appointment;

namespace SistemaApae.Api.Controllers;

/// <summary>
/// Controller com endpoints de CRUD da entidade Atendimento
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class AtendimentoController : ControllerBase
{
    private readonly IService<Atendimento, AtendimentoFilterRequest> _service;
    private readonly AtendimentoService _atendimentoService;

    /// <summary>
    /// Inicializa uma nova instância do AtendimentoController
    /// </summary>
    public AtendimentoController(IService<Atendimento, AtendimentoFilterRequest> service, AtendimentoService atendimentoService)
    {
        _service = service;
        _atendimentoService = atendimentoService;
    }

    /// <summary>
    /// Lista atendimentos por filtros de pesquisa
    /// </summary>
    /// /// <returns> Lista de Atendimento dos filtros de pesquisa </returns>
    [HttpGet("filter")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<Atendimento>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<Atendimento>>>> GetAppointmentByFilters([FromQuery] AtendimentoFilterRequest filters)
    {
        var result = await _service.GetByFilters(filters);

        if (!result.Success)
        {
            if (result.Message.Contains("Registros não foram encontrados"))
                return NotFound();

            return StatusCode(500, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Buscar um atendimento por id
    /// </summary>
    /// <returns> Atendimento do id </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<Atendimento>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<Atendimento>>> GetAppointmentById([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos"));

        var result = await _service.GetById(id);

        if (!result.Success)
        {
            if (result.Message.Contains("Registro não foi encontrado"))
                return NotFound();

            return StatusCode(500, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Criar um atendimento
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> CreateAppointment([FromBody] Atendimento appointment)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos", errors));
        }

        var result = await _atendimentoService.Create(appointment);

        if (!result.Success)
        {
            if (result.Message.Contains("Registro não foi adicionado"))
                return NoContent();

            if (result.Message.Contains("não existe"))
                return BadRequest(ApiResponse<object>.ErrorResponse(result.Message));

            return StatusCode(500, result);
        }

        return Created();
    }

    /// <summary>
    /// Atualiza um atendimento existente
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> UpdateAppointment([FromBody] Atendimento appointment)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos", errors));
        }

        var result = await _atendimentoService.Update(appointment);

        if (!result.Success)
        {
            if (result.Message.Contains("Registro não foi atualizado"))
                return NoContent();

            if (result.Message.Contains("não existe"))
                return BadRequest(ApiResponse<object>.ErrorResponse(result.Message));

            return StatusCode(500, result);
        }

        return Ok();
    }
}
