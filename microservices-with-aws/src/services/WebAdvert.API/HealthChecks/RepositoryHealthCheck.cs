using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;
using WebAdvert.Data.Repository;

namespace WebAdvert.API.HealthChecks
{
    public class RepositoryHealthCheck : IHealthCheck
    {
        private readonly IAdvertRepository _advertRepository;

        public RepositoryHealthCheck(IAdvertRepository advertRepository)
        {
            _advertRepository = advertRepository;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var isRepositoryHealthy = await _advertRepository.CheckHealthAsync();

            HealthCheckResult healthCheckStatus = HealthCheckResult.Unhealthy();
            if (isRepositoryHealthy)
            {
                healthCheckStatus = HealthCheckResult.Healthy();
            }

            return await Task.FromResult(healthCheckStatus);
        }
    }
}
