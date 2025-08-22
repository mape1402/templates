namespace DTemplate.Business.Core.Infrastructure
{
    using FluentValidation;
    using Microsoft.Extensions.DependencyInjection;
    using DTemplate.Business.Core.Services;

    /// <summary>
    /// Provides an implementation of <see cref="IValidatorAdapter"/> using FluentValidation for model validation.
    /// </summary>
    internal class ValidatorAdapter : IValidatorAdapter
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorAdapter"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve validators.</param>
        public ValidatorAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Asynchronously validates the specified model using a registered FluentValidation validator.
        /// Throws <see cref="InvalidOperationException"/> if no validator is registered for the model type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model to validate.</typeparam>
        /// <param name="model">The model to validate.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A ValueTask representing the asynchronous validation operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no validator is registered for the model type.</exception>
        public async ValueTask ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken = default)
        {
            var validator = _serviceProvider.GetService<IValidator<TModel>>();

            if(validator == null)
                throw new InvalidOperationException($"No validator registered for type {typeof(TModel).FullName}");

            await validator.ValidateAndThrowAsync(model, cancellationToken);
        }
    }
}
