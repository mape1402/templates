using Microsoft.EntityFrameworkCore;
using DTemplate.Persistence;
using DTemplate.Persistence.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class PersistenceExtensions
    {
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
