using System;
using Dagger.Data.Services;

namespace Dagger.Data.Queues
{

    public interface IConsumerFactory
    {
        IConsumer GetConsumer();
    }

    /// <summary>
    /// Factory Class To Get Consumer
    /// </summary>
    public class ConsumerFactory:IConsumerFactory
    {

        private IConfigurationService _configurationService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="configurationService"></param>
        public ConsumerFactory(
            IConfigurationService configurationService
        )
        {
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
        }

        /// <summary>
        /// Get Consumer
        /// </summary>
        /// <returns></returns>
        public IConsumer GetConsumer()
        {
            return new RabbitMQClient();
        }
    }
}