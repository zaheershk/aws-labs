using Amazon.S3;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAdvert.API.HealthChecks
{
    public class S3HealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Health check in progress.");
            HealthCheckResult healthCheckStatus = HealthCheckResult.Unhealthy();

            using var client = new AmazonS3Client();
            var bucketData = await client.ListBucketsAsync();            
            if (bucketData != null && bucketData.Buckets != null && bucketData.Buckets.Any())
            {
                healthCheckStatus = HealthCheckResult.Healthy();
            }

            return await Task.FromResult(healthCheckStatus);
        }
    }
}
