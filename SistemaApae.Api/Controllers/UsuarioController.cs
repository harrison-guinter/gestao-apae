using Microsoft.AspNetCore.Mvc;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Users;
using SistemaApae.Api.Services;
using SistemaApae.Api.Services.Users;

namespace SistemaApae.Api.Controllers.Users;

/// <summary>
/// Controller publico com endpoints de CRUD da entidade Usuario
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsuarioController : ControllerBase
{
    private readonly ISupabaseService _supabaseService;
    private readonly ILogger<PublicController> _logger;
    private readonly IUsuarioService _usuarioService;

    /// <summary>
    /// Inicializa uma nova instância do UsuarioController
    /// </summary>
    public UsuarioController(ISupabaseService supabaseService, ILogger<PublicController> logger, IUsuarioService usuarioService)
    {
        _supabaseService = supabaseService;
        _logger = logger;
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Buscar um usuário por e-mail
    /// </summary>
    /// <returns> Usuário do email </returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<UsuarioDto>>> GetUserByEmail([FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos"));

        var result = await _usuarioService.GetUserByEmail(email);

        if (!result.Success)
        {
            if (result.Message.Contains("Usuário não foi encontrado"))
                return NotFound();

            return StatusCode(500, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Buscar um usuário por id
    /// </summary>
    /// <returns> Usuário do id </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<UsuarioDto>>> GetUserById([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos"));

        var result = await _usuarioService.GetUserById(id);

        if (!result.Success)
        {
            if (result.Message.Contains("Usuário não foi encontrado"))
                return NotFound();

            return StatusCode(500, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Lista todos os usuários
    /// </summary>
    /// <returns> Lista de usuários </returns>
    [HttpGet("all")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<UsuarioDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<UsuarioDto>>>> GetAllUsers()
    {
        var result = await _usuarioService.GetAllUsers();

        if (!result.Success)
        {
            if (result.Message.Contains("Usuários não foram encontrados"))
                return NotFound();

            return StatusCode(500, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Criar um usuário
    /// </summary>
    [HttpPost("create"!)]
    [ProducesResponseType(typeof(ApiResponse<UsuarioDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
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

        var result = await _usuarioService.CreateUser(user);

        if (!result.Success)
        {
            if (result.Message.Contains("Usuário não foi adicionado"))
                return NoContent();

            return StatusCode(500, result);
        }

        return Created();
    }

    /// <summary>
    /// Atualiza um usuário existente
    /// </summary>
    [HttpPut("update")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioDto>), StatusCodes.Status201Created)]
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

        var result = await _usuarioService.UpdateUser(user);

        if (!result.Success)
        {
            if (result.Message.Contains("Usuário não foi atualizado"))
                return NoContent();

            return StatusCode(500, result);
        }

        return Ok();
    }
}