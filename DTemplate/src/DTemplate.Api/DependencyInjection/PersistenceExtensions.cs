using Microsoft.EntityFrameworkCore;
using DTemplate.Persistence;
using DTemplate.Persistence.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    /// <summary>
    /// Provides dependency injection extensions for persistence.
    /// </summary>
    public static class PersistenceExtensions
    {
        /// <summary>
        /// Registers persistence services using the provided connection string.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="connectionString">The database connection string.</param>
        public static void AddPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(opts =>
            {
                // Uncomment the following line to use SQL Server
                opts.UseSqlServer(connectionString);

                // Uncomment the following line to use PostgreSQL
                //opts.UseNpgsql(connectionString);
            });

            services.AddScoped<IDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        }
    }
}
