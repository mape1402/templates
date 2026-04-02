using Microsoft.EntityFrameworkCore;
using DTemplate.Domain.Identifier;
using DTemplate.Persistence.Abstractions;

namespace DTemplate.Persistence
{
    /// <summary>
    /// Represents the EF Core database context for the application.
    /// </summary>
    public sealed class AppDbContext : DbContext, IDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to configure the context.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    var clrType = property.ClrType;
                    var underlyingType = Nullable.GetUnderlyingType(clrType);

                    if (clrType == typeof(CId) || underlyingType == typeof(CId))
                    {
                        property.SetValueConverter(CIdMetadata.DbConverter);

                        if(CIdMetadata.HasDbType)
                            property.SetColumnType(CIdMetadata.DbType);
                    }
                }

            }
        }
    }
}
