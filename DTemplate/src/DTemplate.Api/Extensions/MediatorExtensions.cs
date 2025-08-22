using Krackend.Sagas.Orchestration.Working;
using Pelican.Mediator;
using Spider.Pipelines.Core;

namespace DTemplate.Api.Extensions
{
    public static class MediatorExtensions
    {
        public static IServiceBridge<IMediator> AsMediator(this ISpider spider)
            => spider.InitBridge<IMediator>();

        public static Task Send<TRequest>(this IServiceBridge<IMediator, TRequest> bridge, TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest
            => bridge.ExecuteAsync(mediator => mediator.Send, request, cancellationToken);

        public static Task<TResponse> Send<TRequest, TResponse>(this IServiceBridge<IMediator, TRequest, TResponse> bridge, TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest<TResponse>
        {
            return bridge.ExecuteAsync(mediator => (req, ct) => mediator.Send(req, ct), request, cancellationToken);
        }

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

        public static Task DefaultSend<TRequest>(this ISpider spider, TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest
        {
            return spider
                    .AsMediator()
                    .DefaultForwading<TRequest>()
                    .Send(request, cancellationToken);
        }

        public static Task<TResponse> DefaultSend<TResponse>(this ISpider spider, IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            return spider
                    .AsMediator()
                    .DefaultForwading<IRequest<TResponse>, TResponse>()
                    .Send(request, cancellationToken);
        }
    }
}
