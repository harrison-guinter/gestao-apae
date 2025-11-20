using System;

namespace SistemaApae.Api.Models.Reports;

/// <summary>
/// Atributo para mapear propriedades de DTO para colunas do Excel.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class ExcelColumnAttribute : Attribute
{
	public ExcelColumnAttribute(string header)
	{
		Header = header;
	}

	/// <summary>
	/// Texto do cabeçalho da coluna no Excel.
	/// </summary>
	public string Header { get; }

	/// <summary>
	/// Ordem da coluna (1-based). Se não informado, usa a ordem natural das propriedades.
	/// </summary>
	public int Order { get; set; } = int.MaxValue;
}


