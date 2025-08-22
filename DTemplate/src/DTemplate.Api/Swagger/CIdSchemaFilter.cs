namespace DTemplate.Api.Swagger
{
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using DTemplate.Domain.Identifier;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class CIdSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(CId))
            {
                schema.Type = "string";
                schema.Format = "string";
                schema.Pattern = "";
                schema.Example = new OpenApiString(Ulid.NewUlid().ToString());
            }
        }
    }
}
