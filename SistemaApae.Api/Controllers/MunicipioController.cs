using Microsoft.AspNetCore.Mvc;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Administrative;
using SistemaApae.Api.Services.Administrative;

namespace SistemaApae.Api.Controllers.Administrative;

/// <summary>
/// Controller público com endpoints de leitura para a entidade Município
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MunicipioController : ControllerBase
{
    private readonly ILogger<MunicipioController> _logger;
    private readonly IMunicipioService _municipioService;

    public MunicipioController(ILogger<MunicipioController> logger, IMunicipioService municipioService)
    {
        _logger = logger;
        _municipioService = municipioService;
    }

    /// <summary>
    /// Lista todos os municípios
    /// </summary>
    /// <returns> Lista de municípios </returns>
    [HttpGet("all")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MunicipioDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<MunicipioDto>>>> GetAll()
    {
        var result = await _municipioService.GetAll();

        if (!result.Success)
        {
            if (result.Message.Contains("não foram encontrados", StringComparison.OrdinalIgnoreCase))
                return NotFound();

            return StatusCode(500, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Buscar um município por id
    /// </summary>
    /// <returns> Município do id </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<MunicipioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<MunicipioDto>>> GetById([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos"));

        var result = await _municipioService.GetById(id);

        if (!result.Success)
        {
            if (result.Message.Contains("não foi encontrado", StringComparison.OrdinalIgnoreCase))
                return NotFound();

            return StatusCode(500, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Buscar municípios por nome (parcial, case-insensitive)
    /// </summary>
    /// <returns> Lista de municípios que correspondem ao nome informado </returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MunicipioDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<MunicipioDto>>>> GetByName([FromQuery] string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos"));

        var result = await _municipioService.GetByName(nome);

        if (!result.Success)
        {
            if (result.Message.Contains("não foram encontrados", StringComparison.OrdinalIgnoreCase))
                return NotFound();

            return StatusCode(500, result);
        }

        return Ok(result);
    }
}