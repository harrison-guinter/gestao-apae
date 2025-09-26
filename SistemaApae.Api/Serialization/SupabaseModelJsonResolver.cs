using Supabase.Postgrest.Models;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace SistemaApae.Api.Serialization;

/// <summary>
/// Json resolver que remove propriedades internas do Supabase BaseModel
/// para evitar serialização de metadados como BaseUrl, TableName e PrimaryKey.
/// </summary>
public sealed class SupabaseModelJsonResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var jsonTypeInfo = base.GetTypeInfo(type, options);

        if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object && typeof(BaseModel).IsAssignableFrom(type))
        {
            var propertiesToIgnore = new HashSet<string>(StringComparer.Ordinal)
            {
                "BaseUrl",
                "RequestClientOptions",
                "GetHeaders",
                "TableName",
                "PrimaryKey"
            };

            // Filtra a lista de propriedades mantendo somente as não ignoradas
            var filtered = jsonTypeInfo.Properties
                .Where(p => !propertiesToIgnore.Contains(p.Name))
                .ToList();

            jsonTypeInfo.Properties.Clear();
            foreach (var prop in filtered)
                jsonTypeInfo.Properties.Add(prop);
        }

        return jsonTypeInfo;
    }

}


