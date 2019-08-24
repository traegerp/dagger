using System;
using System.Threading.Tasks;
using Dagger.Data.Queues;
using Microsoft.Extensions.Logging;

namespace Dagger.Digraph
{

    public interface IManager
    {
        Task Execute();
    }

    /// <summary>
    /// Process Manager
    /// </summary>
    public class Manager : IManager
    {
        private ILogger<Manager> _logger;
        private IConsumerFactory _consumerFactory;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="consumerFactory"></param>
        public Manager(
            ILogger<Manager> logger,
            IConsumerFactory consumerFactory
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _consumerFactory = consumerFactory ?? throw new ArgumentNullException(nameof(consumerFactory));
        }

        /// <summary>
        /// Entry Point of Application Business Logic
        /// </summary>
        /// <returns></returns>
        public async Task Execute()
        {
            try
            {
                var consumer = _consumerFactory.GetConsumer();

            }
            catch(Exception error)
            {
                _logger.LogError(error, nameof(Execute));
            }
        }
    }
}