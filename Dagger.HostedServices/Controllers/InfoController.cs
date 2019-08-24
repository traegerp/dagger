using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Dagger.HostedServices.Application.Configuration;
using Dagger.HostedServices.Application.Information;

namespace Dagger.HostedServices.Controllers
{
    /// <summary>
    /// Get Info of Node
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private ILogger<InfoController> _logger;
        private Settings _settings;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="settings"></param>
        public InfoController(
            ILogger<InfoController> logger,
            Settings settings
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        [HttpGet]
        /// <summary>
        /// Health End Point
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetInfo()
        {
            try
            {
                return Ok(
                    new Info()
                    {
                        Settings = _settings,
                        IpAddress = Info.GetLocalIPAddress()
                    }
                );
            }
            catch(Exception error)
            {
                _logger.LogError(error, nameof(Info));
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


    }
}
