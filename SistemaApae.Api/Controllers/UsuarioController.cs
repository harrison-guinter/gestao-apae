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
    /// Criar usuário
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> CreateUser([FromBody] Usuario request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos", errors));
        }

        var result = await _usuarioService.CreateUser(request);

        if (!result.Success)
        {
            if (result.Message.Contains("Usuário não foi adicionado"))
                return NoContent();

            return StatusCode(500, result);
        }

        return Created($"/usuario/{result.Data!.IdUsuario}", result);
    }
}