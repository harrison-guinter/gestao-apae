using SistemaApae.Api.Models.Filters;
using System;

namespace SistemaApae.Api.Models.Reports.Presencas;

/// <summary>
/// Filtros para geração do Relatório de Presença
/// </summary>
public class PresencaReportFilterRequest : BaseFilter
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
	/// Profissional responsável
	/// </summary>
	public Guid IdProfissional { get; set; } = Guid.Empty;

	/// <summary>
	/// Município do assistido
	/// </summary>
	public Guid IdMunicipio { get; set; } = Guid.Empty;

    /// <summary>
    /// Convênio do assistido
    /// </summary>
    public Guid IdConvenio { get; set; } = Guid.Empty;
}





