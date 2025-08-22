using Microsoft.EntityFrameworkCore;
using DTemplate.Domain.Identifier;
using DTemplate.Persistence.Abstractions;

namespace DTemplate.Persistence
{
    public sealed class AppDbContext : DbContext, IDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(CId))
                        property.SetValueConverter(CIdMetadata.DbConverter);

                    if (property.ClrType == typeof(CId?))
                        property.SetValueConverter(CIdMetadata.DbNulleableConverter);
                }

            }
        }
    }
}
