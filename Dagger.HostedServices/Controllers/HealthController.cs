using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Dagger.HostedServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private ILogger<HealthController> _logger;
        private IHealthCheck _healthCheck;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger"></param>
        public HealthController(
            ILogger<HealthController> logger,
            IHealthCheck healthCheck
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _healthCheck = healthCheck ?? throw new ArgumentNullException(nameof(healthCheck));
        }

        [HttpGet]
        /// <summary>
        /// Health End Point
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> HealthCheckPoint()
        {
            try
            {
                HealthCheckContext context = new HealthCheckContext();

                var result = await _healthCheck.CheckHealthAsync(context);

                return Ok(Enum.GetName(typeof(HealthStatus), result.Status));

            }
            catch(Exception error)
            {
                _logger.LogError(error, nameof(HealthCheckPoint));
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


    }
}
