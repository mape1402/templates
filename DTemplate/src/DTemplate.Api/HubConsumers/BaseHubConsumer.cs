using Pigeon.Messaging.Consuming.Dispatching;
using Spider.Pipelines.Core;

namespace DTemplate.Api.HubConsumers
{
    public abstract class BaseHubConsumer : HubConsumer
    {
        private ISpider _spider;

        public ISpider Spider => _spider ??= Context.Services.GetRequiredService<ISpider>();
    }
}
