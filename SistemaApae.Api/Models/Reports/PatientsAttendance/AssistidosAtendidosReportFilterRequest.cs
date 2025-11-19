using SistemaApae.Api.Models.Filters;

namespace SistemaApae.Api.Models.Reports.PatientsAttendance;

public class AssistidosAtendidosReportFilterRequest : BaseFilter
{
    /// <summary>
    /// Data de início do período
    /// </summary>
    public DateTime? DataInicio { get; set; }

    /// <summary>
    /// Data de fim do período
    /// </summary>
    public DateTime? DataFim { get; set; }

    /// <summary>
    /// Assistido
    /// </summary>
    public Guid IdAssistido { get; set; } = Guid.Empty;

    /// <summary>
    /// Profissional responsável pelo atendimento
    /// </summary>
    public Guid IdProfissional { get; set; } = Guid.Empty;

    /// <summary>
    /// Município do assistido
    /// </summary>
    public Guid IdMunicipio { get; set; } = Guid.Empty;
}
