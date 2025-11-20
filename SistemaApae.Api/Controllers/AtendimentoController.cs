using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaApae.Api.Models.Appointment;
using SistemaApae.Api.Models.Appointments;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Reports.Faltas;
using SistemaApae.Api.Models.Reports.PatientsAttendance;
using SistemaApae.Api.Models.Reports.Presencas;
using SistemaApae.Api.Services.Appointment;
using SistemaApae.Api.Services.Excel;
using System.Globalization;

namespace SistemaApae.Api.Controllers;

/// <summary>
/// Controller com endpoints de CRUD da entidade Atendimento
/// </summary>
[ApiController]
//[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class AtendimentoController : ControllerBase
{
    private readonly AtendimentoService _atendimentoService;

    /// <summary>
    /// Inicializa uma nova instância do AtendimentoController
    /// </summary>
    public AtendimentoController(AtendimentoService atendimentoService)
    {
        _atendimentoService = atendimentoService;
    }

    /// <summary>
    /// Lista atendimentos por filtros de pesquisa
    /// </summary>
    /// /// <returns> Lista de Atendimento dos filtros de pesquisa </returns>
    [HttpGet("filter")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<Atendimento>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<Atendimento>>>> GetAppointmentByFilters([FromQuery] AtendimentoFilterRequest filters)
    {
        var result = await _atendimentoService.GetByFilters(filters);

        if (!result.Success)
        {
            if (result.Message.Contains("Registros não foram encontrados"))
                return NotFound();

            return StatusCode(500, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Relatório de presenças para entrega aos municípios
    /// </summary>
    /// <remarks>
    /// Filtra por data (início/fim), IdProfissional, IdMunicipio, IdConvenio.
    /// </remarks>
    [HttpGet("reports/presencas")]
    [ProducesResponseType(typeof(ApiResponse<PresencaListaReportDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PresencaListaReportDto>>> GetPresencasLista([FromQuery] PresencaReportFilterRequest filtros)
    {
        var result = await _atendimentoService.GetRelatorioPresencasLista(filtros);
        return Ok(result);
    }

    /// <summary>
    /// Exporta o relatório de presenças em Excel
    /// </summary>
    [HttpGet("reports/presencas/excel")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportPresencasExcel([FromQuery] PresencaReportFilterRequest filtros)
    {
        var detailed = await _atendimentoService.GetRelatorioPresencasLista(filtros);
        if (!detailed.Success || detailed.Data == null)
            return StatusCode(500, ApiResponse<object>.ErrorResponse("Falha ao obter dados para exportação."));

        var bytes = ExcelExportService.Export(detailed.Data.Itens, detailed.Data, "Presenças");
        var fileName = $"presencas_{DateTime.Now:yyyyMMddHHmm}.xlsx";
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

    /// <summary>
    /// Relatório de faltas (ausências) por paciente
    /// </summary>
    /// <remarks>
    /// Filtra por profissional, data (início/fim) e município. Retorna somente atendimentos com status FALTA ou JUSTIFICADA.
    /// </remarks>
    [HttpGet("reports/faltas")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<FaltaReportItemDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<FaltaReportItemDto>>>> GetAbsencesReport([FromQuery] FaltaReportFilterRequest filtros)
    {
        var result = await _atendimentoService.GetRelatorioFaltas(filtros);

        if (!result.Success && !string.IsNullOrWhiteSpace(result.Message))
            return StatusCode(500, result);

        return Ok(result);
    }

    /// <summary>
    /// Relatório de assistidos atendidos
    /// </summary>
    /// <remarks>
    /// Filtra por assistido, profissional, data (início/fim) e município.
    /// </remarks>
    [HttpGet("reports/assistidos")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AssistidosAtendidosReportDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<AssistidosAtendidosReportDto>>>> GetPatientsAttendanceReport(
        [FromQuery] AssistidosAtendidosReportFilterRequest filtros
    )
    {
        var result = await _atendimentoService.GetPatientsAttendanceReport(filtros);

        if (!result.Success && !string.IsNullOrWhiteSpace(result.Message))
            return StatusCode(500, result);

        return Ok(result);
    }

    /// <summary>
    /// Buscar um atendimento por id
    /// </summary>
    /// <returns> Atendimento do id </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<Atendimento>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<Atendimento>>> GetAppointmentById([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos"));

        var result = await _atendimentoService.GetById(id);

        if (!result.Success)
        {
            if (result.Message.Contains("Registro não foi encontrado"))
                return NotFound();

            return StatusCode(500, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Criar um atendimento
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> CreateAppointment([FromBody] AtendimentoCreateDto appointment)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos", errors));
        }

        var result = await _atendimentoService.Create(appointment);

        if (!result.Success)
        {
            if (result.Message.Contains("Registro não foi adicionado"))
                return NoContent();

            if (result.Message.Contains("não existe"))
                return BadRequest(ApiResponse<object>.ErrorResponse(result.Message));

            return StatusCode(500, result);
        }

        return Created();
    }

    /// <summary>
    /// Atualiza um atendimento existente
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> UpdateAppointment([FromBody] Atendimento appointment)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object>.ErrorResponse("Dados de entrada inválidos", errors));
        }

        var result = await _atendimentoService.Update(appointment);

        if (!result.Success)
        {
            if (result.Message.Contains("Registro não foi atualizado"))
                return NoContent();

            if (result.Message.Contains("não existe"))
                return BadRequest(ApiResponse<object>.ErrorResponse(result.Message));

            return StatusCode(500, result);
        }

        return Ok();
    }
}
