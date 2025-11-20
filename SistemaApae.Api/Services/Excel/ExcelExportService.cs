using ClosedXML.Excel;
using System.Reflection;
using SistemaApae.Api.Models.Reports;
using System.Linq.Expressions;
using System.Reflection;

namespace SistemaApae.Api.Services.Excel;

/// <summary>
/// Serviço genérico para exportação de listas em Excel a partir de DTOs anotados com ExcelColumnAttribute e ExcelHeaderAttribute.
/// </summary>
public static class ExcelExportService
{
	public static byte[] Export<T, TMeta>(IEnumerable<T> data, TMeta meta, string? sheetName = null)
	{
		var header = BuildHeader(meta);
		return Export(data, sheetName, header);
	}

	public static IEnumerable<KeyValuePair<string, string>> BuildHeader<T>(T meta)
	{
		if (meta == null) return Array.Empty<KeyValuePair<string, string>>();

		var type = typeof(T);
		var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
			.Select(p => new
			{
				Property = p,
				Attr = p.GetCustomAttribute<ExcelHeaderAttribute>()
			})
			.Where(x => x.Attr != null)
			.OrderBy(x => x.Attr!.Order)
			.ToList();

		if (props.Count == 0) return Array.Empty<KeyValuePair<string, string>>();

		var results = new List<KeyValuePair<string, string>>(props.Count);
		foreach (var p in props)
		{
			var value = p.Property.GetValue(meta);
			results.Add(new KeyValuePair<string, string>(p.Attr!.Label, value?.ToString() ?? string.Empty));
		}
		return results;
	}

	public static byte[] Export<T>(IEnumerable<T> data, string? sheetName = null, IEnumerable<KeyValuePair<string, string>>? header = null)
	{
		var items = data?.ToList() ?? new List<T>();
		var type = typeof(T);
		var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
			.Select(p => new
			{
				Property = p,
				Attr = p.GetCustomAttribute<ExcelColumnAttribute>()
			})
			.Where(x => x.Attr != null)
			.OrderBy(x => x.Attr!.Order)
			.ToList();

		// Se não houver atributos, usa todas as propriedades públicas na ordem natural
		if (props.Count == 0)
		{
			props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				.Select(p => new
				{
					Property = p,
					Attr = new ExcelColumnAttribute(p.Name) { Order = int.MaxValue }
				})
				.ToList();
		}

		using var wb = new XLWorkbook();
		var ws = wb.Worksheets.Add(string.IsNullOrWhiteSpace(sheetName) ? "Relatório" : sheetName);

		// Header (filtros) opcional: duas colunas (Label: | Valor)
		var currentRow = 1;
		if (header != null)
		{
			foreach (var kv in header)
			{
				ws.Cell(currentRow, 1).Value = string.IsNullOrWhiteSpace(kv.Key) ? string.Empty : kv.Key + ":";
				ws.Cell(currentRow, 1).Style.Font.Bold = true;
				ws.Cell(currentRow, 2).Value = kv.Value ?? string.Empty;
				ws.Cell(currentRow, 2).Style.Font.Bold = true;
				currentRow++;
			}
			currentRow++; // linha em branco
		}

		// Cabeçalho da tabela
		for (int i = 0; i < props.Count; i++)
		{
			var cell = ws.Cell(currentRow, i + 1);
			cell.Value = props[i].Attr!.Header;
			cell.Style.Font.Bold = true;
		}

		// Pré-compilar getters e writers por coluna para evitar reflexão no loop
		var getters = props.Select(p => CompileGetter<T>(p.Property)).ToArray();
		var writers = props.Select(p => BuildWriter(p.Property.PropertyType)).ToArray();

		// Linhas
		var row = currentRow + 1;
		foreach (var item in items)
		{
			for (int col = 0; col < props.Count; col++)
			{
				var value = getters[col](item);
				var cell = ws.Cell(row, col + 1);
				writers[col](cell, value);
			}
			row++;
		}

		ws.Columns(1, Math.Max(props.Count, 2)).AdjustToContents();

		using var ms = new MemoryStream();
		wb.SaveAs(ms);
		return ms.ToArray();
	}

	private static Func<T, object?> CompileGetter<T>(PropertyInfo prop)
	{
		var instance = Expression.Parameter(typeof(T), "x");
		Expression access = Expression.Property(Expression.Convert(instance, prop.DeclaringType!), prop);
		var convert = Expression.Convert(access, typeof(object));
		return Expression.Lambda<Func<T, object?>>(convert, instance).Compile();
	}

	private static Action<IXLCell, object?> BuildWriter(Type propertyType)
	{
		var underlying = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

		// DateOnly / TimeOnly precisam de tratamento específico
		if (underlying == typeof(DateOnly))
		{
			return (cell, value) =>
			{
				if (value is DateOnly d) cell.SetValue(d.ToDateTime(TimeOnly.MinValue));
				else cell.SetValue(string.Empty);
			};
		}
		if (underlying == typeof(TimeOnly))
		{
			return (cell, value) =>
			{
				if (value is TimeOnly t) cell.SetValue(t.ToTimeSpan());
				else cell.SetValue(string.Empty);
			};
		}

		// Enums como string
		if (underlying.IsEnum)
		{
			return (cell, value) => cell.SetValue(value?.ToString() ?? string.Empty);
		}

		switch (Type.GetTypeCode(underlying))
		{
			case TypeCode.String:
				return (cell, value) => cell.SetValue(value as string ?? string.Empty);
			case TypeCode.Boolean:
				return (cell, value) => cell.SetValue(value is bool b ? b : false);
			case TypeCode.DateTime:
				return (cell, value) => cell.SetValue(value is DateTime dt ? dt : default);
			case TypeCode.Byte:
				return (cell, value) => cell.SetValue(value is byte v ? v : default);
			case TypeCode.SByte:
				return (cell, value) => cell.SetValue(value is sbyte v ? v : default);
			case TypeCode.Int16:
				return (cell, value) => cell.SetValue(value is short v ? v : default);
			case TypeCode.UInt16:
				return (cell, value) => cell.SetValue(value is ushort v ? v : default);
			case TypeCode.Int32:
				return (cell, value) => cell.SetValue(value is int v ? v : default);
			case TypeCode.UInt32:
				return (cell, value) => cell.SetValue(value is uint v ? v : default);
			case TypeCode.Int64:
				return (cell, value) => cell.SetValue(value is long v ? v : default);
			case TypeCode.UInt64:
				return (cell, value) => cell.SetValue(value is ulong v ? v : default);
			case TypeCode.Single:
				return (cell, value) => cell.SetValue(value is float v ? v : default);
			case TypeCode.Double:
				return (cell, value) => cell.SetValue(value is double v ? v : default);
			case TypeCode.Decimal:
				return (cell, value) => cell.SetValue(value is decimal v ? v : default);
			default:
				return (cell, value) => cell.SetValue(value?.ToString() ?? string.Empty);
		}
	}
}


