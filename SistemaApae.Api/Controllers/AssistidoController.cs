using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Patients;
using SistemaApae.Api.Services;

namespace SistemaApae.Api.Controllers.Patients;

/// <summary>
/// Controller com endpoints de CRUD da entidade Assistido
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class AssistidoController : ControllerBase
{
    private readonly IService<Assistido, AssistidoFilterRequest> _service;

    /// <summary>
    /// Inicializa uma nova instância do AssistidoController
    /// </summary>
    public AssistidoController(IService<Assistido, AssistidoFilterRequest> service)
    {
        _service = service;
    }

    /// <summary>
    /// Lista Assistidos por filtros de pesquisa (paginado)
    /// </summary>
    [HttpGet("filter")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<Assistido>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<Assistido>>>> GetByFilters([FromQuery] AssistidoFilterRequest request)
    {
        var result = await _service.GetByFilters(request);
        if (!result.Success)
        {
            if (result.Message.Contains("não foram encontrados") || result.Message.Contains("não foi encontrado"))
                return Ok(Enumerable.Empty<Assistido>());

            return StatusCode(500, result);
        }
        return Ok(result);
    }

    /// <summary>
    /// Buscar um Assistido por id
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<Assistido>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<Assistido>>> GetById([FromRoute] Guid id)
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
    /// Criar um Assistido
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Assistido>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<Assistido>>> Create([FromBody] Assistido request)
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
        if (!result.Success)
        {
            if (result.Message.Contains("não foi adicionado") || result.Data is null)
                return NoContent();
            return StatusCode(500, result);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
    }

    /// <summary>
    /// Atualiza um Assistido existente
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<Assistido>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<Assistido>>> Update([FromBody] Assistido request)
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
            if (result.Message.Contains("não foi atualizado"))
                return NoContent();
            return StatusCode(500, result);
        }

        return Ok(result);
    }
}


