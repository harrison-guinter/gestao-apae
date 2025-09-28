using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SistemaApae.Api.Serialization;

/// <summary>
/// Adiciona ao schema dos enums a lista de valores no formato "Nome = Número"
/// </summary>
public class EnumDescriptionsSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var type = context.Type;
        if (!type.IsEnum)
            return;

        var names = Enum.GetNames(type);
        var values = Enum.GetValues(type);

        var lines = new List<string>();
        for (int i = 0; i < names.Length; i++)
        {
            var name = names[i];
            var value = Convert.ToInt64(values.GetValue(i)!);
            lines.Add($"{name} = {value}");
        }

        var appendix = string.Join("; ", lines);
        var prefix = string.IsNullOrWhiteSpace(schema.Description) ? string.Empty : schema.Description + "\n";
        schema.Description = prefix + "Valores: " + appendix;
    }
}


