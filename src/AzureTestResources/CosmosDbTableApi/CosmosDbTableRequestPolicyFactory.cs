using System.Collections.Immutable;
using System.Net;
using Azure;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace AzureTestResources.CosmosDbTableApi;

public static class CosmosDbTableRequestPolicyFactory
{
    private static readonly IReadOnlyList<HttpStatusCode> CreateResourceRetryCodes =
      new List<HttpStatusCode>
      {
      HttpStatusCode.ServiceUnavailable,
      HttpStatusCode.InternalServerError,
      HttpStatusCode.Conflict
      }.ToImmutableArray();

    public static AsyncRetryPolicy CreateCreateResourcePolicy(ILogger logger)
    {
        return CreateDefaultPolicy(logger, CreateResourceRetryCodes);
    }

    private static AsyncRetryPolicy CreateDefaultPolicy(ILogger logger,
      IReadOnlyList<HttpStatusCode> createResourceRetryCodes)
    {
        return Policy.Handle<RequestFailedException>(
            e => createResourceRetryCodes.Any(c => (int)c == e.Status))
          .WaitAndRetryAsync(5,
            _ => TimeSpan.FromSeconds(10),
            (exception, _, retryCount, _) =>
            {
                logger.LogWarning(
              $"Retry {retryCount} due to status code {((RequestFailedException)exception).Status} and error {((RequestFailedException)exception).ErrorCode}");
            });
    }
}