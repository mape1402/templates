using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    internal static class HealthCheckExtensions
    {
        private const string TagServices = "services";

        internal static void AddHealthChecks(this IServiceCollection services, string connectionString)
        {
            services.AddHealthChecks()
                .AddCheck("Self", () => HealthCheckResult.Healthy());
        }

        internal static void MapHealthCheckEndPoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks("/health");
            endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("Self")
            });
            endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains(TagServices)
            });
        }
    }
}
