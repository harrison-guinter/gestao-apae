using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Patients;
using SistemaApae.Api.Models.Users;
using SistemaApae.Api.Services.Users;

namespace SistemaApae.Api.Controllers.Users;

/// <summary>
/// Controller publico com endpoints de CRUD da entidade Usuario
/// </summary>
[ApiController]
[Route("api/[controller]")]
//[Authorize]
[Produces("application/json")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _service;

    /// <summary>
    /// Inicializa uma nova instância do UsuarioController
    /// </summary>
    public UsuarioController(UsuarioService service)
    {
        _service = service;
    }

    /// <summary>
    /// Lista usuários por filtros de pesquisa
    /// </summary>
    /// <returns> Lista de Usuario dos filtros de pesquisa </returns>
    [HttpGet("filter")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<Usuario>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<Usuario>>> GetUserByFilters([FromQuery] UsuarioFilterRequest filters)
    {
        var result = await _service.GetByFilters(filters);

        if (!result.Success)
        {
            if (result.Message.Contains("Registros não foram encontrados"))
                return Ok(Enumerable.Empty<Assistido>());

            return StatusCode(500, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Buscar um usuário por id
    /// </summary>
    /// <returns> Usuario do id </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<UsuarioDto>>> GetUserById([FromRoute] Guid id)
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
    /// Criar um usuário
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<UsuarioDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<UsuarioDto>>> CreateUser([FromBody] Usuario user)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos", errors));
        }

        var result = await _service.Create(user);

        if (!result.Success)
        {
            if (result.Message.Contains("Registro não foi adicionado"))
                return NoContent();

            if (result.Message.Contains("Erro de duplicidade de registro"))
                return Conflict(result.Message);

            return StatusCode(500, result);
        }

        return Created();
    }

    /// <summary>
    /// Atualiza um usuário existente
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<UsuarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<UsuarioDto>>> UpdateUser([FromBody] Usuario user)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos", errors));
        }

        var result = await _service.Update(user);

        if (!result.Success)
        {
            if (result.Message.Contains("Registro não foi atualizado"))
                return NoContent();

            return StatusCode(500, result);
        }

        return Ok();
    }
}