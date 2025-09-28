using Microsoft.AspNetCore.Mvc;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Administrative;
using SistemaApae.Api.Services.Administrative;
using SistemaApae.Api.Models.Patients;
using SistemaApae.Api.Services;

namespace SistemaApae.Api.Controllers.Administrative;

/// <summary>
/// Controller com endpoints de leitura para a entidade Município
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MunicipioController : ControllerBase
{
    private readonly IService<Municipio, MunicipioFiltroRequest> _service;
    public MunicipioController(IService<Municipio, MunicipioFiltroRequest> service)
    {
        _service = service;
    }

    /// <summary>
    /// Buscar um município por id
    /// </summary>
    /// <returns> Município do id </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<Municipio>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<Municipio>>> GetById([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos"));

        var result = await _service.GetById(id);

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
    [HttpGet("filter")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<Municipio>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<Assistido>>>> GetByFilters([FromQuery] MunicipioFiltroRequest request)
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
}