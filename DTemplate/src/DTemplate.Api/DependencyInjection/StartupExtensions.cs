using Crabalidator.DependencyInjection;
using DTemplate.Api.Boundaries;
using DTemplate.Api.DependencyInjection;
using DTemplate.Business;
using DTemplate.Persistence;
using OctoMap;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using TurtlePath.Crabalidator;
using TurtlePath.Domain.Identifier;
using TurtlePath.OctoMap;

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

            services.AddPigeon(configuration, builder =>
            {
                //builder
                    //.ScanConsumersFromAssemblies(typeof(Program).Assembly) // uncomment this line to scan for consumers in the current assembly
                    //.UseRabbitMq(); // uncomment this line to use RabbitMQ as the message broker
            });

            services.AddSpider(builder =>
            {
                builder.AddExecutionBoundary<TransactionExecutionBoundary>();
            });

            services.AddCrabalidator(typeof(Constants).Assembly);

            services.AddOctoMap(registration =>
            {
                registration.Options.EnableRuntimeImplicitMaps = true;
                registration.Options.DuplicateMapPolicy = DuplicateMapPolicy.Throw;
                registration.AddMaps(typeof(Constants).Assembly);
            });

            services.AddTurtlePath(typeof(Constants).Assembly)
                .UseOctoMap()
                .UseCrabalidator()
                .UseSieve()
                .UseCId<Ulid, string>(config =>
                {
                    config.DefaultFactory = () => CId.From(Ulid.NewUlid());
                    config.ConvertToDb = id => id.ToString();
                    config.ConvertFromDb = value => CId.From(Ulid.Parse(value));
                    config.JsonConverter = value => string.IsNullOrEmpty(value) ? CId.From(Ulid.Empty) : CId.From(Ulid.Parse(value));
                    config.NullableJsonConverter = value => string.IsNullOrEmpty(value) ? null : CId.From(Ulid.Parse(value));
                    config.ParseFunction = value => CId.From(Ulid.Parse(value));
                })
                .UseEntityFrameworkCore<AppDbContext>();

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
