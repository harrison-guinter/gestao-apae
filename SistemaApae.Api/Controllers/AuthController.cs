using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Services;

namespace SistemaApae.Api.Controllers;

/// <summary>
/// Controller para operações de autenticação
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Inicializa uma nova instância do AuthController
    /// </summary>
    /// <param name="authService">Serviço de autenticação</param>
    /// <param name="logger">Logger para registro de eventos</param>
    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Autentica um usuário com email e senha
    /// </summary>
    /// <remarks>
    /// Endpoint para autenticação de usuários. Retorna um token JWT válido
    /// quando as credenciais estão corretas.
    /// </remarks>
    /// <param name="request">Dados de login (email e senha)</param>
    /// <returns>Token JWT e informações do usuário</returns>
    /// <response code="200">Login realizado com sucesso</response>
    /// <response code="400">Dados de entrada inválidos</response>
    /// <response code="401">Credenciais inválidas</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<LoginResponse>.ErrorResponse("Dados de entrada inválidos", errors));
        }

        var result = await _authService.LoginAsync(request);

        if (!result.Success)
        {
            if (result.Message.Contains("Credenciais inválidas"))
            {
                return Unauthorized(result);
            }
            return StatusCode(500, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Solicita recuperação de senha
    /// </summary>
    /// <remarks>
    /// Envia um email com instruções para redefinir a senha.
    /// Por segurança, sempre retorna sucesso, mesmo se o email não existir.
    /// </remarks>
    /// <param name="request">Email para recuperação de senha</param>
    /// <returns>Confirmação de envio do email</returns>
    /// <response code="200">Email enviado com sucesso (se o email existir)</response>
    /// <response code="400">Dados de entrada inválidos</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos", errors));
        }

        var result = await _authService.ForgotPasswordAsync(request);

        if (!result.Success)
        {
            return StatusCode(500, result);
        }

        return Ok(result);
    }
}