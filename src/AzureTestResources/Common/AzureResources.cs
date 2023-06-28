using Microsoft.Extensions.Logging;
using Polly;

namespace AzureTestResources.Common;

public static class AzureResources
{
  public static async Task<TApi> CreateApiToUnderlyingResource<TApi>(
    IAzureService<TApi> service,
    string resourceName,
    ILogger logger) where TApi : IAzureResourceApi
  {
    var response = await Policy
      .HandleResult<ICreateAzureResourceResponse<TApi>>(e => e.ShouldBeRetried())
      .WaitAndRetryAsync(30,
        _ => TimeSpan.FromSeconds(1),
        onRetry: (delegateResult, retryCount, _) =>
        {
          logger.LogWarning($"Retry {retryCount} due to {delegateResult.Result.GetReasonForRetry()}");
        })
      .ExecuteAsync(async () => await service.CreateResourceInstance());
    response.AssertResourceCreated();
    var apiToUnderlyingResource = response.CreateResourceApi();
    logger.Created(resourceName, apiToUnderlyingResource.Name);
    return apiToUnderlyingResource;
  }

  public static readonly TimeSpan DefaultZombieToleranceForEmulator = TimeSpan.FromMinutes(1);
}