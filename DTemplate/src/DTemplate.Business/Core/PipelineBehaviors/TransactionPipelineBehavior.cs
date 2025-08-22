using Pelican.Mediator;
using System.Transactions;

namespace DTemplate.Business.Core.PipelineBehaviors
{
    public sealed class TransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, Handler<TResponse> next, CancellationToken cancellationToken = default)
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.MaximumTimeout
            };

            using (var transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
            {
                var response = await next();

                transaction.Complete();

                return response;
            }
        }
    }
}
