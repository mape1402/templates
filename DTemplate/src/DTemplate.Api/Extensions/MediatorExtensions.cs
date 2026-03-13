using Krackend.Sagas.Orchestration.Working;
using Pelican.Mediator;
using Spider.Pipelines.Core;

namespace DTemplate.Api.Extensions
{
    /// <summary>
    /// Provides mediator-related extensions for the Spider service bridge.
    /// </summary>
    public static class MediatorExtensions
    {
        /// <summary>
        /// Creates a mediator service bridge from a spider instance.
        /// </summary>
        /// <param name="spider">The spider instance.</param>
        /// <returns>A mediator service bridge.</returns>
        public static IServiceBridge<IMediator> AsMediator(this ISpider spider)
            => spider.InitBridge<IMediator>();

        /// <summary>
        /// Sends a request through a mediator service bridge.
        /// </summary>
        /// <typeparam name="TRequest">The request type.</typeparam>
        /// <param name="bridge">The mediator bridge.</param>
        /// <param name="request">The request to send.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task Send<TRequest>(this IServiceBridge<IMediator, TRequest> bridge, TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest
            => bridge.ExecuteAsync(mediator => mediator.Send, request, cancellationToken);

        /// <summary>
        /// Sends a request through a mediator service bridge and returns a response.
        /// </summary>
        /// <typeparam name="TRequest">The request type.</typeparam>
        /// <typeparam name="TResponse">The response type.</typeparam>
        /// <param name="bridge">The mediator bridge.</param>
        /// <param name="request">The request to send.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, with the response as the result.</returns>
        public static Task<TResponse> Send<TRequest, TResponse>(this IServiceBridge<IMediator, TRequest, TResponse> bridge, TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest<TResponse>
        {
            return bridge.ExecuteAsync(mediator => (req, ct) => mediator.Send(req, ct), request, cancellationToken);
        }

        /// <summary>
        /// Creates a default forwarding bridge for a request type.
        /// </summary>
        /// <typeparam name="TRequest">The request type.</typeparam>
        /// <param name="bridge">The mediator bridge.</param>
        /// <returns>The configured service bridge.</returns>
        public static IServiceBridge<IMediator, TRequest> DefaultForwading<TRequest>(this IServiceBridge<IMediator> bridge)
            where TRequest : IRequest
        {
            return bridge
                    .Attach<TRequest>(pipeline =>
                    {
                        pipeline
                            .DefaultForwading();
                    });
        }

        /// <summary>
        /// Creates a default forwarding bridge for a request and response type.
        /// </summary>
        /// <typeparam name="TRequest">The request type.</typeparam>
        /// <typeparam name="TResponse">The response type.</typeparam>
        /// <param name="bridge">The mediator bridge.</param>
        /// <returns>The configured service bridge.</returns>
        public static IServiceBridge<IMediator, TRequest, TResponse> DefaultForwading<TRequest, TResponse>(this IServiceBridge<IMediator> bridge)
            where TRequest : IRequest<TResponse>
        {
            return bridge
                    .Attach<TRequest, TResponse>(pipeline =>
                    {
                        pipeline
                            .DefaultForwading();
                    });
        }

        /// <summary>
        /// Sends a request using the default forwarding bridge.
        /// </summary>
        /// <typeparam name="TRequest">The request type.</typeparam>
        /// <param name="spider">The spider instance.</param>
        /// <param name="request">The request to send.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task DefaultSend<TRequest>(this ISpider spider, TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest
        {
            return spider
                    .AsMediator()
                    .DefaultForwading<TRequest>()
                    .Send(request, cancellationToken);
        }

        /// <summary>
        /// Sends a request using the default forwarding bridge and returns a response.
        /// </summary>
        /// <typeparam name="TResponse">The response type.</typeparam>
        /// <param name="spider">The spider instance.</param>
        /// <param name="request">The request to send.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, with the response as the result.</returns>
        public static Task<TResponse> DefaultSend<TResponse>(this ISpider spider, IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            return spider
                    .AsMediator()
                    .DefaultForwading<IRequest<TResponse>, TResponse>()
                    .Send(request, cancellationToken);
        }
    }
}
