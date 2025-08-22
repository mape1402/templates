namespace DTemplate.Business.Core.Commands
{
    using Microsoft.Extensions.DependencyInjection;
    using DTemplate.Business.Core.Exceptions;
    using DTemplate.Business.Core.Models.Requests;
    using DTemplate.Business.Core.Models.Responses;
    using DTemplate.Business.Core.Services;
    using DTemplate.Domain.Contracts;
    using Pelican.Mediator;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides a base implementation for handling update commands that return a response, including entity retrieval, validation, mapping, updating, and response mapping.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <typeparam name="TEntity">The type of the entity being updated.</typeparam>
    public abstract class UpdateCommandHandler<TRequest, TResponse, TEntity> : BaseCommandHandler<TRequest, TResponse>
        where TRequest : BaseRequest, IRequest<TResponse>
        where TResponse : BaseResponse
        where TEntity : BaseEntity
    {
        /// <summary>
        /// Gets the service provider used to resolve dependencies.
        /// </summary>
        protected IServiceProvider Services { get; }

        /// <summary>
        /// Gets the storage adapter for saving and updating entities.
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
        protected virtual bool ValidateRequest => true;

        /// <summary>
        /// Gets a value indicating whether to use a projection from storage for the response mapping.
        /// </summary>
        protected virtual bool UseProjectionFromStorage => false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler{TRequest, TResponse, TEntity}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
        protected UpdateCommandHandler(IServiceProvider serviceProvider)
        {
            Services = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            StorageWriterAdapter = Services.GetRequiredService<IStorageWriterAdapter>();
            StorageReaderAdapter = Services.GetRequiredService<IStorageReaderAdapter>();
            ValidatorAdapter = Services.GetRequiredService<IValidatorAdapter>();
            MapperAdapter = Services.GetRequiredService<IMapperAdapter>();
        }

        /// <summary>
        /// Handles the update command by retrieving, validating, mapping, updating the entity, and returning the response.
        /// </summary>
        /// <param name="request">The request to handle.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, with the response for the update command as the result.</returns>
        public override async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await GetEntityAsync(request, cancellationToken);
            await ValidateAsync(request, entity, cancellationToken);
            await MapEntityAsync(request, entity, cancellationToken);
            await UpdateEntityAsync(request, entity, cancellationToken);           
            return await MapToResponseAsync(request, entity, cancellationToken);
        }

        /// <summary>
        /// Retrieves the entity to be updated based on the request. Throws <see cref="NotFoundException"/> if the entity is not found.
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
        /// Validates the request and entity using the validator adapter if <see cref="ValidateRequest"/> is <c>true</c>.
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
        /// Maps the request onto the entity using the mapper adapter.
        /// </summary>
        /// <param name="request">The request containing updated values.</param>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A ValueTask representing the asynchronous mapping operation.</returns>
        protected virtual ValueTask MapEntityAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)
            => MapperAdapter.UpdateMapAsync(request, entity, cancellationToken);

        /// <summary>
        /// Updates the entity in the storage using the storage adapter.
        /// </summary>
        /// <param name="request">The request associated with the entity.</param>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous update operation.</returns>
        protected virtual Task UpdateEntityAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)
            => StorageWriterAdapter.UpdateAsync(entity, cancellationToken);

        /// <summary>
        /// Maps the updated entity to a response using the mapper adapter or retrieves a projection from storage if <see cref="UseProjectionFromStorage"/> is <c>true</c>.
        /// </summary>
        /// <param name="request">The request associated with the entity.</param>
        /// <param name="entity">The updated entity to map to a response.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A ValueTask representing the asynchronous mapping operation, with the mapped response as the result.</returns>
        protected virtual async ValueTask<TResponse> MapToResponseAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)
            => UseProjectionFromStorage
                ? await StorageReaderAdapter.GetOneAsync<TEntity, TResponse>(new GetOneCriteria<TEntity>
                {
                    FiltersExpression = e => e.Id == request.Id,
                    UseTracking = false,
                }, cancellationToken)
                : await MapperAdapter.MapAsync<TEntity, TResponse>(entity, cancellationToken);
    }

    /// <summary>
    /// Provides a base implementation for handling update commands that do not return a response, including entity retrieval, validation, mapping, and updating.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TEntity">The type of the entity being updated.</typeparam>
    public abstract class UpdateCommandHandler<TRequest, TEntity> : NoReturnCommandHandler<TRequest>
        where TRequest : BaseRequest, IRequest
        where TEntity : BaseEntity
    {
        /// <summary>
        /// Gets the service provider used to resolve dependencies.
        /// </summary>
        protected IServiceProvider Services { get; }

        /// <summary>
        /// Gets the storage adapter for saving and updating entities.
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
        protected virtual bool ValidateRequest => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler{TRequest, TEntity}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
        protected UpdateCommandHandler(IServiceProvider serviceProvider)
        {
            Services = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            StorageWriterAdapter = Services.GetRequiredService<IStorageWriterAdapter>();
            StorageReaderAdapter = Services.GetRequiredService<IStorageReaderAdapter>();
            ValidatorAdapter = Services.GetRequiredService<IValidatorAdapter>();
            MapperAdapter = Services.GetRequiredService<IMapperAdapter>();
        }

        /// <summary>
        /// Handles the update command by retrieving, validating, mapping, and updating the entity.
        /// </summary>
        /// <param name="request">The request to handle.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await GetEntityAsync(request, cancellationToken);
            await ValidateAsync(request, entity, cancellationToken);
            await MapEntityAsync(request, entity, cancellationToken);
            await UpdateEntityAsync(request, entity, cancellationToken);
        }

        /// <summary>
        /// Retrieves the entity to be updated based on the request. Throws <see cref="NotFoundException"/> if the entity is not found.
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
        /// Validates the request and entity using the validator adapter if <see cref="ValidateRequest"/> is <c>true</c>.
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
        /// Maps the request onto the entity using the mapper adapter.
        /// </summary>
        /// <param name="request">The request containing updated values.</param>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A ValueTask representing the asynchronous mapping operation.</returns>
        protected virtual ValueTask MapEntityAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)
            => MapperAdapter.UpdateMapAsync(request, entity, cancellationToken);

        /// <summary>
        /// Updates the entity in the storage using the storage adapter.
        /// </summary>
        /// <param name="request">The request associated with the entity.</param>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous update operation.</returns>
        protected virtual Task UpdateEntityAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)
            => StorageWriterAdapter.UpdateAsync(entity, cancellationToken);
    }
}
