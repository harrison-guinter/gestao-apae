using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaApae.Api.Models.Agenda;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Services.Agenda;

namespace SistemaApae.Api.Controllers;

/// <summary>
/// Controller com endpoints de CRUD da entidade Agendamento
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class AgendamentoController : ControllerBase
{
    private readonly AgendamentoService _service;

    /// <summary>
    /// Inicializa uma nova instância do AgendamentoController
    /// </summary>
    public AgendamentoController(AgendamentoService service)
    {
        _service = service;
    }

    /// <summary>
    /// Lista Agendamentos por filtros de pesquisa (paginado) com assistidos
    /// </summary>
    [HttpGet("filter")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AgendamentoResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<AgendamentoResponseDto>>>> GetByFilters([FromQuery] AgendamentoFilterRequest request)
    {
        var result = await _service.GetByFilters(request);
        if (!result.Success)
        {
            if (result.Message.Contains("não foram encontrados") || result.Message.Contains("não foi encontrado"))
                return NotFound(result);
            return StatusCode(500, result);
        }
        return Ok(result);
    }

    /// <summary>
    /// Buscar um Agendamento por id com assistidos
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<AgendamentoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<AgendamentoResponseDto>>> GetById([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos"));

        var result = await _service.GetById(id);
        if (!result.Success)
        {
            if (result.Message.Contains("não foi encontrado"))
                return NotFound(result);
            return StatusCode(500, result);
        }
        return Ok(result);
    }

    /// <summary>
    /// Criar um Agendamento com assistidos
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AgendamentoResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<AgendamentoResponseDto>>> Create([FromBody] AgendamentoCreateDto request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos", errors));
        }

        var result = await _service.Create(request);
        if (!result.Success || result.Data == null)
        {
            return StatusCode(500, result);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
    }

    /// <summary>
    /// Atualiza um Agendamento existente com assistidos
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<AgendamentoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<AgendamentoResponseDto>>> Update([FromBody] AgendamentoUpdateDto request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos", errors));
        }

        var result = await _service.Update(request);
        if (!result.Success)
        {
            return StatusCode(500, result);
        }

        return Ok(result);
    }
}


