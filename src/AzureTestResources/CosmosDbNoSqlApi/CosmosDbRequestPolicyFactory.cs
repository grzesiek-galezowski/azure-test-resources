using System.Collections.Immutable;
using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace AzureTestResources.CosmosDbNoSqlApi;

public static class CosmosDbRequestPolicyFactory
{
    private static readonly IReadOnlyList<HttpStatusCode> CreateResourceRetryCodes =
      new List<HttpStatusCode>
      {
      HttpStatusCode.ServiceUnavailable,
      HttpStatusCode.InternalServerError,
      HttpStatusCode.Conflict
      }.ToImmutableArray();

    private static readonly IReadOnlyList<HttpStatusCode> CreateSubResourceRetryCodes =
      new List<HttpStatusCode>
      {
      HttpStatusCode.ServiceUnavailable,
      HttpStatusCode.InternalServerError
      }.ToImmutableArray();

    public static AsyncRetryPolicy CreateCreateResourcePolicy(ILogger logger)
    {
        return CreateDefaultPolicy(logger, CreateResourceRetryCodes);
    }

    public static AsyncRetryPolicy CreateCreateSubResourcePolicy(ILogger logger)
    {
        return CreateDefaultPolicy(logger, CreateSubResourceRetryCodes);
    }

    private static AsyncRetryPolicy CreateDefaultPolicy(ILogger logger,
      IReadOnlyList<HttpStatusCode> createResourceRetryCodes)
    {
        return Policy.Handle<CosmosException>(
            e => createResourceRetryCodes.Any(c => c == e.StatusCode))
          .WaitAndRetryAsync(5,
            _ => TimeSpan.FromSeconds(10),
            (exception, _, retryCount, _) =>
            {
                logger.LogWarning($"Retry {retryCount} due to status code {((CosmosException)exception).StatusCode}");
            });
    }
}