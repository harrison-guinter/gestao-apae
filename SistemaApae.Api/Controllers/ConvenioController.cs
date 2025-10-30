using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaApae.Api.Models.Agenda;
using SistemaApae.Api.Models.Agreements;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Patients;
using SistemaApae.Api.Services;

namespace SistemaApae.Api.Controllers;

/// <summary>
/// Controller com endpoints de CRUD da entidade Convenio
/// </summary>
[ApiController]
//[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class ConvenioController : ControllerBase
{
    private readonly IService<Convenio, ConvenioFilterRequest> _service;

    /// <summary>
    /// Inicializa uma nova instância do ConvenioController
    /// </summary>
    public ConvenioController(IService<Convenio, ConvenioFilterRequest> service)
    {
        _service = service;
    }

    /// <summary>
    /// Lista Convenios por filtros de pesquisa (paginado)
    /// </summary>
    [HttpGet("filter")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<Convenio>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<Convenio>>>> GetByFilters([FromQuery] ConvenioFilterRequest request)
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
    /// Buscar um Convenio por id
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<Convenio>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<Convenio>>> GetById([FromRoute] Guid id)
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
    /// Criar um Convenio
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Convenio>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<Convenio>>> Create([FromBody] Convenio request)
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
        return Created();
    }

    /// <summary>
    /// Atualiza um Convenio existente
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<Convenio>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<Convenio>>> Update([FromBody] Convenio request)
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
