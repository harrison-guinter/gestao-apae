using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Users;
using SistemaApae.Api.Services;

namespace SistemaApae.Api.Controllers.Users;

/// <summary>
/// Controller publico com endpoints de CRUD da entidade Usuario
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class UsuarioController : ControllerBase
{
    private readonly IService<Usuario, UsuarioFiltroRequest> _service;
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;

    /// <summary>
    /// Inicializa uma nova instância do UsuarioController
    /// </summary>
    public UsuarioController(IService<Usuario, UsuarioFiltroRequest> service, IAuthService authService, IEmailService emailService)
    {
        _service = service;
        _authService = authService;
        _emailService = emailService;
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
    public async Task<ActionResult<ApiResponse<Usuario>>> GetUserByFilters([FromQuery] UsuarioFiltroRequest filters)
    {
        var result = await _service.GetByFilters(filters);

        if (!result.Success)
        {
            if (result.Message.Contains("Registros não foram encontrados"))
                return NotFound();

            return StatusCode(500, result);
        }

        return Ok(result.Data!.Select(u => { u.Senha = null; return u; }));
    }

    /// <summary>
    /// Buscar um usuário por id
    /// </summary>
    /// <returns> Usuario do id </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<Usuario>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<Usuario>>> GetUserById([FromRoute] Guid id)
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

        result.Data!.Senha = null;

        return Ok(result);
    }

    /// <summary>
    /// Criar um usuário
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> CreateUser([FromBody] Usuario user)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos", errors));
        }

        user.UpdatedAt = DateTime.UtcNow;
        user.Senha = BCrypt.Net.BCrypt.HashPassword(_authService.GenerateRandomPassword());

        var result = await _service.Create(user);

        if (!result.Success)
        {
            if (result.Message.Contains("Registro não foi adicionado"))
                return NoContent();

            return StatusCode(500, result);
        }

        await _emailService.SendEmailAsync(result.Data!.Email, result.Data.Nome, result.Data!.Senha!);

        return Created();
    }

    /// <summary>
    /// Atualiza um usuário existente
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> UpdateUser([FromBody] Usuario user)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos", errors));
        }

        user.Senha = (await _service.GetById(user.Id)).Data!.Senha;

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