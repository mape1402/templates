namespace DTemplate.Business.Core.Commands
{
    using Microsoft.Extensions.DependencyInjection;
    using DTemplate.Business.Core.Models.Responses;
    using DTemplate.Business.Core.Services;
    using DTemplate.Domain.Contracts;
    using Pelican.Mediator;
    using System;

    /// <summary>
    /// Provides a base implementation for handling create commands that return a response, including validation, mapping, saving, and response mapping.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <typeparam name="TEntity">The type of the entity being created.</typeparam>
    public abstract class CreateCommandHandler<TRequest, TResponse, TEntity> : BaseCommandHandler<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
        where TEntity : BaseEntity
        where TResponse : BaseResponse
    {
        /// <summary>
        /// Gets the service provider used to resolve dependencies.
        /// </summary>
        protected IServiceProvider Services { get; }

        /// <summary>
        /// Gets the storage adapter for saving entities.
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
        /// Initializes a new instance of the <see cref="CreateCommandHandler{TRequest, TResponse, TEntity}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
        protected CreateCommandHandler(IServiceProvider serviceProvider)
        {
            Services = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            StorageWriterAdapter = Services.GetRequiredService<IStorageWriterAdapter>();
            StorageReaderAdapter = Services.GetRequiredService<IStorageReaderAdapter>();
            ValidatorAdapter = Services.GetRequiredService<IValidatorAdapter>();
            MapperAdapter = Services.GetRequiredService<IMapperAdapter>();
        }

        /// <summary>
        /// Handles the create command by validating, mapping, saving, and returning the response.
        /// </summary>
        /// <param name="request">The request to handle.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, with the response for the create command as the result.</returns>
        public override async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            await ValidateAsync(request, cancellationToken);
            var entity = await MapToEntityAsync(request, cancellationToken);
            await SaveEntityAsync(request, entity, cancellationToken);
            return await MapToResponseAsync(request, entity, cancellationToken);
        }

        /// <summary>
        /// Validates the request using the validator adapter if <see cref="ValidateRequest"/> is <c>true</c>.
        /// </summary>
        /// <param name="request">The request to validate.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A ValueTask representing the asynchronous validation operation.</returns>
        protected virtual ValueTask ValidateAsync(TRequest request, CancellationToken cancellationToken)
        {
            if (!ValidateRequest)
                return ValueTask.CompletedTask;

            return ValidatorAdapter.ValidateAsync(request, cancellationToken);
        }

        /// <summary>
        /// Maps the request to an entity using the mapper adapter.
        /// </summary>
        /// <param name="request">The request to map.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A ValueTask representing the asynchronous mapping operation, with the mapped entity as the result.</returns>
        protected virtual ValueTask<TEntity> MapToEntityAsync(TRequest request, CancellationToken cancellationToken)
            => MapperAdapter.MapAsync<TRequest, TEntity>(request, cancellationToken);

        /// <summary>
        /// Saves the entity using the storage adapter.
        /// </summary>
        /// <param name="request">The request associated with the entity.</param>
        /// <param name="entity">The entity to save.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A Task representing the asynchronous save operation.</returns>
        protected virtual Task SaveEntityAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)
            => StorageWriterAdapter.SaveAsync(entity, cancellationToken);

        /// <summary>
        /// Maps the entity to a response using the mapper adapter or retrieves a projection from storage if <see cref="UseProjectionFromStorage"/> is <c>true</c>.
        /// </summary>
        /// <param name="request">The request associated with the entity.</param>
        /// <param name="entity">The entity to map to a response.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A ValueTask representing the asynchronous mapping operation, with the mapped response as the result.</returns>
        protected virtual async ValueTask<TResponse> MapToResponseAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)
            => UseProjectionFromStorage ?
               await StorageReaderAdapter.GetOneAsync<TEntity, TResponse>(new GetOneCriteria<TEntity>
               {
                   FiltersExpression = e => e.Id == entity.Id,
                   UseTracking = false,
               }, cancellationToken) :
               await MapperAdapter.MapAsync<TEntity, TResponse>(entity, cancellationToken);
    }

    /// <summary>
    /// Provides a base implementation for handling create commands that do not return a response, including validation, mapping, and saving.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TEntity">The type of the entity being created.</typeparam>
    public abstract class CreateCommandHandler<TRequest, TEntity> : NoReturnCommandHandler<TRequest>
        where TRequest : class, IRequest
        where TEntity : BaseEntity
    {
        /// <summary>
        /// Gets the storage adapter for saving entities.
        /// </summary>
        protected IStorageWriterAdapter StorageWriterAdapter { get; }

        /// <summary>
        /// Gets the validator adapter for validating requests.
        /// </summary>
        protected IValidatorAdapter ValidatorAdapter { get; }

        /// <summary>
        /// Gets the mapper adapter for mapping between types.
        /// </summary>
        protected IMapperAdapter MapperAdapter { get; }

        /// <summary>
        /// Gets the service provider used to resolve dependencies.
        /// </summary>
        protected IServiceProvider Services { get; }

        /// <summary>
        /// Gets a value indicating whether the request should be validated before processing.
        /// </summary>
        protected virtual bool ValidateRequest => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler{TRequest, TEntity}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
        protected CreateCommandHandler(IServiceProvider serviceProvider)
        {
            Services = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            StorageWriterAdapter = Services.GetRequiredService<IStorageWriterAdapter>();
            ValidatorAdapter = Services.GetRequiredService<IValidatorAdapter>();
            MapperAdapter = Services.GetRequiredService<IMapperAdapter>();
        }

        /// <summary>
        /// Handles the create command by validating, mapping, and saving the entity.
        /// </summary>
        /// <param name="request">The request to handle.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public override async Task Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            await ValidateAsync(request, cancellationToken);
            var entity = await MapToEntityAsync(request, cancellationToken);
            await SaveEntityAsync(request, entity, cancellationToken);
        }

        /// <summary>
        /// Validates the request using the validator adapter if <see cref="ValidateRequest"/> is <c>true</c>.
        /// </summary>
        /// <param name="request">The request to validate.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A ValueTask representing the asynchronous validation operation.</returns>
        protected virtual ValueTask ValidateAsync(TRequest request, CancellationToken cancellationToken)
        {
            if (!ValidateRequest)
                return ValueTask.CompletedTask;

            return ValidatorAdapter.ValidateAsync(request, cancellationToken);
        }

        /// <summary>
        /// Maps the request to an entity using the mapper adapter.
        /// </summary>
        /// <param name="request">The request to map.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A ValueTask representing the asynchronous mapping operation, with the mapped entity as the result.</returns>
        protected virtual ValueTask<TEntity> MapToEntityAsync(TRequest request, CancellationToken cancellationToken)
            => MapperAdapter.MapAsync<TRequest, TEntity>(request, cancellationToken);

        /// <summary>
        /// Saves the entity using the storage adapter.
        /// </summary>
        /// <param name="request">The request associated with the entity.</param>
        /// <param name="entity">The entity to save.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A Task representing the asynchronous save operation.</returns>
        protected virtual Task SaveEntityAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)
            => StorageWriterAdapter.SaveAsync(entity, cancellationToken);
    }
}
