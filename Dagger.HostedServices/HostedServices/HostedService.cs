using System;
using System.Threading;
using System.Threading.Tasks;
using Dagger.Digraph;
using Dagger.HostedServices.Application.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dagger.HostedServices.HostedServices
{
    /// <summary>
    /// Hosted Service for Application
    /// </summary>
    public class HostedService:BackgroundService
    {
        private ILogger<HostedService> _logger;
        private Settings _settings;
        private IManager _manager;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="settings"></param>
        /// <param name="manager"></param>
        public HostedService(
            ILogger<HostedService> logger,
            Settings settings,
            IManager manager
        )
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new System.ArgumentNullException(nameof(settings));
            _manager = manager ?? throw new System.ArgumentNullException(nameof(manager));
        }

        /// <summary>
        /// Executes Background Task
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _manager.Execute();
                    await Task.Delay(_settings.Polling, stoppingToken);                    
                }
                catch(Exception error)
                {
                    _logger.LogError(error, nameof(ExecuteAsync));
                }
            }
        }
    }
}


