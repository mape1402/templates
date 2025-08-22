using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using DTemplate.Api.Swagger;

namespace DTemplate.Api.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    internal static class SwaggerExtensions
    {
        internal static void AddSwaggerDefaults(this IServiceCollection services)
        {
            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc(SwaggerConstants.Docs.ApiVersion, new OpenApiInfo { Title = SwaggerConstants.Docs.ApiName, Version = SwaggerConstants.Docs.ApiVersion });

                opts.ResolveConflictingActions(x => x.First());
                opts.OperationFilter<RemoveVersionParametersFilter>();
                opts.DocumentFilter<SetVersionInPathsFilter>();
                opts.SchemaFilter<CIdSchemaFilter>();

                foreach (var fileName in Directory.GetFiles(AppContext.BaseDirectory, "*.xml"))
                    opts.IncludeXmlComments(fileName);
            });
        }

        internal static void UseSwaggerDefaults(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger(opts =>
            {
                opts.RouteTemplate = "/swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(opts =>
            {
                opts.RoutePrefix = string.Empty;
                opts.DocumentTitle = SwaggerConstants.Docs.ApiName;

                //Display
                opts.DefaultModelExpandDepth(3);
                opts.DefaultModelsExpandDepth(0);
                opts.DisplayRequestDuration();
                opts.EnableDeepLinking();
                opts.ShowExtensions();

                // Network
                opts.EnableValidator();

                if (env.IsDevelopment())
                {
                    opts.SwaggerEndpoint("/swagger/v1/swagger.json", "DTemplate.Api v1");
                }
            });
        }
    }
}