using SistemaApae.Api.Models.Appointment;

namespace SistemaApae.Api.Models.Reports.PatientsAttendance;

public class AssistidosAtendidosReportDto
{
    /// <summary>
    /// Atendimento do assistido
    /// </summary>
    public AtendimentoAssistidoDto Atendimento { get; set; }

    /// <summary>
    /// Profissional responsável pelo atendimento
    /// </summary>
    public ProfissionalAtendimentoDto Profissional { get; set; }

    /// <summary>
    /// Assistido atendido
    /// </summary>
    public AssistidoAtendimentoDto Assistido { get; set; }

    /// <summary>
    /// Municipio de origem do assistido
    /// </summary>
    public MunicipioAssistidoDto Municipio { get; set; }
}

public class AtendimentoAssistidoDto
{
    public AtendimentoAssistidoDto(Guid id, DateTime? data)
    {
        Id = id;
        Data = data;
    }

    /// <summary>
    /// ID do atendimento
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Data do atendimento
    /// </summary>
    public DateTime? Data { get; set; }
}

public class MunicipioAssistidoDto
{
    public MunicipioAssistidoDto(Guid? id, string? nome)
    {
        Id = id;
        Nome = nome;
    }

    /// <summary>
    /// ID do municipio do assistido
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Nome do municipio
    /// </summary>
    public string? Nome { get; set; } = string.Empty;
}



