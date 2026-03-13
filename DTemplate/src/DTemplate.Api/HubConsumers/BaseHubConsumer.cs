using Pigeon.Messaging.Consuming.Dispatching;
using Spider.Pipelines.Core;

namespace DTemplate.Api.HubConsumers
{
    /// <summary>
    /// Provides a base hub consumer with access to the spider services.
    /// </summary>
    public abstract class BaseHubConsumer : HubConsumer
    {
        private ISpider _spider;

        /// <summary>
        /// Gets the spider instance from the current context.
        /// </summary>
        public ISpider Spider => _spider ??= Context.Services.GetRequiredService<ISpider>();
    }
}
