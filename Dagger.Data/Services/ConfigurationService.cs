using System;
using System.Linq;
using System.Threading.Tasks;
using Dagger.Data.DTOs;
using Dagger.Data.Repositories;
using Dagger.Domain.Entities;

namespace Dagger.Data.Services
{
    public interface IConfigurationService
    {
        Task<Configuration> GetConfiguration();
    }

    /// <summary>
    /// Get Process Configuration
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        private IRepository<ConfigurationDTO> _repository;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="repository"></param>
        public ConfigurationService(
            IRepository<ConfigurationDTO> repository
        )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Get Configuration
        /// </summary>
        /// <returns></returns>
        public async Task<Configuration> GetConfiguration()
        {
            var config = (await _repository.ReadAll()).FirstOrDefault();

            return new Configuration()
            {
                Id = config.id,
                QueueSystem = config.queue_system
            };
        }
    }
}