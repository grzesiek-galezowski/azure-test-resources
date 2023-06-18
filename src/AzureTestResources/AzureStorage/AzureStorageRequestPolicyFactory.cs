using System.Net;
using Azure;
using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace AzureTestResources.AzureStorage;

public static class AzureStorageRequestPolicyFactory
{
  public static AsyncRetryPolicy<Response<QueueClient>> CreateCreateResourcePolicy(ILogger logger)
  {
    return Policy.HandleResult<Response<QueueClient>>(
        e => e.GetRawResponse().Status == (int)HttpStatusCode.NoContent)
      .WaitAndRetryAsync(30,
        _ => TimeSpan.FromSeconds(1),
        (delegateResult, _, retryCount, _) =>
        {
          logger.LogWarning(
            $"Retry {retryCount} due to status code {delegateResult.Result.GetRawResponse().Status} " +
            $"and error {delegateResult.Result.GetRawResponse().ReasonPhrase}");
        });
  }
}