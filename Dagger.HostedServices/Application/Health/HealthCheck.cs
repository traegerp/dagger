using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Dagger.HostedServices.Application.Health
{
    public class HealthCheck : IHealthCheck
    {
        public HealthCheck()
        {

        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return HealthCheckResult.Healthy();
        }
    }
}