using Microsoft.Extensions.Logging;
using Polly;

namespace AzureTestResources.AzureStorage.Common;

public static class AzureResources
{
    public static async Task<TApi> CreateApiToUnderlyingResource<TApi>(
      IAzureService<TApi> service,
      ILogger logger) where TApi : IAzureResourceApi
    {
        var response = await Policy.HandleResult<ICreateAzureResourceResponse<TApi>>(e => e.ShouldBeRetried())
          .WaitAndRetryAsync(30,
            _ => TimeSpan.FromSeconds(1),
            (delegateResult, _, retryCount, _) =>
            {
                logger.LogWarning($"Retry {retryCount} due to {delegateResult.Result.GetReasonForRetry()}");
            })
          .ExecuteAsync(async () =>
          {
              var response = await service.CreateResourceInstance();
              response.AssertValidResponse();
              return response;
          });
        return response.CreateResourceApi();
    }
}