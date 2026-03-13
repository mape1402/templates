using DTemplate.Api.DependencyInjection;
using DTemplate.Business;
using DTemplate.Business.Core.Exceptions;
using DTemplate.Business.Core.PipelineBehaviors;
using DTemplate.Domain.Identifier;
using Pelican.Mediator;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    /// <summary>
    /// Provides startup service configuration extensions.
    /// </summary>
    public static class StartupExtensions
    {
        /// <summary>
        /// Registers default services and middleware dependencies.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="environment">The hosting environment.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddDefaults(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var connectionString = configuration.GetConnectionString("Default");

            services.AddEndpointsApiExplorer();
            services.AddSwaggerDefaults();

            services.AddPersistence(connectionString);
            services.AddPelican(typeof(Constants).Assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>));

            services.AddPigeon(configuration, builder =>
            {
                //builder
                    //.ScanConsumersFromAssemblies(typeof(Program).Assembly) // ucomment this line to scan for consumers in the current assembly
                    //.UseRabbitMq(); // uncomment this line to use RabbitMQ as the message broker
            });

            services.AddSpider();
            services.AddOrchestrationWorking()
                .AddErrorCodeBinding<BadRequestException>(400)
                .AddErrorCodeBinding<NotFoundException>(404)
                .AddErrorCodeBinding<Exception>(500);

            services.AddBusiness();

            services.UseCId<Ulid, string>(config =>
            {
                config.DefaultFactory = () => new CId(Ulid.NewUlid());
                config.ConvertToDb = id => id.ToString();
                config.ConvertFromDb = value => CId.Parse(value);
                config.ConvertToDbNullable = id => id == null ? null : id.ToString();
                config.ConvertFromDbNullable = value => string.IsNullOrWhiteSpace(null) ? null : CId.Parse(value);
                config.JsonConverter = value => string.IsNullOrEmpty(value) ? new CId(Ulid.Empty) : CId.Parse(value);
                config.NulleableJsonConverter = value => string.IsNullOrEmpty(value) ? null : CId.Parse(value);
                config.ParseFunction = value => new CId(Ulid.Parse(value));
            });

            services.AddMvcDefaults();

            services.AddHealthChecks(connectionString); // TODO: Add health checks for Database and other services

            return services;
        }
        
        /// <summary>
        /// Configures default middleware for the application.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="environment">The hosting environment.</param>
        /// <returns>The application builder.</returns>
        public static IApplicationBuilder UseDefaults(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseSerilogRequestLogging();
                app.UseSwaggerDefaults(environment);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthCheckEndPoints();
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
