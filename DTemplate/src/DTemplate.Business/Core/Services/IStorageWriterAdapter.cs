namespace DTemplate.Business.Core.Services
{
    using DTemplate.Domain.Contracts;

    /// <summary>
    /// Defines an abstraction for storage operations on entities, including save, update, delete, and retrieval.
    /// </summary>
    public interface IStorageWriterAdapter
    {
        /// <summary>
        /// Saves the entity to the storage.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to save.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SaveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : BaseEntity;
       
        /// <summary>
        /// Updates the entity in the storage.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : BaseEntity;

        /// <summary>
        /// Deletes an entity from the storage.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : BaseEntity;
    }
}
