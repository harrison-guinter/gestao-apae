using SistemaApae.Api.Models.Enums;

namespace SistemaApae.Api.Models.Reports.Faltas;

/// <summary>
/// Item do relatório de faltas (ausências) por paciente
/// </summary>
public class FaltaReportItemDto
{
    public Guid IdAtendimento { get; set; }
    public DateTime? DataAtendimento { get; set; }
    public StatusAtendimentoEnum? StatusFrequencia { get; set; }

    public Guid IdAssistido { get; set; }
    public string? NomeAssistido { get; set; }

    public Guid? IdMunicipio { get; set; }
    public string? NomeMunicipio { get; set; }

    public Guid IdProfissional { get; set; }
    public string? NomeProfissional { get; set; }
}

