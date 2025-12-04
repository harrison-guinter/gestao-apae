using System.Collections.Generic;
using SistemaApae.Api.Models.Reports;

namespace SistemaApae.Api.Models.Reports.Presencas;

/// <summary>
/// Resultado do relatório de presenças com metadados (cabeçalho) e itens.
/// </summary>
public class PresencaListaReportDto
{
	[ExcelHeader("Mês", Order = 1)]
	public string Mes { get; set; } = string.Empty;

	[ExcelHeader("Período", Order = 2)]
	public string Periodo { get; set; } = string.Empty;

	[ExcelHeader("Município", Order = 3)]
	public string Municipio { get; set; } = string.Empty;

	[ExcelHeader("Convênio", Order = 4)]
	public string Convenio { get; set; } = string.Empty;

	public List<PresencaListaItemDto> Itens { get; set; } = new();
}

/// <summary>
/// Linha agregada do relatório de presença (entrega para municípios)
/// </summary>
public class PresencaListaItemDto
{
    [ExcelColumn("Nº", Order = 1)]
    public int Numero { get; set; }

    [ExcelColumn("Nome", Order = 2)]
    public string Nome { get; set; } = string.Empty;

    [ExcelColumn("Data Nascimento", Order = 3)]
    public string DataNascimento { get; set; } = string.Empty;

    [ExcelColumn("Endereço", Order = 4)]
    public string? Endereco { get; set; }

    [ExcelColumn("Tipo de Atendimento", Order = 5)]
    public string? TipoAtendimento { get; set; }

    [ExcelColumn("Dia Semana", Order = 6)]
    public string? DiaSemana { get; set; }

    [ExcelColumn("Dia Terapias", Order = 7)]
    public string? DiaTerapias { get; set; }

    [ExcelColumn("Turno", Order = 8)]
    public string? Turno { get; set; }
}


