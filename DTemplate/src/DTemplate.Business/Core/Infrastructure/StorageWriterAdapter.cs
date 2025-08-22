namespace DTemplate.Business.Core.Infrastructure
{
    using DTemplate.Business.Core.Services;
    using DTemplate.Domain.Contracts;
    using DTemplate.Persistence.Abstractions;

    /// <summary>
    /// Provides an implementation of <see cref="IStorageWriterAdapter"/> for saving, updating, and deleting entities using a database context.
    /// </summary>
    internal class StorageWriterAdapter : IStorageWriterAdapter
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageWriterAdapter"/> class.
        /// </summary>
        /// <param name="dbContext">The database context to use for storage operations.</param>
        public StorageWriterAdapter(IDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Asynchronously saves the specified entity to the database.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to save.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SaveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : BaseEntity
        {
            await _dbContext.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously updates the specified entity in the database.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : BaseEntity
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously deletes the specified entity from the database.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : BaseEntity
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
