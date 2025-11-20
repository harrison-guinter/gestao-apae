using System;

namespace SistemaApae.Api.Models.Reports;

/// <summary>
/// Atributo para mapear propriedades de metadados para o cabeçalho do Excel.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class ExcelHeaderAttribute : Attribute
{
	public ExcelHeaderAttribute(string label)
	{
		Label = label;
	}

	/// <summary>
	/// Rótulo a ser exibido na coluna 1 do cabeçalho.
	/// </summary>
	public string Label { get; }

	/// <summary>
	/// Ordem de exibição (1-based). Menor aparece primeiro.
	/// </summary>
	public int Order { get; set; } = int.MaxValue;
}


