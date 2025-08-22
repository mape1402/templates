using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DTemplate.Domain.Contracts;
using DTemplate.Domain.Identifier;

namespace DTemplate.Persistence.Configurations
{
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd().HasValueGenerator(typeof(CIdDbValueGenerator));
        }
    }
}
