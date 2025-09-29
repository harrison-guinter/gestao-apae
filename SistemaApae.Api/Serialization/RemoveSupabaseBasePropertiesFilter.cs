using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SistemaApae.Api.Serialization
{
    /// <summary>
    /// Schema filter que remove propriedades internas do Supabase BaseModel
    /// para evitar que apareçam no Swagger UI.
    /// </summary>
    public class RemoveSupabaseBasePropertiesFilter : ISchemaFilter
    {
        private static readonly HashSet<string> PropertiesToIgnore = new()
        {
            "BaseUrl",
            "RequestClientOptions",
            "GetHeaders",
            "TableName",
            "PrimaryKey"
        };

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties == null) return;

            foreach (var prop in PropertiesToIgnore)
            {
                schema.Properties.Remove(prop);
            }
        }
    }
}
