namespace DTemplate.Api.Swagger
{
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using DTemplate.Domain.Identifier;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Adjusts the schema for <see cref="CId"/> types in Swagger.
    /// </summary>
    public class CIdSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Applies the schema customization.
        /// </summary>
        /// <param name="schema">The schema to update.</param>
        /// <param name="context">The schema filter context.</param>
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
