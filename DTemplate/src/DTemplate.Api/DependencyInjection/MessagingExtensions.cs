using Pigeon.Messaging.Azure.ServiceBus;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    internal static class MessagingExtensions
    {
        internal static IServiceCollection AddMessagingDefaults(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPigeon(configuration, builder =>
            {
                // Uncomment this line to scan for consumers in the current assembly:
                // builder.ScanConsumersFromAssemblies(typeof(Program).Assembly);

                builder.UseAzureServiceBus();
            });

            return services;
        }
    }
}
