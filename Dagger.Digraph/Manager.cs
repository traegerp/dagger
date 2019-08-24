using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dagger.Data.Queues;
using Microsoft.Extensions.Logging;
using Dagger.Digraph.Graphes;
using Microsoft.Extensions.Caching.Memory;
using Dagger.Domain.Models;

namespace Dagger.Digraph
{

    public interface IManager
    {
        Task<bool> Execute();
    }

    /// <summary>
    /// Process Manager
    /// </summary>
    public class Manager : IManager
    {
        private ILogger<Manager> _logger;
        private IConsumerFactory _consumerFactory;
        private IMemoryCache _memoryCache;
        private IProcessor _processor;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="cache"></param>
        /// <param name="consumerFactory"></param>
        public Manager(
            ILogger<Manager> logger,
            IMemoryCache cache,
            IConsumerFactory consumerFactory,
            IProcessor processor
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _consumerFactory = consumerFactory ?? throw new ArgumentNullException(nameof(consumerFactory));
            _memoryCache = cache ?? throw new ArgumentNullException(nameof(cache));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        /// <summary>
        /// Entry Point of Application Business Logic
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Execute()
        {
            try
            {

                bool hasItems = false;

                //get consumer 
                var consumer = _consumerFactory.GetConsumer();

                //get items from queue
                await consumer.Consume<IEnumerable<Payload>>(async (IEnumerable<Payload> items) =>
                {

                    if (items != null && items.Any())
                    {

                        hasItems = true;

                        var graph = await GetGraph();                        
                        
                        await _processor.Run(items, graph);

                    }

                });

                return hasItems;

            }
            catch (Exception error)
            {
                _logger.LogError(error, nameof(Execute));
                throw;
            }
        }

        /// <summary>
        /// Get Graph from cache
        /// </summary>
        /// <returns></returns>
        private async Task<Graph> GetGraph()
        {
            return await _memoryCache.GetOrCreateAsync<Graph>(nameof(Graph), async (opt) => {
                return new Graph();
            });
        }

    }
}