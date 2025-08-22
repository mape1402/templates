namespace DTemplate.Business.Core.Commands
{
    using Microsoft.Extensions.DependencyInjection;
    using DTemplate.Business.Core.Exceptions;
    using DTemplate.Business.Core.Models.Requests;
    using DTemplate.Business.Core.Services;
    using DTemplate.Domain.Contracts;
    using Pelican.Mediator;

    /// <summary>
    /// Provides a base implementation for handling delete commands that return a response, including entity retrieval, validation, deletion, and response building.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <typeparam name="TEntity">The type of the entity being deleted.</typeparam>
    public abstract class DeleteCommandHandler<TRequest, TResponse, TEntity> : BaseCommandHandler<TRequest, TResponse>
        where TRequest : BaseRequest, IRequest<TResponse>
        where TResponse : class
        where TEntity : BaseEntity
    {
        /// <summary>
        /// Gets the service provider used to resolve dependencies.
        /// </summary>
        protected IServiceProvider Services { get; }

        /// <summary>
        /// Gets the storage adapter for deleting entities.
        /// </summary>
        protected IStorageWriterAdapter StorageWriterAdapter { get; }

        /// <summary>
        /// Gets the storage adapter for reading entities.
        /// </summary>
        protected IStorageReaderAdapter StorageReaderAdapter { get; }

        /// <summary>
        /// Gets the validator adapter for validating requests.
        /// </summary>
        protected IValidatorAdapter ValidatorAdapter { get; }

        /// <summary>
        /// Gets the mapper adapter for mapping between types.
        /// </summary>
        protected IMapperAdapter MapperAdapter { get; }

        /// <summary>
        /// Gets a value indicating whether the request should be validated before processing.
        /// </summary>
        protected virtual bool ValidateRequest => false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler{TRequest, TResponse, TEntity}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
        protected DeleteCommandHandler(IServiceProvider serviceProvider)
        {
            Services = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            StorageWriterAdapter = serviceProvider.GetRequiredService<IStorageWriterAdapter>();
            StorageReaderAdapter = serviceProvider.GetRequiredService<IStorageReaderAdapter>();
            ValidatorAdapter = serviceProvider.GetRequiredService<IValidatorAdapter>();
            MapperAdapter = serviceProvider.GetRequiredService<IMapperAdapter>();
        }

        /// <summary>
        /// Handles the delete command by retrieving, validating, deleting the entity, and building the response.
        /// </summary>
        /// <param name="request">The request to handle.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, with the response for the delete command as the result.</returns>
        public override async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await GetEntityAsync(request, cancellationToken);
            await ValidateAsync(request, entity, cancellationToken);    
            await DeleteEntityAsync(entity, cancellationToken);
            return await BuildResponseAsync(request, entity, cancellationToken);
        }

        /// <summary>
        /// Retrieves the entity to be deleted based on the request. Throws <see cref="NotFoundException"/> if the entity is not found.
        /// </summary>
        /// <param name="request">The request containing information to identify the entity.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, with the entity as the result.</returns>
        /// <exception cref="NotFoundException">Thrown if the entity is not found.</exception>
        protected virtual async Task<TEntity> GetEntityAsync(TRequest request, CancellationToken cancellationToken)
        {
            var criteria = new GetOneCriteria<TEntity>
            {
                FiltersExpression = e => e.Id == request.Id
            };

            return await StorageReaderAdapter.GetOneAsync<TEntity, TEntity>(criteria, cancellationToken)
                ?? throw new NotFoundException(typeof(TEntity).Name, request.Id.ToString());
        }

        /// <summary>
        /// Validates the request and entity using the validator adapter if <see cref="ValidateRequest"/> is <c>false</c>.
        /// </summary>
        /// <param name="request">The request to validate.</param>
        /// <param name="entity">The entity to validate.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A ValueTask representing the asynchronous validation operation.</returns>
        protected virtual ValueTask ValidateAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)
        {
            if(!ValidateRequest)
                return ValueTask.CompletedTask;

            return ValidatorAdapter.ValidateAsync(request, cancellationToken);
        }

        /// <summary>
        /// Deletes the entity using the storage adapter.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous delete operation.</returns>
        protected virtual Task DeleteEntityAsync(TEntity entity, CancellationToken cancellationToken)
            => StorageWriterAdapter.DeleteAsync(entity, cancellationToken);

        /// <summary>
        /// Builds the response after the entity has been deleted.
        /// </summary>
        /// <param name="request">The request associated with the entity.</param>
        /// <param name="entity">The deleted entity.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A ValueTask representing the asynchronous operation, with the response as the result.</returns>
        protected abstract ValueTask<TResponse> BuildResponseAsync(TRequest request, TEntity entity, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Provides a base implementation for handling delete commands that do not return a response, including entity retrieval, validation, and deletion.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TEntity">The type of the entity being deleted.</typeparam>
    public abstract class DeleteCommandHandler<TRequest, TEntity> : NoReturnCommandHandler<TRequest>
        where TRequest : BaseRequest, IRequest
        where TEntity : BaseEntity
    {
        /// <summary>
        /// Gets the service provider used to resolve dependencies.
        /// </summary>
        protected IServiceProvider Services { get; }

        /// <summary>
        /// Gets the storage adapter for deleting entities.
        /// </summary>
        protected IStorageWriterAdapter StorageWriterAdapter { get; }

        /// <summary>
        /// Gets the storage adapter for reading entities.
        /// </summary>
        protected IStorageReaderAdapter StorageReaderAdapter { get; }

        /// <summary>
        /// Gets the validator adapter for validating requests.
        /// </summary>
        protected IValidatorAdapter ValidatorAdapter { get; }

        /// <summary>
        /// Gets the mapper adapter for mapping between types.
        /// </summary>
        protected IMapperAdapter MapperAdapter { get; }

        /// <summary>
        /// Gets a value indicating whether the request should be validated before processing.
        /// </summary>
        protected virtual bool ValidateRequest => false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler{TRequest, TEntity}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
        protected DeleteCommandHandler(IServiceProvider serviceProvider)
        {
            Services = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            StorageWriterAdapter = serviceProvider.GetRequiredService<IStorageWriterAdapter>();
            StorageReaderAdapter = serviceProvider.GetRequiredService<IStorageReaderAdapter>();
            ValidatorAdapter = serviceProvider.GetRequiredService<IValidatorAdapter>();
            MapperAdapter = serviceProvider.GetRequiredService<IMapperAdapter>();
        }

        /// <summary>
        /// Handles the delete command by retrieving, validating, and deleting the entity.
        /// </summary>
        /// <param name="request">The request to handle.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await GetEntityAsync(request, cancellationToken);
            await ValidateAsync(request, entity, cancellationToken);
            await DeleteEntityAsync(entity, cancellationToken);
        }

        /// <summary>
        /// Retrieves the entity to be deleted based on the request. Throws <see cref="NotFoundException"/> if the entity is not found.
        /// </summary>
        /// <param name="request">The request containing information to identify the entity.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, with the entity as the result.</returns>
        /// <exception cref="NotFoundException">Thrown if the entity is not found.</exception>
        protected virtual async Task<TEntity> GetEntityAsync(TRequest request, CancellationToken cancellationToken)
        {
            var criteria = new GetOneCriteria<TEntity>
            {
                FiltersExpression = e => e.Id == request.Id
            };

            return await StorageReaderAdapter.GetOneAsync<TEntity, TEntity>(criteria, cancellationToken)
                ?? throw new NotFoundException(typeof(TEntity).Name, request.Id.ToString());
        }

        /// <summary>
        /// Validates the request and entity using the validator adapter if <see cref="ValidateRequest"/> is <c>false</c>.
        /// </summary>
        /// <param name="request">The request to validate.</param>
        /// <param name="entity">The entity to validate.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A ValueTask representing the asynchronous validation operation.</returns>
        protected virtual ValueTask ValidateAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)
        {
            if (!ValidateRequest)
                return ValueTask.CompletedTask;

            return ValidatorAdapter.ValidateAsync(request, cancellationToken);
        }

        /// <summary>
        /// Deletes the entity using the storage adapter.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous delete operation.</returns>
        protected virtual Task DeleteEntityAsync(TEntity entity, CancellationToken cancellationToken)
            => StorageWriterAdapter.DeleteAsync(entity, cancellationToken);
    }
}
