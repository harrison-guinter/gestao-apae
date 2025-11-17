using SistemaApae.Api.Models.Enums;

namespace SistemaApae.Api.Models.Reports.Faltas;

/// <summary>
/// Item do relatório de faltas (ausências) por paciente
/// </summary>
public class FaltaReportItemDto
{
    public DateTime? DataAtendimento { get; set; }
    public StatusAtendimentoEnum? StatusFrequencia { get; set; }

    public string? NomeAssistido { get; set; }

    public string? NomeMunicipio { get; set; }

    public string? NomeProfissional { get; set; }

    public string? NomeConvenio { get; set; }

    public string? ObservacaoAtendimento { get; set; }
}

